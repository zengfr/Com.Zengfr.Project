Ext.define('app.view.IndexViewport', {
    extend: 'Ext.container.Viewport',
    requires: [
       'app.view.main.Top',
       'app.view.main.Bottom',
       'app.view.main.Content'
    ],
    layout: 'border',
    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
               Ext.create('app.view.main.Top'),
               Ext.create('app.view.main.Bottom'),
               Ext.create('app.view.main.Content')
            ]
        });

        this.callParent(arguments);
    }

});

Ext.define('app.view.main.Top', {
    extend: 'Ext.Panel',
    alias: 'widget.top',
    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            region: 'north',
            height:28,
            layout: 'border',
            html:'',
            tbar: [ {
                text: 'Company',
                handler: function () {
                    addTab('companyTab', 'Company', 'widget.companyTabPanel', {});
                }
            }, '-', {
                text: 'Database',
                handler: function () {
                    addTab('databaseTab', 'Database', 'widget.databaseTabPanel', {});
                }
            }, '-', {
                text: 'Table',
                handler: function () {
                    addTab('tableTab', 'Table', 'widget.tableTabPanel', {});
                }
            }, '-', {
                text: 'Model',
                handler: function () {
                    addTab('modelTab', 'Model', 'widget.modelTabPanel', {});
                }
            },  '-', {
                text: 'Template',
                handler: function () {
                    addTab('templateTab', 'Template', 'widget.template', {});
                }
            }, '-', {
                text: 'Generator',
                handler: function () {
                    addTab('q55', '555', '');
                }
            }, '-', {
                text: 'GenTask',
                handler: function () {
                    addTab('q7', '7', 'widget.modelGrid', {});
                }
            },  '-'],

        });
        me.callParent(arguments);
    }
});

Ext.define('app.view.main.Bottom', {
    extend: 'Ext.Panel',
    alias: 'widget.bottom',
    initComponent: function () {
        var me = this;
        var scroller = Ext.create('Ext.scroll.Scroller');
        Ext.apply(me, {
            region: 'south',
            title: 'Log',
            height: 100,
            layout: 'border',
            collapsible: true,
            split: true,
            defaults: { border: false },
            scrollable: scroller,
            html: 'log', id: 'logPanel', logText: '',
        });
        me.callParent(arguments);
    },
    setLog: function (log) {
        var me = this;
        var v7 = new Date();
        me.logText = me.logText + Ext.util.Format.date(v7, "Y-m-d H:i:s") + ' ' + log + '<br/>';
        me.setHtml(me.logText);
    }
});

Ext.define('app.view.main.Content', {
    extend: 'Ext.tab.Panel',
    alias: 'widget.content',
    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            region: 'center',
            //title: 'center',
            layout: 'fit',
            id: 'tabPanel', 
            items: [{
                title: 'Home',
                loader: {
                    loadMask: 'loading...', scripts: false, autoLoad: true,
                    ajaxOptions: {
                        headers: {
                            'Access-Control-Allow-Origin': '*'
                        }
                    },
                    url: 'http://news.baidu.com/'
                }
            }],
            listeners: {
                beforetabchange: function (tabs, newTab, oldTab) {

                }
            },
        });
        me.callParent(arguments);
    }
});

