using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace ServiceCaller
{
    abstract class FileBase<TModel>
    {
        abstract protected string FileName { get; }

        private string _filePath;
        private string FilePath
        {
            get
            {
                if (_filePath == null)
                {
                    string persistentDataPath = null;

                    //#if TestApp
                    //                    persistentDataPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                    //#else
                    persistentDataPath = Application.persistentDataPath;
                    //#endif

                    _filePath = Path.Combine(persistentDataPath, FileName);
                }
                return _filePath;
            }
        }
        public void Save(TModel data)
        {
            var binnaryFormatter = new BinaryFormatter();

            using (var fileStream = File.Create(FilePath))
            {
                binnaryFormatter.Serialize(fileStream, data);
                fileStream.Close();
            }
        }
        public bool Exists()
        {
            return File.Exists(FilePath);
        }
        public TModel Load()
        {
            if (!Exists())
            {
                return default(TModel);
            }
            TModel result = default(TModel);
            var binnaryFormatter = new BinaryFormatter();
            using (var fileStream = File.Open(FilePath, FileMode.Open))
            {
                result = (TModel)binnaryFormatter.Deserialize(fileStream);
                fileStream.Close();
            }
            return result;
        }
        public void Delete()
        {
            if (Exists())
            {
                File.Delete(FilePath);
            }
        }
    }
}
