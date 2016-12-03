using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public User()
        {
        }

        public User(Guid id, string userName, string email, string firstName, string lastName, bool acceptsEmails, int experiencePoints, int level)
        {
            Id = id;
            UserName = userName;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            AcceptsEmails = acceptsEmails;
            ExperiencePoints = experiencePoints;
            Level = level;
        }
    }

}
