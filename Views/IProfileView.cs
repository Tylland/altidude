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
        ProfileSummary GetSummaryById(Guid id);
        List<ProfileSummary> GetSummaries();
        List<ProfileSummary> GetLatestSummaries(int nrOfProfiles);
        List<ProfileSummary> GetLatestSummaries(Guid[] userIds, int start, int nrOfProfiles);
        long Count();
        int GetTotalNrOfViews();
    }
}
