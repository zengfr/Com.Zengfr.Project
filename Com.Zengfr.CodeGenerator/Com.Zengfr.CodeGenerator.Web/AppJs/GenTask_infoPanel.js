Ext.define("App.GenTaskInfoPanel", {
    extend: 'Ext.form.Panel',
    title: '详细信息',
	autoDestroy: true,autoScroll: true,
    tpl: Ext.create('Ext.XTemplate',
	                '                      GenTaskId:{GenTaskId}<br/>                      Result:{Result}<br/>                      ResultStatus:{ResultStatus}<br/>                      Description:{Description}<br/>                      DataItemIsDeleted:{DataItemIsDeleted}<br/>                      DataItemStatus:{DataItemStatus}<br/>                      DataCreateLastTime:{DataCreateLastTime}<br/>                      DataChangeLastTime:{DataChangeLastTime}<br/>'
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

