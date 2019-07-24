using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageData.Model;
using StorageData.TransferData;
using StorageData.DBContext;

namespace StorageData.Service
{
    public class DBData
    {
        public Context dataBase;
        public Dictionary<string, Guid> parametersKey;

        public DBData()
        {
            this.dataBase = new Context();
            this.parametersKey = GetParametersKey();
        }

        public void AddData(JsonData data)
        {
            if (dataBase.Parameters.Select(parametr => parametr.Id).Count() == 0)
            {
                dataBase.Parameters.Add(new Parameter { Name = "Type" });
                dataBase.Parameters.Add(new Parameter { Name = "CameraId" });
                dataBase.Parameters.Add(new Parameter { Name = "Coordinate_X" });
                dataBase.Parameters.Add(new Parameter { Name = "Coordinate_Y" });
                dataBase.Parameters.Add(new Parameter { Name = "BackgroundId" });
                dataBase.Parameters.Add(new Parameter { Name = "DateTime" });
                dataBase.SaveChanges();
            }

            if (data.Type == "Background")
            {
                AddBackground(data);
            }
            else if (data.Type == "Frame")
            {
            }
        }

        public void AddBackground(JsonData data)
        {
            var frame = new Frame { EventId = data.EventId, Timestamp = data.DateTime };
            dataBase.Frames.Add(frame);
            dataBase.SaveChanges();
            dataBase.EventAttributes.Add(new EventAttribute { Frames = frame, Parameters = getObjectParametr("CameraId"), Value = data.CameraId.ToString() });
            dataBase.EventAttributes.Add(new EventAttribute { Frames = frame, Parameters = getObjectParametr("BackgroundId"), Value = data.BackgroundId.ToString() });
            dataBase.EventAttributes.Add(new EventAttribute { Frames = frame, Parameters = getObjectParametr("DateTime"), Value = data.DateTime.ToString() });
            dataBase.SaveChanges();
        }

        private Parameter getObjectParametr(string parametr)
        {
            //var name = dataBase.Parameters.Where(item => item.Id == parametersKey[parametr]).Select(item => item.Name).FirstOrDefault();
            return dataBase.Parameters.First(item => item.Id == parametersKey[parametr]);
        }

        private Dictionary<string, Guid> GetParametersKey()
        {
            var parametersKey = new Dictionary<string, Guid>();
            var cameraId = dataBase.Parameters.Where(parametr => parametr.Name == "CameraId").Select(item => item.Id);
            var backgroundId = dataBase.Parameters.Where(parametr => parametr.Name == "BackgroundId").Select(item => item.Id);
            var typeId = dataBase.Parameters.Where(parametr => parametr.Name == "Type").Select(item => item.Id);
            var dateTime = dataBase.Parameters.Where(parametr => parametr.Name == "DateTime").Select(item => item.Id);
            var coordinateX = dataBase.Parameters.Where(parametr => parametr.Name == "Coordinate_X").Select(item => item.Id);
            var coordinateY = dataBase.Parameters.Where(parametr => parametr.Name == "Coordinate_Y").Select(item => item.Id);

            parametersKey.Add("CameraId", cameraId.FirstOrDefault());
            parametersKey.Add("BackgroundId", backgroundId.FirstOrDefault());
            parametersKey.Add("Type", typeId.FirstOrDefault());
            parametersKey.Add("DateTime", dateTime.FirstOrDefault());
            parametersKey.Add("Coordinate_X", coordinateX.FirstOrDefault());
            parametersKey.Add("Coordinate_Y", coordinateY.FirstOrDefault());
            return parametersKey;
        }
    }
}
