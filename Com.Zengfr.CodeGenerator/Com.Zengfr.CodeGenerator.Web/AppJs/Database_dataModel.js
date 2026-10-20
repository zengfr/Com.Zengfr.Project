Ext.define('App.DatabaseModel',{
        extend: 'Ext.data.Model',
        idProperty: 'DatabaseID',
        fields: [
        {name: 'DatabaseId', mapping: 'DatabaseId'},
{name: 'Company', mapping: 'Company'},
{name: 'DatabaseName', mapping: 'DatabaseName'},
{name: 'DatabaseDescription', mapping: 'DatabaseDescription'},
{name: 'DatabaseType', mapping: 'DatabaseType'},
{name: 'ConnctionString', mapping: 'ConnctionString'},
{name: 'DataItemIsDeleted', mapping: 'DataItemIsDeleted'},
{name: 'DataItemStatus', mapping: 'DataItemStatus'},
{name: 'DataCreateLastTime', mapping: 'DataCreateLastTime'},
{name: 'DataChangeLastTime', mapping: 'DataChangeLastTime'},
        ]
    });

