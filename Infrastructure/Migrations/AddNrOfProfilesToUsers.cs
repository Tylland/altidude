using System.Data;
using ServiceStack.OrmLite;

namespace Altidude.Infrastructure.Migrations
{
    public class AddNrOfProfilesToUsers : Migration
    {
        public override void Apply(IDbConnection db)
        {
            db.AddColumn<UserView>(tbl => tbl.NrOfProfiles);
        }
    }
}
