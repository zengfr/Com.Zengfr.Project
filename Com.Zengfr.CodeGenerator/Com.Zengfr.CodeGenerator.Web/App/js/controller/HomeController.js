Ext.define('app.controller.HomeController', {
    extend: 'Ext.app.Controller',

    init: function (application) {
        this.control({
            '[id=btnSavePwd]': {
                click: this.onOK
            }
        });
    },

    //保存
    onOK: function (obj) {
        var form = obj.up('form').getForm();
        if (form.isValid()) {
            Ext.Msg.alert('信息提示', '已保存');
        }
    }
});
