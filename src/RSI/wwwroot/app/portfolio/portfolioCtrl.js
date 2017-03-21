tradingApp.controller('portfolioCtrl', ['$scope', '$timeout', 'tradingSvc', function ($scope, $timeout, tradingSvc) {

    $scope.load = function () {
        $scope.loading = true;

        tradingSvc.portfolio().success(function (data) {
            $scope.portfolio = data;
            $scope.tabellaTraderLinkUrl = tradingSvc.getTabellaTraderLinkUrl(data.items);
            $scope.loading = false;
        });
    };

    $scope.load();

    $scope.showChart = function () {
        $scope.loadingChart = true;
        $timeout(renderChart, 200);
    };


    $scope.savePortfolioItem = function (item) {
        console.log(item);
        tradingSvc.savePortfolioItem(item).then(function () {
            $scope.load();
        });
    };

    $scope.addPortfolioItem = function () {
        console.log($scope.newItem);
        tradingSvc.savePortfolioItem($scope.newItem).then(function () {
            $scope.load();
            $scope.newItem = getNewItem();
        });
    };

    $scope.saveBilancio = function () {
        var bilancio = {
            data: $scope.portfolio.dataBilancio,
            invested: $scope.portfolio.totaleInvestito,
            cash: $scope.portfolio.cash,
            minusvalenze: $scope.portfolio.minusvalenze
        };
        console.log(bilancio);
        tradingSvc.saveBilancio(bilancio).then(function () {
            $scope.load();
        });
    };

    $scope.updatePortfolioQuotes = function () {
        $scope.loadingChart = true;

        tradingSvc.updatePortfolioQuotes().success(function (data) {
            $scope.quoteAggiornate = ' (' + data + ')';
            $scope.showChart();
        })
    };

    var getNewItem = function () {
        return {
            data: tradingSvc.getDataLocaleString(new Date()),
            ticker: '',
            prezzoAcquisto: '',
            quantita: ''
        };
    }

    $scope.newItem = getNewItem();

    var renderChart = function () {

        tradingSvc.portfolioPerformance().success(function (data) {
            var times = _.union(['x'], data.times);
            var values = _.union(['%'], data.values);

            $timeout(() => $scope.loadingChart = false, 500)

            c3.generate({
                bindto: '#performance-chart',
                data: {
                    x: 'x',
                    columns: [times, values]
                },
                axis: {
                    x: {
                        type: 'timeseries',
                        tick: {
                            format: '%d-%m-%Y'
                        }
                    }
                },
                grid: {
                    y: {
                        lines: [
                            {value: 0}
                        ]
                    }
                },
                type: 'spline'
            });
        });
    };

}]);
