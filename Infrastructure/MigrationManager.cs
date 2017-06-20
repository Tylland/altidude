using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
using Altidude.Infrastructure.Migrations;
using Serilog;

namespace Altidude.Infrastructure
{
    public class MigrationManager
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<MigrationManager>();
        private readonly IDbConnection _db;
        private readonly IList<Migration> _migrations;

        public MigrationManager(IDbConnection db)
        {
            _db = db;

            _migrations = new List<Migration>
            {
                new InitialDbUpgrade(),
                new RebuildProfileView(),
                new AddUserStatistics(),
                new AddNrOfProfilesToUsers(),
            };
        }

        public void ApplyAsync()
        {
            Task.Run(() => Apply()).Wait();
        }

        public void Apply()
        {
            var versions = _db.Select<MigrationVersion>();
            var appliedMigrationNames = versions.Select(ver => ver.MigrationName);

            var pendingMigrations =
                _migrations.Where(migration => !appliedMigrationNames.Contains(migration.GetType().Name));

            foreach (var pendingMigration in pendingMigrations)
            {
                try
                {
                    pendingMigration.Apply(_db);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Applying pending migration '{Migration}' failed", pendingMigration);
                    throw;
                }

                _db.Insert(new MigrationVersion
                {
                    MigrationName = pendingMigration.GetType().Name,
                    AppliedTime = DateTime.Now
                });
            }
        }
    }
}
