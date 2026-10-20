
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK66F26B99BDCF6DBE]') AND parent_object_id = OBJECT_ID('g_Database'))
alter table g_Database  drop constraint FK66F26B99BDCF6DBE
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKE662D7BAD06647E4]') AND parent_object_id = OBJECT_ID('g_ModelAttr'))
alter table g_ModelAttr  drop constraint FKE662D7BAD06647E4
;

    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK1BC5E597F50ADFD0]') AND parent_object_id = OBJECT_ID('g_Model'))
alter table g_Model  drop constraint FK1BC5E597F50ADFD0
;

    if exists (select * from dbo.sysobjects where id = object_id(N'g_Company') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table g_Company;

    if exists (select * from dbo.sysobjects where id = object_id(N'g_Database') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table g_Database;

    if exists (select * from dbo.sysobjects where id = object_id(N'g_ModelAttr') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table g_ModelAttr;

    if exists (select * from dbo.sysobjects where id = object_id(N'g_Model') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table g_Model;

    create table g_Company (
        CompanyId BIGINT IDENTITY NOT NULL,
       CompanyName NVARCHAR(255) null,
       primary key (CompanyId)
    );

    create table g_Database (
        DatabaseId BIGINT IDENTITY NOT NULL,
       DatabaseName NVARCHAR(255) null,
       Company_id BIGINT null,
       primary key (DatabaseId)
    );

    create table g_ModelAttr (
        ModelAttrId BIGINT IDENTITY NOT NULL,
       AttrName NVARCHAR(255) null,
       Model_id BIGINT null,
       primary key (ModelAttrId)
    );

    create table g_Model (
        ModelId BIGINT IDENTITY NOT NULL,
       ModelName NVARCHAR(255) null,
       Database_id BIGINT null,
       primary key (ModelId)
    );

    --

    --

    --
