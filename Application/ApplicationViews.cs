using Altidude.Views;

namespace Altidude.Application
{
    public class ApplicationViews : IViews
    {
        public IUserView Users { get; private set; }
        public IUserLevelView Levels { get; private set; }
        public IProfileView Profiles { get; private set; }
        public IPlaceView Places { get; private set; }
        public IChartTypeView ChartTypes { get; private set; }

        public ApplicationViews(IUserView users, IUserLevelView levels, IProfileView profiles, IPlaceView places, IChartTypeView chartTypes)
        {
            Users = users;
            Levels = levels;
            Profiles = profiles;
            Places = places;
            ChartTypes = chartTypes;
        }
    }
}
