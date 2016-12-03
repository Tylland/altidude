module Controllers {
    export class ProfileImportController {
        private isConnected: boolean = false;
        private activities: Array<any> = [];
        private chartId: string = '';

        private trackFile = null;
        private importedProfile = null;
        private isCreating: boolean = false;


        static $inject = ['$scope', '$window', 'Upload', 'stravaService'];
        constructor(private $scope: ng.IScope, private $window: any, private Upload: any, private stravaService: Services.StravaService) {
        }

        public init(chartId: any, userId: any): void {

            this.$scope.$watch(() => { return this.trackFile; }, (newValue, oldValue) => {
                if (newValue !== oldValue) {
                    this.importFromFile(this.trackFile);
                }
            });
            

            this.stravaService.isConnected().then((results: ng.IHttpPromiseCallbackArg<boolean>) => {
                this.isConnected = results.data;

                if (this.isConnected)
                    this.getActivities();
            });
        }

        public importFromFile(file) {
            this.Upload.upload({
                url: '/api/v1/profiles/upload/trackfile',
                method: 'POST',
                data: { file: file }
            }).then(response => {
                console.log('Success ' + response.config.data.file.name + 'uploaded. Response: ' + response.data);

                this.importedProfile = response.data;

                this.$window.location.href = "/profile/edit/" + this.importedProfile.id;

            }, resp => {
                console.log('Error status: ' + resp.status);
            }, evt => {
                var progressPercentage = 100.0 * evt.loaded / evt.total;
                console.log('progress: ' + progressPercentage + '% ' + evt.config.data.file.name);
            });
        };

        public getActivities() {
            this.stravaService.getAllActivities().then((results: ng.IHttpPromiseCallbackArg<any[]>) => {
                this.activities = results.data;

                this.activities.forEach(activity => activity.creating = false);

            });
        };

        public importFromActivity = function (activityId: string) {
            this.stravaService.importFromActivity(activityId).then(response => {
                this.importedProfile = response.data;

                this.$window.location.href = "/profile/edit/" + this.importedProfile.id;
            });
        };

        public createFromActivity = function (activity: any) {

            if (!this.isCreating) {
                this.isCreating = true;
                activity.creating = true;
                this.stravaService.importFromActivity(activity.id).then(response => {
                    this.importedProfile = response.data;

                    this.$window.location.href = "/profile/edit/" + this.importedProfile.id;
                });

                this.isCreating = false;
            }
        };

        public convertDate(dateToConvert): any {
            var convertedDateString = dateToConvert.toLocaleString();
            convertedDateString = convertedDateString.replace('at ', '');
            var convertedDate = new Date(convertedDateString);

            return convertedDate;
        };

        public disconnect(): void {
            this.stravaService.disconnect().then(() => {
                this.stravaService.isConnected().then((results: ng.IHttpPromiseCallbackArg<boolean>) => {
                    this.isConnected = results.data;

                    if (!this.isConnected)
                        this.activities = [];

                });
            });
        };
    }

    angular.module('altidudeApp').controller('profileImportController', Controllers.ProfileImportController);
}



//angular.module('altidudeApp')
//    .controller('profileImportController', ['$scope', '$rootScope', '$log', '$location', '$window', 'Upload', 'serviceConfig', 'stravaService', function ($scope, $rootScope, $log, $location, $window, Upload, serviceConfig, stravaService: Strava.StravaService) {

//        $scope.dateFormat = 'yyyy-MM-dd';
//        $scope.timeFormat = 'yyyy-MM-dd HH:mm:ss';

//        $scope.files = null;
//        $scope.trackFile = null;
//        $scope.importedProfile = null;

//        $scope.isConnected = false;
//        $scope.activities = [];
//        $scope.chartId = '';

//        init();

//        function init() {

//            var url = $location.$$url;

//            $scope.$watch('trackFile', function (newValue, oldValue, scope) {
//                if (newValue != undefined) {
//                    $scope.importFromFile($scope.trackFile);
//                }
//            });
            

//            stravaService.isConnected().then(response => {
//                $scope.isConnected = response.data;

//                if ($scope.isConnected)
//                    $scope.getActivities();
//            });
//        }

//        $scope.importFromFile = function (file) {
//            Upload.upload({
//                url: serviceConfig.altidudeApiBaseUri + 'api/v1/profiles/upload/trackfile',
//                method: 'POST',
//                data: { file: file }
//            }).then(response => {
//                console.log('Success ' + response.config.data.file.name + 'uploaded. Response: ' + response.data);

//                $scope.importedProfile = response.data;

//                $window.location.href = "/profile/edit/" + $scope.importedProfile.id;

//            }, resp => {
//                console.log('Error status: ' + resp.status);
//            }, evt => {
//                var progressPercentage = 100.0 * evt.loaded / evt.total;
//                console.log('progress: ' + progressPercentage + '% ' + evt.config.data.file.name);
//            });
//        };

//        $scope.connect = () => {
//            stravaService.connect().then(() => {
//                stravaService.isConnected().then((response) => {
//                    $scope.isConnected = response.data;
//                });
//            });
//        };

//        $scope.convertDate = (dateToConvert) => {
//            var convertedDateString = dateToConvert.toLocaleString();
//            convertedDateString = convertedDateString.replace('at ', '');
//            var convertedDate = new Date(convertedDateString);

//            return convertedDate;
//        };

//        $scope.disconnect = () => {
//            stravaService.disconnect().then(() => {
//                stravaService.isConnected().then((response) => {
//                    $scope.isConnected = response.data;
//                });
//            });
//        };
         

//        $scope.getActivities = function () {
//            stravaService.getAllActivities().then(response => {
//                $scope.activities = response.data;
//            });
//        };

//        $scope.importFromActivity = function (activityId: string) {
//            stravaService.importFromActivity(activityId).then(response => {
//                $scope.importedProfile = response.data;

//                $window.location.href = "/profile/edit/" + $scope.importedProfile.id;
//            });
//        };

//    }]);

