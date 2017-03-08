window.tradingApp = new (Backbone.Router.extend({
    routes: { '': 'index' },
    initialize: function(){
        this.rankingItems = new RankingItems();
        this.rankingView = new RankingView({
            collection: this.rankingItems
        });
        this.rankingView.render();
    },
    index: function () {

    },
    start: function () {
        Backbone.history.start();
    }
}));