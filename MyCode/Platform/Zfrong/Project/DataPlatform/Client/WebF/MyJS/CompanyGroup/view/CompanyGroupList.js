Ext.define("Pro.View.CompanyGroupList", {
    extend: 'Ext.container.Container',
    initComponent: function () {
        var formPanel = Ext.create('CompanyGroupForm',
			{ id: 'CompanyGroupFormPanel',
			    height: 180,
			    listeners: {
			        create: function (form, data) {
			            Ext.getCmp('CompanyGroupGridPanel').store.insert(0, data);
			        }
			    }
			});
        var infoPanel = Ext.create('CompanyGroupInfoPanel', { id: 'CompanyGroupInfoPanel', height: 180 });

        var gridPanel = Ext.create('CompanyGroupGrid', {
            height: 290, id: 'CompanyGroupGridPanel',
            listeners: {
                selectionchange: function (selModel, selected) {
                    var len = selected.length;
                    if (len == 0) return;
                    Ext.getCmp('CompanyGroupFormPanel').setActiveRecord(selected[len - 1] || null);
                    Ext.getCmp('CompanyGroupInfoPanel').setActiveRecord(selected[len - 1] || null);
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