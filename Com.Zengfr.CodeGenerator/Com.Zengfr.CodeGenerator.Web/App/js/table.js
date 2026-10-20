

Ext.define("app.model.Table", {
    extend: "Ext.data.Model",
    fields: [
        { name: 'ModelId', type: 'int', mapping: 'ModelId' },
        { name: 'ModelName', type: 'string', mapping: 'ModelName' },

        { name:'DataItemIsDeleted', type: 'date', mapping: 'DataItemIsDeleted' },
        { name:'DataItemStatus' ,type: 'date', mapping: 'DataItemStatus' },
        { name:'DataCreateLastTime' ,type: 'date', mapping: 'DataCreateLastTime' },
        { name:'DataChangeLastTime' ,type: 'date', mapping: 'DataChangeLastTime' },
    ],
    idProperty: 'ModelId',
    api: {
        create: 'api/Model/SaveOrUpdate',
        read: 'api/Model/GetList?modelType=1',
        update: 'api/Model/SaveOrUpdate',
        destroy: 'api/Model/Delete'
    }
});
Ext.define("app.store.TableStore", {
    extend: "Ext.data.Store",
    alias: 'tableStore',
    model: "app.model.Table",
    nodeParam: 'ModelId',
    proxy: {
        type: 'ajax',
        reader: { type: 'json' },
        writer: { type: 'json' },
        api: {
            create: 'api/Model/SaveOrUpdate',
            read: 'api/Model/GetList?modelType=1',
            update: 'api/Model/SaveOrUpdate',
            destroy: 'api/Model/Delete'
        }
    },
    sorters: [{
        property: 'ModelId',
        direction: 'DESC'
    }]
});
Ext.define("app.store.TableTreeStore", {
    extend: "Ext.data.TreeStore",
    alias: 'tableTreeStore',
    model: "app.model.Table",
    nodeParam: 'ModelId',
    proxy: {
        type: 'ajax',
        reader: { type: 'json' },
        writer: { type: 'json' },
        api: {
            read: 'api/Table/GetTree',
        }
    },
    sorters: [{
        property: 'ModelName',
        direction: 'ASC'
    }],
    root: {
        text: 'Table', fullName: '.',
        ModelId:0, checked: null
    }
});

Ext.define('app.view.TableTabPanel', {
    extend: 'Ext.container.Container',
    alias: 'widget.tableTabPanel',
    layout: {
        type: 'border',
        align: 'stretch'
    },  
    items: [{
        xtype: 'databaseTreePanel', region: 'west', width: 300, split: true, collapsible: true,
    }, {
        xtype: 'tableGrid', region: 'center',
    },
       {
           xtype: 'tablePropertyGrid', region: 'east', width: 300, split: true, collapsible: true,
       }]
});
Ext.define('app.view.TableForm', {
    extend: 'Ext.form.Panel',
    alias: 'widget.TableForm',
    layout: 'form',
    defaultType: 'textfield',
    items: [{
        xtype: "hidden",
        name: 'ModelId',
     
    }, {
        fieldLabel: 'ModelName',
        name: 'ModelName',
        allowBlank: false
    }, {
        fieldLabel: 'DataItemIsDeleted',
        name: 'DataItemIsDeleted',
        allowBlank: false
    }
    ],
    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            buttons: [
            {
                xtype: "button", text: "Save",
                handler: function () {
                    var record = me.getRecord();
                    var form = me.getForm();
                    form.updateRecord(record);
                }
            },
             {
                 id: "btnReset",
                 iconCls: 'a_cross',  //样式
                 text: 'reset',
                 handler: function () {
                     var form = me.getForm();
                     form.reset();
                 }
             }
            ]
        });
        me.callParent(arguments);
    }
    
    
});
Ext.define('app.view.TablePropertyGrid', {
    extend: 'Ext.grid.property.Grid',
    alias: 'widget.tablePropertyGrid',
    title: 'Table',
    requires: [
        'Ext.button.Button',
        'Ext.grid.property.*',
        'Ext.layout.container.VBox',
        'Ext.layout.container.HBox'
    ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    
    initComponent: function(){
        Ext.apply(this, {
            items: [{
                xtype: 'propertygrid',
                source: {
                    'ModelId': 'ModelId',
                    'ModelName': 'ModelName',
                    },
                    //sourceConfig: {}
                }]
        });
        this.callParent(arguments);
    }
});
Ext.define('app.view.TableGrid', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.tableGrid',
    requires: [
        'Ext.grid.plugin.RowEditing',
        'Ext.grid.*',
        'Ext.data.*',
    'Ext.util.*',
    'Ext.state.*',
    'Ext.form.*'
    ],

    initComponent: function () {
        var me = this;
        var rowEditing = Ext.create('Ext.grid.plugin.RowEditing', {
            autoCancel: false, clicksToEdit: 1
        });
        var store = Ext.create('tableStore');
        var scroller = Ext.create('Ext.scroll.Scroller');
        Ext.apply(me, {
            store: store,
            viewConfig: {
                stripeRows: true,
                enableTextSelection: true,
                align: 'stretch', forceFit: true,
            },
             selModel: {
                  injectCheckbox: 0,
                  mode: "SIMPLE",     //"SINGLE"/"SIMPLE"/"MULTI"
                  checkOnly: true     //只能通过checkbox选择
              },
            selType: "checkboxmodel",
            loadMask: true,
            columns: [{
                header: 'ModelId',
                dataIndex: 'ModelId',
                editor: {
                    allowBlank: false
                }
            }, {
                header: 'ModelName',
                dataIndex: 'ModelName',
                flex: 1,
                editor: {
                    allowBlank: false
                }
            }],
            columnLines:true,
            frame: true, scrollable: true,scroller:scroller,
            tbar: [{
                itemId: 'add',
                text: 'Add',
                iconCls: '', cc:0,
                handler: function () {
                    rowEditing.cancelEdit();
                    this.cc = this.cc - 1;
                    var r = Ext.create('app.model.Table', {
                        ModelId: this.cc,
                        ModelName: 'TableName'
                    });
                    store.insert(0, r);
                    rowEditing.startEdit(0, 0);
                }
            },'-', {
                itemId: 'delete',
                text: 'Delete',
                iconCls: '',
                handler: function () {
                    var sm = me.getSelectionModel();
                    rowEditing.cancelEdit();
                    store.remove(sm.getSelection());
                    if (store.getCount() > 0) {
                        sm.select(0);
                    }
                },
                disabled: true
            }, '-', {
                text: 'Save',
                handler: function () {
                    store.sync();
                }
            }, '-'],
            dockedItems: [{
                xtype: 'pagingtoolbar',
                store: store,  
                dock: 'bottom',
                displayInfo: true
            }],
            plugins: [rowEditing],
            listeners: {
                selectionchange: function (view, records) {
                    me.down('#delete').setDisabled(!records.length);
                }
            }
        });

        me.callParent(arguments);
    }
});