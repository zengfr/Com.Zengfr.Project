Ext.define('App.CompanyModel',{
        extend: 'Ext.data.Model',
        idProperty: 'CompanyID',
        fields: [
        {name: 'CompanyId', mapping: 'CompanyId'},
{name: 'CompanyName', mapping: 'CompanyName'},
{name: 'DataItemIsDeleted', mapping: 'DataItemIsDeleted'},
{name: 'DataItemStatus', mapping: 'DataItemStatus'},
{name: 'DataCreateLastTime', mapping: 'DataCreateLastTime'},
{name: 'DataChangeLastTime', mapping: 'DataChangeLastTime'},
        ]
    });

