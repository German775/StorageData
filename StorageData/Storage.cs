using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageData.Model;
using System.IO;
using StorageData.DBContext;

namespace StorageData
{
    interface IStorage
    {
        void AddData(string path, int fileId, string type, DateTime timeStamp, int eventId, byte[] data);
    }

    public class Storage : IStorage
    {
        public Context dataBase; 

        public Storage()
        {
            this.dataBase = new Context();
        }

        public void AddData(string path, int fileId, string type, DateTime timeStamp, int eventId, byte[] data)
        {
            using (var transaction = dataBase.Database.BeginTransaction())
            {
                try
                {
                    DataStore(fileId, type, timeStamp, eventId);
                    FileStorage(path, fileId, eventId, data, type);
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    transaction.Rollback();
                }
            }
        }

        private void DataStore(int fileId, string type, DateTime timeStamp, int eventId)
        {
            dataBase.Datas.Add(new Data { FileId = fileId, Type = type, TimeStamp = timeStamp, EventId = eventId });
            dataBase.SaveChanges();
        }

        private void FileStorage(string path, int fileId, int eventId, byte[] data, string type)
        {
            if (path == null || path == "")
            {
                throw new Exception("There is no image storage path in the configuration file.");
            }
            else
            {
                if (!Directory.Exists($"{path}\\{eventId}\\{type}"))
                {
                    Directory.CreateDirectory($"{path}\\{eventId}\\{type}");
                }
                File.Create($"{path}\\{eventId}\\{type}\\{fileId}.txt");
            }
        }
    }
}
