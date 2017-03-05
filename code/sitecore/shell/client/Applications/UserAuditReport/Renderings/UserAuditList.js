define(["sitecore"], function (Sitecore) {
    var model = Sitecore.Definitions.Models.ControlModel.extend({
        initialize: function (options) {
            this._super();
            app = this;
            app.set('userlist', '');
            app.GetUsers(app, '', -1);
            app.BindEvents(app);
        },

        GetUsers: function (app, userName, dateRange) {
            jQuery.ajax({
                type: "GET",
                dataType: "json",
                url: "/api/sitecore/UserAuditReport/GetUsersOverview?username=" + userName + '&dateRange=' + dateRange,
                cache: false,
                success: function (data) {
                    app.set("userlist", data);
                },
                error: function () {
                    console.log("Error in GetUsers() function");
                }
            });
        },

        GetUserDetails: function (app, userName, dateRange, row) {
            jQuery.ajax({
                type: "GET",
                dataType: "json",
                url: "/api/sitecore/UserAuditReport/GetUserDetails?username=" + userName + '&dateRange=' + dateRange,
                cache: false,
                success: function (data) {
                    content = '<ul style="list-style-type: none;">';
                    for (var i = 0; i < data.ItemsSaved.length; i++) {
                        var item = data.ItemsSaved[i];
                        var inner = '';
                        for (var j = 0; j < item.FieldsChanged.length; j++) {
                            var field = item.FieldsChanged[j];
                            inner += '<div style="margin-left:10px;">Changed field:' + field.FieldName + '</div>';
                        }
                        content += '<li>Modified: ' + item.Path + '<span style="margin-left:15px">&#64;' + item.Date + '</span>' + inner + '</li>';
                    }
                    for (var i = 0; i < data.ItemsDeleted.length; i++) {
                        var item = data.ItemsDeleted[i];
                        content += '<li>Deleted: ' + item.Path + '<span style="margin-left:15px">&#64;' + item.Date + '</span></li>';
                    }
                    for (var i = 0; i < data.ItemsCopied.length; i++) {
                        var item = data.ItemsCopied[i];
                        content += '<li>Copied: ' + item.Path + '<span style="margin-left:15px">&#64;' + item.Date + '</span></li>';
                    }
                    for (var i = 0; i < data.ItemsMoved.length; i++) {
                        var item = data.ItemsMoved[i];
                        content += '<li>Moved: ' + item.Path + '<span style="margin-left:15px">&#64;' + item.Date + '</span></li>';
                    }
                    for (var i = 0; i < data.ItemsCloned.length; i++) {
                        var item = data.ItemsCloned[i];
                        content += '<li>Cloned: ' + item.Path + '<span style="margin-left:15px">&#64;' + item.Date + '</span></li>';
                    }
                    content += '</ul>'

                    $(".user-audit-details").remove();
                    $(row).after('<tr class="user-audit-details"><td colspan="7">' + content + '</td></tr>');
                },
                error: function () {
                    console.log("Error in GetUserDetails() function");
                }
            });
        },

        BindEvents: function (app) {
            jQuery(document).on("click", '.audit-user-row', function (event) {
                var userName = jQuery(this).find('.audit-user-name').text();
                app.GetUserDetails(app, userName, -1, this);
            });

            jQuery(document).on("change", '#user-audit-filter', function (event) {
                var userName = jQuery(this).val();
                var period = jQuery('#user-audit-datepicker').val();
                app.GetUsers(app, userName, period);
            });

            jQuery(document).on("change", '#user-audit-datepicker', function (event) {
                var userName = jQuery('#user-audit-filter').val();
                var period = jQuery(this).val();
                app.GetUsers(app, userName, period);
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
