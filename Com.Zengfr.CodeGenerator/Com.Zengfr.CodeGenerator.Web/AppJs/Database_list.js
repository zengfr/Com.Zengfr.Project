Ext.define("App.View.DatabaseList", {
    extend: 'Ext.container.Container',
    initComponent: function () {
        var formPanel = Ext.create('App.DatabaseFormPanel',
			{ id: 'DatabaseFormPanel',
			    height: 180,
			    listeners: {
			        create: function (form, data) {
					    data.DatabaseID=0;
			            Ext.getCmp('DatabaseGridPanel').store.insert(0, data);
			        }
			    }
			});
		var searchPanel = Ext.create('App.DatabaseSearchPanel',
			{ id: 'DatabaseSearchPanel',
			    height: 180,
			    listeners: {
			        search: function (form, data) {
					  var store=Ext.getCmp('DatabaseGridPanel').store;
					   store.on('beforeload',function (store, options){  
                           Ext.apply(store.proxy.extraParams, data);});  
			           store.reload();
			        }
			    }
			});
        var infoPanel = Ext.create('App.DatabaseInfoPanel', { 
		id: 'DatabaseInfoPanel', 
		height: 180 });

		var infoPropertyGridPanel = Ext.create('App.DatabasePropertyGridPanel', { 
		id: 'DatabasePropertyGridPanel', 
		height: 180 }); 
		
        var gridPanel = Ext.create('App.DatabaseGridPanel', {
            height: 420, id: 'DatabaseGridPanel',
            listeners: {
                selectionchange: function (selModel, selected) {
                    var len = selected.length;
                    if (len == 0) return;
                    Ext.getCmp('DatabaseFormPanel').setActiveRecord(selected[len - 1] || null);
                    Ext.getCmp('DatabaseInfoPanel').setActiveRecord(selected[len - 1] || null);
					Ext.getCmp('DatabasePropertyGridPanel').setSource(selected[len - 1].data || null);
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
