/*--------------------ModelDefine--------------------*/
Ext.define("UserCommon",{
    StoreApi: { read: 'http://127.0.0.1:99/js/pro/data.txt',
        create: 'http://127.0.0.1:99/js/pro/data.txt',
        update: 'http://127.0.0.1:99/js/pro/data.txt',
        destroy: 'http://127.0.0.1:99/js/pro/data.txt'
    },
   PropertyValuesStoreUrl:'http://127.0.0.1:6254/User/PropertyValuesDistinct?property=',
   Fields:[{ name: 'ID', type: 'int' }, { name: 'UserName', type: 'string' },
    { name: 'Password', type: 'string' },
    { name: 'NickName', type: 'string' },
    { name: 'RealName', type: 'string' },
   { name: 'QQ', type: 'string' },
   { name: 'MSN', type: 'string' },
   { name: 'Email', type: 'string' },
   { name: 'TEL', type: 'string' },
   { name: 'Phone', type: 'string' }, { name: 'DoState.IsActive', mapping: 'DoState.IsActive', type: 'bool', defaultValue: false },
        { name: 'DoState.IsDelete', mapping: 'DoState.IsDelete', type: 'bool', defaultValue: false },
        { name: 'DoState.CheckStatus', mapping: 'DoState.CheckStatus', type: 'int', defaultValue: 0 },
        { name: 'DoState.Status', mapping: 'DoState.Status', type: 'int', defaultValue: 0}]
});

var userCommon=Ext.create('UserCommon');

ModelDefineFactory('UserModel', userCommon.Fields);
/*--------------------StoreDefine--------------------*/
StoreDefineFactory('UserStore','UserModel',userCommon.StoreApi);
PropertyValuesStoreDefineFactory('UserPStore','App.KeyValueModel',{read:'http://127.0.0.1/'});

/*--------------------InfoPanelDefine--------------------*/

  var  userInfoPanelTpl='<div>ID:{ID}</div>'+
                '<div>RealName:{RealName}</div>'+
                '<div>QQ:{QQ}</div><div>MSN:{MSN}</div><div>TEL:{TEL}</div><div>Phone:{Phone}</div>'; 
 InfoPanelDefineFactory('UserInfoPanel',userCommon.InfoPanelTpl);
 /*--------------------FormPanelDefine--------------------*/
 var  userFormItems=[{ fieldLabel: 'Email',xtype: 'textfield', name: 'Email', allowBlank: false},
      { fieldLabel: '手机',xtype: 'textfield', name: 'TEL',allowBlank: false},
	    ComboFactory('Phone', 'Phone', 'UserPStore', userCommon.PropertyValuesStoreUrl + 'Phone'),
		ComboFactory('TEL', 'TEL', 'UserPStore', userCommon.PropertyValuesStoreUrl + 'TEL')];
 FormPanelDefineFactory('UserForm', userFormItems);
 /*--------------------GridPanelDefine--------------------*/
 var userColumns = [{
                xtype: 'rownumberer', header: '行号', width: 30, sortable: false
            }
            , {
                header: 'ID', dataIndex: 'ID', width: 30, filter: {}
            }, {
                dataIndex: 'RealName', header: '真实姓名', width: 70, filter: {}
            }
             , {
                 dataIndex: 'Email', header: 'Email', width: 70, filter: {}
             }
             , {
                 dataIndex: 'TEL', header: 'TEL', width: 70, filter: {}
             }
             , {
                 dataIndex: 'QQ', header: 'QQ', width: 70, filter: {}
             }
            , {
                dataIndex: 'IDCode', header: 'IDCode', width: 70, filter: {}
            }];
 GridPanelDefineFactory('UserGrid', userColumns, 'UserStore');