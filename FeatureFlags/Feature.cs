using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Microsoft.Win32;

namespace Altidude.FeatureFlags
{
    public class Feature
    {
        public FeatureState Flag { get; set; }
        public Func<PreviewCriteriaContext, bool> PreviewCriteria;
        private readonly List<Feature> _dependencies;

        public Feature(FeatureState flag, Func<PreviewCriteriaContext, bool> previewCriteria, params Feature[] dependencies)
        {
            Flag = flag;
            PreviewCriteria = previewCriteria;
            _dependencies = new List<Feature>(dependencies);
        }
        public Feature(FeatureState flag, params Feature[] dependencies)
            : this(flag, feature => false, dependencies)
        {
        }

        private bool DependenciesAvailable(PreviewCriteriaContext context)
        {
            return _dependencies.All(feature => feature.IsAvailable(context));
        }

        public Feature WithUser(IPrincipal user)
        {
            return null;
        }

        public bool IsAvailable()
        {
            var context = new PreviewCriteriaContext { Time = DateTime.Now };

            return IsAvailable(context);
        }
        public bool IsAvailable(IPrincipal user)
        {
            var context = new PreviewCriteriaContext { Time = DateTime.Now, User = user };

            return IsAvailable(context);
        }
        public bool IsAvailable(PreviewCriteriaContext context)
        {
            context.Feature = this;

            if (Flag == FeatureState.Established)
                return DependenciesAvailable(context);

            if (Flag == FeatureState.Preview && PreviewCriteria != null)
                return DependenciesAvailable(context) && PreviewCriteria(context);

            return false;
        }
    }
}
