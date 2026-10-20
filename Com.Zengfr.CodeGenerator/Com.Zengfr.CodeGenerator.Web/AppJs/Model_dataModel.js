Ext.define('App.ModelModel',{
        extend: 'Ext.data.Model',
        idProperty: 'ModelID',
        fields: [
        {name: 'ModelId', mapping: 'ModelId'},
{name: 'Database', mapping: 'Database'},
{name: 'ModelType', mapping: 'ModelType'},
{name: 'ModelName', mapping: 'ModelName'},
{name: 'ModelDescription', mapping: 'ModelDescription'},
{name: 'DataItemIsDeleted', mapping: 'DataItemIsDeleted'},
{name: 'DataItemStatus', mapping: 'DataItemStatus'},
{name: 'DataCreateLastTime', mapping: 'DataCreateLastTime'},
{name: 'DataChangeLastTime', mapping: 'DataChangeLastTime'},
        ]
    });

