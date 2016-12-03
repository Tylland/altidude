using Altidude.Contracts.Types;
using System;
using System.Collections.Generic;

namespace Altidude.net.Models
{
    public class ProfileViewModels
    {
    }
    public class ProfileIndexViewModel
    {
        public List<Profile> Profiles { get; set; }
    }

    public class ProfileEditViewModel
    {
        public Guid ProfileId { get; set; }
    }
    public class ProfileDetailViewModel
    {
        public Guid ProfileId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ChartImageUrl { get; set; }

        public ProfileDetailViewModel(Guid profileId, Guid userId, string title, string description, string chartImageUrl)
        {
            ProfileId = profileId;
            UserId = userId;
            Title = title;
            Description = description;
            ChartImageUrl = chartImageUrl;
        }
        public ProfileDetailViewModel()
        {

        }

    }


}