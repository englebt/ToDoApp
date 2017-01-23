function ToDoModel() {
    var self = this;

    self.toDoListUrl = "/api/ToDoList";
    self.toDoItemUrl = "/api/ToDoItem";
    
    self.toDoLists = ko.observableArray();
    self.toDoItems = ko.observableArray();

    function ToDoList(data) {
        this.UserId = ko.observable(data.UserId);
        this.Title = ko.observable(data.Title);
        this.ToDoItems = ko.observableArray(data.ToDoItems);
    }

    function ToDoItem(data) {
        this.UserId = ko.observable(data.UserId);
        this.ToDoListId = ko.observable(data.ToDoListId);
        this.Title = ko.observable(data.Title);
        this.IsComplete = ko.observable(data.IsComplete);
    }

    self.addList = function () {
        self.toDoLists.push(new ToDoList({ Title: "New List" }));
    }

    self.addItem = function () {
        self.toDoItems.push(new ToDoItem({ Title: "New Task" }));
    }
}