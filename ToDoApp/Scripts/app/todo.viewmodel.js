function ToDoViewModel(app, dataModel) {
    var self = this;

    self.toDoLists = ko.observableArray();
    self.error = ko.observable("");

    self.addToDoList = function () {

    };
    
    Sammy(function () {
        this.get('#todo', function () {
            // Make a call to the protected Web API by passing in a Bearer Authorization Header
            $.get({
                url: app.dataModel.toDoList,
                contentType: "application/json; charset=utf-8",
                headers: {
                    'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
                },
                done: function (data) {
                    self.toDoLists(data);
                }
            });
        });

        this.get('/', function () { this.app.runRoute('get', '#todo') });
    });
    
    return self;
}

app.addViewModel({
    name: "ToDo",
    bindingMemberName: "todo",
    factory: ToDoViewModel
});
