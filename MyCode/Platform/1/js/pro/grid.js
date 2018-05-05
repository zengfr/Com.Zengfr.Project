
Ext.onReady(function () {

var formPanel=Ext.create('UserForm',
			{id:'UserFormPanel',
	         height:210,
	         listeners: {
						create: function(form, data){
							Ext.getCmp('UserGridPanel').store.insert(0, data);
						 }
					}
			});
var infoPanel=Ext.create('UserInfoPanel',{id:'UserInfoPanel',height:210});

var gridPanel=Ext.create('UserGrid',{
		      renderTo: 'maincontainer',height:290,
			  listeners: {
							selectionchange: function(selModel, selected) {
								var len=selected.length;
								if(len==0) return;
								Ext.getCmp('UserFormPanel').setActiveRecord(selected[len - 1] || null);
								Ext.getCmp('UserInfoPanel').setActiveRecord(selected[len - 1] || null);
						     }
					   }
			 });	 
	Ext.suspendLayouts();
    gridPanel.createPagingToolbar();
	gridPanel.createToolbar();
	formPanel.createBar();

	var tabPanel=Ext.create('Ext.tab.Panel',{renderTo: 'maincontainer2',
		items: [formPanel,infoPanel]});
    Ext.resumeLayouts(true);
	

 });