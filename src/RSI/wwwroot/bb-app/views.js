window.RankingItemView = Backbone.View.extend({
    initialize: function () {
        this.model.on('change', this.render, this);
    },
    events: {
        'change input[type=checkbox]': 'toggle'
    },
    toggle: function(){
        this.model.toggle();
    },
    template: _.template(''),
    render: function () {
        this.$el.html(this.template(this.model.toJSON()));
        return this;
    },
    className: ''
});

window.RankingView = Backbone.View.extend({
    initialize: function () {
        this.collection.on('add', this.addOne, this);
        this.collection.on('reset', this.addAll, this);
        this.collection.on('destroy', this.render, this);
    },
    addOne: function (rankingItem) {
        var itemView = new RankingItemView({
            model: rankingItem
        });
        this.$el.append(itemView.render().$el);
    },
    addAll: function () {
        this.$el.empty();
        this.collection.forEach(this.addOne, this);
    },
    render: function () {
        this.addAll();
        return this;
    }
});