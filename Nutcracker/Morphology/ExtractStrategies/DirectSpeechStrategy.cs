using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nutcracker.Morphology.ExtractStrategies
{
    public class DirectSpeechStrategy : IExtractStrategy
    {
        private const int _minOffsetShift = 3;

        public DirectSpeechStrategy()
        {
            _patterns = new[]
            {
                ":— ",
                "^— ",
                ", — ",
                @"\? — ",
                @"\! — "
            };
            _regexes = _patterns.Select(x => new Regex(x));
        }

        private string[] _patterns { get; }
        private IEnumerable<Regex> _regexes { get; }


        public IEnumerable<LexicalUnitDescriptor> Extract(LexicalUnitDescriptor lexicalUnitDescriptor)
        {
            var returnDescriptors = new List<LexicalUnitDescriptor>();
            var matchPositions = FindMatchPositions(_regexes, lexicalUnitDescriptor);
            if (!matchPositions.Any())
                return new[] {lexicalUnitDescriptor};

            if (matchPositions.First().Offset > _minOffsetShift)
                ProcessInterval(lexicalUnitDescriptor
                    , new ValueTuple<int, int>(0, 0)
                    , matchPositions.First().Offset
                    , returnDescriptors);


            for (var i = 0; i < matchPositions.Count - 1; i++)
                ProcessInterval(lexicalUnitDescriptor
                    , matchPositions[i]
                    , matchPositions[i + 1].Offset
                    , returnDescriptors);

            ProcessInterval(lexicalUnitDescriptor
                , matchPositions.Last()
                , lexicalUnitDescriptor.Data.Length
                , returnDescriptors);

            return returnDescriptors;
        }

        private List<(int Offset, int Length)> FindMatchPositions(IEnumerable<Regex> regexList,
            LexicalUnitDescriptor lexicalUnitDescriptor)
        {
            var returnList = new List<(int Offset, int Length)>();
            foreach (var regex in _regexes)
            {
                var matches = regex.Matches(lexicalUnitDescriptor.Data);
                foreach (Match match in matches) returnList.Add(new ValueTuple<int, int>(match.Index, match.Length));
            }

            return returnList;
        }

        public void ProcessInterval(LexicalUnitDescriptor lexicalUnitDescriptor
            , (int Offset, int Length) currnetPosition
            , int nextOffset
            , List<LexicalUnitDescriptor> returnDescriptors)
        {
            var startPosition = currnetPosition.Offset;
            var length = nextOffset - startPosition;
            var data = lexicalUnitDescriptor.Data.Substring(startPosition, length);
            returnDescriptors.Add(new LexicalUnitDescriptor
            {
                SentenceId = lexicalUnitDescriptor.SentenceId,
                Data = data,
                Offset = startPosition + lexicalUnitDescriptor.Offset,
                UnitType = EnumLexicalUnitType.DirectSpeech
            });
        }
    }
}