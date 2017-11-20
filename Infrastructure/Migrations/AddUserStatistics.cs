using System.Data;
using Altidude.Contracts.Events;
using Microsoft.CSharp.RuntimeBinder;
using ServiceStack.OrmLite;

namespace Altidude.Infrastructure.Migrations
{
    public class AddUserStatistics : Migration
    {
        public override void Apply(IDbConnection db)
        {
            db.CreateTable<UserView>();

            db.ExecuteNonQuery(
                "INSERT INTO Users(Id, UserName, Email, FirstName, LastName, AcceptsEmails, ExperiencePoints, Level , CreatedTime, FollowingUserIds, FollowedByUserIds, NrOfProfiles, HighestAltitude, LowestAltitude)" +
                "SELECT Id, UserName, Email, FirstName, LastName, AcceptsEmails, ExperiencePoints, Level, CreatedTime, FollowingUserIds, FollowedByUserIds,0 , -10000, 10000" +
                " FROM UserView ");

            var view = new OrmLiteUserView(db);

            var eventStore = new NEventStoreDomainRepository("AltidudeConnection");

            var events = eventStore.GetAllEvents();


            foreach (var evt in events)
            {
                
                try
                {
                    if(evt is ProfileCreated)
                        view.Handle((dynamic)evt);
                }
                catch (RuntimeBinderException ex)
                {
                }

            }
        }
    }
}
