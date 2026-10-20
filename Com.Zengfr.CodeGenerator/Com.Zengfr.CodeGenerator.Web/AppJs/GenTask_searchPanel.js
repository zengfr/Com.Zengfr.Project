Ext.define("App.GenTaskSearchPanel", {
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
                   {fieldLabel: 'GenTaskId',
                    name: 'GenTaskId',
                    anchor:'100%'},{fieldLabel: 'Description',
                    name: 'Description',
                    anchor:'100%'},{fieldLabel: 'DataCreateLastTime',
                    name: 'DataCreateLastTime',
                    anchor:'100%'},]
              },{
                xtype: 'container',
                defaultType: 'textfield',
                items: [
                     {fieldLabel: 'Result',
                    name: 'Result',
                    anchor:'100%'},{fieldLabel: 'DataItemIsDeleted',
                    name: 'DataItemIsDeleted',
                    anchor:'100%'},{fieldLabel: 'DataChangeLastTime',
                    name: 'DataChangeLastTime',
                    anchor:'100%'},]
               },{
                xtype: 'container',
                flex: 1,
                defaultType: 'textfield',
                items: [
                     {fieldLabel: 'ResultStatus',
                    name: 'ResultStatus',
                    anchor:'100%'},{fieldLabel: 'DataItemStatus',
                    name: 'DataItemStatus',
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

