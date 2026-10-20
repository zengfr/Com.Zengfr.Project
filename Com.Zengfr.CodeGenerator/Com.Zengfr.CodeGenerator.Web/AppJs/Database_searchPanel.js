Ext.define("App.DatabaseSearchPanel", {
    extend: 'Ext.form.Panel',
    title: '搜索面板',
	autoDestroy: true,autoScroll: true,collapsible:true,
    items: [{
                xtype: 'container',
				layout:'hbox',
                border:false,
                defaultType: 'textBox',
            items: [{
                xtype: 'container',
                defaultType: 'textfield',
                items: [
                   {fieldLabel: 'DatabaseId',
                    name: 'DatabaseId',
                    anchor:'100%'},{fieldLabel: 'DatabaseDescription',
                    name: 'DatabaseDescription',
                    anchor:'100%'},{fieldLabel: 'DataItemIsDeleted',
                    name: 'DataItemIsDeleted',
                    anchor:'100%'},{fieldLabel: 'DataChangeLastTime',
                    name: 'DataChangeLastTime',
                    anchor:'100%'},]
              },{
                xtype: 'container',
                defaultType: 'textfield',
                items: [
                     {fieldLabel: 'Company',
                    name: 'Company',
                    anchor:'100%'},{fieldLabel: 'DatabaseType',
                    name: 'DatabaseType',
                    anchor:'100%'},{fieldLabel: 'DataItemStatus',
                    name: 'DataItemStatus',
                    anchor:'100%'},]
               },{
                xtype: 'container',
                flex: 1,
                defaultType: 'textfield',
                items: [
                     {fieldLabel: 'DatabaseName',
                    name: 'DatabaseName',
                    anchor:'100%'},{fieldLabel: 'ConnctionString',
                    name: 'ConnctionString',
                    anchor:'100%'},{fieldLabel: 'DataCreateLastTime',
                    name: 'DataCreateLastTime',
                    anchor:'100%'},]
                }]
			}],	
	buttons: [{
            iconCls: 'icon-search',itemId: 'search',text: '查询',
            handler: function() {
                this.up().up().onSearch();
            }
        }],
	onSearch: function () {
        var form = this.getForm();
        if (form.isValid()) {
            this.fireEvent('search', this, form.getValues());
        }
        form = null;
    }
   });

