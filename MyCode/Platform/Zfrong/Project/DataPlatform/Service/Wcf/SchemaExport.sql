
    
alter table ut_Product  drop foreign key FKFB3681DD1A70F7B
;

    
alter table ut_Brand  drop foreign key FK3F72E6FAA07EF946
;

    
alter table ut_SearchTerm  drop foreign key FKE2A478B76A43541B
;

    drop table if exists ut_permissionItem;

    drop table if exists ut_Agent;

    drop table if exists ut_user;

    drop table if exists ut_Statistics;

    drop table if exists ut_CompanyGroup;

    drop table if exists ut_Product;

    drop table if exists ut_DataFilter;

    drop table if exists ut_Brand;

    drop table if exists ut_SearchTerm;

    create table ut_permissionItem (
        ID BIGINT NOT NULL AUTO_INCREMENT,
       PID BIGINT,
       Name VARCHAR(24),
       GroupID INTEGER,
       primary key (ID)
    );

    create table ut_Agent (
        ID BIGINT NOT NULL AUTO_INCREMENT,
       Password VARCHAR(128),
       Email VARCHAR(32),
       Phone VARCHAR(16),
       TEL VARCHAR(16),
       QQ VARCHAR(18),
       RealName VARCHAR(12),
       IDCode VARCHAR(20),
       IsActive TINYINT(1) default 0 ,
       IsDelete TINYINT(1) default 0 ,
       Sort TINYINT UNSIGNED default 0 ,
       Status TINYINT UNSIGNED default 0 ,
       CheckStatus TINYINT UNSIGNED default 0 ,
       CreateTime DATETIME,
       UpdateTime DATETIME,
       DeleteTime DATETIME,
       primary key (ID)
    );

    create table ut_user (
        ID BIGINT NOT NULL AUTO_INCREMENT,
       Password VARCHAR(128),
       RealName VARCHAR(12),
       IDCode VARCHAR(20),
       Email VARCHAR(32),
       TEL VARCHAR(16),
       Phone VARCHAR(16),
       QQ VARCHAR(18),
       IsActive TINYINT(1) default 0 ,
       IsDelete TINYINT(1) default 0 ,
       Sort TINYINT UNSIGNED default 0 ,
       Status TINYINT UNSIGNED default 0 ,
       CheckStatus TINYINT UNSIGNED default 0 ,
       CreateTime DATETIME,
       UpdateTime DATETIME,
       DeleteTime DATETIME,
       primary key (ID)
    );

    create table ut_Statistics (
        ID BIGINT NOT NULL AUTO_INCREMENT,
       DateTime DATETIME,
       Type VARCHAR(255),
       V1 VARCHAR(255),
       V2 VARCHAR(255),
       V3 VARCHAR(255),
       D1 VARCHAR(255),
       D2 VARCHAR(255),
       D3 VARCHAR(255),
       UserID BIGINT,
       Adversary TINYINT(1),
       IsActive TINYINT(1) default 0 ,
       IsDelete TINYINT(1) default 0 ,
       Sort TINYINT UNSIGNED default 0 ,
       Status TINYINT UNSIGNED default 0 ,
       CheckStatus TINYINT UNSIGNED default 0 ,
       CreateTime DATETIME,
       UpdateTime DATETIME,
       DeleteTime DATETIME,
       primary key (ID)
    );

    create table ut_CompanyGroup (
        ID BIGINT NOT NULL AUTO_INCREMENT,
       Name VARCHAR(128),
       UserID BIGINT,
       Adversary TINYINT(1),
       IsActive TINYINT(1) default 0 ,
       IsDelete TINYINT(1) default 0 ,
       Sort TINYINT UNSIGNED default 0 ,
       Status TINYINT UNSIGNED default 0 ,
       CheckStatus TINYINT UNSIGNED default 0 ,
       CreateTime DATETIME,
       UpdateTime DATETIME,
       DeleteTime DATETIME,
       primary key (ID)
    );

    create table ut_Product (
        ID BIGINT NOT NULL AUTO_INCREMENT,
       Name VARCHAR(128),
       Brand BIGINT,
       UserID BIGINT,
       Adversary TINYINT(1),
       IsActive TINYINT(1) default 0 ,
       IsDelete TINYINT(1) default 0 ,
       Sort TINYINT UNSIGNED default 0 ,
       Status TINYINT UNSIGNED default 0 ,
       CheckStatus TINYINT UNSIGNED default 0 ,
       CreateTime DATETIME,
       UpdateTime DATETIME,
       DeleteTime DATETIME,
       primary key (ID)
    );

    create table ut_DataFilter (
        ID BIGINT NOT NULL AUTO_INCREMENT,
       Content VARCHAR(128),
       Note VARCHAR(255),
       IsTitle TINYINT(1),
       IsContent TINYINT(1),
       IsURL TINYINT(1),
       UserID BIGINT,
       IsActive TINYINT(1) default 0 ,
       IsDelete TINYINT(1) default 0 ,
       Sort TINYINT UNSIGNED default 0 ,
       Status TINYINT UNSIGNED default 0 ,
       CheckStatus TINYINT UNSIGNED default 0 ,
       CreateTime DATETIME,
       UpdateTime DATETIME,
       DeleteTime DATETIME,
       primary key (ID)
    );

    create table ut_Brand (
        ID BIGINT NOT NULL AUTO_INCREMENT,
       Name VARCHAR(128),
       CompanyGroup BIGINT,
       UserID BIGINT,
       Adversary TINYINT(1),
       IsActive TINYINT(1) default 0 ,
       IsDelete TINYINT(1) default 0 ,
       Sort TINYINT UNSIGNED default 0 ,
       Status TINYINT UNSIGNED default 0 ,
       CheckStatus TINYINT UNSIGNED default 0 ,
       CreateTime DATETIME,
       UpdateTime DATETIME,
       DeleteTime DATETIME,
       primary key (ID)
    );

    create table ut_SearchTerm (
        ID BIGINT NOT NULL AUTO_INCREMENT,
       Title VARCHAR(255),
       Content VARCHAR(128),
       URL VARCHAR(255),
       SearcherType INTEGER,
       MediaType INTEGER,
       GoodOrBad TINYINT UNSIGNED,
       UserID BIGINT,
       Adversary TINYINT(1),
       IsActive TINYINT(1) default 0 ,
       IsDelete TINYINT(1) default 0 ,
       Sort TINYINT UNSIGNED default 0 ,
       Status TINYINT UNSIGNED default 0 ,
       CheckStatus TINYINT UNSIGNED default 0 ,
       CreateTime DATETIME,
       UpdateTime DATETIME,
       DeleteTime DATETIME,
       Name VARCHAR(128),
       Product BIGINT,
       primary key (ID)
    );

    alter table ut_Product 
        add index (Brand), 
        add constraint FKFB3681DD1A70F7B 
        foreign key (Brand) 
        references ut_Brand (ID);

    alter table ut_Brand 
        add index (CompanyGroup), 
        add constraint FK3F72E6FAA07EF946 
        foreign key (CompanyGroup) 
        references ut_CompanyGroup (ID);

    alter table ut_SearchTerm 
        add index (Product), 
        add constraint FKE2A478B76A43541B 
        foreign key (Product) 
        references ut_Product (ID);
