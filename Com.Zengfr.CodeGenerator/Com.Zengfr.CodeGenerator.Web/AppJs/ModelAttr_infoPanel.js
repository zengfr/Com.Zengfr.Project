Ext.define("App.ModelAttrInfoPanel", {
    extend: 'Ext.form.Panel',
    title: '详细信息',
	autoDestroy: true,autoScroll: true,
    tpl: Ext.create('Ext.XTemplate',
	                '                      ModelAttrId:{ModelAttrId}<br/>                      Model:{Model}<br/>                      AttrName:{AttrName}<br/>                      AttDescription:{AttDescription}<br/>                      TypeName:{TypeName}<br/>                      Length:{Length}<br/>                      DefaultValue:{DefaultValue}<br/>                      IsUnique:{IsUnique}<br/>                      IsKey:{IsKey}<br/>                      IsID:{IsID}<br/>                      IsIndex:{IsIndex}<br/>                      DataItemIsDeleted:{DataItemIsDeleted}<br/>                      DataItemStatus:{DataItemStatus}<br/>                      DataCreateLastTime:{DataCreateLastTime}<br/>                      DataChangeLastTime:{DataChangeLastTime}<br/>'
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

