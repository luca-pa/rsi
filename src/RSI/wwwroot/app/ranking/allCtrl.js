tradingApp.controller('allCtrl', ['$scope', '$state', 'tradingSvc', 'rankingSvc', function ($scope, $state, tradingSvc, rankingSvc) {
    var self = this;

    $scope.ranking = function (datarif) {
        $scope.loading = true;

        if (datarif == null) {
            datarif = rankingSvc.addMonths(new Date(), 1);
        }
        tradingSvc.all(datarif, $scope.shorts).success(function (data) {
            $scope.etfs = data;
            $scope.dataRif = datarif;
            $scope.dataPrecedente = rankingSvc.addMonths(datarif, -1);
            $scope.dataSuccessiva = rankingSvc.addMonths(datarif, 1);
            $scope.loading = false;
        });
    };

    $scope.title = 'Ranking (All ETFs)';
    $scope.all = true;
    $scope.shorts = false;
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

    $scope.addToSelection = function (etf) {
        tradingSvc.addToSelection(etf.ticker).then(function (data) {
            $state.go('ranking');
        });
    };

}]);
