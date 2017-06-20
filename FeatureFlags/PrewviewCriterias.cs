using System;
using System.Collections.Generic;

namespace Altidude.FeatureFlags
{
    public class PreviewCriterias
    {
        private static readonly List<string> PowerUsers = new List<string> {"andgusta@hotmail.com"};
        public static bool IsPowerUser(PreviewCriteriaContext context)
        {
            return PowerUsers.Contains(context.User.Identity.Name);
        }
        public static bool IsSuperUser(PreviewCriteriaContext context)
        {
            return context.User.IsInRole("SuperUser");
        }
        public static bool IsPreviewUser(PreviewCriteriaContext context)
        {
            return context.User.IsInRole("PreviewUser");
        }
    }
}
