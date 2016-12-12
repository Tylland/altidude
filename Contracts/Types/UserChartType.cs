namespace Altidude.Contracts.Types
{
    public class UserChartType : ChartType
    {
        public bool IsUnlocked { get; private set; }

        public UserChartType(ChartType type, bool isUnlocked)
        {
            Id = type.Id;
            Name = type.Name;
            TitleTemplate = type.TitleTemplate;
            DescriptionTemplate = type.DescriptionTemplate;
            UnlockOnLevel = type.UnlockOnLevel;
            UnlockTomDate = type.UnlockTomDate;
            IsUnlocked = isUnlocked;
        }
    }
}
