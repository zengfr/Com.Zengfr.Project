Ext.define("App.ModelAttrSearchPanel", {
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
                   {fieldLabel: 'ModelAttrId',
                    name: 'ModelAttrId',
                    anchor:'100%'},{fieldLabel: 'AttDescription',
                    name: 'AttDescription',
                    anchor:'100%'},{fieldLabel: 'DefaultValue',
                    name: 'DefaultValue',
                    anchor:'100%'},{fieldLabel: 'IsID',
                    name: 'IsID',
                    anchor:'100%'},{fieldLabel: 'DataItemStatus',
                    name: 'DataItemStatus',
                    anchor:'100%'},]
              },{
                xtype: 'container',
                defaultType: 'textfield',
                items: [
                     {fieldLabel: 'Model',
                    name: 'Model',
                    anchor:'100%'},{fieldLabel: 'TypeName',
                    name: 'TypeName',
                    anchor:'100%'},{fieldLabel: 'IsUnique',
                    name: 'IsUnique',
                    anchor:'100%'},{fieldLabel: 'IsIndex',
                    name: 'IsIndex',
                    anchor:'100%'},{fieldLabel: 'DataCreateLastTime',
                    name: 'DataCreateLastTime',
                    anchor:'100%'},]
               },{
                xtype: 'container',
                flex: 1,
                defaultType: 'textfield',
                items: [
                     {fieldLabel: 'AttrName',
                    name: 'AttrName',
                    anchor:'100%'},{fieldLabel: 'Length',
                    name: 'Length',
                    anchor:'100%'},{fieldLabel: 'IsKey',
                    name: 'IsKey',
                    anchor:'100%'},{fieldLabel: 'DataItemIsDeleted',
                    name: 'DataItemIsDeleted',
                    anchor:'100%'},{fieldLabel: 'DataChangeLastTime',
                    name: 'DataChangeLastTime',
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

