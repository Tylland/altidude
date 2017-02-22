using System;

namespace Altidude.Contracts.Types
{
    public class Profile
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ChartId { get; set; }
        public string Name { get; set; }
        public Track Track { get; set; }
        public ProfilePlace[] Places { get; set; }
        public Leg[] Legs { get; set; }
        public Result Result { get; set; }
        public int Kudos { get; set; }

        public Profile(Guid id, Guid userId, string name, Track track, ProfilePlace[] places, Leg[] legs, Result result, int kudos)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Track = track;
            Places = places;
            Legs = legs;
            Result = result;
            Kudos = kudos;
        }
        public Profile()
        {

        }

    }
}
