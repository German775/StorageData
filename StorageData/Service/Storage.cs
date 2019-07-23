using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageData.Model;
using System.IO;
using StorageData.DBContext;
using Microsoft.AspNetCore.Http;
using StorageData.TransferData;

namespace StorageData.Service
{
    interface IStorage
    {
        void AddData(string path, JsonData data);
    }
    public class Storage : IStorage
    {
        public Context dataBase;

        public Storage()
        {
            this.dataBase = new Context();
        }

        public void AddData(string path, JsonData data)
        {
            using (var transaction = dataBase.Database.BeginTransaction())
            {
                try
                {
                    DBDataStorage(data);
                    FileStorage(path, data);
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    transaction.Rollback();
                }
            }
        }

        private void DBDataStorage(JsonData data)
        {
            if (dataBase.Parameters.Select(item => item.Id).Count() == 0)
            {
                dataBase.Parameters.Add(new Parameter { Name = "Type" });
                dataBase.Parameters.Add(new Parameter { Name = "CameraId" });
                dataBase.Parameters.Add(new Parameter { Name = "Coordinate_X" });
                dataBase.Parameters.Add(new Parameter { Name = "Coordinate_Y" });
                dataBase.Parameters.Add(new Parameter { Name = "Coordinate_Z" });
                dataBase.Parameters.Add(new Parameter { Name = "Coordinate_D" });
                dataBase.Parameters.Add(new Parameter { Name = "BackgroundId" });
                dataBase.SaveChanges();
            }
            var Type = dataBase.Parameters.Where(item => item.Name == "CameraId").Select(item => item);

            dataBase.EventAttributes.Add(new EventAttribute {  });
            //var str = dataBase.Parameters.Select(item => item.Name == "CameraId");
            Console.WriteLine();
            //if (dataBase.EventAttributes.Select(item => item.))){ }

            //dataBase.Events.Add(new Event { DateTime_Start = data.DateTime });

            //Заглушка
            //dataBase.Events.Add(new Event { DateTime_Start = data.DateTime, DateTime_End = data.DateTime });
            //
            //dataBase.Frames.Add(new Frame { EventId = dataBase.Events.Select(item => item.) });
            dataBase.SaveChanges();
        }

        private void FileStorage(string path, JsonData data)
        {
            /*
            if (path == null || path == "")
            {
                throw new Exception("There is no image storage path in the configuration file.");
            }
            else
            {
                if (!Directory.Exists($"{path}\\{data.EventId}\\{data.Type}"))
                {
                    Directory.CreateDirectory($"{path}\\{data.EventId}\\{data.Type}");
                }

                var bytes = Convert.FromBase64String(data.Data);
                using (var imageFile = new FileStream($"{path}\\{data.EventId}\\{data.Type}\\{data.FileId}.jpg", FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
            }
            */
        }
    }
}
