using Altidude.Contracts.Types;

namespace Altidude.Domain
{
    public interface IUserLevelService
    {
        UserLevel GetLevel(int level);
        UserLevel CalcLevel(int experiencePoints);
    }
}
