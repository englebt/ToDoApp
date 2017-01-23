function HomeViewModel(app, dataModel) {
    var self = this;

    //self.myHometown = ko.observable("");

    Sammy(function () {
        this.get('#home', function () { this.app.runRoute('get', '#home') });
        this.get('/', function () { this.app.runRoute('get', '#home') });
    });

    return self;
}

app.addViewModel({
    name: "Home",
    bindingMemberName: "home",
    factory: HomeViewModel
});
