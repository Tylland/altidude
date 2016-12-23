var Directives;
(function (Directives) {
    var StaticProfileChart = (function () {
        function StaticProfileChart(profileService, chartService) {
            this.profileService = profileService;
            this.chartService = chartService;
        }
        StaticProfileChart.$inject = ['profileService', 'chartService'];
        return StaticProfileChart;
    })();
    Directives.StaticProfileChart = StaticProfileChart;
})(Directives || (Directives = {}));
///// <reference path="../../typings/angularjs/angular.d.ts" />
///// <reference path="SimpleClickCountService.ts" />
//class SimpleDirectiveController {   
//    // This is the initializer that we will pass to AngularJS.
//    public static initializer: ng.IDirectiveFactory = (simpleClickCountService: SimpleClickCountService) => {
//        return {
//            controller: () => new SimpleDirectiveController(simpleClickCountService),
//            controllerAs: 'simpleDirective',
//            scope: {},
//            templateUrl: 'SimpleStuff/SimpleDirective.html'
//        };
//    };
//    // Keep our Angular dependencies as a static variable
//    public static AngularDependencies: any[] = [
//        'SimpleClickCountService',
//        SimpleDirectiveController.initializer];
//    private _simpleClickCountService: SimpleClickCountService
//    public constructor(simpleClickCountService: SimpleClickCountService) {
//        this._simpleClickCountService = simpleClickCountService;
//    }
//    public message: string = 'Hello from SimpleDirective!';
//    public getButtonText(): string {
//        var count = this._simpleClickCountService.GetClickCount();
//        if (count == 0) return 'Click me!';
//        else return 'You clicked me ' + count + ' times!';
//    }
//    public clickMe(): void {
//        this._simpleClickCountService.Increment();
//    }
//}
//myApp.directive('myAppSimpleDirective', SimpleDirectiveController.AngularDependencies);
//angular.module('altidudeApp').directive('profileChart', ['$http', '$window', function ($http, $window) {
//    var chartTypeTemplateUrl;
//    var templateLoaded = false;
//    return {
//        restrict: 'C', //attribute or element
//        scope: {
//            profile: '=',
//            result: '=',
//            chart: '=',
//        },
//        controller: function () {
//        },
//        link: function (scope, el, attr, ngModel) {
//            console.debug(scope);
//            scope.$watch('chart', function (newValue, oldValue) {
//                if (newValue != undefined) {
//                    $http.get(scope.chart.templateUrl).success(function (templateContent) {
//                        el.empty();
//                        el.append(templateContent);
//                        templateLoaded = true;
//                        render(el.width());
//                    });
//                }
//            });
//            scope.$watch('profile', function (newValue, oldValue) {
//                if (newValue != undefined) {
//                    render(el.width());
//                }
//            });
//            scope.$watch('result', function (newValue, oldValue) {
//                if (newValue != undefined) {
//                    render(el.width());
//                }
//            });
//            angular.element($window).bind('resize', function () {
//                render(el.width());
//            });
//            function render(width) {
//                if (templateLoaded && scope.chart != undefined && scope.profile != undefined && scope.result != undefined)
//                    scope.chart.render(scope.profile, scope.result, width);
//            }
//        }
//    };
//}]);
//# sourceMappingURL=StaticProfileChart.js.map