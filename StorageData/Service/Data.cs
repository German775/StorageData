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
        public Context dbContext;
        public Dictionary<string, Guid> parametersKey;

        public Data()
        {
            this.dbContext = new Context();
            this.parametersKey = GetParametersKey();
        }

        public void AddData(JsonData data)
        {
            if (data.Type == "Background" && data.EventId != null)
            {
                DBAddBackground(data, true);
            }
            else if (data.Type == "Frame")
            {
                DBAddFrame(data);
            }
            else if (data.Type == "Background" && data.EventId == null)
            {
                DBAddBackground(data, false);
            }
        }

        private void DBAddBackground(JsonData data, bool eventAvailability)
        {
            if (eventAvailability == true)
            {
                var frame = AddFrame(data);
                dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = getObjectParametr("Type"), Value = data.Type.ToString() });
                dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = getObjectParametr("CameraId"), Value = data.CameraId.ToString() });
                dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = getObjectParametr("BackgroundId"), Value = data.BackgroundId.ToString() });
                dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = getObjectParametr("Width"), Value = data.Width.ToString() });
                dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = getObjectParametr("Height"), Value = data.Height.ToString() });
                dbContext.SaveChanges();
            }
            else
            {
                var frame = AddFrame(data);
            }
        }

        private void DBAddFrame(JsonData data)
        {
            var frame = AddFrame(data);
            dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = getObjectParametr("Type"), Value = data.Type.ToString() });
            dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = getObjectParametr("CameraId"), Value = data.CameraId.ToString() });
            dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = getObjectParametr("BackgroundId"), Value = data.BackgroundId.ToString() });
            dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = getObjectParametr("Coordinate_X"), Value = data.Coordinate_X.ToString() });
            dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = getObjectParametr("Coordinate_Y"), Value = data.Coordinate_Y.ToString() });
            dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = getObjectParametr("Width"), Value = data.Width.ToString() });
            dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = getObjectParametr("Height"), Value = data.Height.ToString() });
            dbContext.SaveChanges();
        }

        private Frame AddFrame(JsonData data)
        {
            var frame = new Frame { Id = Guid.NewGuid(), EventId = data.EventId, Timestamp = data.DateTime };
            dbContext.Frames.Add(frame);
            dbContext.SaveChanges();
            var configuration = new Configuration().GetConfiguration().PathForStoreImage;
            FileStorage(configuration, frame.Id.ToString(), data );
            return frame;
        }

        private Parameter getObjectParametr(string parametr)
        {
            return dbContext.Parameters.First(item => item.Id == parametersKey[parametr]);
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
            parametersKey.Add("CameraId", dbContext.Parameters.Where(parametr => parametr.Name == "CameraId").Select(item => item.Id).First());
            parametersKey.Add("BackgroundId", dbContext.Parameters.Where(parametr => parametr.Name == "BackgroundId").Select(item => item.Id).First());
            parametersKey.Add("Type", dbContext.Parameters.Where(parametr => parametr.Name == "Type").Select(item => item.Id).First());
            parametersKey.Add("Coordinate_X", dbContext.Parameters.Where(parametr => parametr.Name == "Coordinate_X").Select(item => item.Id).First());
            parametersKey.Add("Coordinate_Y", dbContext.Parameters.Where(parametr => parametr.Name == "Coordinate_Y").Select(item => item.Id).First());
            parametersKey.Add("Width", dbContext.Parameters.Where(parametr => parametr.Name == "Width").Select(item => item.Id).First());
            parametersKey.Add("Height", dbContext.Parameters.Where(parametr => parametr.Name == "Height").Select(item => item.Id).First());
            return parametersKey;
        }
    }
}
