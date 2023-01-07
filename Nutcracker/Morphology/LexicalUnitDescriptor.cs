using System.Collections.Generic;
using Nutcracker.Morphology;

namespace Nutcracker.Morphology
{
    public class LexicalUnitDescriptor
    {
        public string Data;
        public int Offset;
        public Entity Owner;
        public int SentenceId;
        public EnumLexicalUnitType UnitType;
        public IEnumerable<Entity> Words;
    }
}