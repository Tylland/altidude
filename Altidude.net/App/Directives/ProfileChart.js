angular.module('altidudeApp').directive('profileChart', ['$http', '$window', function ($http, $window) {
        var chartTypeTemplateUrl;
        var templateLoaded = false;
        return {
            restrict: 'C',
            scope: {
                profile: '=',
                result: '=',
                chart: '=',
                createBitmap: '=',
                onBitmapCreated: '&'
            },
            controller: function () {
            },
            link: function (scope, el, attr, ngModel) {
                console.debug(scope);
                scope.$watch('chart', function (newValue, oldValue) {
                    if (newValue != undefined) {
                        $http.get(scope.chart.templateUrl).success(function (templateContent) {
                            el.empty();
                            el.append(templateContent);
                            templateLoaded = true;
                            render(el.width());
                        });
                    }
                });
                scope.$watch('profile', function (newValue, oldValue) {
                    if (newValue != undefined) {
                        render(el.width());
                    }
                });
                scope.$watch('result', function (newValue, oldValue) {
                    if (newValue != undefined) {
                        render(el.width());
                    }
                });
                angular.element($window).bind('resize', function () {
                    render(el.width());
                });
                function convertToBitmap() {
                    var svg = el[0].children[0];
                    svg.toDataURL("image/png", {
                        callback: function (data) {
                            scope.onBitmapCreated({ base64Image: data });
                        }
                    });
                }
                function render(width) {
                    if (templateLoaded && scope.chart != undefined && scope.profile != undefined && scope.result != undefined) {
                        scope.chart.render(scope.profile, scope.result, width);
                        // if (scope.createBitmap)
                        convertToBitmap();
                    }
                }
                var width = el.width();
                var height = (width / 1900) * 1000;
                var loadingDiv = '<div class="loading-chart" style="width:' + width + 'px;height:' + height + 'px;line-height: ' + height + 'px">Blank paper has always inspired us...</div>';
                el.empty();
                el.append(loadingDiv);
            }
        };
    }]);
//# sourceMappingURL=ProfileChart.js.map