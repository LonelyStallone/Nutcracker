using System.Collections.Generic;

namespace Nutcracker.Morphology.ExtractStrategies
{
    public interface IContextStrategy
    {
        IEnumerable<LexicalUnitDescriptor> Extract(IEnumerable<LexicalUnitDescriptor> lexicalUnitDescriptor);
    }
}