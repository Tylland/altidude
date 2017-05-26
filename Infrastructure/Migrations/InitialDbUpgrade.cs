using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Infrastructure.Migrations
{
    public class InitialDbUpgrade : Migration
    {
        public override void Apply(IDbConnection db)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["AltidudeConnection"].ConnectionString;

            var result = DbUpgrader.Upgrade(connectionString, true);
        }
    }
}
