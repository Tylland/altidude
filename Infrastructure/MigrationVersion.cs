using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace Altidude.Infrastructure
{
    [Alias("MigrationVersions")]
    public class MigrationVersion
    {
        [PrimaryKey]
        [StringLength(255)]
        public string MigrationName { get; set; }
        public DateTime AppliedTime { get; set; }
    }
}
