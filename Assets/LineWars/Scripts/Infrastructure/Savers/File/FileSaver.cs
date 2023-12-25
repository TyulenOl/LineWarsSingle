﻿using System.IO;

namespace LineWars.Model
{
    public class FileSaver<T>: ISaver<T>
    {
        private readonly ISinglePathGenerator<T> pathGenerator;
        private readonly IConverter<T, string> converter;

        public FileSaver(ISinglePathGenerator<T> pathGenerator, IConverter<T, string> converter)
        {
            this.pathGenerator = pathGenerator;
            this.converter = converter;
        }

        public void Save(T value, int id)
        {
            var path = pathGenerator.GeneratePath(id);
            var dir = Path.GetDirectoryName(path);
            var str = converter.Convert(value);
            if (dir != null)
                Directory.CreateDirectory(dir);
            File.WriteAllText(path, str);
        }
    }
}