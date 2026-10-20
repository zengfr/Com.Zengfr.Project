Ext.define('App.ModelStore', {
        extend: 'Ext.data.Store',
        pageSize: 10,
        model:'App.ModelModel',
        remoteSort:true,autoLoad:false,
        autoSync:false,autoSave:false,
        proxy :{
                type: 'ajax',simpleSortMode: true,
                url: 'Model_data.xml',
				api: {
    create  : '/Model/CreateModel',
    read    : '/Model/SlicedFindAllModel',
    update  : '/Model/UpdateModel',
    destroy : '/Model/DeleteModel'
              },reader:{
                    type: 'xml',
                    record:'Item',
                    root:'Response',
                    idProperty:'ModelID',
                    successProperty: 'Success',messageProperty: 'Message',
                    totalProperty: 'TotalCount'
				},
				writer:{
                    type: 'json',
                    writeAllFields: false, listful: true, encode: false, allowSingle: false,
                    nameProperty: 'data',
					writeRecordId:true
				},
		create: function (operation, callback, scope) {
            return this.doRequest.apply(this, [operation, this.showMessage, scope]);
        },
        destroy: function (operation, callback, scope) {
            return this.doRequest.apply(this, [operation, this.showMessage, scope]);
        },
        update: function (operation, callback, scope) {
            return this.doRequest.apply(this, [operation, this.showMessage, scope]);
        },
        showMessage: function (operation) {
            if (operation != null) {
                var error = "";
                if (!operation.wasSuccessful())
                    error = "错误:" + operation.getError();
                this.proxy.showMessagePanel("提示信息", "操作:{0} 成功:{1} {2}", operation.action, operation.wasSuccessful(), error);
            }
         },
		showMessagePanel:function(title, format){
		   var msgCt=Ext.get("msg-div");
            if(!msgCt){
                msgCt = Ext.core.DomHelper.insertFirst(document.body, {id:'msg-div'}, true);
            }
            var s = Ext.String.format.apply(String, Array.prototype.slice.call(arguments, 1));
            var m = Ext.core.DomHelper.append(msgCt, createBox(title, s), true);
            m.hide();
            m.slideIn('t').ghost("t", { delay: 999, remove: true});
		  }
		},
       //data:[       //      {ModelId:'0ModelId',Database:'0Database',ModelType:'0ModelType',ModelName:'0ModelName',ModelDescription:'0ModelDescription',DataItemIsDeleted:'0DataItemIsDeleted',DataItemStatus:'0DataItemStatus',DataCreateLastTime:'0DataCreateLastTime',DataChangeLastTime:'0DataChangeLastTime',},
       //        //      {ModelId:'1ModelId',Database:'1Database',ModelType:'1ModelType',ModelName:'1ModelName',ModelDescription:'1ModelDescription',DataItemIsDeleted:'1DataItemIsDeleted',DataItemStatus:'1DataItemStatus',DataCreateLastTime:'1DataCreateLastTime',DataChangeLastTime:'1DataChangeLastTime',},
       //        //      {ModelId:'2ModelId',Database:'2Database',ModelType:'2ModelType',ModelName:'2ModelName',ModelDescription:'2ModelDescription',DataItemIsDeleted:'2DataItemIsDeleted',DataItemStatus:'2DataItemStatus',DataCreateLastTime:'2DataCreateLastTime',DataChangeLastTime:'2DataChangeLastTime',},
       //        //      {ModelId:'3ModelId',Database:'3Database',ModelType:'3ModelType',ModelName:'3ModelName',ModelDescription:'3ModelDescription',DataItemIsDeleted:'3DataItemIsDeleted',DataItemStatus:'3DataItemStatus',DataCreateLastTime:'3DataCreateLastTime',DataChangeLastTime:'3DataChangeLastTime',},
       //        //      {ModelId:'4ModelId',Database:'4Database',ModelType:'4ModelType',ModelName:'4ModelName',ModelDescription:'4ModelDescription',DataItemIsDeleted:'4DataItemIsDeleted',DataItemStatus:'4DataItemStatus',DataCreateLastTime:'4DataCreateLastTime',DataChangeLastTime:'4DataChangeLastTime',},
       //        //      {ModelId:'5ModelId',Database:'5Database',ModelType:'5ModelType',ModelName:'5ModelName',ModelDescription:'5ModelDescription',DataItemIsDeleted:'5DataItemIsDeleted',DataItemStatus:'5DataItemStatus',DataCreateLastTime:'5DataCreateLastTime',DataChangeLastTime:'5DataChangeLastTime',},
       //        //      {ModelId:'6ModelId',Database:'6Database',ModelType:'6ModelType',ModelName:'6ModelName',ModelDescription:'6ModelDescription',DataItemIsDeleted:'6DataItemIsDeleted',DataItemStatus:'6DataItemStatus',DataCreateLastTime:'6DataCreateLastTime',DataChangeLastTime:'6DataChangeLastTime',},
       //        //      {ModelId:'7ModelId',Database:'7Database',ModelType:'7ModelType',ModelName:'7ModelName',ModelDescription:'7ModelDescription',DataItemIsDeleted:'7DataItemIsDeleted',DataItemStatus:'7DataItemStatus',DataCreateLastTime:'7DataCreateLastTime',DataChangeLastTime:'7DataChangeLastTime',},
       //        //      {ModelId:'8ModelId',Database:'8Database',ModelType:'8ModelType',ModelName:'8ModelName',ModelDescription:'8ModelDescription',DataItemIsDeleted:'8DataItemIsDeleted',DataItemStatus:'8DataItemStatus',DataCreateLastTime:'8DataCreateLastTime',DataChangeLastTime:'8DataChangeLastTime',},
       //        //      {ModelId:'9ModelId',Database:'9Database',ModelType:'9ModelType',ModelName:'9ModelName',ModelDescription:'9ModelDescription',DataItemIsDeleted:'9DataItemIsDeleted',DataItemStatus:'9DataItemStatus',DataCreateLastTime:'9DataCreateLastTime',DataChangeLastTime:'9DataChangeLastTime',},
       // ] 
    });
