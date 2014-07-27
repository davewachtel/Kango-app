var AssetItemAssetType = Backbone.View.extend({

    tagName: "div",

    className: "fa fa-2x",

    initialize: function() {
        this.listenTo(this.model, "change", this.render);
    },

    render: function () {

        this.$el.removeClass("fa-photo fa-video-camera fa-youtube-square");
        if (this.model)
        {    
            var cssClass;
            switch(this.model.get('assettype'))
            {
                case 1: //Photo
                    cssClass = "fa-photo";
                    break;
                case 2: //Animation
                    cssClass = "fa-video-camera";
                    break;
                case 3: //YouTube
                    cssClass = "fa-youtube-square";
                    break;
            }
            if(cssClass)
                this.$el.addClass(cssClass);
        }
       
        return this;
    }

});