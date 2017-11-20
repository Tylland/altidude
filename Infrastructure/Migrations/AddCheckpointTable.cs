using System.Data;
using Altidude.Contracts.Events;
using Microsoft.CSharp.RuntimeBinder;
using ServiceStack.OrmLite;

namespace Altidude.Infrastructure.Migrations
{
    public class AddCheckpointTable : Migration
    {
        public override void Apply(IDbConnection db)
        {
            db.CreateTable<OrmLiteCheckpointStorage.CheckpontEntity>();
        }
    }
}
