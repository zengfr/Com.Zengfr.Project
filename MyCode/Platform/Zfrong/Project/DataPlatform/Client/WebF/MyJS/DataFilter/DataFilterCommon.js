/*--------------------ModelDefine--------------------*/
Ext.define("DataFilterCommon",{
    StoreApi: { read: '/DataFilter/SlicedFindAll',
        create: '/DataFilter/Create',
        update: '/DataFilter/Update',
        destroy: '/DataFilter/Delete'
    },
    PVStoreUrl: '/DataFilter/SlicedFindAllPV?property=Name',
    PVDistinctStoreUrl: '/DataFilter/SlicedFindAllPVDistinct?property=',
    Fields: [{ name: 'ID', type: 'int' },
    { name: 'Note', type: 'string' },
    { name: 'Content', type: 'string' },
      { name: 'IsTitle', type: 'bool' },
       { name: 'IsContent', type: 'bool' },
        { name: 'IsURL', type: 'bool' },

    { name: 'Adversary', type: 'bool' },
        { name: 'UserID', type: 'int' },
        { name: 'DoState.IsActive', mapping: 'DoState.IsActive', type: 'bool', defaultValue: false },
        { name: 'DoState.IsDelete', mapping: 'DoState.IsDelete', type: 'bool', defaultValue: false },
        { name: 'DoState.CheckStatus', mapping: 'DoState.CheckStatus', type: 'int', defaultValue: 0 },
        { name: 'DoState.Status', mapping: 'DoState.Status', type: 'int', defaultValue: 0}]
});

var dataFilterCommon=Ext.create('DataFilterCommon');

ModelDefineFactory('DataFilterModel', dataFilterCommon.Fields);
/*--------------------StoreDefine--------------------*/
StoreDefineFactory('DataFilterStore','DataFilterModel',dataFilterCommon.StoreApi);
PVStoreDefineFactory('DataFilterPStore', 'App.KeyValueModel', { read: 'http://127.0.0.1/' });
PVStoreDefineFactory('DataFilterPIDStore', 'App.KeyIDModel', { read: dataFilterCommon.PVStoreUrl });

/*--------------------InfoPanelDefine--------------------*/

var dataFilterInfoPanelTpl = '<div>ID:{ID}</div><div>内容:{Content}</div><div>备注:{Note}</div>';
  InfoPanelDefineFactory('DataFilterInfoPanel', dataFilterInfoPanelTpl);
 /*--------------------FormPanelDefine--------------------*/
 var dataFilterFormItems = [
      { fieldLabel: '内容', name: 'Content', allowBlank: false },
      { fieldLabel: '备注', name: 'Note', allowBlank: false },
      { fieldLabel: '应用标题', xtype: 'checkboxfield', name: 'IsTitle', allowBlank: false },
      { fieldLabel: '应用内容', xtype: 'checkboxfield', name: 'IsContent', allowBlank: false },
      { fieldLabel: '应用URL', xtype: 'checkboxfield', name: 'IsURL', allowBlank: false },
	   
	];
 FormPanelDefineFactory('DataFilterForm', dataFilterFormItems);
 /*--------------------GridPanelDefine--------------------*/
 var dataFilterColumns = [{
                xtype: 'rownumberer', header: '行号', width: 30, sortable: false
            }
            , {
                header: 'ID', dataIndex: 'ID', width: 36, filter: {}
            }, {
                dataIndex: 'Content', header: '内容', width: 160, filter: {}
            }
             , {
                 dataIndex: 'Note', header: '备注', width: 280, filter: {}
             },{
                 dataIndex: 'IsTitle', header: '应用标题', width: 60, filter: {}
            }, {
                dataIndex: 'IsContent', header: '应用内容', width: 60, filter: {}
            }, {
                dataIndex: 'IsURL', header: '应用URL', width: 60, filter: {}
            }
            ];
 GridPanelDefineFactory('DataFilterGrid', dataFilterColumns, 'DataFilterStore');