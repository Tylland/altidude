module Controllers {
    export class ProfileDetailController {
        private static noUserId = "00000000-0000-0000-0000-000000000000";
        public chart: ProfileChart.Chart;
        public profile: any;
        public result: any;
        public isOwner: boolean;
        public isAuthenticated: boolean;
        public kudos: number;

        static $inject = ['$window', 'profileService', 'chartService'];
        constructor(private $window: any, private profileService: Profile.ProfileService, private chartService: Services.ChartService) {
            //this.chart = new ProfileChart.LoadingChart();
        }

        public init(profileId: any, userId: any): void {

            this.profileService.getProfile(profileId).then(response => {
                this.profile = response.data;
                this.result = this.profile.result;

                this.chart = this.chartService.getChart(this.profile.chartId);

                this.isAuthenticated = userId !== ProfileDetailController.noUserId;
                this.isOwner = this.profile.userId === userId;

                this.kudos = this.profile.kudos;
            });
        }

        public giveKudos(): void {

            if (!this.isAuthenticated)
                this.$window.location.href = "/account/login/?returnurl=" + this.$window.location.pathname;

            this.profileService.giveKudos(this.profile.id).then((response: ng.IHttpPromiseCallbackArg<number>) => {
                this.kudos = response.data;
            });
        }

        public shareOnFacebook(): void {
            //profileService.
            //alert('share profile ' + this.profile.id);
            this.profileService.shareOnFacebook(this.profile);
        }
    }

    angular.module('altidudeApp').controller('profileDetailController', Controllers.ProfileDetailController);
}



//angular.module('altidudeApp')
//    .controller('profileEditController', ['$scope', '$rootScope', '$log', '$location', 'profileService', 'serviceConfig', function ($scope, $rootScope, $log, $location, profileService: Profile.ProfileService, serviceConfig) {

//        $scope.dateFormat = 'yyyy-MM-dd';
//        $scope.timeFormat = 'yyyy-MM-dd HH:mm:ss';

//        $scope.files = null;
//        $scope.trackFile = null;
//        $scope.importedProfile = null;

//        $scope.profileId;
//        $scope.profile = {};
//        //$scope.chart = new ProfileChart.ForestChart(new ProfileChart.ChartRenderingSettings(false, false));

//        $scope.chart = new ProfileChart.TestChart();
//        $scope.result = null;


//        init();

//        function init() {

//        }

//        $scope.init = function(profileId)
//        {
//            $scope.profileId = profileId;

//            $scope.getProfile(profileId);
//        }

//        $scope.getProfile = function (profileId) {
//            profileService.getProfile(profileId).then(response => {
//                $scope.profile = response.data;
//                $scope.result = $scope.profile.result;
//            });
//        };


//    }]);

