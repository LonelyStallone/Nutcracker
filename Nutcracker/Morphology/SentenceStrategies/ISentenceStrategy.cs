using System.Collections.Generic;

namespace Nutcracker.Morphology.ExtractStrategies
{
    public interface ISentenceStrategy
    {
        IEnumerable<LexicalUnitDescriptor> Extract(IEnumerable<LexicalUnitDescriptor> lexicalUnitDescriptor);
    }
}