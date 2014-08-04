var AssetModel = Backbone.Model.extend({
    urlRoot: CL.url + '/api/asset',
    defaults: {
        "id"            :   null,
        "assettype": "",
        "title"         :   "",
        "description"          :   "",
        "url"           :   "",
        "tags"          :   []
    }
});