Ext.define("App.View.GenTaskList", {
    extend: 'Ext.container.Container',
    initComponent: function () {
        var formPanel = Ext.create('App.GenTaskFormPanel',
			{ id: 'GenTaskFormPanel',
			    height: 180,
			    listeners: {
			        create: function (form, data) {
					    data.GenTaskID=0;
			            Ext.getCmp('GenTaskGridPanel').store.insert(0, data);
			        }
			    }
			});
		var searchPanel = Ext.create('App.GenTaskSearchPanel',
			{ id: 'GenTaskSearchPanel',
			    height: 180,
			    listeners: {
			        search: function (form, data) {
					  var store=Ext.getCmp('GenTaskGridPanel').store;
					   store.on('beforeload',function (store, options){  
                           Ext.apply(store.proxy.extraParams, data);});  
			           store.reload();
			        }
			    }
			});
        var infoPanel = Ext.create('App.GenTaskInfoPanel', { 
		id: 'GenTaskInfoPanel', 
		height: 180 });

		var infoPropertyGridPanel = Ext.create('App.GenTaskPropertyGridPanel', { 
		id: 'GenTaskPropertyGridPanel', 
		height: 180 }); 
		
        var gridPanel = Ext.create('App.GenTaskGridPanel', {
            height: 420, id: 'GenTaskGridPanel',
            listeners: {
                selectionchange: function (selModel, selected) {
                    var len = selected.length;
                    if (len == 0) return;
                    Ext.getCmp('GenTaskFormPanel').setActiveRecord(selected[len - 1] || null);
                    Ext.getCmp('GenTaskInfoPanel').setActiveRecord(selected[len - 1] || null);
					Ext.getCmp('GenTaskPropertyGridPanel').setSource(selected[len - 1].data || null);
                }
            }
        });
        Ext.suspendLayouts();
        var tabPanel = Ext.create('Ext.tab.Panel', {
            items: [formPanel, infoPanel,infoPropertyGridPanel]
        });
        Ext.apply(this, {
            items: [searchPanel,gridPanel, tabPanel]
        });
		Ext.resumeLayouts(true);
		
        this.callParent();
    }
});
