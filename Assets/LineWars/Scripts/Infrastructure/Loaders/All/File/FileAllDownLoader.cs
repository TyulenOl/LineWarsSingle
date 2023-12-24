using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LineWars.Model
{
    public class FileAllDownLoader<TValue> : IAllDownloader<TValue>
    {
        private readonly IAllPathGenerator<TValue> pathGenerator;
        private readonly IConverter<string, TValue> converter;

        public FileAllDownLoader(
            IAllPathGenerator<TValue> pathGenerator,
            IConverter<string, TValue> converter)
        {
            this.pathGenerator = pathGenerator;
            this.converter = converter;
        }

        public IEnumerable<TValue> LoadAll()
        {
            return pathGenerator.GeneratePaths()
                .Select(File.ReadAllText)
                .Select(converter.Convert);
        }
    }
}