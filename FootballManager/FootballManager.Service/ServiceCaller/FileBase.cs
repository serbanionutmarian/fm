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
                    _filePath = string.Format("{0}/{1}", Application.persistentDataPath, FileName);
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
        public TModel Load()
        {
            if (!File.Exists(FilePath))
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
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}
