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
    self.newTaskText = ko.observable();

    self.getAllTasks = function () {
        app.ajaxHelper(toDoUrl, 'GET').done(function (data) {
            var mapped = $.map(data, function (task) { return new ToDoItem(task); });
            self.toDoItems(mapped);
        });
    }

    self.addItem = function () {
        self.toDoItems.push(new ToDoItem({ title: self.newTaskText() }));
        self.newTaskText("");
    }
    
    self.removeItem = function (item) { self.toDoItems.destroy(item); }

    self.save = function () {
        var data = ko.toJSON({ toDoItems: self.toDoItems });
        app.ajaxHelper(toDoUrl, 'POST', self.toDoItems);
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