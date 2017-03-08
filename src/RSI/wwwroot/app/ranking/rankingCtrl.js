tradingApp.controller('rankingCtrl', ['$scope', 'tradingSvc', 'rankingSvc', function ($scope, tradingSvc, rankingSvc) {
    var self = this;

    $scope.ranking = function (datarif) {
        $scope.loading = true;

        if (datarif == null) {
            datarif = rankingSvc.addMonths(new Date(), 1);
        }
        tradingSvc.ranking(datarif).success(function (data) {
            $scope.etfs = data;
            $scope.dataRif = datarif;
            $scope.dataPrecedente = rankingSvc.addMonths(datarif, -1);
            $scope.dataSuccessiva = rankingSvc.addMonths(datarif, 1);
            $scope.tabellaTraderLinkUrl = tradingSvc.getTabellaTraderLinkUrl(data);
            $scope.loading = false;
        });
    };

    $scope.title = 'Ranking';
    $scope.selection = true;
    $scope.ranking();

    $scope.meseAnnoPrecedente = function () {
        if ($scope.dataPrecedente != null) {
            return rankingSvc.getMeseAnnoString($scope.dataPrecedente);
        }
    };
    $scope.meseAnnoSuccessivo = function () {
        if ($scope.dataSuccessiva != null)
            return rankingSvc.getMeseAnnoString($scope.dataSuccessiva);
    };
    $scope.meseAnnoCorrente = function () {
        if ($scope.dataRif != null)
            return rankingSvc.getMeseAnnoString($scope.dataRif);
    };

    $scope.aggiornaQuote = function () {
        $scope.loading = true;
        tradingSvc.updateQuotes().success(function (data) {
            $scope.ranking($scope.dataRif);
            $scope.quoteAggiornate = ' (' + data + ')';
        });
    };

    $scope.aggiornaQuoteMeseSuccessivo = function () {
        $scope.loading = true;
        tradingSvc.updateQuotesNextMonth().success(function (data) {
            $scope.ranking($scope.dataRif);
            $scope.quoteAggiornateMeseSuccessivo = ' (' + data + ')';
        });
    };

    $scope.removeFromSelection = function (etf) {
        tradingSvc.removeFromSelection(etf.ticker).then(function (data) {
            $scope.ranking();
        });
    };
}]);
