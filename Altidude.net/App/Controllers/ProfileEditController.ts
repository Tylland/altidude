module Controllers {
    export class ProfileEditController {
        private profileId: any;
        public charts: ProfileChart.Chart[] = [];
        private base64Image: string;

        public chartNames: string[] = [];
        public selectedChartName: string;

        public chart: ProfileChart.Chart;
        public profile: any;
        public result: any;

        public chartSelectionVisible: boolean;

        static $inject = ['$window', 'profileService', 'serviceConfig', 'chartService', 'modalService'];
        constructor(private $window: any, private profileService: Profile.ProfileService, private serviceConfig, private chartService: Services.ChartService, private modalService: Services.ModalService) {
            //this.chart = new ProfileChart.LoadingChart();
        }

        public init(profileId: any): void {
            this.chartSelectionVisible = false;
            this.profileId = profileId;

            this.profileService.getProfile(profileId).then(response => {
                this.profile = response.data;
                this.result = this.profile.result;
                this.chart = this.chartService.getChart(this.profile.chartId);

                this.charts = this.chartService.getAllCharts();
            });
        }

        public selectChart(chart)
        {
            this.chartSelectionVisible = false;
            this.chart = chart;
        }

        public onBitmapCreated(base64Image: string): void {

            this.base64Image = base64Image.replace('data:image/png;base64,', '');

            this.profileService.changeChart(this.profileId, this.chart.id, this.base64Image);

            //            alert(base64Image);
        }

        public delete(): void {

            var modalOptions = {
                closeButtonText: 'Cancel',
                actionButtonText: 'Delete Profile',
                headerText: 'Delete ' + this.profile.name + '?',
                bodyText: 'Are you sure you want to delete this profile?'
            };

            this.modalService.showModal({}, modalOptions).then(response => {
                this.profileService.delete(this.profileId).then(res => {
                    this.$window.location.href = "/profile/";
                });

            });
        }

    }

    angular.module('altidudeApp').controller('profileEditController', Controllers.ProfileEditController);
}





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

