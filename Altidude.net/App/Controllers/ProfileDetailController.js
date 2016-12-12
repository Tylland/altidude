var Controllers;
(function (Controllers) {
    var ProfileDetailController = (function () {
        function ProfileDetailController(profileService, chartService) {
            this.profileService = profileService;
            this.chartService = chartService;
            //this.chart = new ProfileChart.LoadingChart();
        }
        ProfileDetailController.prototype.init = function (profileId, userId) {
            var _this = this;
            this.profileService.getProfile(profileId).then(function (response) {
                _this.profile = response.data;
                _this.result = _this.profile.result;
                _this.chart = _this.chartService.getChart(_this.profile.chartId);
                _this.isOwner = _this.profile.userId === userId;
            });
        };
        ProfileDetailController.prototype.shareOnFacebook = function () {
            //profileService.
            //alert('share profile ' + this.profile.id);
            this.profileService.shareOnFacebook(this.profile);
        };
        ProfileDetailController.$inject = ['profileService', 'chartService'];
        return ProfileDetailController;
    })();
    Controllers.ProfileDetailController = ProfileDetailController;
    angular.module('altidudeApp').controller('profileDetailController', Controllers.ProfileDetailController);
})(Controllers || (Controllers = {}));
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
