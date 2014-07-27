var AssetItem = Backbone.View.extend({
    tagName: "tr",

    events: {
        
        "click .asset-edit": "edit",
        "click .asset-remove": "destroy"
    },

    template: _.template($("#asset-item").html()),

    initialize: function ()
    {
        this.listenTo(this.model, "change", this.render);
    },

    edit:function(){
        console.log("Edit: " + this.model.get('id'));
    },

    destroy: function () {
        
        console.log("Delete: " + this.model.get('id'));
        this.model.destroy();

    },

    render: function () {
        var assetType = new AssetItemAssetType({ model: this.model });
        var $atHtml = assetType.render().$el;

        this.$el.html(this.template(this.model.toJSON()));
        $("td:first-child", this.$el).append($atHtml);

        return this;
    }
});