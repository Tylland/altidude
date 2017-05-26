using System;
using System.Collections.Generic;

namespace Altidude.Contracts.Types
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName
        {
            get
            {
                string displayName = string.Empty;

                if (!string.IsNullOrEmpty(FirstName))
                    displayName = FirstName;

                if (!string.IsNullOrEmpty(LastName))
                {
                    if (displayName != string.Empty)
                        displayName += " ";

                    displayName += LastName;
                }

                if (displayName == string.Empty)
                    displayName = UserName;

                return displayName;
            }
        }

        public bool AcceptsEmails { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public List<Guid> FollowingUserIds { get; set; }
        public bool IsFollowing(Guid userId)
        {
            return FollowingUserIds.Contains(userId);
        }
        public List<Guid> FollowedByUserIds { get; set; }
        public bool IsFollowedBy(Guid userId)
        {
            return FollowedByUserIds.Contains(userId);
        }

        public UserProfileSummary ProfileSummary { get; set; }

        public User()
        {
        }

        public User(Guid id, string userName, string email, string firstName, string lastName, bool acceptsEmails, int experiencePoints, int level, List<Guid> followingUserIds, List<Guid> followedByUserIds, UserProfileSummary profileSummary)
        {
            Id = id;
            UserName = userName;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            AcceptsEmails = acceptsEmails;
            ExperiencePoints = experiencePoints;
            Level = level;
            FollowingUserIds = followingUserIds ?? new List<Guid>();  
            FollowedByUserIds = followedByUserIds ?? new List<Guid>();
            ProfileSummary = profileSummary;
        }
    }

}
