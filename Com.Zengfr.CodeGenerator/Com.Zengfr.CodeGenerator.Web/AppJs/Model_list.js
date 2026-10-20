Ext.define("App.View.ModelList", {
    extend: 'Ext.container.Container',
    initComponent: function () {
        var formPanel = Ext.create('App.ModelFormPanel',
			{ id: 'ModelFormPanel',
			    height: 180,
			    listeners: {
			        create: function (form, data) {
					    data.ModelID=0;
			            Ext.getCmp('ModelGridPanel').store.insert(0, data);
			        }
			    }
			});
		var searchPanel = Ext.create('App.ModelSearchPanel',
			{ id: 'ModelSearchPanel',
			    height: 180,
			    listeners: {
			        search: function (form, data) {
					  var store=Ext.getCmp('ModelGridPanel').store;
					   store.on('beforeload',function (store, options){  
                           Ext.apply(store.proxy.extraParams, data);});  
			           store.reload();
			        }
			    }
			});
        var infoPanel = Ext.create('App.ModelInfoPanel', { 
		id: 'ModelInfoPanel', 
		height: 180 });

		var infoPropertyGridPanel = Ext.create('App.ModelPropertyGridPanel', { 
		id: 'ModelPropertyGridPanel', 
		height: 180 }); 
		
        var gridPanel = Ext.create('App.ModelGridPanel', {
            height: 420, id: 'ModelGridPanel',
            listeners: {
                selectionchange: function (selModel, selected) {
                    var len = selected.length;
                    if (len == 0) return;
                    Ext.getCmp('ModelFormPanel').setActiveRecord(selected[len - 1] || null);
                    Ext.getCmp('ModelInfoPanel').setActiveRecord(selected[len - 1] || null);
					Ext.getCmp('ModelPropertyGridPanel').setSource(selected[len - 1].data || null);
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
