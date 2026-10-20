Ext.define('App.DatabaseStore', {
        extend: 'Ext.data.Store',
        pageSize: 10,
        model:'App.DatabaseModel',
        remoteSort:true,autoLoad:false,
        autoSync:false,autoSave:false,
        proxy :{
                type: 'ajax',simpleSortMode: true,
                url: 'Database_data.xml',
				api: {
    create  : '/Database/CreateDatabase',
    read    : '/Database/SlicedFindAllDatabase',
    update  : '/Database/UpdateDatabase',
    destroy : '/Database/DeleteDatabase'
              },reader:{
                    type: 'xml',
                    record:'Item',
                    root:'Response',
                    idProperty:'DatabaseID',
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
       //data:[       //      {DatabaseId:'0DatabaseId',Company:'0Company',DatabaseName:'0DatabaseName',DatabaseDescription:'0DatabaseDescription',DatabaseType:'0DatabaseType',ConnctionString:'0ConnctionString',DataItemIsDeleted:'0DataItemIsDeleted',DataItemStatus:'0DataItemStatus',DataCreateLastTime:'0DataCreateLastTime',DataChangeLastTime:'0DataChangeLastTime',},
       //        //      {DatabaseId:'1DatabaseId',Company:'1Company',DatabaseName:'1DatabaseName',DatabaseDescription:'1DatabaseDescription',DatabaseType:'1DatabaseType',ConnctionString:'1ConnctionString',DataItemIsDeleted:'1DataItemIsDeleted',DataItemStatus:'1DataItemStatus',DataCreateLastTime:'1DataCreateLastTime',DataChangeLastTime:'1DataChangeLastTime',},
       //        //      {DatabaseId:'2DatabaseId',Company:'2Company',DatabaseName:'2DatabaseName',DatabaseDescription:'2DatabaseDescription',DatabaseType:'2DatabaseType',ConnctionString:'2ConnctionString',DataItemIsDeleted:'2DataItemIsDeleted',DataItemStatus:'2DataItemStatus',DataCreateLastTime:'2DataCreateLastTime',DataChangeLastTime:'2DataChangeLastTime',},
       //        //      {DatabaseId:'3DatabaseId',Company:'3Company',DatabaseName:'3DatabaseName',DatabaseDescription:'3DatabaseDescription',DatabaseType:'3DatabaseType',ConnctionString:'3ConnctionString',DataItemIsDeleted:'3DataItemIsDeleted',DataItemStatus:'3DataItemStatus',DataCreateLastTime:'3DataCreateLastTime',DataChangeLastTime:'3DataChangeLastTime',},
       //        //      {DatabaseId:'4DatabaseId',Company:'4Company',DatabaseName:'4DatabaseName',DatabaseDescription:'4DatabaseDescription',DatabaseType:'4DatabaseType',ConnctionString:'4ConnctionString',DataItemIsDeleted:'4DataItemIsDeleted',DataItemStatus:'4DataItemStatus',DataCreateLastTime:'4DataCreateLastTime',DataChangeLastTime:'4DataChangeLastTime',},
       //        //      {DatabaseId:'5DatabaseId',Company:'5Company',DatabaseName:'5DatabaseName',DatabaseDescription:'5DatabaseDescription',DatabaseType:'5DatabaseType',ConnctionString:'5ConnctionString',DataItemIsDeleted:'5DataItemIsDeleted',DataItemStatus:'5DataItemStatus',DataCreateLastTime:'5DataCreateLastTime',DataChangeLastTime:'5DataChangeLastTime',},
       //        //      {DatabaseId:'6DatabaseId',Company:'6Company',DatabaseName:'6DatabaseName',DatabaseDescription:'6DatabaseDescription',DatabaseType:'6DatabaseType',ConnctionString:'6ConnctionString',DataItemIsDeleted:'6DataItemIsDeleted',DataItemStatus:'6DataItemStatus',DataCreateLastTime:'6DataCreateLastTime',DataChangeLastTime:'6DataChangeLastTime',},
       //        //      {DatabaseId:'7DatabaseId',Company:'7Company',DatabaseName:'7DatabaseName',DatabaseDescription:'7DatabaseDescription',DatabaseType:'7DatabaseType',ConnctionString:'7ConnctionString',DataItemIsDeleted:'7DataItemIsDeleted',DataItemStatus:'7DataItemStatus',DataCreateLastTime:'7DataCreateLastTime',DataChangeLastTime:'7DataChangeLastTime',},
       //        //      {DatabaseId:'8DatabaseId',Company:'8Company',DatabaseName:'8DatabaseName',DatabaseDescription:'8DatabaseDescription',DatabaseType:'8DatabaseType',ConnctionString:'8ConnctionString',DataItemIsDeleted:'8DataItemIsDeleted',DataItemStatus:'8DataItemStatus',DataCreateLastTime:'8DataCreateLastTime',DataChangeLastTime:'8DataChangeLastTime',},
       //        //      {DatabaseId:'9DatabaseId',Company:'9Company',DatabaseName:'9DatabaseName',DatabaseDescription:'9DatabaseDescription',DatabaseType:'9DatabaseType',ConnctionString:'9ConnctionString',DataItemIsDeleted:'9DataItemIsDeleted',DataItemStatus:'9DataItemStatus',DataCreateLastTime:'9DataCreateLastTime',DataChangeLastTime:'9DataChangeLastTime',},
       // ] 
    });
