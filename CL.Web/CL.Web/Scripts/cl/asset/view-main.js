// Our overall **AppView** is the top-level piece of UI.
var AssetMain = Backbone.View.extend({

    // Instead of generating a new element, bind to the existing skeleton of
    // the App already present in the HTML.
    el: $("#asset-container"),

    // Delegated events for creating new items, and clearing completed ones.
    events:
    {
        'click #btn-add-asset' : 'show_add_asset_dialog'
    },

    collection: null,

    dialog: null,

    initialize: function() {
        var self = this;
        
        this.dialog = new AssetDialog({
            selector: "#modal-add-asset"
        });

        this.collection = new AssetCollection();
        this.collection.fetch({
            success: function () {
                self.render();
            }
        });
    },

    show_add_asset_dialog : function(){
        this.dialog.show(null);
    },

    // Re-rendering the App just means refreshing the statistics -- the rest
    // of the app doesn't change.
    render: function() {
        var self = this;
        this.collection.each(function (s) {
            var item = new AssetItem({
                model: s, 
                dialog: self.dialog
            });

            var html = item.render();
            $("#tbl-assets tbody", this.$el).append(html.$el);
        });

        return this;
    }
});

// Finally, we kick things off by creating the **App**.
var App = new AssetMain;