using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DeepMorphy;
using Nutcracker.Morphology;

namespace Nutcracker.Morphology.ExtractStrategies
{
    public class ApplyMorphologyStrategy : IExtractStrategy
    {
        public int _wordCounter;

        public ApplyMorphologyStrategy()
        {
            _morphAnalyzer = new MorphAnalyzer(true);
        }

        private MorphAnalyzer _morphAnalyzer { get; }

        public IEnumerable<LexicalUnitDescriptor> Extract(LexicalUnitDescriptor lexicalUnitDescriptor)
        {
            return new[]
            {
                new LexicalUnitDescriptor
                {
                    Data = lexicalUnitDescriptor.Data,
                    Offset = lexicalUnitDescriptor.Offset,
                    UnitType = lexicalUnitDescriptor.UnitType,
                    SentenceId = lexicalUnitDescriptor.SentenceId,
                    Words = DetectMeeting(lexicalUnitDescriptor.Data, lexicalUnitDescriptor.Offset)
                }
            };
        }

        private IEnumerable<Entity> DetectMeeting(string text, int globalOffset)
        {
            var regex = new Regex(@"[\d\w]{1,2048}");
            var Entities = new List<Entity>();
            var matches = regex.Matches(text);
            var words = matches.Select(x => x.Value);
            var infos = _morphAnalyzer.Parse(words).ToArray();
            for (var i = 0; i < infos.Count(); i++)
            {
                _wordCounter++;
                Entities.Add(new Entity(globalOffset + matches[i].Index, _wordCounter, infos[i]));
            }

            return Entities;
        }
    }
}