module Services {
    export class StravaService {
        private stravaUrlBase: string = 'api/v1/strava/';

        static $inject = ['$http', 'serviceConfig'];
        constructor(private $http: ng.IHttpService, private serviceConfig) {
        }

        public connect(): ng.IHttpPromise<{}> {
            var url = this.serviceConfig.altidudeApiBaseUri + this.stravaUrlBase + 'connect';

            return this.$http.get(url);
        } 

        public isConnected() 
        {
            var url = this.serviceConfig.altidudeApiBaseUri + this.stravaUrlBase + 'isconnected';

            return this.$http.get(url);
        } 

        public disconnect(): ng.IHttpPromise<{}> {
            var url = this.serviceConfig.altidudeApiBaseUri + this.stravaUrlBase + 'disconnect';

            return this.$http.get(url);
        } 

        public getActivities(from: Date, tom: Date) {

            var url = this.serviceConfig.altidudeApiBaseUri + this.stravaUrlBase + 'activities';

            return this.$http.get(url);
        };

        public getAllActivities() {

            var url = this.serviceConfig.altidudeApiBaseUri + this.stravaUrlBase + 'activities/all';

            return this.$http.get(url);
        };

        public importFromActivity(activityId: string) {

            var url = this.serviceConfig.altidudeApiBaseUri + this.stravaUrlBase + 'activities/' + activityId + '/profile/import';

            return this.$http.get(url);
        };
    }


    angular.module('altidudeApp').service('stravaService', StravaService);
}

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