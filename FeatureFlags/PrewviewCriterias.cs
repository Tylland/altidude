using System;
using System.Collections.Generic;

namespace Altidude.FeatureFlags
{
    public class PreviewCriterias
    {
        private static readonly List<string> PowerUsers = new List<string> {"andgusta@hotmail.com"};
        public static bool IsPowerUser(PreviewCriteriaContext context)
        {
            //if (string.IsNullOrEmpty(context.User.Identity.Name))
            //    return false;

            return PowerUsers.Contains(context.User.Identity.Name);
        }
    }
}
