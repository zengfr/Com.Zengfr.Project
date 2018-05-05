Ext.define('Pro.View.UserTest', {
    extend: 'Ext.container.Container',
    layout: { type: 'vbox',
        align: 'center',
        pack: 'center'
    },
        id: 'UserList', bodyPadding: 0, autoScroll: true,margin: '1 1',
        items: [{
            frame:true,height:64,layout: 'fit',width:'100%',
            items:[{
                    xtype: 'toolbar',
                    items:[{
                    text: 'Button',height:64
                    },
                    {
                    xtype: 'splitbutton',
                    text: 'Split Button', height: 64
                    },
                    '->', 
                    {
                    xtype    : 'textfield',
                    name     : 'field1',
                    emptyText: 'enter search term', height: 64
                    }]
              }]
           
        },{
		    xtype: 'Pro.Component.UserGridPanel', id: 'UserGridPanel', width: 896,
				            listeners: {
				                selectionchange: function(selModel, selected) {
		                            var len=selected.length;
		                            if(len==0) return;
		                            Ext.getCmp('UserInfoPanel').setActiveRecord(selected[len - 1] || null);
		                            Ext.getCmp('UserFormPanel').setActiveRecord(selected[len - 1] || null);
				                }
		                     }
	                   },
                     {
                         xtype: 'tabpanel', tabPosition: 'top', width: 896,
		            items: [{ xtype: 'Pro.Component.UserFormPanel', id: 'UserFormPanel',
				            	listeners: {
				                    create: function(form, data){
				                        Ext.getCmp('UserGridPanel').store.insert(0, data);
                                     }
                                }
                           },
		                  { xtype: 'Pro.Component.UserInfoPanel', id: 'UserInfoPanel' }
	                 ]
		          }
	       ]
   
});