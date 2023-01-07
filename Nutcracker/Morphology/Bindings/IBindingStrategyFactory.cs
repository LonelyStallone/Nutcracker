using System.Collections.Generic;
using Nutcracker.Morphology;

namespace Nutcracker.Morphology.Bindings
{
    public interface IBindingStrategyFactory
    {
        public abstract Binding CreateBinding(Entity owner
            , Entity entity
            , List<LexicalUnitDescriptor> descriptors
            , LexicalUnitDescriptor current);
    }
}