using System.Collections.Generic;

namespace Nutcracker.Morphology.ExtractStrategies
{
    public interface IExtractStrategy
    {
        IEnumerable<LexicalUnitDescriptor> Extract(LexicalUnitDescriptor lexicalUnitDescriptor);
    }
}