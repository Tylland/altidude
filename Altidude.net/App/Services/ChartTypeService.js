var Services;
(function (Services) {
    var ChartTypeService = (function () {
        function ChartTypeService($http, serviceConfig) {
            this.$http = $http;
            this.serviceConfig = serviceConfig;
            this.profilesUrlBase = 'api/v1/charttypes/';
        }
        ChartTypeService.prototype.getUserChartTypes = function () {
            var url = this.serviceConfig.altidudeApiBaseUri + this.profilesUrlBase + 'user';
            return this.$http.get(url);
        };
        ;
        ChartTypeService.$inject = ['$http', 'serviceConfig'];
        return ChartTypeService;
    }());
    Services.ChartTypeService = ChartTypeService;
    angular.module('altidudeApp').service('chartTypeService', Services.ChartTypeService);
})(Services || (Services = {}));
//# sourceMappingURL=ChartTypeService.js.map