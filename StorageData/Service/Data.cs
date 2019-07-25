using System;
using System.Collections.Generic;
using System.Linq;
using StorageData.Model;
using StorageData.TransferData;
using StorageData.DBContext;
using System.IO;

namespace StorageData.Service
{

    interface IData
    {
        void AddData(JsonData data);
    }

    public class Data : IData
    {
        public Context dataBase;
        public Dictionary<string, Guid> parametersKey;

        public Data()
        {
            this.dataBase = new Context();
            this.parametersKey = GetParametersKey();
        }

        public void AddData(JsonData data)
        {
            if (data.Type == "Background")
            {
                DBAddBackground(data);
            }
            else if (data.Type == "Frame")
            {
                DBAddFrame(data);
            }
        }

        private void DBAddBackground(JsonData data)
        {
            var frame = AddFrame(data);
            dataBase.EventAttributes.Add(new EventAttribute { Frames = frame, Parameters = getObjectParametr("Type"), Value = data.Type.ToString() });
            dataBase.EventAttributes.Add(new EventAttribute { Frames = frame, Parameters = getObjectParametr("CameraId"), Value = data.CameraId.ToString() });
            dataBase.EventAttributes.Add(new EventAttribute { Frames = frame, Parameters = getObjectParametr("BackgroundId"), Value = data.BackgroundId.ToString() });
            dataBase.EventAttributes.Add(new EventAttribute { Frames = frame, Parameters = getObjectParametr("DateTime"), Value = data.DateTime.ToString() });
            dataBase.SaveChanges();
        }

        private void DBAddFrame(JsonData data)
        {
            var frame = AddFrame(data);
            dataBase.EventAttributes.Add(new EventAttribute { Frames = frame, Parameters = getObjectParametr("Type"), Value = data.Type.ToString() });
            dataBase.EventAttributes.Add(new EventAttribute { Frames = frame, Parameters = getObjectParametr("CameraId"), Value = data.CameraId.ToString() });
            dataBase.EventAttributes.Add(new EventAttribute { Frames = frame, Parameters = getObjectParametr("BackgroundId"), Value = data.BackgroundId.ToString() });
            dataBase.EventAttributes.Add(new EventAttribute { Frames = frame, Parameters = getObjectParametr("DateTime"), Value = data.DateTime.ToString() });
            dataBase.EventAttributes.Add(new EventAttribute { Frames = frame, Parameters = getObjectParametr("Coordinate_X"), Value = data.Coordinate_X.ToString() });
            dataBase.EventAttributes.Add(new EventAttribute { Frames = frame, Parameters = getObjectParametr("Coordinate_Y"), Value = data.Coordinate_Y.ToString() });
            dataBase.EventAttributes.Add(new EventAttribute { Frames = frame, Parameters = getObjectParametr("Width"), Value = data.Width.ToString() });
            dataBase.EventAttributes.Add(new EventAttribute { Frames = frame, Parameters = getObjectParametr("Length"), Value = data.Length.ToString() });
            dataBase.SaveChanges();
        }

        private Frame AddFrame(JsonData data)
        {
            var frame = new Frame { Id = Guid.NewGuid(), EventId = data.EventId, Timestamp = data.DateTime };
            dataBase.Frames.Add(frame);
            dataBase.SaveChanges();
            var configuration = new Configuration().GetConfiguration().PathForStoreImage;
            FileStorage(configuration, frame.Id.ToString(), data );
            return frame;
        }

        private Parameter getObjectParametr(string parametr)
        {
            return dataBase.Parameters.First(item => item.Id == parametersKey[parametr]);
        }

        private void FileStorage(string path, string name, JsonData data)
        {
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
                using (var imageFile = new FileStream($"{path}\\{data.EventId}\\{data.Type}\\{name}.jpg", FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
            }
        }

        private Dictionary<string, Guid> GetParametersKey()
        {
            var parametersKey = new Dictionary<string, Guid>();
            parametersKey.Add("CameraId", dataBase.Parameters.Where(parametr => parametr.Name == "CameraId").Select(item => item.Id).First());
            parametersKey.Add("BackgroundId", dataBase.Parameters.Where(parametr => parametr.Name == "BackgroundId").Select(item => item.Id).First());
            parametersKey.Add("Type", dataBase.Parameters.Where(parametr => parametr.Name == "Type").Select(item => item.Id).First());
            parametersKey.Add("DateTime", dataBase.Parameters.Where(parametr => parametr.Name == "DateTime").Select(item => item.Id).First());
            parametersKey.Add("Coordinate_X", dataBase.Parameters.Where(parametr => parametr.Name == "Coordinate_X").Select(item => item.Id).First());
            parametersKey.Add("Coordinate_Y", dataBase.Parameters.Where(parametr => parametr.Name == "Coordinate_Y").Select(item => item.Id).First());
            parametersKey.Add("Width", dataBase.Parameters.Where(parametr => parametr.Name == "Width").Select(item => item.Id).First());
            parametersKey.Add("Length", dataBase.Parameters.Where(parametr => parametr.Name == "Length").Select(item => item.Id).First());
            return parametersKey;
        }
    }
}
