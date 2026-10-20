Ext.define('App.ModelGridPanel', {
        extend: 'Ext.grid.Panel',
        loadMask: true,
        stripeRows: true,
        autoScroll: true,
        columnLines: true,
        filterable: false,
        scroll: 'both',
width:'100%',
height:398,
columns:[{xtype: 'rownumberer', header: '行号', width: 30, sortable: false}
          ,            {text:'ModelId',flex:1,width:125, dataIndex:'ModelId', sortable:'true'},//
                           {text:'Database',flex:1,width:125, dataIndex:'Database', sortable:'true'},//
                           {text:'ModelType',flex:1,width:125, dataIndex:'ModelType', sortable:'true'},//
                           {text:'ModelName',flex:1,width:125, dataIndex:'ModelName', sortable:'true'},//
                           {text:'ModelDescription',flex:1,width:125, dataIndex:'ModelDescription', sortable:'true'},//
                           {text:'DataItemIsDeleted',flex:1,width:125, dataIndex:'DataItemIsDeleted', sortable:'true'},//
                           {text:'DataItemStatus',flex:1,width:125, dataIndex:'DataItemStatus', sortable:'true'},//
                           {text:'DataCreateLastTime',flex:1,width:125, dataIndex:'DataCreateLastTime', sortable:'true'},//
                           {text:'DataChangeLastTime',flex:1,width:125, dataIndex:'DataChangeLastTime', sortable:'true'},//
                       ],
selModel: Ext.create('Ext.selection.CheckboxModel', {
                mode: 'MULTI', //'SINGLE'
          }),
 store: new App.ModelStore({
                 url: 'Model_data.xml'
            }),
onSelectChange: function (selModel, selections) {
        var f1 = selections.length ==1;
		var f2 = selections.length > 0;
        this.down('#editbtn').setDisabled(!f1);
        this.down('#deletebtn').setDisabled(!f2);
    },
	onAddBtnClick: function () {
        var rec = new this.store.model();
        this.store.insert(0, rec);
        this.getView().refresh();
        rec = null;
    },onDeleteBtnClick: function () {
        var selection = this.getView().getSelectionModel().getSelection();
        if (selection) {
            this.store.remove(selection);
        }
        selection = null;
    },
        initComponent : function() {
		var filtersCfg = {
    ftype: 'filters',
    autoReload: false, 
    local: true, 
     filters: [            {type:'Int64', dataIndex:'ModelId'},
                         {type:'Database', dataIndex:'Database'},
                         {type:'Int32', dataIndex:'ModelType'},
                         {type:'String', dataIndex:'ModelName'},
                         {type:'String', dataIndex:'ModelDescription'},
                         {type:'Boolean', dataIndex:'DataItemIsDeleted'},
                         {type:'Byte', dataIndex:'DataItemStatus'},
                         {type:'DateTime', dataIndex:'DataCreateLastTime'},
                         {type:'DateTime', dataIndex:'DataChangeLastTime'},
             ]
};
		   Ext.apply(this,{
		  // features: [filtersCfg]
		   });
            this.bbar=Ext.create('Ext.PagingToolbar', {
            displayInfo: true,store:this.store,
            displayMsg: '当前:{0}-{1} 总计:{2}',
            emptyMsg: "当前无数据",
            items:['-', {
					text: '新增', id: 'addbtn', scope:this,
				 handler: function () { 
						this.onAddBtnClick();
					}
				}, '-', {
					text: '编辑', id: 'editbtn', scope:this,
				 handler: function () { 
						//this.onEditBtnClick();
					}
				}, '-', {
					text: '删除', id: 'deletebtn',
					disabled: true, scope:this,
				 handler: function () { 
						this.onDeleteBtnClick();
					}
				},'-', {
					text: '存入', scope:this,
				 handler: function () { 
						this.store.save();
					}
				},'-']
             }),
            this.store.loadPage(1);
            this.getSelectionModel().on('selectionchange', this.onSelectChange, this);
			this.callParent();
        }
    });

