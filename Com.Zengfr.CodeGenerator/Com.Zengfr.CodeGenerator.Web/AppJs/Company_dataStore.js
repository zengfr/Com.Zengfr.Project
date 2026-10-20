Ext.define('App.CompanyStore', {
        extend: 'Ext.data.Store',
        pageSize: 10,
        model:'App.CompanyModel',
        remoteSort:true,autoLoad:false,
        autoSync:false,autoSave:false,
        proxy :{
                type: 'ajax',simpleSortMode: true,
                url: 'Company_data.xml',
				api: {
    create  : '/Company/CreateCompany',
    read    : '/Company/SlicedFindAllCompany',
    update  : '/Company/UpdateCompany',
    destroy : '/Company/DeleteCompany'
              },reader:{
                    type: 'xml',
                    record:'Item',
                    root:'Response',
                    idProperty:'CompanyID',
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
       //data:[       //      {CompanyId:'0CompanyId',CompanyName:'0CompanyName',DataItemIsDeleted:'0DataItemIsDeleted',DataItemStatus:'0DataItemStatus',DataCreateLastTime:'0DataCreateLastTime',DataChangeLastTime:'0DataChangeLastTime',},
       //        //      {CompanyId:'1CompanyId',CompanyName:'1CompanyName',DataItemIsDeleted:'1DataItemIsDeleted',DataItemStatus:'1DataItemStatus',DataCreateLastTime:'1DataCreateLastTime',DataChangeLastTime:'1DataChangeLastTime',},
       //        //      {CompanyId:'2CompanyId',CompanyName:'2CompanyName',DataItemIsDeleted:'2DataItemIsDeleted',DataItemStatus:'2DataItemStatus',DataCreateLastTime:'2DataCreateLastTime',DataChangeLastTime:'2DataChangeLastTime',},
       //        //      {CompanyId:'3CompanyId',CompanyName:'3CompanyName',DataItemIsDeleted:'3DataItemIsDeleted',DataItemStatus:'3DataItemStatus',DataCreateLastTime:'3DataCreateLastTime',DataChangeLastTime:'3DataChangeLastTime',},
       //        //      {CompanyId:'4CompanyId',CompanyName:'4CompanyName',DataItemIsDeleted:'4DataItemIsDeleted',DataItemStatus:'4DataItemStatus',DataCreateLastTime:'4DataCreateLastTime',DataChangeLastTime:'4DataChangeLastTime',},
       //        //      {CompanyId:'5CompanyId',CompanyName:'5CompanyName',DataItemIsDeleted:'5DataItemIsDeleted',DataItemStatus:'5DataItemStatus',DataCreateLastTime:'5DataCreateLastTime',DataChangeLastTime:'5DataChangeLastTime',},
       //        //      {CompanyId:'6CompanyId',CompanyName:'6CompanyName',DataItemIsDeleted:'6DataItemIsDeleted',DataItemStatus:'6DataItemStatus',DataCreateLastTime:'6DataCreateLastTime',DataChangeLastTime:'6DataChangeLastTime',},
       //        //      {CompanyId:'7CompanyId',CompanyName:'7CompanyName',DataItemIsDeleted:'7DataItemIsDeleted',DataItemStatus:'7DataItemStatus',DataCreateLastTime:'7DataCreateLastTime',DataChangeLastTime:'7DataChangeLastTime',},
       //        //      {CompanyId:'8CompanyId',CompanyName:'8CompanyName',DataItemIsDeleted:'8DataItemIsDeleted',DataItemStatus:'8DataItemStatus',DataCreateLastTime:'8DataCreateLastTime',DataChangeLastTime:'8DataChangeLastTime',},
       //        //      {CompanyId:'9CompanyId',CompanyName:'9CompanyName',DataItemIsDeleted:'9DataItemIsDeleted',DataItemStatus:'9DataItemStatus',DataCreateLastTime:'9DataCreateLastTime',DataChangeLastTime:'9DataChangeLastTime',},
       // ] 
    });
