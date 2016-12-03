using Altidude.Domain;
using System.Collections.Generic;
using Altidude.Contracts.Types;
using Altidude.Views;

namespace Altidude.Infrastructure
{
    public class InMemoryUserLevelService : IUserLevelService, IUserLevelView
    {
        private const int MinLevel = 1;
        private const int MaxLevel = 20;

        private List<UserLevel> _levels = new List<UserLevel>();

        public UserLevel CalcLevel(int experiencePoints)
        {
            foreach (var level in _levels)
            {
                if (level.MinPoints <= experiencePoints && experiencePoints < level.MaxPoints)
                    return level;
            }

            return _levels[MaxLevel - 1];
        }

        public UserLevel GetLevel(int level)
        {
            if (level < MinLevel)
                level = MinLevel;

            if (level >= MaxLevel)
                level = MaxLevel;

            return _levels[level - 1];
        }

        public InMemoryUserLevelService()
        {
            int level = 1;
            int levelPoints = 0;
            int prevValue1 = 1;
            int prevValue2 = 0;

            for (int i = 0; i <= MaxLevel; i++)
            {
                var levelValue = prevValue1 + prevValue2;
                var levelRange = levelValue * 100;

                _levels.Add(new UserLevel(level, levelPoints, levelPoints + levelRange));

                levelPoints += levelRange;
                prevValue2 = prevValue1;
                prevValue1 = levelValue;
                level++;
            }
        }
    }
}
