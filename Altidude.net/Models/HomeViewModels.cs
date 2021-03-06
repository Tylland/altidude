﻿using Altidude.Contracts.Types;
using System.Collections.Generic;

namespace Altidude.net.Models
{
    public class HomeViewModel
    {
        public List<ProfileSummary> Profiles { get; set; }
        public List<ChartType> ChartTypes { get; set; }
    }

    public class ContactUsMessageModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
    public class ShowMessageModel
    {
        public string Message { get; set; }
    }


}