/*--------------------ModelDefine--------------------*/
Ext.define("StatisticsCommon",{
    StoreApi: { read: '/Statistics/SlicedFindAll',
        create: '/Statistics/Create',
        update: '/Statistics/Update',
        destroy: '/Statistics/Delete'
    },
    PVStoreUrl: '/Statistics/SlicedFindAllPV?property=Name',
    PVDistinctStoreUrl: '/Statistics/SlicedFindAllPVDistinct?property=',
    Fields: [{ name: 'ID', type: 'int' },
    { name: 'DateTime', type: 'date' },
    { name: 'DataType', type: 'string' },
      { name: 'D1', type: 'string' },
      { name: 'D2', type: 'string' },
       { name: 'D3', type: 'string' },
        { name: 'V1', type: 'string' },
         { name: 'V2', type: 'string' },
          { name: 'V3', type: 'string' },

    { name: 'Adversary', type: 'bool' },
        { name: 'UserID', type: 'int' },
        { name: 'DoState.IsActive', mapping: 'DoState.IsActive', type: 'bool', defaultValue: false },
        { name: 'DoState.IsDelete', mapping: 'DoState.IsDelete', type: 'bool', defaultValue: false },
        { name: 'DoState.CheckStatus', mapping: 'DoState.CheckStatus', type: 'int', defaultValue: 0 },
        { name: 'DoState.Status', mapping: 'DoState.Status', type: 'int', defaultValue: 0}]
});

var statisticsCommon=Ext.create('StatisticsCommon');

ModelDefineFactory('StatisticsModel', statisticsCommon.Fields);
/*--------------------StoreDefine--------------------*/
StoreDefineFactory('StatisticsStore','StatisticsModel',statisticsCommon.StoreApi);
PVStoreDefineFactory('StatisticsPStore', 'App.KeyValueModel', { read: 'http://127.0.0.1/' });
PVStoreDefineFactory('StatisticsPIDStore', 'App.KeyIDModel', { read: statisticsCommon.PVStoreUrl });

/*--------------------InfoPanelDefine--------------------*/

var statisticsInfoPanelTpl = '<div>ID:{ID}</div>';
  InfoPanelDefineFactory('StatisticsInfoPanel', statisticsInfoPanelTpl);
 /*--------------------FormPanelDefine--------------------*/
 var statisticsFormItems = [
      { fieldLabel: '时间', name: 'DateTime', allowBlank: false },
      { fieldLabel: '类型', name: 'DataType', allowBlank: false }
     
	   
	];
 FormPanelDefineFactory('StatisticsForm', statisticsFormItems);
 /*--------------------GridPanelDefine--------------------*/
 var statisticsColumns = [{
                xtype: 'rownumberer', header: '行号', width: 30, sortable: false
            }
            , {
                header: 'ID', dataIndex: 'ID', width: 36, filter: {}
            }, {
                dataIndex: 'DateTime', header: '时间', width: 160, filter: {}
            }
             , {
                 dataIndex: 'DataType', header: '类型', width: 280, filter: {}
             },{
                 dataIndex: 'D1', header: 'D1', width: 60, filter: {}
             }, , {
                 dataIndex: 'D2', header: 'D2', width: 60, filter: {}
             }, , {
                 dataIndex: 'V1', header: 'V1', width: 60, filter: {}
             },
             , {
                 dataIndex: 'V2', header: 'V2', width: 60, filter: {}
             }
            ];
 GridPanelDefineFactory('StatisticsGrid', statisticsColumns, 'StatisticsStore');