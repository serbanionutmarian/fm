using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class DeviceStorage
    {
        #region singleton

        private static DeviceStorage _instance;
        public static DeviceStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DeviceStorage();
                }
                return _instance;
            }
        }
        private DeviceStorage()
        {
        }

        #endregion

        private string GetFileFullPath(string fileName)
        {
            var persistentDataPath = Application.persistentDataPath;
            return string.Format("{0}/{1}", persistentDataPath, fileName);
        }
        // T must be serializable
        public T Load<T>(string fileName)
        {
            var fileFullPath = GetFileFullPath(fileName);
            if (File.Exists(fileFullPath))
            {
                BinaryFormatter binnaryFormatter = new BinaryFormatter();
                using (var fileSteam = File.Open(fileFullPath, FileMode.Open))
                {
                    return (T)binnaryFormatter.Deserialize(fileSteam);
                }
            }
            return default(T);
        }
        // T must be serializable
        public void Save<T>(string fileName, T value)
        {
            var fileFullPath = GetFileFullPath(fileName);
            BinaryFormatter binnaryFormatter = new BinaryFormatter();
            using (var fileSteam = File.Create(fileFullPath))
            {
                binnaryFormatter.Serialize(fileSteam, value);
            }
        }
    }
}
