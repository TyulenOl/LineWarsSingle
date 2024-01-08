using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LineWars.Model
{
    public class SimpleFilePathGenerator<T>: 
        ISinglePathGenerator<T>,
        IAllPathGenerator<T>
    {
        private readonly string fileExtension;

        public SimpleFilePathGenerator(string fileExtension)
        {
            this.fileExtension = fileExtension.Replace(".", "");
        }
        
        public string GeneratePath(int id)
        {
            return $"{GetDirectoryPath()}/{typeof(T).Name}_{id}.{fileExtension}";
        }

        public IEnumerable<string> GeneratePaths()
        {
            var dir = GetDirectoryPath();
            if (Directory.Exists(dir))
                return Directory.EnumerateFiles(dir, $"*.{fileExtension}");
            return Enumerable.Empty<string>();
        }

        private string GetDirectoryPath()
        {
            return $"{PathsHelper.RootPathForSaves}/{typeof(T).Name}";
        }
    }
}