Ext.define('app.view.main.Template', {
    extend: 'Ext.tree.Panel',
    alias: 'widget.template',
    initComponent: function () {
        var me = this;
        var cmenu = Ext.create('widget.templateMenu');
        me.contextMenu = cmenu;
        Ext.apply(me, {
            title: 'Template',
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
            }, '-', {
                text: 'Import',
                handler: function () {
                    me.collapseAll();
                    me.getRootNode().expand();
                }
            }, '-'],
            useArrows: true, animate: true,
            store: Ext.create('TemplateTreeStore'),
            listeners: {
                checkchange: function (node, checked) {
                    // node.
                    node.expand();
                    node.set('checked', checked);
                    node.eachChild(function (child) {
                        child.set('checked', checked);
                        child.fireEvent('checkchange', child, checked);
                    });
                },
                render: function () {
                    Ext.getBody().on("contextmenu", Ext.emptyFn, null, { preventDefault: true });
                },
                itemcontextmenu: function (tree, record, item, index, e, eOpts) {
                    e.preventDefault();
                    if (record.isLeaf()) {
                        cmenu.items.items.forEach(function (element, index, array) {
                            if (element.getItemId().indexOf('edit') != -1) {
                                element.setVisible(true);
                            }
                            else {
                                element.setVisible(false);
                            }
                        });
                    }
                    else {
                        cmenu.items.items.forEach(function (element, index, array) {
                            if (element.getItemId().indexOf('edit') == -1) {
                                element.setVisible(true);
                            }
                            else {
                                element.setVisible(false);
                            }
                        });
                    }
                    cmenu.showAt(e.getXY());
                    e.stopEvent();
                }
            }
        });
        me.callParent(arguments);
    }
});
Ext.define("app.model.Template", {
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
        create: 'api/Template/createTemplate',
        read: 'api/Template/readTemplate',
        update: 'api/Template/updateTemplate',
        destroy: 'api/Template/destroyTemplate'
    }
});

Ext.define('app.view.TemplateView', {
    extend: 'Ext.grid.Panel',
});

Ext.define("app.store.TemplateTreeStore", {
    extend: "Ext.data.TreeStore",
    alias: 'TemplateTreeStore',
    model: "app.model.Template",
    nodeParam: 'path',
    proxy: {
        type: 'ajax',
        reader: { type: 'json' },
        writer: { type: 'json' },
        api: {
            create: 'api/Template/createTemplatel',
            read: 'api/Template/readTemplate',
            update: 'api/Template/updateTemplate',
            destroy: 'api/Template/destroyTemplate'
        }
    },
    sorters: [{
        property: 'text',
        direction: 'ASC'
    }],
    root: {
        text: 'Template',fullName:'.',
        id: '0', checked: null,
    }
});

Ext.define('app.view.template.menu', {
    extend: 'Ext.menu.Menu',
    alias: 'widget.templateMenu',
    width: 100,
    margin: '0 0 10 0',
    items: [{
        text: 'Edit', id: 'editTemplate', handler: function (widget, event) {
            addTab('66', '666', '');
        }
    },
    {
        text: 'Create', id: 'createTemplate', handler: function (widget, event) {
            addTab('66', '666', '');
        }
    },{
    text: 'Delete', id: 'deleteTemplate', handler: function (widget, event) {
    addTab('66', '666', '');
}
}]
});