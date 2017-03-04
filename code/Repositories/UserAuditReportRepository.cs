using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using Sitecore.Diagnostics;
using UserAuditReport.DTO;

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
            Assert.IsNotNullOrEmpty(connectionStringName, "UserAuditReport.ConnectionStringName setting required");
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            _userAuditCollection = GetCollection(connectionString, _collectionName);
        }

        //https://laubplusco.net/working-custom-mongodb-collections-sitecore-8-using-webapi/
        public bool Add(UserChangeDto changesInfo)
        {
            return _userAuditCollection.Insert(changesInfo).Ok;
        }

        public UserChangeDto GetByUserName(string userName)
        {
            return _userAuditCollection.AsQueryable<UserChangeDto>().FirstOrDefault(r => r.UserName.Equals(userName));
        }

        public bool Update(UserChangeDto changesInfo)
        {
            var query = Query<UserChangeDto>.EQ(c => c.Id, changesInfo.Id);
            var replacement = Update<UserChangeDto>.Replace(changesInfo);
            return _userAuditCollection.Update(query, replacement).Ok;
        }

        //public IEnumerable<Comment> GetAll()
        //{
        //    return _userAuditCollection.FindAllAs<Comment>();
        //}
        //public Comment Get(ObjectId id)
        //{
        //    return _userAuditCollection.FindOneAs<Comment>(GetIDQuery(id));
        //}
        //public bool Set(Comment comment)
        //{
        //}
        //public IEnumerable<Comment> GetByDateRange(DateTime startDate, int numberOfDays)
        //{
        //    return _userAuditCollection.AsQueryable<Comment>().Where(c => c.Date >= startDate && c.Date < startDate.AddDays(numberOfDays)).ToArray();
        //}
        //protected IMongoQuery GetIDQuery(ObjectId id)
        //{
        //    return Query<Comment>.EQ(c => c.Id, id);
        //}

        private static MongoCollection GetCollection(string connectionString, string collectionName)
        {
            var url = new MongoUrl(connectionString);
            return new MongoClient(url).GetServer().GetDatabase(url.DatabaseName).GetCollection(collectionName);
        }
    }
}