
namespace Altidude.Contracts.Types
{
    public class ClimbCategory
    {
        public const double MinSlope = 0.03;

        public static readonly ClimbCategory HC = new ClimbCategory("HC", 80000);
        public static readonly ClimbCategory Cat1 = new ClimbCategory("1", 64000);
        public static readonly ClimbCategory Cat2 = new ClimbCategory("2", 32000);
        public static readonly ClimbCategory Cat3 = new ClimbCategory("3", 16000);
        public static readonly ClimbCategory Cat4 = new ClimbCategory("4", 8000);

        public string Name { get; set; }
        public double Threshold { get; set; }

        public static ClimbCategory GetCategory(double points)
        {
            if (points >= HC.Threshold)
                return HC;

            if (points >= Cat1.Threshold)
                return Cat1;

            if (points >= Cat2.Threshold)
                return Cat2;

            if (points >= Cat3.Threshold)
                return Cat3;

            if (points >= Cat4.Threshold)
                return Cat4;

            return null;
        }
        public ClimbCategory(string name, double threshold)
        {
            Name = name;
            Threshold = threshold;
        }
    }
}
