using System;
using System.Collections.Generic;
using Nutcracker.Morphology;

namespace Nutcracker.Morphology.Bindings
{
    public static class BindingStrategyFactory
    {
        public static IBindingStrategyFactory Create(string grammKey, LexicalUnitDescriptor currentDescriptor , IEnumerable<Entity> speakers, List<LexicalUnitDescriptor> descriptors)
        {
            switch (grammKey)
            {
                case GrammConst.Adjf:
                {
                    return new AdjfBindingStrategy();
                }
                case GrammConst.Npro:
                {
                    return new NproBindingStrategy(currentDescriptor.Owner, speakers, descriptors);
                }
                case GrammConst.Verb:
                {
                    return new VerbBindingStrategy();
                }
                default:
                    return null;
            }
        }
    }
}