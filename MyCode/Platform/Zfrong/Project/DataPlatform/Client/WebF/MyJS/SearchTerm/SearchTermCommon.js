/*--------------------ModelDefine--------------------*/
Ext.define("SearchTermCommon",{
    StoreApi: { read: '/SearchTerm/SlicedFindAll',
        create: '/SearchTerm/Create',
        update: '/SearchTerm/Update',
        destroy: '/SearchTerm/Delete'
    },
    PVStoreUrl: '/SearchTerm/SlicedFindAllPV?property=Name',
    PVDistinctStoreUrl: '/SearchTerm/SlicedFindAllPVDistinct?property=',
    Fields: [{ name: 'ID', type: 'int' },
    { name: 'Name', type: 'string' },

        { name: 'Adversary', type: 'bool' },
        { name: 'Product.ID', mapping: 'Product.ID', type: 'int' },
        { name: 'Product.Name', mapping: 'Product.Name', type: 'string' },
        { name: 'UserID', type: 'int' },
        { name: 'DoState.IsActive', mapping: 'DoState.IsActive', type: 'bool', defaultValue: false },
        { name: 'DoState.IsDelete', mapping: 'DoState.IsDelete', type: 'bool', defaultValue: false },
        { name: 'DoState.CheckStatus', mapping: 'DoState.CheckStatus', type: 'int', defaultValue: 0 },
        { name: 'DoState.Status', mapping: 'DoState.Status', type: 'int', defaultValue: 0}]
});

var searchTermCommon=Ext.create('SearchTermCommon');

ModelDefineFactory('SearchTermModel', searchTermCommon.Fields);
/*--------------------StoreDefine--------------------*/
StoreDefineFactory('SearchTermStore','SearchTermModel',searchTermCommon.StoreApi);
PVStoreDefineFactory('SearchTermPStore','App.KeyValueModel',{read:'http://127.0.0.1/'});
PVStoreDefineFactory('SearchTermPIDStore', 'App.KeyIDModel', { read: searchTermCommon.PVStoreUrl });
/*--------------------InfoPanelDefine--------------------*/

  var  searchTermInfoPanelTpl='<div>ID:{ID}</div><div>关键字:{Name}</div><div>竞争对手:{Adversary}</div>';
  InfoPanelDefineFactory('SearchTermInfoPanel', searchTermInfoPanelTpl);
 /*--------------------FormPanelDefine--------------------*/
 var searchTermFormItems = [
       ComboFactory('产品名称', 'Product.ID', 'ProductPIDStore', productCommon.PVStoreUrl, false),
       ComboFactory('关键字', 'Name', 'SearchTermPStore', searchTermCommon.PVDistinctStoreUrl + 'Name',true),
      { fieldLabel: '竞争对手', xtype: 'checkboxfield', name: 'Adversary', allowBlank: false },
	   
	];
 FormPanelDefineFactory('SearchTermForm', searchTermFormItems);
 /*--------------------GridPanelDefine--------------------*/
 var searchTermColumns = [{
                xtype: 'rownumberer', header: '行号', width: 30, sortable: false
            }
            , {
                header: 'ID', dataIndex: 'ID', width: 36, filter: {}
            }, {
                dataIndex: 'Name', header: '关键字', width: 270, filter: {}
            }, {
                dataIndex: 'Product.Name', header: '产品名称', width: 270, filter: {}
            }
             , {
                 dataIndex: 'Adversary', header: '竞争对手', width: 70, filter: {}
             }
            ];
 GridPanelDefineFactory('SearchTermGrid', searchTermColumns, 'SearchTermStore');