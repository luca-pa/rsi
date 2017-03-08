var tradingApp = angular.module('tradingApp', ['ui.router']);


tradingApp.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state({
        name: 'index',
        url: '/'
    });
    $stateProvider.state({
        name: 'ranking',
        url: '/ranking',
        templateUrl: 'app/ranking/ranking.html',
        controller: 'rankingCtrl'
    });
    $stateProvider.state({
        name: 'charts',
        url: '/charts',
        templateUrl: 'app/charts/charts.html',
        controller: 'chartsCtrl'
    });
    $stateProvider.state({
        name: 'all',
        url: '/all',
        templateUrl: 'app/ranking/ranking.html',
        controller: 'allCtrl'
    });
    $stateProvider.state({
        name: 'portfolio',
        url: '/portfolio',
        templateUrl: 'app/portfolio/portfolio.html'
    });
}]);


tradingApp.run(['$state', function ($state) {
    $state.transitionTo('index');
}]);
