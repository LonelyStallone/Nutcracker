using System.Collections.Generic;
using System.Linq;

namespace Nutcracker.Morphology.ExtractStrategies
{
    public class SourceSpeechStrategy : ISentenceStrategy
    {
        public IEnumerable<LexicalUnitDescriptor> Extract(IEnumerable<LexicalUnitDescriptor> sentences)
        {
            if (sentences.All(x => x.UnitType == EnumLexicalUnitType.DirectSpeech))
            {
                var firstIsSpeeker = sentences.First().Data.StartsWith("—");
                var counter = firstIsSpeeker ? 1 : 0;
                foreach (var descriptor in sentences)
                {
                    descriptor.UnitType = counter % 2 == 0 ? EnumLexicalUnitType.Speaker : EnumLexicalUnitType.Speech;
                    counter++;
                }
            }

            return sentences;
        }
    }
}