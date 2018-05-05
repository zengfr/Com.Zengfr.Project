/*--------------------ModelDefine--------------------*/
Ext.define("BrandCommon",{
    StoreApi: { read: '/Brand/SlicedFindAll',
        create: '/Brand/Create',
        update: '/Brand/Update',
        destroy: '/Brand/Delete'
    },
    PVStoreUrl: '/Brand/SlicedFindAllPV?property=Name',
    PVDistinctStoreUrl: '/Brand/SlicedFindAllPVDistinct?property=',
    Fields: [{ name: 'ID', type: 'int' },
    { name: 'Name', type: 'string' },

        { name: 'Adversary', type: 'bool' },
        { name: 'CompanyGroup.ID', mapping: 'CompanyGroup.ID', type: 'int' },
        { name: 'CompanyGroup.Name', mapping: 'CompanyGroup.Name', type: 'string' },
        { name: 'UserID', type: 'int' },
        { name: 'DoState.IsActive', mapping: 'DoState.IsActive', type: 'bool', defaultValue: false },
        { name: 'DoState.IsDelete', mapping: 'DoState.IsDelete', type: 'bool', defaultValue: false },
        { name: 'DoState.CheckStatus', mapping: 'DoState.CheckStatus', type: 'int', defaultValue: 0 },
        { name: 'DoState.Status', mapping: 'DoState.Status', type: 'int', defaultValue: 0}]
});

var brandCommon=Ext.create('BrandCommon');

ModelDefineFactory('BrandModel', brandCommon.Fields);
/*--------------------StoreDefine--------------------*/
StoreDefineFactory('BrandStore','BrandModel',brandCommon.StoreApi);
PVStoreDefineFactory('BrandPStore','App.KeyValueModel',{read:'http://127.0.0.1/'});
PVStoreDefineFactory('BrandPIDStore', 'App.KeyIDModel', { read: brandCommon.PVStoreUrl });
/*--------------------InfoPanelDefine--------------------*/

  var  brandInfoPanelTpl='<div>ID:{ID}</div><div>品牌名称:{Name}</div><div>竞争对手:{Adversary}</div>';
  InfoPanelDefineFactory('BrandInfoPanel', brandInfoPanelTpl);
 /*--------------------FormPanelDefine--------------------*/
 var brandFormItems = [
       ComboFactory('集团名称', 'CompanyGroup.ID', 'CompanyGroupPIDStore', companyGroupCommon.PVStoreUrl,false),
       ComboFactory('品牌名称', 'Name', 'BrandPStore', brandCommon.PVDistinctStoreUrl + 'Name',true),
      { fieldLabel: '竞争对手', xtype: 'checkboxfield', name: 'Adversary', allowBlank: false },
	   
	];
 FormPanelDefineFactory('BrandForm', brandFormItems);
 /*--------------------GridPanelDefine--------------------*/
 var brandColumns = [{
                xtype: 'rownumberer', header: '行号', width: 30, sortable: false
            }
            , {
                header: 'ID', dataIndex: 'ID', width: 36, filter: {}
            }, {
                dataIndex: 'Name', header: '品牌名称', width: 270, filter: {}
            }, {
                dataIndex: 'CompanyGroup.Name', header: '集团名称', width: 270, filter: {}
            }
             , {
                 dataIndex: 'Adversary', header: '竞争对手', width: 70, filter: {}
             }
            ];
 GridPanelDefineFactory('BrandGrid', brandColumns, 'BrandStore');