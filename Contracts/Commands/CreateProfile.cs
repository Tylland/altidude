using System;

using Altidude.Contracts.Types;

namespace Altidude.Contracts.Commands
{
    public class CreateProfile : ICommand
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ChartId { get; set; }
        public string Name { get; set; }
        public Track Track { get; set; }

        public CreateProfile(Guid id, Guid userId, Guid chartId, string name, Track track)
        {
            Id = id;
            UserId = userId;
            ChartId = chartId;
            Name = name;
            Track = track;
        }

        public CreateProfile()
        {

        }
    }
}
