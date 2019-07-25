using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using StorageData.DBContext;
using StorageData.TransferData;

namespace StorageData.Service
{
    interface IStorage
    {
        void AddData(JsonData data);
    }

    public class Storage : IStorage
    {
        public Context dataBase;
        public Data dbData;

        public Storage()
        {
            this.dataBase = new Context();
            this.dbData = new Data();
        }

        public void AddData(JsonData data)
        {
            using (var transaction = dataBase.Database.BeginTransaction())
            {
                try
                {
                    dbData.AddData(data);
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    transaction.Rollback();
                }
            }
        }
    }
}
