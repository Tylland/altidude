using System;
using System.Security.Principal;

namespace Altidude.FeatureFlags
{
    public class PreviewCriteriaContext
    {
        public Feature Feature { get; set; }
        public IPrincipal User { get; set; }
        public DateTime Time { get; set; }
    }
}
