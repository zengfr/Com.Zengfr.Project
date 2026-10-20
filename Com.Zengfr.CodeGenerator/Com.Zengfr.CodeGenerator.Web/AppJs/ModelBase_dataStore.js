Ext.define('App.ModelBaseStore', {
        extend: 'Ext.data.Store',
        pageSize: 10,
        model:'App.ModelBaseModel',
        remoteSort:true,autoLoad:false,
        autoSync:false,autoSave:false,
        proxy :{
                type: 'ajax',simpleSortMode: true,
                url: 'ModelBase_data.xml',
				api: {
    create  : '/ModelBase/CreateModelBase',
    read    : '/ModelBase/SlicedFindAllModelBase',
    update  : '/ModelBase/UpdateModelBase',
    destroy : '/ModelBase/DeleteModelBase'
              },reader:{
                    type: 'xml',
                    record:'Item',
                    root:'Response',
                    idProperty:'ModelBaseID',
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
       //data:[       //      {DataItemIsDeleted:'0DataItemIsDeleted',DataItemStatus:'0DataItemStatus',DataCreateLastTime:'0DataCreateLastTime',DataChangeLastTime:'0DataChangeLastTime',},
       //        //      {DataItemIsDeleted:'1DataItemIsDeleted',DataItemStatus:'1DataItemStatus',DataCreateLastTime:'1DataCreateLastTime',DataChangeLastTime:'1DataChangeLastTime',},
       //        //      {DataItemIsDeleted:'2DataItemIsDeleted',DataItemStatus:'2DataItemStatus',DataCreateLastTime:'2DataCreateLastTime',DataChangeLastTime:'2DataChangeLastTime',},
       //        //      {DataItemIsDeleted:'3DataItemIsDeleted',DataItemStatus:'3DataItemStatus',DataCreateLastTime:'3DataCreateLastTime',DataChangeLastTime:'3DataChangeLastTime',},
       //        //      {DataItemIsDeleted:'4DataItemIsDeleted',DataItemStatus:'4DataItemStatus',DataCreateLastTime:'4DataCreateLastTime',DataChangeLastTime:'4DataChangeLastTime',},
       //        //      {DataItemIsDeleted:'5DataItemIsDeleted',DataItemStatus:'5DataItemStatus',DataCreateLastTime:'5DataCreateLastTime',DataChangeLastTime:'5DataChangeLastTime',},
       //        //      {DataItemIsDeleted:'6DataItemIsDeleted',DataItemStatus:'6DataItemStatus',DataCreateLastTime:'6DataCreateLastTime',DataChangeLastTime:'6DataChangeLastTime',},
       //        //      {DataItemIsDeleted:'7DataItemIsDeleted',DataItemStatus:'7DataItemStatus',DataCreateLastTime:'7DataCreateLastTime',DataChangeLastTime:'7DataChangeLastTime',},
       //        //      {DataItemIsDeleted:'8DataItemIsDeleted',DataItemStatus:'8DataItemStatus',DataCreateLastTime:'8DataCreateLastTime',DataChangeLastTime:'8DataChangeLastTime',},
       //        //      {DataItemIsDeleted:'9DataItemIsDeleted',DataItemStatus:'9DataItemStatus',DataCreateLastTime:'9DataCreateLastTime',DataChangeLastTime:'9DataChangeLastTime',},
       // ] 
    });
