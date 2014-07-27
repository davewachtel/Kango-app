// Our overall **AppView** is the top-level piece of UI.
var AssetMain = Backbone.View.extend({

    // Instead of generating a new element, bind to the existing skeleton of
    // the App already present in the HTML.
    el: $("#tbl-assets"),

    // Delegated events for creating new items, and clearing completed ones.
    events: {
        'click btn-add-asset': 'add_asset'
    },

    source : null,

    // At initialization we bind to the relevant events on the `Todos`
    // collection, when items are added or changed. Kick things off by
    // loading any preexisting todos that might be saved in *localStorage*.
    initialize: function() {
        /*
        this.input = this.$("#new-todo");
        this.allCheckbox = this.$("#toggle-all")[0];

        this.listenTo(Todos, 'add', this.addOne);
        this.listenTo(Todos, 'reset', this.addAll);
        this.listenTo(Todos, 'all', this.render);

        this.footer = this.$('footer');
        this.main = $('#main');
        */
        var self = this;
        this.source = new AssetCollection();
        this.source.fetch({
            success: function () {
                self.render();
            }
        });
    },

    // Re-rendering the App just means refreshing the statistics -- the rest
    // of the app doesn't change.
    render: function() {
        this.source.each(function (s) {
            var item = new AssetItem({model: s});
            var html = item.render();

            $("#tbl-assets tbody", this.$el).append(html.$el);
        });

        return this;
    },

    add_asset: function ()
    {
        //Clear Dialog Form fields.
        $("#modal-add-asset form-control").val('');
    },

    /*
    // Add a single todo item to the list by creating a view for it, and
    // appending its element to the `<ul>`.
    addOne: function(todo) {
        var view = new TodoView({model: todo});
        this.$("#todo-list").append(view.render().el);
    },

    // Add all items in the **Todos** collection at once.
    addAll: function() {
        Todos.each(this.addOne, this);
    },

    // If you hit return in the main input field, create new **Todo** model,
    // persisting it to *localStorage*.
    createOnEnter: function(e) {
        if (e.keyCode != 13) return;
        if (!this.input.val()) return;

        Todos.create({title: this.input.val()});
        this.input.val('');
    },

    // Clear all done todo items, destroying their models.
    clearCompleted: function() {
        _.invoke(Todos.done(), 'destroy');
        return false;
    },

    toggleAllComplete: function () {
        var done = this.allCheckbox.checked;
        Todos.each(function (todo) { todo.save({'done': done}); });
    }
    */
});

// Finally, we kick things off by creating the **App**.
var App = new AssetMain;
