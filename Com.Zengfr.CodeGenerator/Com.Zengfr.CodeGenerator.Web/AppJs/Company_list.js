Ext.define("App.View.CompanyList", {
    extend: 'Ext.container.Container',
    initComponent: function () {
        var formPanel = Ext.create('App.CompanyFormPanel',
			{ id: 'CompanyFormPanel',
			    height: 180,
			    listeners: {
			        create: function (form, data) {
					    data.CompanyID=0;
			            Ext.getCmp('CompanyGridPanel').store.insert(0, data);
			        }
			    }
			});
		var searchPanel = Ext.create('App.CompanySearchPanel',
			{ id: 'CompanySearchPanel',
			    height: 180,
			    listeners: {
			        search: function (form, data) {
					  var store=Ext.getCmp('CompanyGridPanel').store;
					   store.on('beforeload',function (store, options){  
                           Ext.apply(store.proxy.extraParams, data);});  
			           store.reload();
			        }
			    }
			});
        var infoPanel = Ext.create('App.CompanyInfoPanel', { 
		id: 'CompanyInfoPanel', 
		height: 180 });

		var infoPropertyGridPanel = Ext.create('App.CompanyPropertyGridPanel', { 
		id: 'CompanyPropertyGridPanel', 
		height: 180 }); 
		
        var gridPanel = Ext.create('App.CompanyGridPanel', {
            height: 420, id: 'CompanyGridPanel',
            listeners: {
                selectionchange: function (selModel, selected) {
                    var len = selected.length;
                    if (len == 0) return;
                    Ext.getCmp('CompanyFormPanel').setActiveRecord(selected[len - 1] || null);
                    Ext.getCmp('CompanyInfoPanel').setActiveRecord(selected[len - 1] || null);
					Ext.getCmp('CompanyPropertyGridPanel').setSource(selected[len - 1].data || null);
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
