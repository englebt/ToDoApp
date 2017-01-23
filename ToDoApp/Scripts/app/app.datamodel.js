function AppDataModel() {
    var self = this;

    // Routes
    self.siteUrl = "/";

    // Route operations

    // Other private operations

    // Operations

    // Data

    // Data access operations
    self.setAccessToken = function (accessToken) {
        sessionStorage.setItem("accessToken", accessToken);
    };

    self.getAccessToken = function () {
        return sessionStorage.getItem("accessToken");
    };
}