define(["sitecore"], function (Sitecore) {
  var model = Sitecore.Definitions.Models.ControlModel.extend({
    initialize: function (options) {
        this._super();
        app = this;
        app.set("userlist", "");
        app.GetUsers(app);
    },

    GetUsers: function (app) {
        jQuery.ajax({
            type: "GET",
            dataType: "json",
            url: "/api/sitecore/UserAuditReport/GetUsers",
            cache: false,
            success: function (data) {
                app.set("userlist", data);
            },
            error: function () {
                console.log("Error in GetUsers() function");
            }
        });
    }
  });

  var view = Sitecore.Definitions.Views.ControlView.extend({
    initialize: function (options) {
      this._super();
    }
  });

  Sitecore.Factories.createComponent("UserAuditList", model, view, ".sc-UserAuditList");
});
