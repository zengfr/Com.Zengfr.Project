

Ext.define("app.model.Database", {
    extend: "Ext.data.Model",
    fields: [
        { name: 'DatabaseId', type: 'int', mapping: 'DatabaseId' },
        { name: 'DatabaseName', type: 'string', mapping: 'DatabaseName' },

        { name:'DataItemIsDeleted', type: 'date', mapping: 'DataItemIsDeleted' },
        { name:'DataItemStatus' ,type: 'date', mapping: 'DataItemStatus' },
        { name:'DataCreateLastTime' ,type: 'date', mapping: 'DataCreateLastTime' },
        { name:'DataChangeLastTime' ,type: 'date', mapping: 'DataChangeLastTime' },
    ],
    idProperty: 'DatabaseId',
    api: {
        create: 'api/Database/SaveOrUpdate',
        read: 'api/Database/GetList',
        update: 'api/Database/SaveOrUpdate',
        destroy: 'api/Database/Delete'
    }
});
Ext.define("app.store.DatabaseTreeStore", {
    extend: "Ext.data.TreeStore",
    alias: 'databaseTreeStore',
    model: "app.model.Database",
    nodeParam: 'DatabaseId',
    proxy: {
        type: 'ajax',
        reader: { type: 'json' },
        writer: { type: 'json' },
        api: {
            read: 'api/Database/GetTree',
        }
    },
    sorters: [{
        property: 'DatabaseName',
        direction: 'ASC'
    }],
    root: {
        text: 'Database', fullName: '.',
        DatabaseId: 0, checked: null
    }
});
Ext.define("app.store.DatabaseStore", {
    extend: "Ext.data.Store",
    alias: 'databaseStore',
    model: "app.model.Database",
    nodeParam: 'path',
    proxy: {
        type: 'ajax',
        reader: { type: 'json' },
        writer: { type: 'json' },
        api: {
            create: 'api/Database/SaveOrUpdate',
            read: 'api/Database/GetList',
            update: 'api/Database/SaveOrUpdate',
            destroy: 'api/Database/Delete'
        }
    },
    sorters: [{
        property: 'DatabaseId',
        direction: 'DESC'
    }]
});
Ext.define('app.view.DatabaseTabPanel', {
    extend: 'Ext.container.Container',
    alias: 'widget.databaseTabPanel',
    layout: {
        type: 'border',
        align: 'stretch'
    },  
    items: [{
        xtype: 'companyTreePanel', region: 'west', width: 300, split: true, collapsible: true,
    }, {
        xtype: 'databaseGrid', region: 'center',  
    },
       {
           xtype: 'databasePropertyGrid', region: 'east', width: 300, split: true, collapsible: true,
       }]
});
Ext.define('app.view.DatabaseForm', {
    extend: 'Ext.form.Panel',
    alias: 'widget.databaseForm',
    layout: 'form',
    defaultType: 'textfield',
    items: [{
        xtype: "hidden",
        name: 'DatabaseId',
     
    }, {
        fieldLabel: 'DatabaseName',
        name: 'DatabaseName',
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
Ext.define('app.view.DatabasePropertyGrid', {
    extend: 'Ext.grid.property.Grid',
    alias: 'widget.databasePropertyGrid',
    title: 'Database',
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
                    'DatabaseId': 'DatabaseId',
                    'DatabaseName': 'DatabaseName',
                    },
                    //sourceConfig: {}
                }]
        });
        this.callParent(arguments);
    }
});
Ext.define('app.view.DatabaseGrid', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.databaseGrid',
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
        var store = Ext.create('databaseStore');
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
                header: 'DatabaseId',
                dataIndex: 'DatabaseId',
                editor: {
                    allowBlank: false
                }
            }, {
                header: 'DatabaseName',
                dataIndex: 'DatabaseName',
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
                    var r = Ext.create('app.model.Database', {
                        DatabaseId: this.cc,
                        DatabaseName: 'DatabaseName'
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

Ext.define('app.view.DatabaseTreePanel', {
    extend: 'Ext.tree.Panel',
    alias: 'widget.databaseTreePanel',
    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            title: 'Database',
            tbar: [{
                text: 'Expand',
                handler: function () {
                    me.expandAll();
                }
            }, '-', {
                text: 'Collapse',
                handler: function () {
                    me.collapseAll();
                    me.getRootNode().expand();
                }
            }, '-'],
            useArrows: true, animate: true,
            store: Ext.create('databaseTreeStore'),
            listeners: {
                click: function (node) {
                    Ext.Msg.alert('消息', '你点击了: "' + node + "'");
                },
            }
        }
        );
        me.callParent(arguments);
    },

});