using System.Collections.Generic;
using System.Linq;
using Nutcracker.Morphology;

namespace Nutcracker.Morphology.Bindings
{
    /// <summary>
    ///     npro - местоимения
    /// </summary>
    public class NproBindingStrategy : BindingStrategyBase
    {
        private Entity _owner;
        private IEnumerable<Entity> _speakers;
        private List<LexicalUnitDescriptor> _descriptors;

        protected override Entity[] GetWindow(List<LexicalUnitDescriptor> descriptors, LexicalUnitDescriptor current)
        {
            return descriptors.Where(x => x.SentenceId <= current.SentenceId).SelectMany(x => x.Words).ToArray();
        }

        public NproBindingStrategy(Entity owner, IEnumerable<Entity> speakers, List<LexicalUnitDescriptor> descriptors)
        {
            _owner = owner;
            _speakers = speakers.Where(x=>!x.Info.CanBeSameLexeme(owner.Info));
            _descriptors = descriptors;;
        }


        public Binding CheckDefault(Entity owner
            , Entity entity
            , IEnumerable<Entity> speakers
            , Entity[] window)
        {
            var entities = new List<Entity>();
            var position = GetCurrentPosition(entity, window);
            if (position > 0)
                entities.Add(Find(new[] {GrammConst.Noun}, position, window.ToArray(), EnumFindDirection.Left));
            return new Binding
            {
                BindingType = EnumBindingType.Alias,
                Owner = owner,
                Main = entity,
                Source = entity,
                Target = GetNearestEntity(entity, entities, speakers)
            };
        }

        public Binding CheckAlias(Entity owner
            , Entity entity
            , Entity[] window)
        {
            var leftEntities = new List<Entity>();
            var rightEntities = new List<Entity>();
            var position = GetCurrentPosition(entity, window);
            if (position > 0)
            {
                leftEntities.Add(Find(new[] {GrammConst.Noun}, position, window.ToArray(), EnumFindDirection.Left));
                rightEntities.Add(Find(new[] {GrammConst.Noun}, position, window.ToArray(), EnumFindDirection.Right));
            }

            return new Binding
            {
                BindingType = EnumBindingType.Alias,
                Owner = owner,
                Main = entity,
                Source = GetNearestEntity(entity, leftEntities),
                Target = GetNearestEntity(entity, rightEntities)
            };
        }

        public override Binding CreateBinding(Entity owner
            , Entity entity
            , List<LexicalUnitDescriptor> descriptors
            , LexicalUnitDescriptor current)
        {
            var window = GetWindow(descriptors, current);
            switch (entity.Info.Text.ToLower())
            {
                case "это":
                    return CheckAlias(owner, entity, window);
                case "эта":
                    return CheckAlias(owner, entity, window);
                case "нее":
                    return CheckDefault(owner, entity,  _speakers, window);
                case "я":
                    return new Binding() 
                        {BindingType = EnumBindingType.Alias, Main = entity, Owner = owner, Source = owner , Target = owner};
                default:
                    return null;
            }
        }
    }
}