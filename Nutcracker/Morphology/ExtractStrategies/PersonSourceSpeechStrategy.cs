using System.Collections.Generic;
using System.Linq;

namespace Nutcracker.Morphology.ExtractStrategies
{
    public class PersonSourceSpeechStrategy : IExtractStrategy
    {
        public IEnumerable<LexicalUnitDescriptor> Extract(LexicalUnitDescriptor lexicalUnitDescriptor)
        {
            if (lexicalUnitDescriptor.Words.Any(x => x.Info.BestTag.Grams.Any(y => y == GrammConst.Person1))
                || lexicalUnitDescriptor.Words.Any(x => x.Info.BestTag.Grams.Any(y => y == GrammConst.Person2)))
                lexicalUnitDescriptor.UnitType = EnumLexicalUnitType.Speech;
            return new[] {lexicalUnitDescriptor};
        }
    }
}