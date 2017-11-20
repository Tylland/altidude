namespace Altidude.Views
{
    public interface IViews
    {
        IChartTypeView ChartTypes { get; }
        IUserLevelView Levels { get; }
        IPlaceView Places { get; }
        IProfileView Profiles { get; }
        IUserView Users { get; }
    }
}