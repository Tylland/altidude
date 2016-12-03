using Altidude.Contracts.Types;

namespace Altidude.Views
{
    public interface IUserLevelView
    {
        UserLevel GetLevel(int level);
    }
}
