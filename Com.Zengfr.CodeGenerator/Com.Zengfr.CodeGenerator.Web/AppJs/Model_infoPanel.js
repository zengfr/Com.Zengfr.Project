Ext.define("App.ModelInfoPanel", {
    extend: 'Ext.form.Panel',
    title: '详细信息',
	autoDestroy: true,autoScroll: true,
    tpl: Ext.create('Ext.XTemplate',
	                '                      ModelId:{ModelId}<br/>                      Database:{Database}<br/>                      ModelType:{ModelType}<br/>                      ModelName:{ModelName}<br/>                      ModelDescription:{ModelDescription}<br/>                      DataItemIsDeleted:{DataItemIsDeleted}<br/>                      DataItemStatus:{DataItemStatus}<br/>                      DataCreateLastTime:{DataCreateLastTime}<br/>                      DataChangeLastTime:{DataChangeLastTime}<br/>'
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

