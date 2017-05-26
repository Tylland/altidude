using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altidude.Contracts;
using Microsoft.CSharp.RuntimeBinder;
using Serilog;
using ServiceStack.OrmLite;

namespace Altidude.Infrastructure.Migrations
{
    public class RebuildProfileView : Migration
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<RebuildProfileView>();
        public override void Apply(IDbConnection db)
        {
            //db.ExecuteNonQuery("SELECT Id, UserId, Name, NrOfViews, Kudos, CreatedTime, Payload INTO ProfileBackup FROM ProfileEnvelope");

            db.CreateTable<ProfileEnvelope>(false);
            db.ExecuteNonQuery("ALTER TABLE Profiles ALTER COLUMN Track VARCHAR(MAX)");
            db.ExecuteNonQuery("ALTER TABLE Profiles ALTER COLUMN Places VARCHAR(MAX)");

            var view = new OrmLiteProfileView(db);

            var eventStore = new NEventStoreDomainRepository("AltidudeConnection");

            var events = eventStore.GetAllEvents();

            foreach (var evt in events)
            {
                try
                {
                    view.Handle((dynamic)evt);
                }
                catch (RuntimeBinderException ex)
                {
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Replay event '{@evt}' in '{view}' failed", evt, view);
                    throw;
                }

            }
        }
    }
}
