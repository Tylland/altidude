using System.Linq;
using System.Collections.Generic;

namespace Qrunchtime.Domain.Models
{
   public class UserProgress
   {
        public int UserId { get; set; }
        public int UserRegistred { get; set; }
        public int CheckpointCreated { get; set; }
        public int CheckpointVisited { get; set; }

        public UserProgress(IEnumerable<UserProgressValue> progressValues)
        {
            UserRegistred = progressValues.GetProgressValue(Progress.UserRegistred);
            CheckpointCreated = progressValues.GetProgressValue(Progress.CheckpointCreated);
            CheckpointVisited = progressValues.GetProgressValue(Progress.CheckpointVisited);
        }

        public UserProgress()
        {

        }
    }

    public static class UserProgressValueExtensions
    {
        public static int GetProgressValue(this IEnumerable<UserProgressValue> progressValues, string progressName)
        {
            return progressValues.FirstOrDefault(value => value.ProgressName == Progress.UserRegistred).Value;
        }
    }
}
