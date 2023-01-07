using Nutcracker.Morphology;

namespace Nutcracker.Morphology.Bindings
{
    public class Binding
    {
        public EnumBindingType BindingType = EnumBindingType.None;
        public Entity Owner { get; set; }
        public Entity Source { get; set; }
        public Entity Target { get; set; }
        public Entity Main { get; set; }
    }
}