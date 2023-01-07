using Nutcracker.Morphology;
using Nutcracker.Morphology.ExtractStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using Nutcracker.Morphology.Bindings;
using Nutcracker.Morphology.Helpers;

namespace Nutcracker.Context
{
    public class DataContext
    {
        private List<LexicalUnitDescriptor> _descriptors;

        public DataContext(string text)
        {
            Text = text;
            _descriptors = InitDirectSpeechAndOwners(InitScenario(CreateDefaultUnit(text))).ToList();
            BindingCreator.InitBindings(_descriptors);
        }

        private LexicalUnitDescriptor CreateDefaultUnit(string text)
        {
            var data = text.Replace("\r", "").Replace("\n", "");
            return new LexicalUnitDescriptor
            {
                Data = data,
                Offset = 0,
                UnitType = EnumLexicalUnitType.FullText
            };
        }

        IEnumerable<LexicalUnitDescriptor> InitScenario(LexicalUnitDescriptor descriptor)
        {
            IExtractStrategy sentenceStrategy = new SentenceStrategy();
            var sentenceUnits = sentenceStrategy.Extract(descriptor);

            IExtractStrategy directSpeechStrategy = new DirectSpeechStrategy();
            var directSpeechUnits = new List<LexicalUnitDescriptor>();
            foreach (var sentenceUnit in sentenceUnits)
                directSpeechUnits.AddRange(directSpeechStrategy.Extract(sentenceUnit));

            IExtractStrategy applyMorphologyStrategy = new ApplyMorphologyStrategy();
            var applyMorphologyUnits = new List<LexicalUnitDescriptor>();
            foreach (var directSpeechUnit in directSpeechUnits)
                applyMorphologyUnits.AddRange(applyMorphologyStrategy.Extract(directSpeechUnit));
            return applyMorphologyUnits;
        }

        IEnumerable<LexicalUnitDescriptor> InitDirectSpeechAndOwners(IEnumerable<LexicalUnitDescriptor> applyMorphologyUnits)
        {
            ISentenceStrategy sourceSpeechStrategy = new SourceSpeechStrategy();
            var sentences = SentenceHelper.GetSetntences(applyMorphologyUnits);


            var sourceSpeechUnits = new List<LexicalUnitDescriptor>();
            foreach (var sentence in sentences)
                sourceSpeechUnits.AddRange(sourceSpeechStrategy.Extract(sentence));


            IExtractStrategy personSourceSpeechStrategy = new PersonSourceSpeechStrategy();
            var personSourceSpeechUnits = new List<LexicalUnitDescriptor>();
            foreach (var sourceSpeech in sourceSpeechUnits)
                personSourceSpeechUnits.AddRange(personSourceSpeechStrategy.Extract(sourceSpeech));

            IContextStrategy setOwnerStrategy = new SetOwnerStrategy();
            return setOwnerStrategy.Extract(personSourceSpeechUnits);
        }

        public IEnumerable<LexicalUnitDescriptor> Descriptors => _descriptors;

        public string Text { get; set; }
    }
}