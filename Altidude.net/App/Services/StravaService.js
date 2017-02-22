var Services;
(function (Services) {
    var StravaService = (function () {
        function StravaService($http, serviceConfig) {
            this.$http = $http;
            this.serviceConfig = serviceConfig;
            this.stravaUrlBase = 'api/v1/strava/';
        }
        StravaService.prototype.connect = function () {
            var url = this.serviceConfig.altidudeApiBaseUri + this.stravaUrlBase + 'connect';
            return this.$http.get(url);
        };
        StravaService.prototype.isConnected = function () {
            var url = this.serviceConfig.altidudeApiBaseUri + this.stravaUrlBase + 'isconnected';
            return this.$http.get(url);
        };
        StravaService.prototype.disconnect = function () {
            var url = this.serviceConfig.altidudeApiBaseUri + this.stravaUrlBase + 'disconnect';
            return this.$http.get(url);
        };
        StravaService.prototype.getActivities = function (from, tom) {
            var url = this.serviceConfig.altidudeApiBaseUri + this.stravaUrlBase + 'activities';
            return this.$http.get(url);
        };
        ;
        StravaService.prototype.getAllActivities = function () {
            var url = this.serviceConfig.altidudeApiBaseUri + this.stravaUrlBase + 'activities/all';
            return this.$http.get(url);
        };
        ;
        StravaService.prototype.importFromActivity = function (activityId) {
            var url = this.serviceConfig.altidudeApiBaseUri + this.stravaUrlBase + 'activities/' + activityId + '/profile/import';
            return this.$http.get(url);
        };
        ;
        StravaService.$inject = ['$http', 'serviceConfig'];
        return StravaService;
    }());
    Services.StravaService = StravaService;
    angular.module('altidudeApp').service('stravaService', StravaService);
})(Services || (Services = {}));
//angular.module('altidudeApp')
//    .factory('stravaFactory', ['$http', '$rootScope', function ($http, $rootScope) {
//        var stravaUrlBase = '/api/v1/strava/';
//        var courseUrlBase = '/api/v1/strava/activities';
//        var factory = {};
//        factory.getActivities = function (from, tom) {
//            var url = stravaUrlBase + 'activities';
//            //    courseListItemUrlBase + '?filterIsPublic=' + filterIsPublic + '&filterIsActive=' + filterIsActive + '&filterAuthenticatedUser=' + filterAuthenticatedUser;
//            return $http.get(url);
//        };
//        return factory;
//    }]); 
//# sourceMappingURL=StravaService.js.map