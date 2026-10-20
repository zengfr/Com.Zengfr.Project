Ext.define("App.View.ModelAttrList", {
    extend: 'Ext.container.Container',
    initComponent: function () {
        var formPanel = Ext.create('App.ModelAttrFormPanel',
			{ id: 'ModelAttrFormPanel',
			    height: 180,
			    listeners: {
			        create: function (form, data) {
					    data.ModelAttrID=0;
			            Ext.getCmp('ModelAttrGridPanel').store.insert(0, data);
			        }
			    }
			});
		var searchPanel = Ext.create('App.ModelAttrSearchPanel',
			{ id: 'ModelAttrSearchPanel',
			    height: 180,
			    listeners: {
			        search: function (form, data) {
					  var store=Ext.getCmp('ModelAttrGridPanel').store;
					   store.on('beforeload',function (store, options){  
                           Ext.apply(store.proxy.extraParams, data);});  
			           store.reload();
			        }
			    }
			});
        var infoPanel = Ext.create('App.ModelAttrInfoPanel', { 
		id: 'ModelAttrInfoPanel', 
		height: 180 });

		var infoPropertyGridPanel = Ext.create('App.ModelAttrPropertyGridPanel', { 
		id: 'ModelAttrPropertyGridPanel', 
		height: 180 }); 
		
        var gridPanel = Ext.create('App.ModelAttrGridPanel', {
            height: 420, id: 'ModelAttrGridPanel',
            listeners: {
                selectionchange: function (selModel, selected) {
                    var len = selected.length;
                    if (len == 0) return;
                    Ext.getCmp('ModelAttrFormPanel').setActiveRecord(selected[len - 1] || null);
                    Ext.getCmp('ModelAttrInfoPanel').setActiveRecord(selected[len - 1] || null);
					Ext.getCmp('ModelAttrPropertyGridPanel').setSource(selected[len - 1].data || null);
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
