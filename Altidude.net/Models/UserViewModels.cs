using System;
using System.Collections.Generic;
using Altidude.Contracts.Types;

namespace Altidude.net.Models
{
    public class UserProfileViewModel
    {
        public User User { get; set; }
        public UserLevel Level { get; set; }
        public int LevelProgress { get; set; }
        public int LevelPoints { get; set; }
        public bool IsFollowedBy { get; set; }
        public string FollowingText { get; set; }
    }

    public class LoginUserViewModel
    {
        public User User { get; set; }
    }
    public class FollowUserViewModel
    {
        public Guid UserId { get; set; }
    }

    public class UserDashboardViewModel
    {
        public UserProfileSummary ProfileSummary { get; set; }
        public List<ProfileSummary> Profiles { get; set; }
    }
}