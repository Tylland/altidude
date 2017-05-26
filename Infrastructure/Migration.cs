using System.Data;

namespace Altidude.Infrastructure
{
    public abstract class Migration
    {
        public abstract void Apply(IDbConnection db);
    }
}
