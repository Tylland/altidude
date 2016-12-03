
using System;

namespace Altidude.Contracts.Types
{
    public class Result
    {
        public Athlete Athlete { get; set; }
        public DateTime StartTime { get; set; }
        public int TotalTimeSeconds { get; set; }

        public ResultSplit[] Splits { get; set; }

        public Result(Athlete athlete, ResultSplit[] splits)
        {
            Athlete = athlete;
            Splits = splits;

            StartTime = Splits[0].Time;
            TotalTimeSeconds = Splits[Splits.Length - 1].TotalTimeSeconds;
        }
        public Result()
        {

        }
    }
}
