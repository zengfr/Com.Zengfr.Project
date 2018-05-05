Ext.define("Pro.View.DataItemList", {
    extend: 'Ext.container.Container',
    initComponent: function () {
        var formPanel = Ext.create('DataItemForm',
			{ id: 'DataItemFormPanel',
			    height: 180,
			    listeners: {
			        create: function (form, data) {
			            Ext.getCmp('DataItemGridPanel').store.insert(0, data);
			        }
			    }
			});
        var infoPanel = Ext.create('DataItemInfoPanel', { id: 'DataItemInfoPanel', height: 180 });

        var gridPanel = Ext.create('DataItemGrid', {
            height: 290, id: 'DataItemGridPanel',
            listeners: {
                selectionchange: function (selModel, selected) {
                    var len = selected.length;
                    if (len == 0) return;
                    Ext.getCmp('DataItemFormPanel').setActiveRecord(selected[len - 1] || null);
                    Ext.getCmp('DataItemInfoPanel').setActiveRecord(selected[len - 1] || null);
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