Ext.define('App.DatabasePropertyGridPanel', {
    extend: 'Ext.grid.property.Grid',
	title: '详细列表',
    requires: [
        'Ext.button.Button',
        'Ext.grid.property.*',
        'Ext.layout.container.VBox',
        'Ext.layout.container.HBox'
    ],
    height:275,
    width: 300,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    
    initComponent: function(){
        Ext.apply(this, {
            items: [{
                xtype: 'propertygrid',
                source: {
                                            'DatabaseId':'@DatabaseId',
                                                 'Company':'@Company',
                                                 'DatabaseName':'@DatabaseName',
                                                 'DatabaseDescription':'@DatabaseDescription',
                                                 'DatabaseType':'@DatabaseType',
                                                 'ConnctionString':'@ConnctionString',
                                                 'DataItemIsDeleted':'@DataItemIsDeleted',
                                                 'DataItemStatus':'@DataItemStatus',
                                                 'DataCreateLastTime':'@DataCreateLastTime',
                                                 'DataChangeLastTime':'@DataChangeLastTime',
                                         },
                sourceConfig: {}
            }]
        });
        this.callParent();
    }
});
