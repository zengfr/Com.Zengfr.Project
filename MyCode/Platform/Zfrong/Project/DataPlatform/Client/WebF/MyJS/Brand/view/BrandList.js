Ext.define("Pro.View.BrandList", {
    extend: 'Ext.container.Container',
    initComponent: function () {
        var formPanel = Ext.create('BrandForm',
			{ id: 'BrandFormPanel',
			    height: 180,
			    listeners: {
			        create: function (form, data) {
			            Ext.getCmp('BrandGridPanel').store.insert(0, data);
			        }
			    }
			});
        var infoPanel = Ext.create('BrandInfoPanel', { id: 'BrandInfoPanel', height: 180 });

        var gridPanel = Ext.create('BrandGrid', {
            height: 290, id: 'BrandGridPanel',
            listeners: {
                selectionchange: function (selModel, selected) {
                    var len = selected.length;
                    if (len == 0) return;
                    Ext.getCmp('BrandFormPanel').setActiveRecord(selected[len - 1] || null);
                    Ext.getCmp('BrandInfoPanel').setActiveRecord(selected[len - 1] || null);
                }
            }
        });
        Ext.suspendLayouts();
        gridPanel.createPagingToolbar();
        gridPanel.createToolbar();
        formPanel.createBar();

        var tabPanel = Ext.create('Ext.tab.Panel', {
            items: [formPanel, infoPanel]
        });
        Ext.resumeLayouts(true);

        Ext.apply(this, {
            items: [gridPanel, tabPanel]
        });
        this.callParent();
    }
});