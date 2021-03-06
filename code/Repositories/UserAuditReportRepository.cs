﻿using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using Sitecore.Diagnostics;
using UserAuditReport.Models;

namespace UserAuditReport.Repositories
{
    public class UserAuditReportRepository : IUserAuditReportRepository
    {
        private readonly string _collectionName = "useraudit";

        private readonly string _connectionStringSetting = "UserAuditReport.ConnectionStringName";

        private readonly MongoCollection _userAuditCollection;

        public UserAuditReportRepository()
        {
            var connectionStringName = Sitecore.Configuration.Settings.GetSetting(_connectionStringSetting);
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            _userAuditCollection = GetCollection(connectionString, _collectionName);
        }

        public bool Add(UserChange changesInfo)
        {
            return _userAuditCollection.Insert(changesInfo).Ok;
        }

        public bool Update(UserChange changesInfo)
        {
            var query = Query<UserChange>.EQ(c => c.Id, changesInfo.Id);
            var replacement = Update<UserChange>.Replace(changesInfo);
            return _userAuditCollection.Update(query, replacement).Ok;
        }

        public UserChange GetByUserName(string userName)
        {
            return _userAuditCollection.AsQueryable<UserChange>().FirstOrDefault(r => r.UserName.Equals(userName));
        }

        public List<UserChange> GetAll()
        {
            return _userAuditCollection.AsQueryable<UserChange>().ToList();
        }

        #region private methods
        private MongoCollection GetCollection(string connectionString, string collectionName)
        {
            var url = new MongoUrl(connectionString);
            return new MongoClient(url).GetServer().GetDatabase(url.DatabaseName).GetCollection(collectionName);
        }

        #endregion private methods
    }
}