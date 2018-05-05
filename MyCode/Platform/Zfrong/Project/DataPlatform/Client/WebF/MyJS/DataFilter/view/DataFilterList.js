Ext.define("Pro.View.DataFilterList", {
    extend: 'Ext.container.Container',
    initComponent: function () {
        var formPanel = Ext.create('DataFilterForm',
			{ id: 'DataFilterFormPanel',
			    height: 180,
			    listeners: {
			        create: function (form, data) {
			            Ext.getCmp('DataFilterGridPanel').store.insert(0, data);
			        }
			    }
			});
        var infoPanel = Ext.create('DataFilterInfoPanel', { id: 'DataFilterInfoPanel', height: 180 });

        var gridPanel = Ext.create('DataFilterGrid', {
            height: 290, id: 'DataFilterGridPanel',
            listeners: {
                selectionchange: function (selModel, selected) {
                    var len = selected.length;
                    if (len == 0) return;
                    Ext.getCmp('DataFilterFormPanel').setActiveRecord(selected[len - 1] || null);
                    Ext.getCmp('DataFilterInfoPanel').setActiveRecord(selected[len - 1] || null);
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