var Controllers;
(function (Controllers) {
    var ProfileImportController = (function () {
        function ProfileImportController($scope, $window, Upload, stravaService) {
            this.$scope = $scope;
            this.$window = $window;
            this.Upload = Upload;
            this.stravaService = stravaService;
            this.isConnected = false;
            this.activities = [];
            this.chartId = '';
            this.trackFile = null;
            this.importedProfile = null;
            this.isCreating = false;
            this.importFromActivity = function (activityId) {
                var _this = this;
                this.stravaService.importFromActivity(activityId).then(function (response) {
                    _this.importedProfile = response.data;
                    _this.$window.location.href = "/profile/edit/" + _this.importedProfile.id;
                });
            };
            this.createFromActivity = function (activity) {
                var _this = this;
                if (!this.isCreating) {
                    this.isCreating = true;
                    activity.creating = true;
                    this.stravaService.importFromActivity(activity.id).then(function (response) {
                        _this.importedProfile = response.data;
                        _this.$window.location.href = "/profile/edit/" + _this.importedProfile.id;
                    });
                    this.isCreating = false;
                }
            };
        }
        ProfileImportController.prototype.init = function (chartId, userId) {
            var _this = this;
            this.$scope.$watch(function () { return _this.trackFile; }, function (newValue, oldValue) {
                if (newValue !== oldValue) {
                    _this.importFromFile(_this.trackFile);
                }
            });
            this.stravaService.isConnected().then(function (results) {
                _this.isConnected = results.data;
                if (_this.isConnected)
                    _this.getActivities();
            });
        };
        ProfileImportController.prototype.importFromFile = function (file) {
            var _this = this;
            this.Upload.upload({
                url: '/api/v1/profiles/upload/trackfile',
                method: 'POST',
                data: { file: file }
            }).then(function (response) {
                console.log('Success ' + response.config.data.file.name + 'uploaded. Response: ' + response.data);
                _this.importedProfile = response.data;
                _this.$window.location.href = "/profile/edit/" + _this.importedProfile.id;
            }, function (resp) {
                console.log('Error status: ' + resp.status);
            }, function (evt) {
                var progressPercentage = 100.0 * evt.loaded / evt.total;
                console.log('progress: ' + progressPercentage + '% ' + evt.config.data.file.name);
            });
        };
        ;
        ProfileImportController.prototype.getActivities = function () {
            var _this = this;
            this.stravaService.getAllActivities().then(function (results) {
                _this.activities = results.data;
                _this.activities.forEach(function (activity) { return activity.creating = false; });
            });
        };
        ;
        ProfileImportController.prototype.convertDate = function (dateToConvert) {
            var convertedDateString = dateToConvert.toLocaleString();
            convertedDateString = convertedDateString.replace('at ', '');
            var convertedDate = new Date(convertedDateString);
            return convertedDate;
        };
        ;
        ProfileImportController.prototype.disconnect = function () {
            var _this = this;
            this.stravaService.disconnect().then(function () {
                _this.stravaService.isConnected().then(function (results) {
                    _this.isConnected = results.data;
                    if (!_this.isConnected)
                        _this.activities = [];
                });
            });
        };
        ;
        ProfileImportController.$inject = ['$scope', '$window', 'Upload', 'stravaService'];
        return ProfileImportController;
    })();
    Controllers.ProfileImportController = ProfileImportController;
    angular.module('altidudeApp').controller('profileImportController', Controllers.ProfileImportController);
})(Controllers || (Controllers = {}));
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
//# sourceMappingURL=ProfileImportController.js.map