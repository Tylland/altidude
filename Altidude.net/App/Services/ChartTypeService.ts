module Services {
    export class ChartTypeService {
        private profilesUrlBase: string = 'api/v1/charttypes/';

        static $inject = ['$http', 'serviceConfig'];
        constructor(private $http: ng.IHttpService, private serviceConfig) {
        }

        public getUserChartTypes() : ng.IPromise<any> {
            var url = this.serviceConfig.altidudeApiBaseUri + this.profilesUrlBase + 'user';

            return this.$http.get(url);
        };
    }

    angular.module('altidudeApp').service('chartTypeService', Services.ChartTypeService);
}