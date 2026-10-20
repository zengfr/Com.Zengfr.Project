Ext.define('App.ModelPropertyGridPanel', {
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
                                            'ModelId':'@ModelId',
                                                 'Database':'@Database',
                                                 'ModelType':'@ModelType',
                                                 'ModelName':'@ModelName',
                                                 'ModelDescription':'@ModelDescription',
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
