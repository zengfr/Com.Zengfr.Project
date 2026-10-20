
Ext.define("app.model.Model", {
    extend: "Ext.data.Model",
    fields: [
        { name: 'id', type: 'int', mapping: 'Id' },
        { name: 'text', type: 'string', mapping: 'Name' },
        { name: 'fullName', type: 'string', mapping: 'FullName' },
        {
            name: 'leaf', type: 'boolean', convert: function (v, r) {
                return r.get("IsFile");
            }
        },
        {
            name: 'checked', type: 'boolean', defaultValue: false
        }
    ],
    idProperty: 'fullName',
    api: {
        create: 'api/Model/UpdateOrSave',
        read: 'api/Model/GetList?modelType=2',
        update: 'api/Model/UpdateOrSave',
        destroy: 'api/Model/Delete'
    }
});
Ext.define("app.store..ModelStore", {
    extend: "Ext.data.Store",
    alias: 'tableStore',
    model: "app.model.Model",
    nodeParam: 'ModelId',
    proxy: {
        type: 'ajax',
        reader: { type: 'json' },
        writer: { type: 'json' },
        api: {
            create: 'api/Model/SaveOrUpdate',
            read: 'api/Model/GetList?modelType=2',
            update: 'api/Model/SaveOrUpdate',
            destroy: 'api/Model/Delete'
        }
    },
    sorters: [{
        property: 'ModelId',
        direction: 'DESC'
    }]
});
Ext.define("app.store.ModelTreeStore", {
    extend: "Ext.data.TreeStore",
    alias: 'modelTreeStore',
    model: "app.model.Model",
    nodeParam: 'ModelId',
    proxy: {
        type: 'ajax',
        reader: { type: 'json' },
        writer: { type: 'json' },
        api: {
            read: 'api/Model/GetTree?modelType=2',
        }
    },
    sorters: [{
        property: 'name',
        direction: 'ASC'
    }],
    root: {
        text: 'Model', fullName: '.',
        ModelId: 0, checked: null
    }
});
Ext.define('app.view.ModelTabPanel', {
    extend: 'Ext.container.Container',
    alias: 'widget.modelTabPanel',
    layout: {
        type: 'border',
        align: 'stretch'
    },
    items: [{
        xtype: 'databaseTreePanel', region: 'west', width: 300, split: true, collapsible: true,
    }, {
        xtype: 'modelGrid', region: 'center',
    },
       {
           xtype: 'modelPropertyGrid', region: 'east', width: 300, split: true, collapsible: true,
       }]
});
Ext.define('app.view.ModelForm', {
    extend: 'Ext.form.Panel',
    alias: 'widget.modelForm',
    layout: 'form',
    bodyPadding: 5, height: 240,
    defaultType: 'textfield',
    items: [{
        xtype: "hidden",
        name: 'ModelId',
     
    },  {
        fieldLabel: 'ModelName',
        name: 'text',
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


Ext.define("app.view.model.ModelWindow", {
    extend: 'Ext.window.Window',
    alias: 'widget.modelWindow',
    iconCls: 'a_add',
    resizable: false,
    closable: true,            //是否可以关闭
    modal: true,                //是否为模态窗口
    initComponent: function () {
        var me = this;
        me.callParent(arguments);
    }
});
Ext.define('app.view.ModelPropertyGrid', {
    extend: 'Ext.grid.property.Grid',
    alias: 'widget.modelPropertyGrid',
    title: 'Model',
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

    initComponent: function () {
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
Ext.define('app.view.ModelGrid', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.modelGrid',
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
        var store = Ext.create('modelTreeStore');
        Ext.apply(me, {
            store: store,
            viewConfig: {
                stripeRows: true,
                enableTextSelection: true
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
            //width: 700,
            height: 200,
            frame: true, scrollable: true,
            tbar: [{
                text: 'Add',
                iconCls: '',
                handler: function () {
                    rowEditing.cancelEdit();
                    var r = Ext.create('app.model.Model', {
                        ModelId: 0,
                        ModelName: 'ModelName',
                    });
                    store.insert(0, r);
                    rowEditing.startEdit(0, 0);
                }
            },'-', {
                itemId: 'delete',
                text: 'Delete',
                iconCls: '',
                handler: function () {
                    var sm = grid.getSelectionModel();
                    rowEditing.cancelEdit();
                    store.remove(sm.getSelection());
                    if (store.getCount() > 0) {
                        sm.select(0);
                    }
                },
                disabled: true
            }, '-'],
            dockedItems: [{
                xtype: 'pagingtoolbar',
                store: store,   // same store GridPanel is using
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

Ext.define('app.view.ModelTreePanel', {
    extend: 'Ext.tree.Panel',
    alias: 'widget.modelTreePanel',
    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            title: 'Model',
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
            store: Ext.create('modelTreeStore'),
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