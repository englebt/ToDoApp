function ToDoList(data) {
    var self = this;
    self.newItemText = ko.observable("New Item");
    self.addItem = function () {
        self.toDoItems.push(new ToDoItem({ title: self.newItemText() }));
        self.newItemText("");
    }
    self.removeItem = function (item) { self.toDoItems.destroy(item); }

    self.UserId = $('#userId').val();
    self.toDoListId = data.toDoListId;
    self.title = ko.observable(data.title);
    self.toDoItems = ko.observableArray();

    self.addItem();
}

function ToDoItem(data) {
    this.userId = $('#userId').val();
    this.toDoItemId = data.toDoItemId;
    this.toDoListId = data.toDoListId;
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
        var data = ko.toJSON({ toDoLists: self.ToDoLists });
        app.ajaxHelper(toDoUrl, 'POST', self.ToDoLists);
    }

    //self.save = function () {
    //    var data = ko.toJSON({ toDoItems: self.toDoItems });
    //    app.ajaxHelper(toDoUrl, 'POST', self.toDoItems);
    //}

    Sammy(function () {
        this.get('#todo', function () { self.getAllLists(); });
        this.get('/', function () { this.app.runRoute('get', '#todo') });
    });
    
    self.initialize = function () {
        self.getAllLists();
        //self.getAllTasks();
    }

    return self;
}

app.addViewModel({
    name: "TODO",
    bindingMemberName: "todo",
    factory: ToDoViewModel
});