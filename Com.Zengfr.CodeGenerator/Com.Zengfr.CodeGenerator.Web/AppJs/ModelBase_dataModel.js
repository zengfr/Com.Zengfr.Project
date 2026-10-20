Ext.define('App.ModelBaseModel',{
        extend: 'Ext.data.Model',
        idProperty: 'ModelBaseID',
        fields: [
        {name: 'DataItemIsDeleted', mapping: 'DataItemIsDeleted'},
{name: 'DataItemStatus', mapping: 'DataItemStatus'},
{name: 'DataCreateLastTime', mapping: 'DataCreateLastTime'},
{name: 'DataChangeLastTime', mapping: 'DataChangeLastTime'},
        ]
    });

