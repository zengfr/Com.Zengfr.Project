Ext.Loader.setConfig({
    enabled: true
});
Ext.application({
    name: 'app',
    appFolder: '/app/js/',
    models: [

    ],
    stores: [

    ],
    controllers: [
        'HomeController'
    ],
    views: [
        'IndexViewport'
    ],
    launch: function () {
        //var app = new app.view.IndexViewport();
        Ext.create("app.view.IndexViewport");
    }
});
Ext.Ajax.useDefaultXhrHeader = true;
Ext.Ajax.defaultHeaders = {
    'Access-Control-Allow-Origin': '*'
};
function addTab(myId, myTitle, myXType,myArgs) {
    var mainTabPanel = Ext.getCmp('tabPanel');
    var tab = mainTabPanel.getComponent(myId);
    if (tab == null) {
        Ext.apply(myArgs, {
            iconCls: 'tab',
            id: myId,
            enableTabScroll: true,
            closable: true,
            title: myTitle
        });

        var item = Ext.create(myXType, myArgs);
        mainTabPanel.add(item);
        tab = item;
    }
    mainTabPanel.setActiveTab(tab);
    showLog('addTab:' + myTitle);
}
function showLog(log) {
    Ext.getCmp('logPanel').setLog(log);
}