using Altidude.Contracts.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Altidude.net.Models
{

    public class AdminViewModel
    {
        public int TotalNrOfUsers { get; set; }
        public int TotalNrOfProfiles { get; set; }
        public int TotalNrOfPlaces { get; set; }
    }

    public class DatabaseViewModel
    {
        public string CreateScript { get; set; }
    }
    public class UsersViewModel
    {
        public List<User> Users { get; set; }
    }
    public class ProfilesViewModel
    {
        public List<ProfileSummary> Profiles { get; set; }
    }
}