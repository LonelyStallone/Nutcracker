using System;
using System.Collections.Generic;
using Nutcracker.Context;
using Nutcracker.Morphology.ExtractStrategies;
using Nutcracker.Morphology.Helpers;

namespace Nutcracker
{
    internal class Program
    {
        private static string Clear(string data)
        {
            return data.Replace("\r", "").Replace("\n", "");
        }

        private static void Main(string[] args)
        {
            INutcrackerRepository nutcrackerRepository = new DirectoryRepository("Data");
            try
            {
                if (nutcrackerRepository.TryGetValue("NUTCRACKER", out var data))
                {
                    DataContext context = new DataContext(data);
                    DataContextScenarios contextScenarios = new DataContextScenarios(context);
                    contextScenarios.Run();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Что то пошло так, результ был загружен из хранилища:");
                if (nutcrackerRepository.TryGetValue("RESULT", out var repositoryResult))
                {
                    Console.WriteLine(repositoryResult);
                }

            }
        }
    }
}