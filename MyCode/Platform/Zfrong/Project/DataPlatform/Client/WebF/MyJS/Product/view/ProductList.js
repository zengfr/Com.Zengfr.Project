Ext.define("Pro.View.ProductList", {
    extend: 'Ext.container.Container',
    initComponent: function () {
        var formPanel = Ext.create('ProductForm',
			{ id: 'ProductFormPanel',
			    height: 180,
			    listeners: {
			        create: function (form, data) {
			            Ext.getCmp('ProductGridPanel').store.insert(0, data);
			        }
			    }
			});
        var infoPanel = Ext.create('ProductInfoPanel', { id: 'ProductInfoPanel', height: 180 });

        var gridPanel = Ext.create('ProductGrid', {
            height: 290, id: 'ProductGridPanel',
            listeners: {
                selectionchange: function (selModel, selected) {
                    var len = selected.length;
                    if (len == 0) return;
                    Ext.getCmp('ProductFormPanel').setActiveRecord(selected[len - 1] || null);
                    Ext.getCmp('ProductInfoPanel').setActiveRecord(selected[len - 1] || null);
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