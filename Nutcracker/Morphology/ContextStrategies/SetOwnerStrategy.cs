using System.Collections.Generic;
using System.Linq;
using Nutcracker.Morphology;
using Nutcracker.Morphology.Helpers;

namespace Nutcracker.Morphology.ExtractStrategies
{
    public class SetOwnerStrategy : IContextStrategy
    {
        public IEnumerable<LexicalUnitDescriptor> Extract(IEnumerable<LexicalUnitDescriptor> lexicalUnitDescriptors)
        {
            var listItems = lexicalUnitDescriptors.ToList();
            for (var i = 0; i < listItems.Count; i++)
            {
                var curretnItem = listItems[i];
                if (curretnItem.UnitType == EnumLexicalUnitType.CommonSentence
                    || curretnItem.UnitType == EnumLexicalUnitType.Speaker)
                {
                    curretnItem.Owner = Entity.GetStoryteller();
                }
                else if (curretnItem.UnitType == EnumLexicalUnitType.Speech)
                {
                    var unit = GetOwnerLexicalUnit(listItems, curretnItem);
                    if (unit != null) curretnItem.Owner = GetOwner(unit);
                }
            }

            return lexicalUnitDescriptors;
        }

        private LexicalUnitDescriptor GetOwnerLexicalUnit(List<LexicalUnitDescriptor> lexicalUnitDescriptors,
            LexicalUnitDescriptor currentDescriptor)
        {
            if (currentDescriptor.UnitType == EnumLexicalUnitType.Speech)
                for (var i = currentDescriptor.SentenceId; i >= 0; i--)
                {
                    var speakerUnit = SentenceHelper.GetSetntence(lexicalUnitDescriptors, i)
                        .FirstOrDefault(x => x.UnitType == EnumLexicalUnitType.Speaker);
                    if (speakerUnit != null)
                        return speakerUnit;
                }

            return null;
        }

        private Entity GetOwner(LexicalUnitDescriptor currentDescriptor)
        {
            var items = currentDescriptor.Words.Where(x => x.Info.BestTag.Grams.Any(y => y == GrammConst.Noun)
                                                           && x.Info.BestTag.Grams.Any(y => y == GrammConst.Nomn));
            if (!items.Any())
                return null;

            Entity maxItem = null;
            var maxValue = int.MinValue;


            foreach (var item in items)
            {
                var localValue = 0;
                foreach (var word in currentDescriptor.Words.Where(x =>
                    x.Info.BestTag.Grams.Any(x => x == GrammConst.Verb)))
                    if (word.IsSameGndr(item))
                        localValue++;

                if (localValue > maxValue)
                {
                    maxItem = item;
                    maxValue = localValue;
                }
            }

            return maxItem;
        }
    }
}