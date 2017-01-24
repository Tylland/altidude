module Services {
    export class ChartEntry {
        constructor(public name: string, public textTemplate: string, public chart: ProfileChart.Chart)
        {
        }
    }

    export class ChartService{
        constructor() {
        }

        public getAllCharts(): ProfileChart.Chart[] {
            var charts: Array<ProfileChart.Chart> = new Array<ProfileChart.Chart>();

            charts.push(new ProfileChart.ForestChart(new ProfileChart.ChartRenderingSettings('33F0009E-ABC8-4CD6-8799-979B7BF5CA6F', 'The Forest', false, false, false)));
//            charts.push(new ProfileChart.TestChart());
            charts.push(new ProfileChart.SimplySunshineChart());
            charts.push(new ProfileChart.ConnectingDotsChart());
            //charts.push(new ProfileChart.MountainSilhouetteChart());
            charts.push(new ProfileChart.GiroItaliaChart());
            charts.push(new ProfileChart.NorthernZodiacChart());
            charts.push(new ProfileChart.SouthernZodiacChart());


            return charts;
        };

        public getChart(chartId: string): ProfileChart.Chart {
            var charts: ProfileChart.Chart[] = this.getAllCharts();

            for (var i = 0; i < charts.length; i++)
                if (charts[i].id.toUpperCase() === chartId.toUpperCase())
                    return charts[i];


            return new ProfileChart.SimplySunshineChart();
        }

    }

    angular.module('altidudeApp').service('chartService', Services.ChartService);
}