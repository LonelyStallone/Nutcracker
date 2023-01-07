using System.Collections.Generic;
using System.Linq;
using Nutcracker.Morphology;

namespace Nutcracker.Morphology.Bindings
{
    /// <summary>
    ///     гл - глагол
    /// </summary>
    public class VerbBindingStrategy : BindingStrategyBase
    {

        public override Binding CreateBinding(Entity owner
            , Entity entity
            , List<LexicalUnitDescriptor> descriptors
            , LexicalUnitDescriptor current)
        {
            var window = GetWindow(descriptors, current);
            var leftEntities = new List<Entity>();
            var rightEntities = new List<Entity>();
            var position = GetCurrentPosition(entity, window);
            if (position >= 0)
            {
                rightEntities.Add(Find(new[] {GrammConst.Noun}, position, window.ToArray(), EnumFindDirection.Right));
                rightEntities.Add(Find(new[] {GrammConst.Npro}, position, window.ToArray(), EnumFindDirection.Right));
                leftEntities.Add(Find(new[] {GrammConst.Noun}, position, window.ToArray(), EnumFindDirection.Left));
                leftEntities.Add(Find(new[] {GrammConst.Npro}, position, window.ToArray(), EnumFindDirection.Left));
            }

            return new Binding
            {
                BindingType = EnumBindingType.Relationship,
                Owner = owner,
                Main = entity,
                Source = GetNearestEntity(entity, leftEntities),
                Target = GetNearestEntity(entity, rightEntities)
            };
        }
    }
}