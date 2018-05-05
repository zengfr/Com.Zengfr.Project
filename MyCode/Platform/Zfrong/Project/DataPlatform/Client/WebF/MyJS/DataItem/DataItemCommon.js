/*--------------------ModelDefine--------------------*/
Ext.define("DataItemCommon",{
    StoreApi: { read: '/DataItem/SlicedFindAll',
        create: '/DataItem/Create',
        update: '/DataItem/Update',
        destroy: '/DataItem/Delete'
    },
    PVStoreUrl: '/DataItem/SlicedFindAllPV?property=Name',
    PVDistinctStoreUrl: '/DataItem/SlicedFindAllPVDistinct?property=',
    Fields: [{ name: 'ID', type: 'int' },
    { name: 'URL', type: 'string' },
    { name: 'Title', type: 'string' },
    { name: 'Content', type: 'string' },
    { name: 'SearcherType', type: 'int' },
    { name: 'MediaType', type: 'int' },
    { name: 'GoodOrBad', type: 'int' },

    { name: 'Adversary', type: 'bool' },
        { name: 'UserID', type: 'int' },
        { name: 'DoState.IsActive', mapping: 'DoState.IsActive', type: 'bool', defaultValue: false },
        { name: 'DoState.IsDelete', mapping: 'DoState.IsDelete', type: 'bool', defaultValue: false },
        { name: 'DoState.CheckStatus', mapping: 'DoState.CheckStatus', type: 'int', defaultValue: 0 },
        { name: 'DoState.Status', mapping: 'DoState.Status', type: 'int', defaultValue: 0}]
});

var dataItemCommon=Ext.create('DataItemCommon');

ModelDefineFactory('DataItemModel', dataItemCommon.Fields);
/*--------------------StoreDefine--------------------*/
StoreDefineFactory('DataItemStore','DataItemModel',dataItemCommon.StoreApi);
PVStoreDefineFactory('DataItemPStore', 'App.KeyValueModel', { read: 'http://127.0.0.1/' });
PVStoreDefineFactory('DataItemPIDStore', 'App.KeyIDModel', { read: dataItemCommon.PVStoreUrl });

/*--------------------InfoPanelDefine--------------------*/

var dataItemInfoPanelTpl = '<div>ID:{ID}</div><div>Title:{Title}</div><div>URL:{URL}</div>';
  InfoPanelDefineFactory('DataItemInfoPanel', dataItemInfoPanelTpl);
 /*--------------------FormPanelDefine--------------------*/
 var dataItemFormItems = [
      { fieldLabel: 'GoodOrBad', xtype: 'checkboxfield', name: 'Adversary', allowBlank: false },
	   
	];
 FormPanelDefineFactory('DataItemForm', dataItemFormItems);
 /*--------------------GridPanelDefine--------------------*/
 var dataItemColumns = [{
                xtype: 'rownumberer', header: '行号', width: 30, sortable: false
            }
            , {
                header: 'ID', dataIndex: 'ID', width: 36, filter: {}
            }, {
                dataIndex: 'Title', header: '标题', width: 270, filter: {}
            }, {
                dataIndex: 'Content', header: 'Content', width: 270, filter: {}
            }
             , {
                 dataIndex: 'URL', header: 'URL', width: 70, filter: {}
             }
            ];
 GridPanelDefineFactory('DataItemGrid', dataItemColumns, 'DataItemStore');