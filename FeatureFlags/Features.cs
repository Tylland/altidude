using System;

namespace Altidude.FeatureFlags
{
    public class Features
    {
        public static Feature UserNameInProfileDetailPage;
        public static Feature UserProfile;
        public static Feature FollowUser;
        public static Feature UserDashboard;
        public static Feature InviteFBFriends;

        static Features()
        {
            UserNameInProfileDetailPage = new Feature(FeatureState.Preview, PreviewCriterias.IsPowerUser);
            UserProfile = new Feature(FeatureState.Preview, PreviewCriterias.IsPowerUser, UserNameInProfileDetailPage);
            FollowUser = new Feature(FeatureState.Preview, PreviewCriterias.IsPowerUser, UserProfile);
            UserDashboard = new Feature(FeatureState.Preview, PreviewCriterias.IsPreviewUser);
            InviteFBFriends = new Feature(FeatureState.Preview, PreviewCriterias.IsPreviewUser);
        }

        //public static void Configure(Action configureAction)
        //{
        //    configureAction(Features);
        //}

    }
}
