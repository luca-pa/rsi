tradingApp.factory('tradingSvc', ['$http', function ($http) {

    return {
        ranking: function (datarif) {
            var url = 'api/ranking/' + this.getDataString(datarif);
            return $http.get(url);
        },
        all: function (datarif, shorts, distributions) {
            var url = 'api/allranking/' + this.getDataString(datarif) + '/' + shorts + '/' + distributions;
            return $http.get(url);
        },
        portfolio: function () {
            var url = 'api/portfolio/';
            return $http.get(url);
        },
        savePortfolioItem: function (item) {
            var url = 'api/portfolio/';
            return $http.post(url, JSON.stringify(item), { contentType: "application/json" });
        },
        saveBilancio: function (bilancio) {
            var url = 'api/bilancio';
            return $http.post(url, JSON.stringify(bilancio), { contentType: "application/json" });
        },
        charts: function () {
            var url = 'api/charts/';
            return $http.get(url);
        },
        updateQuotes: function () {
            var url = 'api/quotes/false';
            return $http.get(url);
        },
        updateQuotesNextMonth: function () {
            var url = 'api/quotes/true';
            return $http.get(url);
        },
        removeFromSelection: function (ticker) {
            var url = 'api/ranking/' + ticker;
            return $http.delete(url);
        },
        addToSelection: function (ticker) {
            var url = 'api/ranking/' + ticker;
            return $http.put(url);
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
