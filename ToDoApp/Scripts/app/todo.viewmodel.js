function ToDoList(data) {
    var self = this;
    self.newItemText = ko.observable("New Item");
    self.addItem = function () {
        var item = new ToDoItem({
            title: self.newItemText(),
            isComplete: false
        });
        self.toDoItems.push(item);
        self.newItemText("");
    }
    self.removeItem = function (item) { self.toDoItems.destroy(item); }

    self.UserId = $('#userId').val();
    self.toDoListId = data.toDoListId;
    self.title = ko.observable(data.title);
    self.toDoItems = ko.observableArray(data.toDoItems);
}

function ToDoItem(data) {
    this.userId = $('#userId').val();
    this.toDoItemId = data.toDoItemId;
    this.title = ko.observable(data.title);
    this.isComplete = ko.observable(data.isComplete);
}

function ToDoViewModel(app) {
    var self = this;
    var toDoUrl = '/api/ToDo';
    
    self.toDoLists = ko.observableArray([]);
    self.newListText = ko.observable("New List");
    
    self.getAllLists = function () {
        app.ajaxHelper(toDoUrl, 'GET').done(function (data) {
            var mapped = $.map(data, function (list) { return new ToDoList(list); });
            self.toDoLists(mapped);
        });
    }

    self.getAllTasks = function () {
        app.ajaxHelper(toDoUrl, 'GET').done(function (data) {
            var mapped = $.map(data, function (task) { return new ToDoItem(task); });
            self.toDoItems(mapped);
        });
    }

    self.addList = function () {
        self.toDoLists.push(new ToDoList({ title: self.newListText() }));
        self.newListText("");
    }

    self.removeTask = function (list) { self.toDoLists.destroy(list); }

    self.save = function () {
        var data = ko.toJSON({ toDoLists: self.toDoLists });
        app.ajaxHelper(toDoUrl, 'POST', self.toDoLists);
    }

    Sammy(function () {
        this.get('#todo', function () { self.getAllLists(); });
        this.get('/', function () { this.app.runRoute('get', '#todo') });
    });
    
    self.initialize = function () {
        self.getAllLists();
    }

    return self;
}

app.addViewModel({
    name: "TODO",
    bindingMemberName: "todo",
    factory: ToDoViewModel
});