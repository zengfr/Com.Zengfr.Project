/*--------------------ModelDefine--------------------*/
Ext.define("ProductCommon",{
    StoreApi: { read: '/Product/SlicedFindAll',
        create: '/Product/Create',
        update: '/Product/Update',
        destroy: '/Product/Delete'
    },
    PVStoreUrl: '/Product/SlicedFindAllPV?property=Name',
    PVDistinctStoreUrl: '/Product/SlicedFindAllPVDistinct?property=',
    Fields: [{ name: 'ID', type: 'int' },
    { name: 'Name', type: 'string' },

        { name: 'Adversary', type: 'bool' },
        { name: 'Brand.ID', mapping: 'Brand.ID', type: 'int' },
        { name: 'Brand.Name', mapping: 'Brand.Name', type: 'string' },
        { name: 'UserID', type: 'int' },
        { name: 'DoState.IsActive', mapping: 'DoState.IsActive', type: 'bool', defaultValue: false },
        { name: 'DoState.IsDelete', mapping: 'DoState.IsDelete', type: 'bool', defaultValue: false },
        { name: 'DoState.CheckStatus', mapping: 'DoState.CheckStatus', type: 'int', defaultValue: 0 },
        { name: 'DoState.Status', mapping: 'DoState.Status', type: 'int', defaultValue: 0}]
});

var productCommon=Ext.create('ProductCommon');

ModelDefineFactory('ProductModel', productCommon.Fields);
/*--------------------StoreDefine--------------------*/
StoreDefineFactory('ProductStore','ProductModel',productCommon.StoreApi);
PVStoreDefineFactory('ProductPStore','App.KeyValueModel',{read:'http://127.0.0.1/'});
PVStoreDefineFactory('ProductPIDStore', 'App.KeyIDModel', { read: productCommon.PVStoreUrl });
/*--------------------InfoPanelDefine--------------------*/

  var  productInfoPanelTpl='<div>ID:{ID}</div><div>产品名称:{Name}</div><div>竞争对手:{Adversary}</div>';
  InfoPanelDefineFactory('ProductInfoPanel', productInfoPanelTpl);
 /*--------------------FormPanelDefine--------------------*/
 var productFormItems = [
       ComboFactory('品牌名称', 'Brand.ID', 'BrandPIDStore', brandCommon.PVStoreUrl, false),
       ComboFactory('产品名称', 'Name', 'ProductPStore', productCommon.PVDistinctStoreUrl + 'Name',true),
      { fieldLabel: '竞争对手', xtype: 'checkboxfield', name: 'Adversary', allowBlank: false },
	   
	];
 FormPanelDefineFactory('ProductForm', productFormItems);
 /*--------------------GridPanelDefine--------------------*/
 var productColumns = [{
                xtype: 'rownumberer', header: '行号', width: 30, sortable: false
            }
            , {
                header: 'ID', dataIndex: 'ID', width: 36, filter: {}
            }, {
                dataIndex: 'Name', header: '产品名称', width: 270, filter: {}
            }, {
                dataIndex: 'Brand.Name', header: '品牌名称', width: 270, filter: {}
            }
             , {
                 dataIndex: 'Adversary', header: '竞争对手', width: 70, filter: {}
             }
            ];
 GridPanelDefineFactory('ProductGrid', productColumns, 'ProductStore');