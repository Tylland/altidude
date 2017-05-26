using Altidude.Contracts.Types;
using System.Collections.Generic;

namespace Altidude.net.Models
{
    public class HomeViewModel
    {
        public List<ProfileSummary> Profiles { get; set; }
    }

    public class ContactUsMessageModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }


}