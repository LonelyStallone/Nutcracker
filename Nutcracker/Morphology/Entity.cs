using System;
using System.Linq;
using DeepMorphy.Model;
using Nutcracker.Morphology.Bindings;

namespace Nutcracker.Morphology
{
    public class Entity
    {
        private const int genderCoefficient = 7;
        private Binding _binding;

        private Entity()
        {
        }

        public Entity(int offset, int wordId, MorphInfo info)
        {
            WordId = wordId;
            Offset = offset;
            Info = info;
            Lemma = info.BestTag.HasLemma ? info.BestTag.Lemma : info.Text;
        }



        public int Offset { get; private set; }

        public MorphInfo Info { get; private set; }

        public string Lemma { get; private set; }

        public int WordId { get; private set; }

        public Binding Binding => _binding;

        public static Entity GetStoryteller()
        {
            var storyteller = new Entity();
            storyteller.WordId = -1;
            storyteller.Offset = -1;
            storyteller.Info = null;
            storyteller.Lemma = "рассказчик";
            return storyteller;
        }

        public void SetBinding(Binding binding)
        {
            _binding = binding;
        }

        public int WordDistance(Entity meeting)
        {
            return Math.Abs(meeting.WordId - WordId);
        }

        public int TagDistance(Entity meeting)
        {
            return meeting.Info.BestTag.Grams.Intersect(Info.BestTag.Grams).Count();
        }

        public int Distance(Entity meeting)
        {
            return WordDistance(meeting) - (IsSameGndr(meeting) ? genderCoefficient : 0);
        }

        public bool IsSameGndr(Entity meeting)
        {
            var existGenderTag = meeting.Info.BestTag.Grams.Intersect(GrammConst.GenderTags).FirstOrDefault();
            if (existGenderTag == null)
                return false;
            return Info.BestTag.Grams.Any(x => x == existGenderTag);
        }
    }
}