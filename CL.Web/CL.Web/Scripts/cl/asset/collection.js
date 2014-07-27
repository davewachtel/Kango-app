var AssetCollection = Backbone.Collection.extend({
    defaults: {
        page: 1,
        size: 25,
        totalcount: 0
    },
    model: AssetModel,

    url: '/services/api/asset',

    parse: function (resp) {
        if (resp) {
            this.total = resp.totalcount;
            return resp.data;
        }

        return null;
    },

    sync: function (method, model, options) {
        if (method == "read")
            options.url = this.url + "?page=1&size=10000"; //HACK
       
        Backbone.sync(method, model, options);
    }
});