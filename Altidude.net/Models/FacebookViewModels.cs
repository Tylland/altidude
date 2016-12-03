using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Altidude.net.Models
{
    public class FacebookSharedProfileViewModel
    {
        public Guid ProfileId { get; set; }
    }
    public class FacebookShareProfileViewModel
    {
        public Guid ProfileId { get; set; }

        public FacebookShareProfileViewModel(Guid profileId)
        {
            ProfileId = profileId;
        }
        public FacebookShareProfileViewModel()
        {

        }
    }
}