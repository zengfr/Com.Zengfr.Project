Ext.define('App.GenTaskStore', {
        extend: 'Ext.data.Store',
        pageSize: 10,
        model:'App.GenTaskModel',
        remoteSort:true,autoLoad:false,
        autoSync:false,autoSave:false,
        proxy :{
                type: 'ajax',simpleSortMode: true,
                url: 'GenTask_data.xml',
				api: {
    create  : '/GenTask/CreateGenTask',
    read    : '/GenTask/SlicedFindAllGenTask',
    update  : '/GenTask/UpdateGenTask',
    destroy : '/GenTask/DeleteGenTask'
              },reader:{
                    type: 'xml',
                    record:'Item',
                    root:'Response',
                    idProperty:'GenTaskID',
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
       //data:[       //      {GenTaskId:'0GenTaskId',Result:'0Result',ResultStatus:'0ResultStatus',Description:'0Description',DataItemIsDeleted:'0DataItemIsDeleted',DataItemStatus:'0DataItemStatus',DataCreateLastTime:'0DataCreateLastTime',DataChangeLastTime:'0DataChangeLastTime',},
       //        //      {GenTaskId:'1GenTaskId',Result:'1Result',ResultStatus:'1ResultStatus',Description:'1Description',DataItemIsDeleted:'1DataItemIsDeleted',DataItemStatus:'1DataItemStatus',DataCreateLastTime:'1DataCreateLastTime',DataChangeLastTime:'1DataChangeLastTime',},
       //        //      {GenTaskId:'2GenTaskId',Result:'2Result',ResultStatus:'2ResultStatus',Description:'2Description',DataItemIsDeleted:'2DataItemIsDeleted',DataItemStatus:'2DataItemStatus',DataCreateLastTime:'2DataCreateLastTime',DataChangeLastTime:'2DataChangeLastTime',},
       //        //      {GenTaskId:'3GenTaskId',Result:'3Result',ResultStatus:'3ResultStatus',Description:'3Description',DataItemIsDeleted:'3DataItemIsDeleted',DataItemStatus:'3DataItemStatus',DataCreateLastTime:'3DataCreateLastTime',DataChangeLastTime:'3DataChangeLastTime',},
       //        //      {GenTaskId:'4GenTaskId',Result:'4Result',ResultStatus:'4ResultStatus',Description:'4Description',DataItemIsDeleted:'4DataItemIsDeleted',DataItemStatus:'4DataItemStatus',DataCreateLastTime:'4DataCreateLastTime',DataChangeLastTime:'4DataChangeLastTime',},
       //        //      {GenTaskId:'5GenTaskId',Result:'5Result',ResultStatus:'5ResultStatus',Description:'5Description',DataItemIsDeleted:'5DataItemIsDeleted',DataItemStatus:'5DataItemStatus',DataCreateLastTime:'5DataCreateLastTime',DataChangeLastTime:'5DataChangeLastTime',},
       //        //      {GenTaskId:'6GenTaskId',Result:'6Result',ResultStatus:'6ResultStatus',Description:'6Description',DataItemIsDeleted:'6DataItemIsDeleted',DataItemStatus:'6DataItemStatus',DataCreateLastTime:'6DataCreateLastTime',DataChangeLastTime:'6DataChangeLastTime',},
       //        //      {GenTaskId:'7GenTaskId',Result:'7Result',ResultStatus:'7ResultStatus',Description:'7Description',DataItemIsDeleted:'7DataItemIsDeleted',DataItemStatus:'7DataItemStatus',DataCreateLastTime:'7DataCreateLastTime',DataChangeLastTime:'7DataChangeLastTime',},
       //        //      {GenTaskId:'8GenTaskId',Result:'8Result',ResultStatus:'8ResultStatus',Description:'8Description',DataItemIsDeleted:'8DataItemIsDeleted',DataItemStatus:'8DataItemStatus',DataCreateLastTime:'8DataCreateLastTime',DataChangeLastTime:'8DataChangeLastTime',},
       //        //      {GenTaskId:'9GenTaskId',Result:'9Result',ResultStatus:'9ResultStatus',Description:'9Description',DataItemIsDeleted:'9DataItemIsDeleted',DataItemStatus:'9DataItemStatus',DataCreateLastTime:'9DataCreateLastTime',DataChangeLastTime:'9DataChangeLastTime',},
       // ] 
    });
