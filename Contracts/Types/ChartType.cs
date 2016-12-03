using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Contracts.Types
{
    public class ChartType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TitleTemplate { get; set; }
        public string DescriptionTemplate { get; set; }

        public string GetTitle(string userDisplayName, string profileName)
        {
            return string.Format(TitleTemplate, userDisplayName, profileName, Name);
        }
        public string GetDescription(string userDisplayName, string profileName)
        {
            return string.Format(DescriptionTemplate, userDisplayName, profileName, Name);
        }

        public ChartType(string idStr, string name, string titleTemplate, string descriptionTemplate)
            : this(Guid.Parse(idStr), name, titleTemplate, descriptionTemplate)
        {
        }
        public ChartType(Guid id, string name, string storyTemplate, string descriptionTemplate)
        {
            Id = id;
            Name = name;
            TitleTemplate = storyTemplate;
            DescriptionTemplate = descriptionTemplate;
        }
        public ChartType()
        {

        }
    }
}
