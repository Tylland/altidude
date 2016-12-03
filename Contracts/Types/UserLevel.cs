using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Contracts.Types
{
    public class UserLevel
    {
        public int Level { get; set; }
        public int MinPoints { get; set; }
        public int MaxPoints { get; set; }
        public int Points
        {
            get
            {
                return MaxPoints - MinPoints;
            }
        }

        public int GetProgress(int points)
        {
            return Convert.ToInt32(((points - MinPoints) * 100) / (MaxPoints - MinPoints));
        }
        public int GetLevelPoints(int points)
        {
            return Math.Min(points - MinPoints, MaxPoints);
        }

        public UserLevel(int level, int minPoints, int maxPoints)
        {
            Level = level;
            MinPoints = minPoints;
            MaxPoints = maxPoints;
        }
        public UserLevel()
        {

        }
    }
}
