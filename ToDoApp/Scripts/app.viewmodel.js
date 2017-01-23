var AppViewModel = function (dataModel) {
    // Private state
    var self = this;
    var toDoUrl = '/api/ToDo';

    // Private operations
    function cleanUpLocation() {
        window.location.hash = "";

        if (typeof (history.pushState) !== "undefined") {
            history.pushState("", document.title, location.pathname);
        }
    }

    function ajaxHelper(url, method, data) {
        self.error(''); // Clear error message
        return $.ajax({
            type: method,
            url: url,
            headers: { 'Authorization': 'Bearer ' + dataModel.getAccessToken() },
            dataType: 'json',
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    }

    // Data
    self.Views = {
        Loading: {} // Other views are added dynamically by app.addViewModel(...).
    };
    self.dataModel = dataModel;

    // UI state
    self.view = ko.observable(self.Views.Loading);
    self.toDoItems = ko.observableArray();
    self.toDoItem = ko.observable();
    self.error = ko.observable();
    self.newToDo = {
        title: ko.observable(),
        isComplete: ko.observable()
    };
    self.loading = ko.computed(function () {
        return self.view() === self.Views.Loading;
    });

    // UI operations
    self.getAllTasks = function () {
        ajaxHelper(toDoUrl, 'GET').done(function (data) {
            self.toDoItems(data);
        });
    }

    self.getItem = function (item) {
        ajaxHelper(toDoUrl + item.ToDoItemId, 'GET').done(function (data) {
            self.toDoItem(data);
        });
    }

    self.addItem = function (item) {
        var task = {
            userId: $('#userId').val(),
            title: "New Task",
            isComplete: false
        };

        ajaxHelper(toDoUrl, 'POST', task).done(function (item) {
            self.toDoItems.push(item);
        });
    };

    self.removeItem = function (item) { self.toDoItems.destroy(item) };

    // Other operations
    self.addViewModel = function (options) {
        var viewItem = new options.factory(self, dataModel),
            navigator;

        // Add view to AppViewModel.Views enum (for example, app.Views.Home).
        self.Views[options.name] = viewItem;

        // Add binding member to AppViewModel (for example, app.home);
        self[options.bindingMemberName] = ko.computed(function () {
            if (!dataModel.getAccessToken()) {
                // The following code looks for a fragment in the URL to get the access token which will be
                // used to call the protected Web API resource
                var fragment = common.getFragment();

                if (fragment.access_token) {
                    // returning with access token, restore old hash, or at least hide token
                    window.location.hash = fragment.state || '';
                    setAccessToken(fragment.access_token);
                }
                //} else {
                //    // no token - so bounce to Authorize endpoint in AccountController to sign in or register
                //    window.location = "/Account/Authorize?client_id=web&response_type=token&state=" + encodeURIComponent(window.location.hash);
                //}
            }

            return self.Views[options.name];
        });

        if (typeof (options.navigatorFactory) !== "undefined") {
            navigator = options.navigatorFactory(self, dataModel);
        } else {
            navigator = function () {
                window.location.hash = options.bindingMemberName;
            };
        }

        // Add navigation member to AppViewModel (for example, app.NavigateToHome());
        self["navigateTo" + options.name] = navigator;
    };

    self.initialize = function () {
        Sammy().run();
        self.getAllTasks();
    }
};

var app = new AppViewModel(new AppDataModel());
app.initialize();

app.addViewModel({
    name: "TODO",
    bindingMemberName: "todo",
    factory: AppViewModel
});

// Activate Knockout
ko.validation.init({ grouping: { observable: false } });
ko.applyBindings(app);