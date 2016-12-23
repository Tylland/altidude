var Controllers;
(function (Controllers) {
    var ProfileEditController = (function () {
        function ProfileEditController($window, profileService, serviceConfig, chartTypeService, chartService, modalService) {
            this.$window = $window;
            this.profileService = profileService;
            this.serviceConfig = serviceConfig;
            this.chartTypeService = chartTypeService;
            this.chartService = chartService;
            this.modalService = modalService;
            this.chartTypes = [];
            this.charts = [];
            this.chartNames = [];
            //this.chart = new ProfileChart.LoadingChart();
        }
        ProfileEditController.prototype.init = function (profileId) {
            var _this = this;
            this.chartSelectionVisible = false;
            this.profileId = profileId;
            this.profileService.getProfile(profileId).then(function (response) {
                _this.profile = response.data;
                _this.result = _this.profile.result;
                _this.chart = _this.chartService.getChart(_this.profile.chartId);
                _this.charts = _this.chartService.getAllCharts();
            });
            this.chartTypeService.getUserChartTypes().then(function (response) {
                _this.chartTypes = response.data;
            });
        };
        ProfileEditController.prototype.getChartType = function (chartId) {
            for (var _i = 0, _a = this.chartTypes; _i < _a.length; _i++) {
                var chartType = _a[_i];
                if (chartId === chartType.id) {
                    return chartType;
                }
            }
            return null;
        };
        ProfileEditController.prototype.selectChartType = function (chartId) {
            var chartType = this.getChartType(chartId);
            if (chartType != undefined && chartType.isUnlocked) {
                this.chart = this.chartService.getChart(chartId);
            }
        };
        ProfileEditController.prototype.selectChart = function (chart) {
            this.chartSelectionVisible = false;
            this.chart = chart;
        };
        ProfileEditController.prototype.onBitmapCreated = function (base64Image) {
            this.base64Image = base64Image.replace('data:image/png;base64,', '');
            this.profileService.changeChart(this.profileId, this.chart.id, this.base64Image);
            //            alert(base64Image);
        };
        ProfileEditController.prototype.delete = function () {
            var _this = this;
            var modalOptions = {
                closeButtonText: 'Cancel',
                actionButtonText: 'Delete Profile',
                headerText: 'Delete ' + this.profile.name + '?',
                bodyText: 'Are you sure you want to delete this profile?'
            };
            this.modalService.showModal({}, modalOptions).then(function (response) {
                _this.profileService.delete(_this.profileId).then(function (res) {
                    _this.$window.location.href = "/profile/";
                });
            });
        };
        ProfileEditController.$inject = ['$window', 'profileService', 'serviceConfig', 'chartTypeService', 'chartService', 'modalService'];
        return ProfileEditController;
    })();
    Controllers.ProfileEditController = ProfileEditController;
    angular.module('altidudeApp').controller('profileEditController', Controllers.ProfileEditController);
})(Controllers || (Controllers = {}));
//angular.module('altidudeApp')
//    .controller('profileEditController', ['$scope', '$rootScope', '$log', '$location', 'profileService', 'serviceConfig', 'chartService', function ($scope, $rootScope, $log, $location, profileService: Profile.ProfileService, serviceConfig, chartService: Services.ChartService) {
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
//        $scope.charts = [];
//        init();
//        function init() {
//        }
//        findChart(chartName)
//        {
//        }
//        $scope.selectChart = function (chartName) {
//            $scope.charts = chartService.getAllCharts();
//            $scope.getProfile(profileId);
//        }
//        $scope.init = function(profileId)
//        {
//            $scope.profileId = profileId;
//            $scope.charts = chartService.getAllCharts();
//            $scope.getProfile(profileId);
//        }
//        $scope.getProfile = function (profileId) {
//            profileService.getProfile(profileId).then(response => {
//                $scope.profile = response.data;
//                $scope.result = $scope.profile.result;
//            });
//        };
//    }]);
//# sourceMappingURL=ProfileEditController.js.map