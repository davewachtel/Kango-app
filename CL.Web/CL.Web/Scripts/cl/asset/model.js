var AssetModel = Backbone.Model.extend({
    defaults: {
        "id"            :   null,
        "assettype": "",
        "title"         :   "",
        "desc"          :   "",
        "url"           :   "",
        "tags"          :   []
    }
});