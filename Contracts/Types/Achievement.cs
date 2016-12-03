using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qrunchtime.Domain.Models
{
//   Name: The Achievement’s name. Currently used as an identifier, needs to be unique.
//Description – Description of the achievement – often described how to earn the achievement.
//IconIncomplete – The image to show before the achievement is earned.
//IconComplete – The image to shotw after the achievement is earned.
//RewardPoints – How many ‘points’ to give the player when he earns this achievement.
//Secret – Is this achievement secret? If so, then we don’t show a description and we don’t show the RewardPoints until after the achievement is earned.
//TargetProgress – Many achievements want to track progress of some sort, so this provides a target that we’re working towards. So if you’re trying to kill 15 goblins, then TargetProgress would be set to 15. More on this below.

   public class Achievement
   {
      public string Name { get; set; }
      public string Description { get; set; }
      public string IconIncomplete { get; set; }
      public string IconComplete { get; set; }
      public string RewardQrumbs { get; set; }
      public bool Secret { get; set; }

   }
}
