tradingApp.factory('rankingSvc', [function () {

    return {

        monthNames: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],

        addMonths: function (datarif, mesi) {
            var mese = datarif.getMonth() + mesi;
            var anno = datarif.getFullYear();
            if (mese === 12) {
                mese = 0;
                anno = anno + 1;
            }
            else if (mese === -1) {
                mese = 11;
                anno = anno - 1;
            }
            var data = new Date(anno, mese, 1);
            //console.log(datarif.toLocaleDateString() + ', ' + datarif.getMonth() + ', ' + mesi + ' -- ' + data.toLocaleDateString());
            return data;
        },

        getMeseAnnoString: function (data) {
            var displayData = this.addMonths(data, -1);
            var mese = this.monthNames[displayData.getMonth()].substring(0,3);
            var anno = displayData.getFullYear();
            return mese + ' / ' + anno;
        }

    };
}]);
