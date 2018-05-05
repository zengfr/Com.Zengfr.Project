/*--------------------ModelDefine--------------------*/
Ext.define("CompanyGroupCommon",{
    StoreApi: { read: '/CompanyGroup/SlicedFindAll',
        create: '/CompanyGroup/Create',
        update: '/CompanyGroup/Update',
        destroy: '/CompanyGroup/Delete'
    },
    PVStoreUrl: '/CompanyGroup/SlicedFindAllPV?property=Name',
    PVDistinctStoreUrl: '/CompanyGroup/SlicedFindAllPVDistinct?property=',
    Fields: [{ name: 'ID', type: 'int' },
    { name: 'Name', type: 'string' },
    { name: 'Adversary', type: 'bool' },
        { name: 'UserID', type: 'int' },
        { name: 'DoState.IsActive', mapping: 'DoState.IsActive', type: 'bool', defaultValue: false },
        { name: 'DoState.IsDelete', mapping: 'DoState.IsDelete', type: 'bool', defaultValue: false },
        { name: 'DoState.CheckStatus', mapping: 'DoState.CheckStatus', type: 'int', defaultValue: 0 },
        { name: 'DoState.Status', mapping: 'DoState.Status', type: 'int', defaultValue: 0}]
});

var companyGroupCommon=Ext.create('CompanyGroupCommon');

ModelDefineFactory('CompanyGroupModel', companyGroupCommon.Fields);
/*--------------------StoreDefine--------------------*/
StoreDefineFactory('CompanyGroupStore','CompanyGroupModel',companyGroupCommon.StoreApi);
PVStoreDefineFactory('CompanyGroupPStore', 'App.KeyValueModel', { read: 'http://127.0.0.1/' });
PVStoreDefineFactory('CompanyGroupPIDStore', 'App.KeyIDModel', { read: companyGroupCommon.PVStoreUrl });

/*--------------------InfoPanelDefine--------------------*/

  var  companyGroupInfoPanelTpl='<div>ID:{ID}</div><div>集团名称:{Name}</div><div>竞争对手:{Adversary}</div>';
  InfoPanelDefineFactory('CompanyGroupInfoPanel', companyGroupInfoPanelTpl);
 /*--------------------FormPanelDefine--------------------*/
 var companyGroupFormItems = [
       ComboFactory('集团名称', 'Name', 'CompanyGroupPStore', companyGroupCommon.PVDistinctStoreUrl + 'Name', true),
      { fieldLabel: '竞争对手', xtype: 'checkboxfield', name: 'Adversary', allowBlank: false },
	   
	];
 FormPanelDefineFactory('CompanyGroupForm', companyGroupFormItems);
 /*--------------------GridPanelDefine--------------------*/
 var companyGroupColumns = [{
                xtype: 'rownumberer', header: '行号', width: 30, sortable: false
            }
            , {
                header: 'ID', dataIndex: 'ID', width: 36, filter: {}
            }, {
                dataIndex: 'Name', header: '集团名称', width: 270, filter: {}
            }
             , {
                 dataIndex: 'Adversary', header: '竞争对手', width: 70, filter: {}
             }
            ];
 GridPanelDefineFactory('CompanyGroupGrid', companyGroupColumns, 'CompanyGroupStore');