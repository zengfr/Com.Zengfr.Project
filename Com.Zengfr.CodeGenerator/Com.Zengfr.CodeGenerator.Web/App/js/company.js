

Ext.define("app.model.Company", {
    extend: "Ext.data.Model",
    fields: [
        { name: 'CompanyId', type: 'int', mapping: 'CompanyId' },
        { name: 'CompanyName', type: 'string', mapping: 'CompanyName' },

        { name:'DataItemIsDeleted', type: 'date', mapping: 'DataItemIsDeleted' },
        { name:'DataItemStatus' ,type: 'date', mapping: 'DataItemStatus' },
        { name:'DataCreateLastTime' ,type: 'date', mapping: 'DataCreateLastTime' },
        { name:'DataChangeLastTime' ,type: 'date', mapping: 'DataChangeLastTime' },
    ],
    idProperty: 'CompanyId',
    api: {
        create: 'api/Company/SaveOrUpdate',
        read: 'api/Company/GetList',
        update: 'api/Company/SaveOrUpdate',
        destroy: 'api/Company/Delete'
    }
});
Ext.define("app.store.CompanyTreeStore", {
    extend: "Ext.data.TreeStore",
    alias: 'companyTreeStore',
    model: "app.model.Company",
    nodeParam: 'CompanyId',
    proxy: {
        type: 'ajax',
        reader: { type: 'json' },
        writer: { type: 'json' },
        api: {
            read: 'api/Company/GetTree',
        }
    },
    sorters: [{
        property: 'CompanyName',
        direction: 'ASC'
    }],
    root: {
        text: 'Company', fullName: '.',
        checked: null, CompanyId:0,
    }
});
Ext.define("app.store.CompanyStore", {
    extend: "Ext.data.Store",
    alias: 'companyStore',
    model: "app.model.Model",
    nodeParam: 'CompanyId',
    proxy: {
        type: 'ajax',
        reader: { type: 'json' },
        writer: { type: 'json' },
        api: {
            create: 'api/Company/SaveOrUpdate',
            read: 'api/Company/GetList',
            update: 'api/Company/SaveOrUpdate',
            destroy: 'api/Company/Delete'
        }
    },
    sorters: [{
        property: 'CompanyId',
        direction: 'DESC'
    }]
});
Ext.define('app.view.CompanyTabPanel', {
    extend: 'Ext.container.Container',
    alias: 'widget.companyTabPanel',
    layout: {
        type: 'border',
        align: 'stretch'
    },  
    items: [ {
        xtype: 'companyGrid', region: 'center',  
    },
       {
           xtype: 'companyPropertyGrid', region: 'east', width: 300, split: true, collapsible: true,
       }]
});
Ext.define('app.view.CompanyForm', {
    extend: 'Ext.form.Panel',
    alias: 'widget.companyForm',
    layout: 'form',
    defaultType: 'textfield',
    items: [{
        xtype: "hidden",
        name: 'CompanyId',
     
    }, {
        fieldLabel: 'CompanyName',
        name: 'CompanyName',
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
Ext.define('app.view.CompanyPropertyGrid', {
    extend: 'Ext.grid.property.Grid',
    alias: 'widget.companyPropertyGrid',
    title: 'Company',
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
                    'CompanyId': 'CompanyId',
                    'CompanyName': 'CompanyName',
                    },
                    //sourceConfig: {}
                }]
        });
        this.callParent(arguments);
    }
});
Ext.define('app.view.CompanyGrid', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.companyGrid',
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
        var store = Ext.create('companyStore');
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
                header: 'CompanyId',
                dataIndex: 'CompanyId',
                editor: {
                    allowBlank: false
                }
            }, {
                header: 'CompanyName',
                dataIndex: 'CompanyName',
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
                    var r = Ext.create('app.model.Company', {
                        CompanyId: this.cc,
                        CompanyName: 'CompanyName'
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
        store.loadPage(1);
        me.callParent(arguments);
        
    }
});


Ext.define('app.view.CompanyTreePanel', {
    extend: 'Ext.tree.Panel',
    alias: 'widget.companyTreePanel',
    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            title: 'Company',
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
            store: Ext.create('companyTreeStore'),
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