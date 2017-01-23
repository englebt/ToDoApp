function ToDoViewModel(app) {
    var self = this;
    var toDoUrl = '/api/ToDo';
    
    self.toDoItems = ko.observableArray();

    self.getAllTasks = function () {
        app.ajaxHelper(toDoUrl, 'GET').done(function (data) {
            self.toDoItems(data);
        });
    }

    self.addItem = function () {
        var task = {
            userId: $('#userId').val(),
            title: "New Task",
            isComplete: false
        };

        self.toDoItems.push(task);
    };
    
    self.removeItem = function (item) { self.toDoItems.destroy(item); };

    self.save = function () {
        app.ajaxHelper(toDoUrl, 'POST', self.toDoItems).done(function (data) {
            self.toDoItems(data);
        });
    };

    Sammy(function () {
        this.get('#todo', function () { self.getAllTasks(); });
        this.get('/', function () { this.app.runRoute('get', '#todo') });
    });
    
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