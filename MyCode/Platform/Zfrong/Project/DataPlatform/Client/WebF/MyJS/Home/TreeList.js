Ext.define('App.TreePanelBase', {
    extend: 'Ext.tree.Panel',
    alias: 'widget.TreePanelBase',
    rootVisible: true,
    bodyBorder: false,
    lines: true,
    minWidth: 200,
    defaultForum:171,
    split: true,useArrows:true,iconCls:"icon-class",
    //enableDD: true,
    
    initComponent: function () {
        Ext.apply(this, {
            viewConfig: {autoScroll: true,
                getRowClass: function (record) {
                    if (!record.get('leaf')) {
                        return 'forum-parent';
                    }
                }
            },
			width: 'auto',animate: true,autoScroll:false,
            dockedItems: [{
                xtype: 'toolbar',
                items: [{
                    text: '展开',
                    handler: function () {
                        var tree = this.ownerCt.ownerCt;
                        tree.expandAll();
                    } 
                }, {
                    text: '合并',
                    handler: function () {
                        var tree = this.ownerCt.ownerCt;
                        tree.collapseAll();
                    }
                }, {
                    text: '刷新',
                    handler: function () {
                        var tree = this.ownerCt.ownerCt;
                        tree.getStore().load();
                    }
                }]
            }]
        });
        this.callParent(arguments);
        this.getSelectionModel().on({
            scope: this,
            select: this.onSelect
        });
    },
    onFirstLoad: function () {
        var rec = this.store.getNodeById(this.defaultForum);
        this.getSelectionModel().select(rec); rec = null;
    },
    onSelect: function (selModel, rec) {
        if (rec.get('leaf')) {
            var w = Ext.getCmp('center-tabpanel');
            w.loadTab(rec); w = null;
        }
    },
    listeners: {
       itemexpand: function (node, options) {
            if (!node.hasChildNodes()) {
                node.data.leaf = true;
                this.getView().refresh();
            }

        },
        itemclick: function (view, rec, item, index, e) {
            if (rec.get('leaf')) {
                var w = Ext.getCmp('center-tabpanel');
                w.loadTab(rec); w = null;
            }
        }
    }
});
Ext.define('App.UserTreePanel', {
    extend: 'App.TreePanelBase',
    alias: 'widget.UserTreePanel',
    initComponent: function () {
		this.store= new TreePanelStore();
        this.store.proxy.url='/Manager/PMenu/UserTreeNodes?node=0&treeId=UserTreePanel';
        this.callParent(arguments);
    }
});
Ext.define('App.TreePanel', {
    extend: 'App.TreePanelBase',
    alias: 'widget.TreePanel',
    initComponent: function () {
		this.store= new TreePanelStore();
        //this.store.proxy.url='/PMenu/UserTreeNodes?node=0&treeId=treelist1';
        this.callParent(arguments);
    }
});
