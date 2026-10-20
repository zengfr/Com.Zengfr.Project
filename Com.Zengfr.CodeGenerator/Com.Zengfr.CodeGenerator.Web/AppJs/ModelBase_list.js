Ext.define("App.View.ModelBaseList", {
    extend: 'Ext.container.Container',
    initComponent: function () {
        var formPanel = Ext.create('App.ModelBaseFormPanel',
			{ id: 'ModelBaseFormPanel',
			    height: 180,
			    listeners: {
			        create: function (form, data) {
					    data.ModelBaseID=0;
			            Ext.getCmp('ModelBaseGridPanel').store.insert(0, data);
			        }
			    }
			});
		var searchPanel = Ext.create('App.ModelBaseSearchPanel',
			{ id: 'ModelBaseSearchPanel',
			    height: 180,
			    listeners: {
			        search: function (form, data) {
					  var store=Ext.getCmp('ModelBaseGridPanel').store;
					   store.on('beforeload',function (store, options){  
                           Ext.apply(store.proxy.extraParams, data);});  
			           store.reload();
			        }
			    }
			});
        var infoPanel = Ext.create('App.ModelBaseInfoPanel', { 
		id: 'ModelBaseInfoPanel', 
		height: 180 });

		var infoPropertyGridPanel = Ext.create('App.ModelBasePropertyGridPanel', { 
		id: 'ModelBasePropertyGridPanel', 
		height: 180 }); 
		
        var gridPanel = Ext.create('App.ModelBaseGridPanel', {
            height: 420, id: 'ModelBaseGridPanel',
            listeners: {
                selectionchange: function (selModel, selected) {
                    var len = selected.length;
                    if (len == 0) return;
                    Ext.getCmp('ModelBaseFormPanel').setActiveRecord(selected[len - 1] || null);
                    Ext.getCmp('ModelBaseInfoPanel').setActiveRecord(selected[len - 1] || null);
					Ext.getCmp('ModelBasePropertyGridPanel').setSource(selected[len - 1].data || null);
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
