Ext.define("Pro.View.StatisticsList", {
    extend: 'Ext.container.Container',
    initComponent: function () {
        var formPanel = Ext.create('StatisticsForm',
			{ id: 'StatisticsFormPanel',
			    height: 180,
			    listeners: {
			        create: function (form, data) {
			            Ext.getCmp('StatisticsGridPanel').store.insert(0, data);
			        }
			    }
			});
        var infoPanel = Ext.create('StatisticsInfoPanel', { id: 'StatisticsInfoPanel', height: 180 });

        var gridPanel = Ext.create('StatisticsGrid', {
            height: 290, id: 'StatisticsGridPanel',
            listeners: {
                selectionchange: function (selModel, selected) {
                    var len = selected.length;
                    if (len == 0) return;
                    Ext.getCmp('StatisticsFormPanel').setActiveRecord(selected[len - 1] || null);
                    Ext.getCmp('StatisticsInfoPanel').setActiveRecord(selected[len - 1] || null);
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