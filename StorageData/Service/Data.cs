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
            if (data.Type == "Background")
            {
                DBAddBackground(data, data.EventId != Guid.Empty);
            }
            else if (data.Type == "Frame")
            {
                DBAddFrame(data);
            }
        }

        private void DBAddBackground(JsonData data, bool eventAvailability)
        {
            if (eventAvailability)
            {
                var frame = AddFrame(data);
                dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = GetObjectParameter("Type"), Value = data.Type.ToString() });
                dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = GetObjectParameter("CameraId"), Value = data.CameraId.ToString() });
                dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = GetObjectParameter("BackgroundId"), Value = data.BackgroundId.ToString() });
                dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = GetObjectParameter("Width"), Value = data.Width.ToString() });
                dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = GetObjectParameter("Height"), Value = data.Height.ToString() });
                dbContext.SaveChanges();
            }
            else
            {
                AddFrame(data);
            }
        }

        private void DBAddFrame(JsonData data)
        {
            var frame = AddFrame(data);
            dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = GetObjectParameter("Type"), Value = data.Type.ToString() });
            dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = GetObjectParameter("CameraId"), Value = data.CameraId.ToString() });
            dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = GetObjectParameter("BackgroundId"), Value = data.BackgroundId.ToString() });
            dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = GetObjectParameter("Coordinate_X"), Value = data.Coordinate_X.ToString() });
            dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = GetObjectParameter("Coordinate_Y"), Value = data.Coordinate_Y.ToString() });
            dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = GetObjectParameter("Width"), Value = data.Width.ToString() });
            dbContext.FrameParameters.Add(new FrameParameter { Frames = frame, Parameters = GetObjectParameter("Height"), Value = data.Height.ToString() });
            dbContext.SaveChanges();
        }

        private Frame AddFrame(JsonData data)
        {
            var frame = new Frame { Id = Guid.NewGuid(), EventId = data.EventId, Timestamp = data.DateTime };
            dbContext.Frames.Add(frame);
            dbContext.SaveChanges();
            FileStorage(Configuration.GetConfiguration().PathForStoreImage, frame.Id.ToString(), data);
            return frame;
        }

        private Parameter GetObjectParameter(string parameter)
        {
            return dbContext.Parameters.First(item => item.Id == parametersKey[parameter]);
        }

        private void FileStorage(string path, string name, JsonData data)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new Exception("There is no image storage path in the configuration file.");
            }

            var directory = Path.Combine(new []{path, data.EventId.ToString(), data.Type});

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var bytes = Convert.FromBase64String(data.Data);
            using (var imageFile = new FileStream(Path.Combine(directory, $"{name}.jpg"), FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            {
                imageFile.Write(bytes, 0, bytes.Length);
            }
        }

        private Dictionary<string, Guid> GetParametersKey()
        {
            var parametersKey = new Dictionary<string, Guid>();
            parametersKey.Add("CameraId", dbContext.Parameters.Where(parameter => parameter.Name == "CameraId").Select(item => item.Id).First());
            parametersKey.Add("BackgroundId", dbContext.Parameters.Where(parameter => parameter.Name == "BackgroundId").Select(item => item.Id).First());
            parametersKey.Add("Type", dbContext.Parameters.Where(parameter => parameter.Name == "Type").Select(item => item.Id).First());
            parametersKey.Add("Coordinate_X", dbContext.Parameters.Where(parameter => parameter.Name == "Coordinate_X").Select(item => item.Id).First());
            parametersKey.Add("Coordinate_Y", dbContext.Parameters.Where(parameter => parameter.Name == "Coordinate_Y").Select(item => item.Id).First());
            parametersKey.Add("Width", dbContext.Parameters.Where(parameter => parameter.Name == "Width").Select(item => item.Id).First());
            parametersKey.Add("Height", dbContext.Parameters.Where(parameter => parameter.Name == "Height").Select(item => item.Id).First());
            return parametersKey;
        }
    }
}
