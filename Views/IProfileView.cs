using System;
using Altidude.Contracts;
using Altidude.Contracts.Events;
using System.Collections.Generic;
using Altidude.Contracts.Types;

namespace Altidude.Views
{
    public interface IProfileView : IHandleEvent<ProfileCreated>
    {
        Profile GetById(Guid id);
        List<Profile> GetByUser(Guid userId);
        List<Profile> GetAll();
        List<Profile> GetLatest(int nrOfProfiles);
        List<ProfileSummary> GetSummaries();
        int GetTotalNrOfViews();
    }
}
