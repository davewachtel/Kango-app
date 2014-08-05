var AssetItem = Backbone.View.extend({
    tagName: "tr",

    dialog: null,

    events: {
        "click .asset-edit": "edit",
        "click .asset-remove": "destroy"
    },

    template: _.template($("#asset-item").html()),

    initialize: function (options)
    {
        if(options && options.dialog)
            this.dialog = options.dialog;

        this.listenTo(this.model, "change", this.render);
    },

    edit: function() {

        if(this.dialog)
        {
            var self = this;
            this.dialog.show(this.model);
        }
    },

    destroy: function () {
        if (confirm("Are you sure you'd like to delete this item?"))
        {
            var self = this;

            this.model.destroy({
                success: function () {
                    self.render();
                    console.log("Delete: " + self.model.get('id'));
                }
            });
        }

    },

    render: function () {

        var assetType = new AssetItemAssetType({ model: this.model });
        var $atHtml = assetType.render().$el;

        this.$el.html(this.template(this.model.toJSON()));
        $("td:first-child", this.$el).append($atHtml);

        return this;
    }
});