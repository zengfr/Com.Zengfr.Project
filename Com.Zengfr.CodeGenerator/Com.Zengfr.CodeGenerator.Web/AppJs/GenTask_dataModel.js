Ext.define('App.GenTaskModel',{
        extend: 'Ext.data.Model',
        idProperty: 'GenTaskID',
        fields: [
        {name: 'GenTaskId', mapping: 'GenTaskId'},
{name: 'Result', mapping: 'Result'},
{name: 'ResultStatus', mapping: 'ResultStatus'},
{name: 'Description', mapping: 'Description'},
{name: 'DataItemIsDeleted', mapping: 'DataItemIsDeleted'},
{name: 'DataItemStatus', mapping: 'DataItemStatus'},
{name: 'DataCreateLastTime', mapping: 'DataCreateLastTime'},
{name: 'DataChangeLastTime', mapping: 'DataChangeLastTime'},
        ]
    });

