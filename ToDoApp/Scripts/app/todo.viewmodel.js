function ToDoItem(data) {
    this.userId = $('#userId').val();
    this.toDoItemId = data.toDoItemId;
    this.title = ko.observable(data.title);
    this.isComplete = ko.observable(data.isComplete);
}

function ToDoViewModel(app) {
    var self = this;
    var toDoUrl = '/api/ToDo';
    
    self.toDoItems = ko.observableArray([]);
    self.newTaskText = ko.observable("New Task");

    self.getAllTasks = function () {
        app.ajaxHelper(toDoUrl, 'GET').done(function (data) {
            var mapped = $.map(data, function (task) { return new ToDoItem(task); });
            self.toDoItems(mapped);
        });
    }

    self.addItem = function () {
        var task = {
            userId: $('#userId').val(),
            title: "New Task",
            isComplete: false
        };
        app.ajaxHelper(toDoUrl, 'POST', task).done(function (data) {
            self.toDoItems.push(new ToDoItem(data));
        });
    };
    
    self.removeItem = function (item) {
        app.ajaxHelper(toDoUrl + '/' + item.toDoItemId, 'DELETE');
        self.toDoItems.destroy(item);
    };

    self.putItem = function (item) {
        app.ajaxHelper(toDoUrl + '/' + item.toDoItemId, 'PUT').done(function (data) {
            self.toDoItems[item](data);
        });
    }

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