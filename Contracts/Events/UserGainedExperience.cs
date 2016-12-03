using System;

namespace Altidude.Contracts.Events
{
    public class UserGainedExperience: IEvent
    {
        public Guid Id { get; set; }
        public int NewPoints { get; set; }
        public int TotalPoints { get; set; }

        public UserGainedExperience(Guid id, int newPoints, int totalPoints)
        {
            Id = id;
            NewPoints = newPoints;
            TotalPoints = totalPoints;
        }
        public UserGainedExperience()
        {

        }
    }
}
