define(["sitecore"], function (Sitecore) {
    var model = Sitecore.Definitions.Models.ControlModel.extend({
        initialize: function (options) {
            this._super();
            app = this;
            app.set('userlist', '');
            app.GetUsers(app, '');
            app.BindEvents(app);
        },

        GetUsers: function (app, userName) {
            jQuery.ajax({
                type: "GET",
                dataType: "json",
                url: "/api/sitecore/UserAuditReport/GetUsersOverview?username="+userName,
                cache: false,
                success: function (data) {
                    app.set("userlist", data);
                },
                error: function () {
                    console.log("Error in GetUsers() function");
                }
            });
        },

        GetUserDetails: function (app, userName, rowDetails) {
            jQuery.ajax({
                type: "GET",
                dataType: "json",
                url: "/api/sitecore/UserAuditReport/GetUserDetails?username="+userName,
                cache: false,
                success: function (data) {
                    //app.set("userlist", data);
                    rowDetails.show();
                },
                error: function () {
                    console.log("Error in GetUserDetails() function");
                }
            });
        },

        BindEvents: function (app) {
            jQuery(document).on("click", '.audit-user-row', function (event) {
                var userName = jQuery(this).find('.audit-user-name').text();
                var rowDetails = jQuery(this).next('audit-details-row');
                app.GetUserDetails(app, userName, rowDetails);
            });

            jQuery(document).on("change", '#user-filter', function (event) {
                var userName = jQuery(this).val();
                app.GetUsers(app, userName);
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
