using System.Collections.Generic;
using System.Linq;

namespace Nutcracker.Morphology.Helpers
{
    public static class SentenceHelper
    {
        public static List<List<LexicalUnitDescriptor>> GetSetntences(
            IEnumerable<LexicalUnitDescriptor> lexicalUnitDescriptors)
        {
            return lexicalUnitDescriptors
                .GroupBy(p => p.SentenceId)
                .Select(g => g.ToList()).ToList();
        }

        public static List<LexicalUnitDescriptor> GetSetntence(
            IEnumerable<LexicalUnitDescriptor> lexicalUnitDescriptors, int id)
        {
            var map = lexicalUnitDescriptors
                .GroupBy(o => o.SentenceId)
                .ToDictionary(g => g.Key, g => g.ToList());
            if (map.TryGetValue(id, out var sentence))
                return sentence;
            return null;
        }
    }
}