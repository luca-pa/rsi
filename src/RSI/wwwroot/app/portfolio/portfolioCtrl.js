tradingApp.controller('portfolioCtrl', ['$scope', 'tradingSvc', function ($scope, tradingSvc) {

    $scope.load = function () {
        $scope.loading = true;

        tradingSvc.portfolio().success(function (data) {
            $scope.portfolio = data;
            $scope.tabellaTraderLinkUrl = tradingSvc.getTabellaTraderLinkUrl(data.items);
            $scope.loading = false;
        });
    };

    $scope.load();

    $scope.savePortfolioItem = function (item) {
        console.log(item);
        tradingSvc.savePortfolioItem(item).then(function () {
            $scope.load();
        });
    };

    $scope.newItem = getNewItem();

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

    $scope.UpdatePortfolioQuotes = function () {
        $scope.loading = true;

        tradingSvc.UpdatePortfolioQuotes().success(function (data) {
            $scope.load();
            $scope.quoteAggiornate = ' (' + data + ')';
        })
    };

    function getNewItem() {
        return {
            data: tradingSvc.getDataLocaleString(new Date()),
            ticker: '',
            prezzoAcquisto: '',
            quantita: ''
        };
    }
}]);
