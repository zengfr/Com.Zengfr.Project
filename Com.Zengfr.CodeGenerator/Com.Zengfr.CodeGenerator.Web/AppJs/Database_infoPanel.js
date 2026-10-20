Ext.define("App.DatabaseInfoPanel", {
    extend: 'Ext.form.Panel',
    title: '详细信息',
	autoDestroy: true,autoScroll: true,
    tpl: Ext.create('Ext.XTemplate',
	                '                      DatabaseId:{DatabaseId}<br/>                      Company:{Company}<br/>                      DatabaseName:{DatabaseName}<br/>                      DatabaseDescription:{DatabaseDescription}<br/>                      DatabaseType:{DatabaseType}<br/>                      ConnctionString:{ConnctionString}<br/>                      DataItemIsDeleted:{DataItemIsDeleted}<br/>                      DataItemStatus:{DataItemStatus}<br/>                      DataCreateLastTime:{DataCreateLastTime}<br/>                      DataChangeLastTime:{DataChangeLastTime}<br/>'
					,{
                    defaultValue: function (v) {
                        return v ? v : 'Unknown';
                    },
                    formatDate: function (value) {
                        if (!value) {
                            return '';
                        }
                        return Ext.Date.format(value, 'M j, Y, g:i a');
                    }
                }
             ),
	setActiveRecord: function (rec) {
        this.Record = rec;
        this.update(rec.data);
    }
   });

