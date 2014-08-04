var AssetDialog = Backbone.View.extend({

    $dialog: null,

    model: null,

    events : {
        'click #modal-add-asset #modal-add-asset-close': 'reset',
        'click #modal-add-asset-save' : 'save'
    },
    
    controls:{
        type: {
            photo: null,
            animation: null,
            youtube: null
        },
        title: null,
        desc: null,
        url: null,
        tags: null
    },

    initialize: function (options) {

        if(!options || !options.selector)
            throw "Selector is required."

        this.$el = $(options.selector);
        this.$dialog = this.$el.modal('hide');  

        var self = this;
        this.controls.type.photo = $("#asset_type_photo", this.$dialog);
        this.controls.type.animation = $("#asset_type_animation", this.$dialog);
        this.controls.type.youtube = $("#asset_type_youtube", this.$dialog);
        this.controls.type.getAssetId = function()
        {
            var radios = [
                self.controls.type.photo,
                self.controls.type.animation,
                self.controls.type.youtube
            ];

            for (var i = 0; i < radios.length; i++)
            {
                var $rb = radios[i];
                if ($rb.is(":checked"))
                    return parseInt($rb.val());
            }

            throw "No Asset Type Selected";
        };

        this.controls.title = $("#ad-title",  this.$dialog);
        this.controls.desc = $("#ad-desc",  this.$dialog);
        this.controls.url = $("#ad-url",  this.$dialog);
        
        var $tags = $("#ad-tags",  this.$dialog);
        this.controls.tags = $tags.tagsinput({
            'defaultText': 'Add a tag',
        })[0];

        var self = this;
        $('#modal-add-asset').on('hidden.bs.modal', function(){self.reset()});
    },

    cancel: function(){

        this.reset();

        $.trigger('ad.cancelled');
    },

    save: function() {

        var assettype = this.controls.type.getAssetId();
        var title = this.controls.title.val();
        var desc = this.controls.desc.val();
        var url = this.controls.url.val();

        var items = this.controls.tags.itemsArray;

        var tags = [];
        for(var i = 0 ; i < items.length; i++)
        {
            tags.push({
                "name" : items[i]
            });
        }

        debugger;
        var asset = {
            assettype: assettype,
            title: title,
            description: desc,
            url: url,
            tags: tags
        };

        if (!this.model) {
            this.model = new AssetModel(asset);
        }
        else
            this.model.set(asset);

        var self = this;
        this.model.save(null, {
            success: function(model, response) {
                self.$dialog.modal('hide');

                console.log('Save Asset');
            },
            error: function (model, response) {
                alert("There was an error saving your changes.");
            }
        });
    },

    reset: function(){
        
        this.model = null;

        this.controls.type.photo.prop("checked", true);
        this.controls.title.val('');
        this.controls.desc.val('');
        this.controls.url.val('');
        this.controls.tags.removeAll();

        console.log('Reset');
    },

    setup: function()
    {
        if(this.model)
       {
            var $rb;
            switch(this.model.get('assettype'))
            {
                case 1:
                    $rb = this.controls.type.photo;
                break;
                case 2:
                    $rb = this.controls.type.animation;
                break;
                case 3:
                    $rb = this.controls.type.youtube;
                break;
            }

            $rb.prop('checked', true);
            this.controls.title.val(this.model.get('title'));
            this.controls.desc.val(this.model.get('description'));
            this.controls.url.val(this.model.get('url'));

            var self = this;
            var arrTag = this.model.get('tags');
            if (arrTag && arrTag.length > 0)
            {
                _.each(arrTag, function (t)
                {
                    self.controls.tags.add(t.name);
                });
            }
       } 
    },

    show: function(m)
    {
        if(m)
        {
            this.model = m;
            this.setup();
        }

        this.$dialog.modal('show');
    },

    render: function () {


        return this;
    }
});