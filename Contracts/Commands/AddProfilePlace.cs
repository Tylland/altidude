using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Contracts.Commands
{
    public class AddProfilePlace : ICommand
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }
        public double Size { get; set; }
        public bool Split { get; set; }
    }
}
