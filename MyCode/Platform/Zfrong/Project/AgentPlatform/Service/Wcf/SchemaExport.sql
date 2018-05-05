
    drop table if exists ut_permissionItem;

    drop table if exists ut_Agent;

    drop table if exists ut_user;

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
