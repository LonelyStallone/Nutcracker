using System;
using System.IO;
using System.Linq;

namespace Nutcracker
{
    internal class DirectoryRepository : INutcrackerRepository
    {
        public DirectoryRepository(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new Exception($"Directory not exists {directoryPath}");
            _directory = new DirectoryInfo(directoryPath);
        }

        private DirectoryInfo _directory { get; }

        public bool TryGetValue(string key, out string data)
        {
            data = null;
            var files = _directory.GetFiles($"{key}", SearchOption.TopDirectoryOnly);
            if (files.Length == 0)
                return false;
            data = File.ReadAllText(files.First().FullName);
            return true;
        }

        public void Set(string key, string data)
        {
            var path = Path.Combine(_directory.FullName, key);
            File.WriteAllText(path, data);
        }
    }
}