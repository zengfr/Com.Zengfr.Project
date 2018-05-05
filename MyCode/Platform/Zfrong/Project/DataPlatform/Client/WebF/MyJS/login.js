Ext.onReady(function () {
    Ext.QuickTips.init();

    var login = new Ext.FormPanel({
        labelWidth: 50,
        url: 'DoIn?json=true',
        frame: true,
        defaultType: 'textfield',
        monitorValid: true,
        items: [{
            fieldLabel: '帐号',
            name: 'userName',
            allowBlank: false
        }, {
            fieldLabel: '密码',
            name: 'pwd',
            inputType: 'password',
            allowBlank: false
        }],
        buttons: [{
            text: '登录',
            formBind: true,
            handler: function () {
                login.getForm().submit({
                    method: 'POST',
                    waitTitle: '提交',
                    waitMsg: '登录中...',
                    success: function (form, action) {
                        var obj = Ext.JSON.decode(action.response.responseText);
                        if (obj.success) {
                            var redirect = '/DataPlatform/Home/PIndex';
                            window.location = redirect;
                        }
                        else
                            Ext.Msg.alert('登录失败!', obj.message);
                    },
                    failure: function (form, action) {
                        if (action.failureType == 'server') {
                            var obj = Ext.JSON.decode(action.response.responseText);
                            Ext.Msg.alert('失败!', obj.message);
                        } else {
                            Ext.Msg.alert('警告!', 'Authentication server is unreachable : ' + action.response.responseText);
                        }
                        login.getForm().reset();
                    }
                });
            }
        }, {
            text: '重置', handler: function () { login.getForm().reset(); }
        }]
    });

    var win = new Ext.Window({
        layout: 'fit', title: '登录',
        width: 280,
        height: 150, modal: true,
        closable: false,
        resizable: false,
        plain: true,
        border: false,
        items: [login]
    });
    win.show();
});