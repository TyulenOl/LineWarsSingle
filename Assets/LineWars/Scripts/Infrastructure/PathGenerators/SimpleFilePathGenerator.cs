using System.Collections.Generic;
using System.IO;

namespace LineWars.Model
{
    public class SimpleFilePathGenerator<T>: 
        ISinglePathGenerator<T>,
        IAllPathGenerator<T>
    {
        private readonly string fileExtension;

        public SimpleFilePathGenerator(string fileExtensionWithoutDot)
        {
            fileExtension = fileExtensionWithoutDot;
        }
        
        public string GeneratePath(int id)
        {
            return $"{GetDirectoryPath()}/{nameof(T)}_{id}.{fileExtension}";
        }

        public IEnumerable<string> GeneratePaths()
        {
            return Directory.EnumerateFiles(GetDirectoryPath(), $"*.{fileExtension}");
        }

        private string GetDirectoryPath()
        {
            return $"{PathsHelper.RootPathForSaves}/{nameof(T)}";
        }
    }
}