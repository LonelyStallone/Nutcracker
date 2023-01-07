using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Nutcracker.Morphology;

namespace Nutcracker.Morphology.Bindings
{

    public static class BindingCreator
    {
        private static string[] _avalibleGramms { get; }

        static BindingCreator()
        {
            _avalibleGramms = new [] { GrammConst.Adjf, GrammConst.Npro, GrammConst.Verb };
        }

        private static IEnumerable<Entity> GetSpeakerListEntities(List<LexicalUnitDescriptor> descriptors)
        {
            return descriptors.Where(x=>x.Owner.Offset>0)
                .GroupBy(g => g.Owner?.Lemma)
                .Select(g => g.First().Owner)
                .ToList();
        }


        public static void InitBindings(List<LexicalUnitDescriptor> descriptors)
        {
            var speakers = GetSpeakerListEntities(descriptors);
            foreach (var descriptor in descriptors)
            {
                foreach (var word in descriptor.Words)
                {
                    var grammKey = _avalibleGramms.Intersect(word.Info.BestTag.Grams).FirstOrDefault();
                    if(grammKey == null)
                        continue;
                    var bindingStrategy = BindingStrategyFactory.Create(grammKey, descriptor, speakers, descriptors);
                    var binding = bindingStrategy?.CreateBinding(descriptor.Owner, word, descriptors, descriptor);
                    word.SetBinding(binding);
                }
            }
        }
    }
}
