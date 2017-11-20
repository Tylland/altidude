using System.ComponentModel.DataAnnotations;
using System.Data;
using Altidude.Domain;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace Altidude.Infrastructure
{
    public class OrmLiteCheckpointStorage : ICheckpointStorage
    {
        [Alias("Checkpoints")]
        public class CheckpontEntity
        {
            [PrimaryKey]
            [StringLength(100)]
            public string Name { get; set; }
            [StringLength(100)]
            public string Token { get; set; }
        }

        private readonly IDbConnection _db;

        public OrmLiteCheckpointStorage(IDbConnection db)
        {
            _db = db;
        }

        public void Save(string name, string token)
        {
            var entity = new CheckpontEntity {Name = name, Token = token};

            _db.Save(entity);
        }

        public string GetToken(string name)
        {
            var checkpoint = _db.GetByIdOrDefault<CheckpontEntity>(name);

            return checkpoint?.Token;
        }
    }
}
