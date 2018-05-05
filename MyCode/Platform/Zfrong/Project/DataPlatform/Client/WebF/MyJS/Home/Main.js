Ext.define('App.Main', {
    extend: 'Ext.container.Viewport',
    initComponent: function () {
        Ext.apply(this, {
            layout: 'border',defaultType:'container',
            items: [
            {
                region: 'north',
                maxSize: 200,
                height: 26,
                items:[{
                    xtype: 'toolbar',margin:1,
                    items:[
                    { 
                       xtype: 'button', text: '权限平台',
                        handler: function () {
                            var p = Ext.getCmp('TreePanel');
                            var store = p.view.getTreeStore();
                            store.proxy.url = "/Home/treedata";
                            store.load(); 
                            p.view.refresh(); p.ownerCt.ownerCt.ownerCt.doLayout();
                        }
                    }
                        , { xtype: 'tbseparator' }
                        , { xtype: 'button', text: '监测平台',
                            handler: function () {
                                var p = Ext.getCmp('TreePanel');
                                var store = p.view.getTreeStore();
                                store.proxy.url = "/Home/treedata?id=2";
                                store.load(); 
                                p.view.refresh(); p.ownerCt.ownerCt.ownerCt.doLayout();
                            }
                        }
                        , { xtype: 'tbseparator' }, { xtype: 'button', text: '退出',
                            handler: function () {
                                document.location = "/Home/Out";
                            }
                        }, { xtype: 'tbseparator'}]
                }]
            }
            , {
                region: 'west',
                id: 'west-panel',
                split: true,
                width: 200,
                minWidth: 156,
                maxWidth: 400, 
                layout: 'accordion', 
                items: [{
                    title: '导航栏', margin: '5 0',
                    iconCls: 'info',layout:'fit',
                    items: [{xtype: 'TreePanel',id:'TreePanel', border: 0}]
                }
			   ]
            },
            Ext.create('Ext.tab.Panel', {
                id: 'center-tabpanel',
                region:'center',
                deferredRender:false,
                activeTab: 0,
                items:[{contentEl: 'center2',title: 'Main'}],
                loadTab: function (rec) {
					var url=rec.get('url');
					if(url==null)return;
					if(url=='#0'){alert('无权限访问!');return;}
                    var id = 'tab_' + rec.get('id');
                    var w = Ext.getCmp('center-tabpanel');
                    AddTab(w, rec.get('text'), id, true, url);
                    id = null; w = null;
                }
            })
		]
        });
        this.callParent();
    }
});
