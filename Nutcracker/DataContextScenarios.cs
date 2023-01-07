using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Nutcracker.Context;
using Nutcracker.Morphology;
using Nutcracker.Morphology.Bindings;

namespace Nutcracker
{
    class DataContextScenarios
    {
        private DataContext _dataContext;

        public DataContextScenarios(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public void Run()
        {
            Scenario0();
            Scenario01();
            Scenario1();
            Scenario2();
            Scenario3();
            Scenario4();
            Scenario5();
        }


        /// <summary>
        /// Вывод всех спикеров
        /// </summary>
        public void Scenario0()
        {
            var speakers = _dataContext.Descriptors.Where(x => x.Owner.Offset > 0)
                .GroupBy(g => g.Owner?.Lemma)
                .Select(g => g.First().Owner)
                .ToList();
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("========== =0= Весь текст: ===========");
            Console.WriteLine(_dataContext.Text);

        }


        /// <summary>
        /// Анализ прямой речи и выделение источников
        /// </summary>
        public void Scenario01()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("========== =1= Весь текст про ролям ==");
            int counter = 0;
            foreach (var item in _dataContext.Descriptors)
            {
                Console.WriteLine($"{++counter} {item.Owner.Lemma}: {string.Join(' ', item.Words.Select(x=>x.Info.Text))}");
            }

        }



        /// <summary>
        /// Вывод всех спикеров
        /// </summary>
        public void Scenario1()
        {
            var speakers = _dataContext.Descriptors.Where(x => x.Owner.Offset > 0)
                .GroupBy(g => g.Owner?.Lemma)
                .Select(g => g.First().Owner)
                .ToList();
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("========== =2= Спикеры: ==============");
            int counter = 0;
            foreach (var speaker in speakers)
            {
                Console.WriteLine($"\t{++counter}) {speaker.Info.Text}");
            }
            
        }

        /// <summary>
        /// Мари говорила
        /// </summary>
        public void Scenario2()
        {
            var allMariPhrases = _dataContext.Descriptors.Where(x => x.Owner.Lemma=="мари").ToList();
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("========== =3= Мари говорила: ========");
            int counter = 0;
            foreach (var phrase in allMariPhrases)
            {
                Console.WriteLine("\t----------");
                Console.Write($"\t{++counter}) ");
                foreach (var word in phrase.Words)
                {
                    Console.Write(word.Info.Text+" ");
                }
            }
        }

        /// <summary>
        /// Действия Фрица
        /// </summary>
        public void Scenario3()
        {
            var words = _dataContext.Descriptors.SelectMany(
                x=>x.Words.Where(
                    y=> y.Binding?.Main.Info.BestTag.Grams.Any(x => x == GrammConst.Verb) == true)
                );
            words = words.Where(x => x.Binding.Target?.Lemma == "фриц");
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("========== =4= Действия Фрица: =======");
            int counter = 0;
            foreach (var word in words)
            {
                Console.WriteLine("\t----------");
                Console.WriteLine($"\t{++counter}){word.Binding?.Source?.Info.Text}");
                Console.WriteLine($"\t{word.Binding?.Main.Info.Text}");
                Console.WriteLine($"\t{word.Binding?.Target?.Info.Text}");
            }

        }



        /// <summary>
        /// Алиасы/Метоимения
        /// </summary>
        public void Scenario4()
        {
            var words = _dataContext.Descriptors.SelectMany(
                x => x.Words.Where(
                    y => y.Binding?.BindingType == EnumBindingType.Alias)
            );
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("========== =5= Алиасы/Метоимения =====");
            Console.WriteLine("источник_сопоставления: обьект1 <== причина_сопоставления ==> обеькт2");
            int counter = 0;
            foreach (var word in words)
            {
                Console.WriteLine("\t----------");
                Console.WriteLine($"\t{++counter}){word.Binding?.Owner?.Info.Text} утрвеждает, что:" );
                Console.WriteLine($"\t{word.Binding?.Source?.Info.Text} <== {word.Binding?.Main.Info.Text} ==> {word.Binding?.Target?.Info.Text}");
            }

        }


        /// <summary>
        /// Все что сделал Фриц
        /// </summary>
        public void Scenario5()
        {
            var words = _dataContext.Descriptors.SelectMany(
                x => x.Words.Where(
                    y => y.Binding?.Main.Info.BestTag.Grams.Any(x => x == GrammConst.Adjf) == true)
            );

            words = words.Where(x => x.Binding?.Main !=null && x.Binding?.Target!= null);
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("========== =6= Свойства: =============");
            int counter = 0;
            foreach (var word in words)
            {
                Console.WriteLine("\t----------");
                Console.WriteLine($"\t{++counter}){word.Binding?.Main.Info.Text}");
                Console.WriteLine($"\t{word.Binding?.Target?.Info.Text}");
            }

        }
    }
}
