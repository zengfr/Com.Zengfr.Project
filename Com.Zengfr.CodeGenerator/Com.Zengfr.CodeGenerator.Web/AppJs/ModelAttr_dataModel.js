Ext.define('App.ModelAttrModel',{
        extend: 'Ext.data.Model',
        idProperty: 'ModelAttrID',
        fields: [
        {name: 'ModelAttrId', mapping: 'ModelAttrId'},
{name: 'Model', mapping: 'Model'},
{name: 'AttrName', mapping: 'AttrName'},
{name: 'AttDescription', mapping: 'AttDescription'},
{name: 'TypeName', mapping: 'TypeName'},
{name: 'Length', mapping: 'Length'},
{name: 'DefaultValue', mapping: 'DefaultValue'},
{name: 'IsUnique', mapping: 'IsUnique'},
{name: 'IsKey', mapping: 'IsKey'},
{name: 'IsID', mapping: 'IsID'},
{name: 'IsIndex', mapping: 'IsIndex'},
{name: 'DataItemIsDeleted', mapping: 'DataItemIsDeleted'},
{name: 'DataItemStatus', mapping: 'DataItemStatus'},
{name: 'DataCreateLastTime', mapping: 'DataCreateLastTime'},
{name: 'DataChangeLastTime', mapping: 'DataChangeLastTime'},
        ]
    });

