tradingApp.factory('tradingSvc', ['$http', function ($http) {

    return {
        ranking: function (datarif) {
            return $http.get('api/ranking/' + this.getDataString(datarif));
        },
        all: function (datarif, shorts, distributions) {
            return $http.get('api/allranking/' + this.getDataString(datarif) + '/' + shorts + '/' + distributions);
        },
        portfolio: function () {
            return $http.get('api/portfolio/');
        },
        portfolioPerformance: function () {
            return $http.get('api/portfolio/true');
        },
        savePortfolioItem: function (item) {
            return $http.post('api/portfolio/', JSON.stringify(item), { contentType: "application/json" });
        },
        saveBilancio: function (bilancio) {
            return $http.post('api/bilancio', JSON.stringify(bilancio), { contentType: "application/json" });
        },
        charts: function () {
            return $http.get('api/charts/');
        },
        updateQuotes: function () {
            return $http.get('api/quotes');
        },
        updateQuotesNextMonth: function () {
            return $http.get('api/quotes/true');
        },
        updatePortfolioQuotes: function () {
            return $http.get('api/quotes/false/true');
        },
        removeFromSelection: function (ticker) {
            return $http.delete('api/ranking/' + ticker);
        },
        addToSelection: function (ticker) {
            return $http.put('api/ranking/' + ticker);
        },
        getDataString: function (datarif) {
            if (datarif !== null) {
                var month = datarif.getMonth() + 1;
                var year = datarif.getFullYear();
                return year + '-' + month + '-1';
            }
            return '';
        },
        getDataLocaleString: function (datarif) {
            if (datarif !== null) {
                var day = datarif.getDate();
                if (day < 10)
                    day = '0' + day;
                var month = datarif.getMonth() + 1;
                if (month < 10)
                    month = '0' + month;
                var year = datarif.getFullYear();
                return day + '/' + month + '/' + year;
            }
            return '';
        },

        getTabellaTraderLinkUrl: function (data) {
            var url = "http://213.92.13.40/phprealtime/index.php?modo=t&lang=it&appear=n";

            for (var i = 0; i < data.length; i++) {
                url += "&id" + (i + 1) + "=" + data[i].isin;
            }
            return url + "&u=43705&p=Z58H50";
        }
    };
}]);
