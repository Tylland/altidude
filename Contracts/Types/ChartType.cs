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
        public int UnlockOnLevel { get; set; }
        public DateTime UnlockTomDate { get; set; }


        public string GetTitle(string userDisplayName, string profileName)
        {
            return string.Format(TitleTemplate, userDisplayName, profileName, Name);
        }
        public string GetDescription(string userDisplayName, string profileName)
        {
            return string.Format(DescriptionTemplate, userDisplayName, profileName, Name);
        }

        public bool GetIsUnlocked(int level, DateTime now)
        {
            return level >= UnlockOnLevel || now <= UnlockTomDate;
        }

        public ChartType(string idStr, string name, string titleTemplate, string descriptionTemplate, int unlockedLevel, DateTime unlockedToDate)
            : this(Guid.Parse(idStr), name, titleTemplate, descriptionTemplate, unlockedLevel, unlockedToDate)
        {
        }

        public ChartType(Guid id, string name, string storyTemplate, string descriptionTemplate, int unlockLevel, DateTime unlockToDate)
        {
            Id = id;
            Name = name;
            TitleTemplate = storyTemplate;
            DescriptionTemplate = descriptionTemplate;
            UnlockOnLevel = unlockLevel;
            UnlockTomDate = unlockToDate;
        }
        public ChartType()
        {

        }
    }
}
