using System;
using System.Collections.Generic;
using System.Linq;
using Nutcracker.Morphology;

namespace Nutcracker.Morphology.Bindings
{
    public abstract class BindingStrategyBase : IBindingStrategyFactory
    {
        protected int _maxRightLeftFindDifference = 5;

        protected virtual Entity[] GetWindow(List<LexicalUnitDescriptor> descriptors, LexicalUnitDescriptor curent)
        {
            return curent.Words.ToArray();
        }

        public abstract Binding CreateBinding(Entity owner
            , Entity entity
            , List<LexicalUnitDescriptor> descriptors
            , LexicalUnitDescriptor curent);


        protected Entity GetNearestEntity(Entity meeting, IEnumerable<Entity> entities)
        {
            return entities.Where(x => x != null).OrderBy(x => x.Distance(meeting)).FirstOrDefault();
        }

        protected Entity GetNearestEntity(Entity meeting, IEnumerable<Entity> entities, IEnumerable<Entity> speakers)
        {
            return entities.Where(x => x != null)
                .OrderBy(x => x.Distance(meeting) 
                              - (speakers.Any(y=>y.Info.CanBeSameLexeme(x.Info))?64:0)).FirstOrDefault();
        }

 

        protected int GetCurrentPosition(Entity current, Entity[] window)
        {
            for (var i = 0; i < window.Length; i++)
                if (window[i].Offset == current.Offset)
                    return i;
            return -1;
        }

        private bool TryFindRight(string[] tags, int currentPosition, Entity[] window, out Entity value, out int shift)
        {
            for (var i = currentPosition + 1; i < window.Length; i++)
                if (window[i].Info.BestTag.Grams.Intersect(tags).Count() == tags.Length)
                {
                    shift = Math.Abs(i - currentPosition);
                    value = window[i];
                    return true;
                }

            value = null;
            shift = int.MaxValue;
            return false;
        }

        private bool TryFindLeft(string[] tags, int currentPosition, Entity[] window, out Entity value, out int shift)
        {
            for (var i = currentPosition - 1; i >= 0; i--)
                if (window[i].Info.BestTag.Grams.Intersect(tags).Count() == tags.Length)
                {
                    shift = Math.Abs(i - currentPosition);
                    value = window[i];
                    return true;
                }

            value = null;
            shift = int.MaxValue;
            return false;
        }

        protected Entity Find(string[] tags, int currentPosition, Entity[] window, EnumFindDirection direction)
        {
            switch (direction)
            {
                case EnumFindDirection.Left:
                {
                    TryFindLeft(tags, currentPosition, window, out var value, out var _);
                    return value;
                }
                case EnumFindDirection.Right:
                {
                    TryFindRight(tags, currentPosition, window, out var value, out var _);
                    return value;
                }
                case EnumFindDirection.LeftThenRight:
                {
                    if (TryFindRight(tags, currentPosition, window, out var rightValue, out var rightShift))
                    {
                        if (TryFindLeft(tags, currentPosition, window, out var leftValue, out var leftShift))
                            return rightShift < leftShift + _maxRightLeftFindDifference ? rightValue : leftValue;
                        return rightValue;
                    }

                    break;
                }
                case EnumFindDirection.RightThenLeft:
                {
                    if (TryFindLeft(tags, currentPosition, window, out var leftValue, out var leftShift))
                    {
                        if (TryFindRight(tags, currentPosition, window, out var rightValue, out var rightShift))
                            return leftShift < rightShift + _maxRightLeftFindDifference ? leftValue : rightValue;
                        return leftValue;
                    }

                    break;
                }
            }

            return null;
        }
    }
}