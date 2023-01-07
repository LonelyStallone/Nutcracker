using System.Collections.Generic;
using System.Linq;

namespace Nutcracker.Morphology.ExtractStrategies
{
    public class SentenceStrategy : IExtractStrategy
    {
        public SentenceStrategy()
        {
            _delimeters = new[] {'.', '!', '?'};
            _ignore = new[] {"? —", " "};
        }

        private char[] _delimeters { get; }
        private string[] _ignore { get; }

        public IEnumerable<LexicalUnitDescriptor> Extract(LexicalUnitDescriptor lexicalUnitDescriptor)
        {
            var returnList = new List<LexicalUnitDescriptor>();
            var lastEndPosition = -1;
            for (var i = 0; i < lexicalUnitDescriptor.Data.Length; i++)
                if (_delimeters.Contains(lexicalUnitDescriptor.Data[i]))
                {
                    if (CheckIgnore(lexicalUnitDescriptor.Data, i))
                        continue;
                    var length = i - lastEndPosition;
                    returnList.Add(new LexicalUnitDescriptor
                    {
                        SentenceId = returnList.Count(),
                        Data = lexicalUnitDescriptor.Data.Substring(lastEndPosition + 1, length).Trim(),
                        Offset = lastEndPosition + 1,
                        UnitType = EnumLexicalUnitType.CommonSentence
                    });
                    lastEndPosition = i;
                }

            return returnList;
        }

        public bool CheckIgnore(string text, int offset)
        {
            var data = text.Substring(offset);
            return _ignore.Any(x => data.StartsWith(x));
        }
    }
}