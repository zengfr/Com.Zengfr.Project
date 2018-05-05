Ext.define("Pro.View.SearchTermList", {
    extend: 'Ext.container.Container',
    initComponent: function () {
        var formPanel = Ext.create('SearchTermForm',
			{ id: 'SearchTermFormPanel',
			    height: 180,
			    listeners: {
			        create: function (form, data) {
			            Ext.getCmp('SearchTermGridPanel').store.insert(0, data);
			        }
			    }
			});
        var infoPanel = Ext.create('SearchTermInfoPanel', { id: 'SearchTermInfoPanel', height: 180 });

        var gridPanel = Ext.create('SearchTermGrid', {
            height: 290, id: 'SearchTermGridPanel',
            listeners: {
                selectionchange: function (selModel, selected) {
                    var len = selected.length;
                    if (len == 0) return;
                    Ext.getCmp('SearchTermFormPanel').setActiveRecord(selected[len - 1] || null);
                    Ext.getCmp('SearchTermInfoPanel').setActiveRecord(selected[len - 1] || null);
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