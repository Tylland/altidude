var Services;
(function (Services) {
    var ChartEntry = (function () {
        function ChartEntry(name, textTemplate, chart) {
            this.name = name;
            this.textTemplate = textTemplate;
            this.chart = chart;
        }
        return ChartEntry;
    })();
    Services.ChartEntry = ChartEntry;
    var ChartService = (function () {
        function ChartService() {
        }
        ChartService.prototype.getAllCharts = function () {
            var charts = new Array();
            charts.push(new ProfileChart.ForestChart(new ProfileChart.ChartRenderingSettings('33F0009E-ABC8-4CD6-8799-979B7BF5CA6F', 'The Forest', false, false, false)));
            //            charts.push(new ProfileChart.TestChart());
            charts.push(new ProfileChart.SimplySunshineChart());
            charts.push(new ProfileChart.ConnectingDotsChart());
            //charts.push(new ProfileChart.MountainSilhouetteChart());
            charts.push(new ProfileChart.GiroItaliaChart());
            return charts;
        };
        ;
        ChartService.prototype.getChart = function (chartId) {
            var charts = this.getAllCharts();
            for (var i = 0; i < charts.length; i++)
                if (charts[i].id.toUpperCase() === chartId.toUpperCase())
                    return charts[i];
            return new ProfileChart.SimplySunshineChart();
        };
        return ChartService;
    })();
    Services.ChartService = ChartService;
    angular.module('altidudeApp').service('chartService', Services.ChartService);
})(Services || (Services = {}));
//# sourceMappingURL=ChartService.js.map