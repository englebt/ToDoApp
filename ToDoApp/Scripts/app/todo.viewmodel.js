function ToDoViewModel(app, dataModel) {
    var self = this;
    var toDoUrl = '/api/ToDo';
    
    self.dataModel = dataModel;

    self.toDoItems = ko.observableArray();
    self.toDoItem = ko.observable();
    self.error = ko.observable();
    self.newToDo = {
        title: ko.observable(),
        isComplete: ko.observable()
    };

    self.getAllTasks = function () {
        app.ajaxHelper(toDoUrl, 'GET').done(function (data) {
            self.toDoItems(data);
        });
    }

    self.getItem = function (item) {
        app.ajaxHelper(toDoUrl + item.ToDoItemId, 'GET').done(function (data) {
            self.toDoItem(data);
        });
    }

    self.addItem = function (item) {
        var task = {
            userId: $('#userId').val(),
            title: "New Task",
            isComplete: false
        };

        app.ajaxHelper(toDoUrl, 'POST', task).done(function (item) {
            self.toDoItems.push(item);
        });
    };

    self.removeItem = function (item) { self.toDoItems.destroy(item) };

    //Sammy(function () {
    //    this.get('#todo', function () {
    //        // Make a call to the protected Web API by passing in a Bearer Authorization Header
    //        $.get({
    //            url: app.dataModel.toDoList,
    //            contentType: "application/json; charset=utf-8",
    //            headers: {
    //                'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
    //            },
    //            done: function (data) {
    //                self.toDoLists(data);
    //            }
    //        });
    //    });

    //    this.get('/', function () { this.app.runRoute('get', '#todo') });
    //});
    
    self.initialize = function () {
        self.getAllTasks();
    }

    return self;
}

app.addViewModel({
    name: "TODO",
    bindingMemberName: "todo",
    factory: ToDoViewModel
});