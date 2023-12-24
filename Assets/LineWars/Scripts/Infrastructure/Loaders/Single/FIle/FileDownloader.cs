using System.IO;
using LineWars.Controllers;

namespace LineWars.Model
{
    public class FileDownloader<T>: IDownloader<T>
    {
        private readonly ISinglePathGenerator<T> pathGenerator;
        private readonly IConverter<string, T> converter;

        public FileDownloader(ISinglePathGenerator<T> pathGenerator, IConverter<string, T> converter)
        {
            this.pathGenerator = pathGenerator;
            this.converter = converter;
        }
        
        
        public T Load(int id)
        {
            var path = pathGenerator.GeneratePath(id);
            if (!File.Exists(path))
                return default(T);

            var str = File.ReadAllText(path);
            var value = converter.Convert(str);
            return value;
        }
    }
}