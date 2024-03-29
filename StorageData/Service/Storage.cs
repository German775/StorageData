﻿using System;
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
        public Context dbContext;
        public Data dbData;

        public Storage()
        {
            this.dbContext = new Context();
            this.dbData = new Data();
        }

        public void AddData(JsonData data)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                dbData.AddData(data);
                transaction.Commit();
            }
        }
    }
}
