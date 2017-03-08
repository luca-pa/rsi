tradingApp.controller('chartsCtrl', ['$scope', '$timeout', 'tradingSvc', function ($scope, $timeout, tradingSvc) {

    $scope.loading = true;

    tradingSvc.charts().success(function (data) {
        $scope.etfs = data;

        $timeout(renderCharts, 1000);
    });


    var renderCharts = function () {
        for (var i = 0; i < $scope.etfs.length; i++) {
            $scope.renderChart($scope.etfs[i]);
        }
        $scope.loading = false;
    };

    $scope.renderChart = function (etf) {

        var times = _.union(['x'], etf.times);
        var smas = _.union(['sma'], etf.smas);
        var values = _.union([etf.ticker], etf.values);

        c3.generate({
            bindto: '#chart-' + etf.ticker,
            data: {
                x: 'x',
                columns: [times, smas, values]
            },
            axis: {
                x: {
                    type: 'timeseries',
                    tick: {
                        format: '%m-%Y'
                    }
                }
            }

        });

    };
}]);
