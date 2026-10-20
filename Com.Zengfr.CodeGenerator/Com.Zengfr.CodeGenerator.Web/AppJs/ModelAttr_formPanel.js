Ext.define('App.ModelAttrFormPanel',{
        extend: 'Ext.form.Panel',
        title: '信息编辑',
        id: 'ModelAttrFormPanel',
        collapsible: true,autoScroll: true,
        bodyPadding: 5,
        width:'100%',
        fieldDefaults: {
            labelAlign: 'top',
            msgTarget: 'side'
        },
        defaults: {
            anchor: '50%'
        },

        items: [{
            xtype: 'container',
            layout:'hbox',
            items:[{
                xtype: 'container',
                defaultType: 'textfield',
                items: [
                   {fieldLabel: 'ModelAttrId',
                    allowBlank: false,
                    name: 'ModelAttrId',
                    anchor:'100%'},{fieldLabel: 'AttDescription',
                    allowBlank: false,
                    name: 'AttDescription',
                    anchor:'100%'},{fieldLabel: 'DefaultValue',
                    allowBlank: false,
                    name: 'DefaultValue',
                    anchor:'100%'},{fieldLabel: 'IsID',
                    allowBlank: false,
                    name: 'IsID',
                    anchor:'100%'},{fieldLabel: 'DataItemStatus',
                    allowBlank: false,
                    name: 'DataItemStatus',
                    anchor:'100%'},]
            },{
                xtype: 'container',
                defaultType: 'textfield',
                items: [
                     {fieldLabel: 'Model',
                    allowBlank: false,
                    name: 'Model',
                    anchor:'100%'},{fieldLabel: 'TypeName',
                    allowBlank: false,
                    name: 'TypeName',
                    anchor:'100%'},{fieldLabel: 'IsUnique',
                    allowBlank: false,
                    name: 'IsUnique',
                    anchor:'100%'},{fieldLabel: 'IsIndex',
                    allowBlank: false,
                    name: 'IsIndex',
                    anchor:'100%'},{fieldLabel: 'DataCreateLastTime',
                    allowBlank: false,
                    name: 'DataCreateLastTime',
                    anchor:'100%'},]
             },{
                xtype: 'container',
                flex: 1,
                defaultType: 'textfield',
                items: [
                     {fieldLabel: 'AttrName',
                    allowBlank: false,
                    name: 'AttrName',
                    anchor:'100%'},{fieldLabel: 'Length',
                    allowBlank: false,
                    name: 'Length',
                    anchor:'100%'},{fieldLabel: 'IsKey',
                    allowBlank: false,
                    name: 'IsKey',
                    anchor:'100%'},{fieldLabel: 'DataItemIsDeleted',
                    allowBlank: false,
                    name: 'DataItemIsDeleted',
                    anchor:'100%'},{fieldLabel: 'DataChangeLastTime',
                    allowBlank: false,
                    name: 'DataChangeLastTime',
                    anchor:'100%'},]
             }]
        }],

        buttons: [{
            iconCls: 'icon-save',itemId: 'save',text: '更新',
            handler: function() {
                this.up().up().onSave();
            }
        },{
            iconCls: 'icon-reset',itemId: 'reset',text: '重置',
            handler: function() {
               this.up().up().onReset();
            }
        },{
            iconCls: 'icon-add',itemId: 'create',text: '创建',
            handler: function() {
               this.up().up().onCreate()();
            }
        }],
		
	setActiveRecord: function (record) {
        this.activeRecord = record;
        if (record) {
            this.down('#save').enable(); this.down('#reset').enable();
            this.getForm().loadRecord(record);
        } else {
            this.down('#save').disable();
            this.getForm().reset();
        }
    },

    onSave: function () {
        var rec = this.activeRecord;
        var form = this.getForm();

        if (!rec) {
            return;
        }
        if (form.isValid()) {
            form.updateRecord(rec);
            this.onReset();
        }
        form = null; rec = null;
    },

    onCreate: function () {
        var form = this.getForm();

        if (form.isValid()) {
            this.fireEvent('create', this, form.getValues());
            form.reset();
        }
        form = null;
    },

    onReset: function () {
        this.setActiveRecord(null);
        this.getForm().reset();
    }
    });

