namespace Nutcracker.Morphology
{
    public static class GrammConst
    {
        public const string Verb = "гл";
        public const string Grnd = "деепр";
        public const string Infn = "инф_гл";
        public const string Adjs = "кр_прил";
        public const string Intj = "межд";
        public const string Npro = "мест";
        public const string Prep = "предл";
        public const string Adjf = "прил";
        public const string Prtf = "прич";
        public const string Conj = "союз";
        public const string Noun = "сущ";
        public const string Prcl = "част";

        public const string Person1 = "1л";
        public const string Person2 = "2л";
        public const string Person3 = "3л";

        public const string Nomn = "им";
        public const string Gent = "рд";
        public const string Datv = "дт";
        public const string Accs = "вн";
        public const string Ablt = "тв";
        public const string Loct = "пр";

        static GrammConst()
        {
            GenderTags = new[] {"муж", "жен", "ср", "общ"};
        }

        public static string[] GenderTags { get; }
    }
}