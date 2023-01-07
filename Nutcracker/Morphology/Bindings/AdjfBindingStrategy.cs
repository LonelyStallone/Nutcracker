using System.Collections.Generic;
using System.Linq;
using Nutcracker.Morphology;

namespace Nutcracker.Morphology.Bindings
{
    /// <summary>
    ///  adjf - прилагательное
    /// </summary>
    public class AdjfBindingStrategy : BindingStrategyBase
    {
        public override Binding CreateBinding(Entity owner, Entity entity, List<LexicalUnitDescriptor> descriptors, LexicalUnitDescriptor current)
        {
            var window = GetWindow(descriptors, current);
            var entities = new List<Entity>();
            var position = GetCurrentPosition(entity, window);
            if (position > 0)
            {
                entities.Add(Find(new[] {GrammConst.Noun}, position, window.ToArray(), EnumFindDirection.Right));
                entities.Add(Find(new[] {GrammConst.Npro}, position, window.ToArray(), EnumFindDirection.Right));
                entities.Add(Find(new[] {GrammConst.Verb}, position, window.ToArray(), EnumFindDirection.Right));
                entities.Add(Find(new[] {GrammConst.Noun}, position, window.ToArray(), EnumFindDirection.Left));
                entities.Add(Find(new[] {GrammConst.Npro}, position, window.ToArray(), EnumFindDirection.Left));
                entities.Add(Find(new[] {GrammConst.Verb}, position, window.ToArray(), EnumFindDirection.Left));
            }

            return new Binding
            {
                BindingType = EnumBindingType.Relationship,
                Owner = owner,
                Main = entity,
                Source = entity,
                Target = GetNearestEntity(entity, entities)
            };
        }

    }
}