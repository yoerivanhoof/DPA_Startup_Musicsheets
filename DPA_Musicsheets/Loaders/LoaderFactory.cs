using System;
using System.Collections.Generic;
using System.Linq;

namespace DPA_Musicsheets.Loaders
{
    public class LoaderFactory
    {
        private readonly Dictionary<string, FileLoader> _loaderTypes;

        public LoaderFactory()
        {
            _loaderTypes = new Dictionary<string, FileLoader>();
            var loaders = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(n => typeof(FileLoader).IsAssignableFrom(n) && !n.IsAbstract).ToList();

            foreach (var loader in loaders)
            {
                var handler = (FileLoader)Activator.CreateInstance(loader);
                _loaderTypes.Add(handler.FileExtension, handler);
            }
        }

        public FileLoader GetLoader(string extension)
        {
            return _loaderTypes[extension];
        }
    }
}