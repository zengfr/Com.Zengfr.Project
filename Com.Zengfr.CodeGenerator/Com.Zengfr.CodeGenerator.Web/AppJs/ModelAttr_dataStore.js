Ext.define('App.ModelAttrStore', {
        extend: 'Ext.data.Store',
        pageSize: 10,
        model:'App.ModelAttrModel',
        remoteSort:true,autoLoad:false,
        autoSync:false,autoSave:false,
        proxy :{
                type: 'ajax',simpleSortMode: true,
                url: 'ModelAttr_data.xml',
				api: {
    create  : '/ModelAttr/CreateModelAttr',
    read    : '/ModelAttr/SlicedFindAllModelAttr',
    update  : '/ModelAttr/UpdateModelAttr',
    destroy : '/ModelAttr/DeleteModelAttr'
              },reader:{
                    type: 'xml',
                    record:'Item',
                    root:'Response',
                    idProperty:'ModelAttrID',
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
       //data:[       //      {ModelAttrId:'0ModelAttrId',Model:'0Model',AttrName:'0AttrName',AttDescription:'0AttDescription',TypeName:'0TypeName',Length:'0Length',DefaultValue:'0DefaultValue',IsUnique:'0IsUnique',IsKey:'0IsKey',IsID:'0IsID',IsIndex:'0IsIndex',DataItemIsDeleted:'0DataItemIsDeleted',DataItemStatus:'0DataItemStatus',DataCreateLastTime:'0DataCreateLastTime',DataChangeLastTime:'0DataChangeLastTime',},
       //        //      {ModelAttrId:'1ModelAttrId',Model:'1Model',AttrName:'1AttrName',AttDescription:'1AttDescription',TypeName:'1TypeName',Length:'1Length',DefaultValue:'1DefaultValue',IsUnique:'1IsUnique',IsKey:'1IsKey',IsID:'1IsID',IsIndex:'1IsIndex',DataItemIsDeleted:'1DataItemIsDeleted',DataItemStatus:'1DataItemStatus',DataCreateLastTime:'1DataCreateLastTime',DataChangeLastTime:'1DataChangeLastTime',},
       //        //      {ModelAttrId:'2ModelAttrId',Model:'2Model',AttrName:'2AttrName',AttDescription:'2AttDescription',TypeName:'2TypeName',Length:'2Length',DefaultValue:'2DefaultValue',IsUnique:'2IsUnique',IsKey:'2IsKey',IsID:'2IsID',IsIndex:'2IsIndex',DataItemIsDeleted:'2DataItemIsDeleted',DataItemStatus:'2DataItemStatus',DataCreateLastTime:'2DataCreateLastTime',DataChangeLastTime:'2DataChangeLastTime',},
       //        //      {ModelAttrId:'3ModelAttrId',Model:'3Model',AttrName:'3AttrName',AttDescription:'3AttDescription',TypeName:'3TypeName',Length:'3Length',DefaultValue:'3DefaultValue',IsUnique:'3IsUnique',IsKey:'3IsKey',IsID:'3IsID',IsIndex:'3IsIndex',DataItemIsDeleted:'3DataItemIsDeleted',DataItemStatus:'3DataItemStatus',DataCreateLastTime:'3DataCreateLastTime',DataChangeLastTime:'3DataChangeLastTime',},
       //        //      {ModelAttrId:'4ModelAttrId',Model:'4Model',AttrName:'4AttrName',AttDescription:'4AttDescription',TypeName:'4TypeName',Length:'4Length',DefaultValue:'4DefaultValue',IsUnique:'4IsUnique',IsKey:'4IsKey',IsID:'4IsID',IsIndex:'4IsIndex',DataItemIsDeleted:'4DataItemIsDeleted',DataItemStatus:'4DataItemStatus',DataCreateLastTime:'4DataCreateLastTime',DataChangeLastTime:'4DataChangeLastTime',},
       //        //      {ModelAttrId:'5ModelAttrId',Model:'5Model',AttrName:'5AttrName',AttDescription:'5AttDescription',TypeName:'5TypeName',Length:'5Length',DefaultValue:'5DefaultValue',IsUnique:'5IsUnique',IsKey:'5IsKey',IsID:'5IsID',IsIndex:'5IsIndex',DataItemIsDeleted:'5DataItemIsDeleted',DataItemStatus:'5DataItemStatus',DataCreateLastTime:'5DataCreateLastTime',DataChangeLastTime:'5DataChangeLastTime',},
       //        //      {ModelAttrId:'6ModelAttrId',Model:'6Model',AttrName:'6AttrName',AttDescription:'6AttDescription',TypeName:'6TypeName',Length:'6Length',DefaultValue:'6DefaultValue',IsUnique:'6IsUnique',IsKey:'6IsKey',IsID:'6IsID',IsIndex:'6IsIndex',DataItemIsDeleted:'6DataItemIsDeleted',DataItemStatus:'6DataItemStatus',DataCreateLastTime:'6DataCreateLastTime',DataChangeLastTime:'6DataChangeLastTime',},
       //        //      {ModelAttrId:'7ModelAttrId',Model:'7Model',AttrName:'7AttrName',AttDescription:'7AttDescription',TypeName:'7TypeName',Length:'7Length',DefaultValue:'7DefaultValue',IsUnique:'7IsUnique',IsKey:'7IsKey',IsID:'7IsID',IsIndex:'7IsIndex',DataItemIsDeleted:'7DataItemIsDeleted',DataItemStatus:'7DataItemStatus',DataCreateLastTime:'7DataCreateLastTime',DataChangeLastTime:'7DataChangeLastTime',},
       //        //      {ModelAttrId:'8ModelAttrId',Model:'8Model',AttrName:'8AttrName',AttDescription:'8AttDescription',TypeName:'8TypeName',Length:'8Length',DefaultValue:'8DefaultValue',IsUnique:'8IsUnique',IsKey:'8IsKey',IsID:'8IsID',IsIndex:'8IsIndex',DataItemIsDeleted:'8DataItemIsDeleted',DataItemStatus:'8DataItemStatus',DataCreateLastTime:'8DataCreateLastTime',DataChangeLastTime:'8DataChangeLastTime',},
       //        //      {ModelAttrId:'9ModelAttrId',Model:'9Model',AttrName:'9AttrName',AttDescription:'9AttDescription',TypeName:'9TypeName',Length:'9Length',DefaultValue:'9DefaultValue',IsUnique:'9IsUnique',IsKey:'9IsKey',IsID:'9IsID',IsIndex:'9IsIndex',DataItemIsDeleted:'9DataItemIsDeleted',DataItemStatus:'9DataItemStatus',DataCreateLastTime:'9DataCreateLastTime',DataChangeLastTime:'9DataChangeLastTime',},
       // ] 
    });
