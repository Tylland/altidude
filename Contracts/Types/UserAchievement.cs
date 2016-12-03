using ServiceStack.DataAnnotations;

namespace Qrunchtime.Domain.Models
{
    public class UserAchievement
    {
        [PrimaryKey]
        [References(typeof(User))]
        public int UserId { get; set; }

        [PrimaryKey]
        [References(typeof(Achievement))]
        public string AchievementName { get; set; }
        public bool Completed { get; set; }
        public bool Claimed { get; set; }

        public UserAchievement(int userId, string achievmentName, bool completed, bool claimed)
        {
            UserId = userId;
            AchievementName = achievmentName;
            Completed = completed;
            Claimed = claimed;
        }
        public UserAchievement()
        {

        }
    }
}
