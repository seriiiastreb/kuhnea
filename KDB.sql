-- SQL Manager 2008 for SQL Server 3.3.0.1
-- ---------------------------------------
-- Host      : SERVER\SQLEXPRESS
-- Database  : kuhnea
-- Version   : Microsoft SQL Server  10.50.2500.0


CREATE DATABASE [kuhnea]
ON PRIMARY
  ( NAME = [kuhnea],
    FILENAME = 'C:\Program Files (x86)\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\kuhnea.mdf',
    SIZE = 3 MB,
    MAXSIZE = UNLIMITED,
    FILEGROWTH = 1 MB )
LOG ON
  ( NAME = [kuhnea_log],
    FILENAME = 'C:\Program Files (x86)\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\kuhnea_log.ldf',
    SIZE = 1 MB,
    MAXSIZE = 0 MB,
    FILEGROWTH = 10 % )
COLLATE Cyrillic_General_CI_AS
GO

USE [kuhnea]
GO

--
-- Definition for table classifiers : 
--

CREATE TABLE [dbo].[classifiers] (
  [typeid] int NOT NULL,
  [code] int NOT NULL,
  [groupcode] int NULL
)
ON [PRIMARY]
GO

--
-- Definition for table classifierstranslate : 
--

CREATE TABLE [dbo].[classifierstranslate] (
  [code] int NOT NULL,
  [lnguage] int NOT NULL,
  [name] nvarchar(50) COLLATE Cyrillic_General_CI_AS NOT NULL
)
ON [PRIMARY]
GO

--
-- Definition for table classifiertype : 
--

CREATE TABLE [dbo].[classifiertype] (
  [typeid] int NOT NULL,
  [system] bit NOT NULL
)
ON [PRIMARY]
GO

--
-- Definition for table classifiertypetranslate : 
--

CREATE TABLE [dbo].[classifiertypetranslate] (
  [typeid] int NOT NULL,
  [language] int NOT NULL,
  [name] nvarchar(50) COLLATE Cyrillic_General_CI_AS NOT NULL
)
ON [PRIMARY]
GO

--
-- Definition for table st_domains : 
--

CREATE TABLE [dbo].[st_domains] (
  [module_id] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
  [domain_id] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
  [description] nvarchar(256) COLLATE Cyrillic_General_CI_AS NOT NULL
)
ON [PRIMARY]
GO

--
-- Definition for table st_modules : 
--

CREATE TABLE [dbo].[st_modules] (
  [module_id] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
  [description] nvarchar(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
ON [PRIMARY]
GO

--
-- Definition for table st_roles : 
--

CREATE TABLE [dbo].[st_roles] (
  [role_id] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
ON [PRIMARY]
GO

--
-- Definition for table st_roles_permissions : 
--

CREATE TABLE [dbo].[st_roles_permissions] (
  [module_id] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
  [role_id] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
  [domain_id] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
  [permissions] int NOT NULL
)
ON [PRIMARY]
GO

--
-- Definition for table st_users_roles : 
--

CREATE TABLE [dbo].[st_users_roles] (
  [user_id] int NOT NULL,
  [role_id] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
ON [PRIMARY]
GO

--
-- Definition for table users : 
--

CREATE TABLE [dbo].[users] (
  [userid] int IDENTITY(1, 1) NOT NULL,
  [firstname] varchar(50) COLLATE Cyrillic_General_CI_AS NOT NULL,
  [lastname] varchar(50) COLLATE Cyrillic_General_CI_AS NOT NULL,
  [login] varchar(50) COLLATE Cyrillic_General_CI_AS NOT NULL,
  [password] varchar(50) COLLATE Cyrillic_General_CI_AS NOT NULL,
  [passwordstatus] int NOT NULL,
  [recordstatus] int NOT NULL,
  [language] int NOT NULL,
  [email] varchar(50) COLLATE Cyrillic_General_CI_AS NOT NULL,
  [sysadmin] bit CONSTRAINT [DF__users__sysadmin__1920BF5C] DEFAULT 0 NOT NULL
)
ON [PRIMARY]
GO

--
-- Data for table dbo.classifiers  (LIMIT 0,500)
--

INSERT INTO [dbo].[classifiers] ([typeid], [code], [groupcode])
VALUES 
  (0, 0, 0)
GO

INSERT INTO [dbo].[classifiers] ([typeid], [code], [groupcode])
VALUES 
  (1, 1, 0)
GO

INSERT INTO [dbo].[classifiers] ([typeid], [code], [groupcode])
VALUES 
  (1, 2, 0)
GO

INSERT INTO [dbo].[classifiers] ([typeid], [code], [groupcode])
VALUES 
  (3, 3, 0)
GO

INSERT INTO [dbo].[classifiers] ([typeid], [code], [groupcode])
VALUES 
  (3, 4, 0)
GO

INSERT INTO [dbo].[classifiers] ([typeid], [code], [groupcode])
VALUES 
  (4, 5, 0)
GO

INSERT INTO [dbo].[classifiers] ([typeid], [code], [groupcode])
VALUES 
  (4, 6, 0)
GO

INSERT INTO [dbo].[classifiers] ([typeid], [code], [groupcode])
VALUES 
  (4, 7, 0)
GO

--
-- Data for table dbo.classifierstranslate  (LIMIT 0,500)
--

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (0, 1, N'**')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (0, 2, N'**')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (0, 3, N'**')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (0, 4, N'**')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (0, 5, N'**')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (1, 1, N'Active')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (1, 2, N'Activ')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (1, 3, N'Активный')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (1, 4, N'Actif')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (1, 5, N'Aktiv')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (2, 1, N'Need change')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (2, 2, N'Necesita schimbat')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (2, 3, N'нужно изменение')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (2, 4, N'besoin de changement')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (2, 5, N'Brauchen Änderung')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (3, 1, N'Male')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (3, 2, N'Maculin')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (3, 3, N'мужской')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (3, 4, N'Mâle')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (3, 5, N'Männlich')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (4, 1, N'Female')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (4, 2, N'Feminin')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (4, 3, N'женский')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (4, 4, N'Femelle')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (4, 5, N'Weiblich')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (5, 1, N'Active')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (5, 2, N'Activ')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (5, 3, N'Активный')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (5, 4, N'Actif')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (5, 5, N'Aktiv')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (6, 1, N'Blocked')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (6, 2, N'Blocat')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (6, 3, N'Заблокирован')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (6, 4, N'Bloqué')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (6, 5, N'Verstopft')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (7, 1, N'Not Activated')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (7, 2, N'Neactivat')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (7, 3, N'Не активирован')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (7, 4, N'Non activé')
GO

INSERT INTO [dbo].[classifierstranslate] ([code], [lnguage], [name])
VALUES 
  (7, 5, N'Nicht aktiviert')
GO

--
-- Data for table dbo.classifiertype  (LIMIT 0,500)
--

INSERT INTO [dbo].[classifiertype] ([typeid], [system])
VALUES 
  (0, 1)
GO

INSERT INTO [dbo].[classifiertype] ([typeid], [system])
VALUES 
  (1, 1)
GO

INSERT INTO [dbo].[classifiertype] ([typeid], [system])
VALUES 
  (2, 1)
GO

INSERT INTO [dbo].[classifiertype] ([typeid], [system])
VALUES 
  (3, 1)
GO

INSERT INTO [dbo].[classifiertype] ([typeid], [system])
VALUES 
  (4, 1)
GO

--
-- Data for table dbo.classifiertypetranslate  (LIMIT 0,500)
--

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (0, 1, N'Not Specified')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (0, 2, N'Nespecificat')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (0, 3, N'Не указано')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (0, 4, N'Non spécifié')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (0, 5, N'Nicht angegeben')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (1, 1, N'Password Status')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (1, 2, N'Statutul parolei')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (1, 3, N'Состояние пароля')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (1, 4, N'Password Status')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (1, 5, N'Password Status')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (2, 1, N'Country List')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (2, 2, N'Lista tarilor')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (2, 3, N'Список стран')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (2, 4, N'Liste des Pays')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (2, 5, N'Länderliste')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (3, 1, N'Gender List')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (3, 2, N'Gen')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (3, 3, N'Пол')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (3, 4, N'Liste des sexes')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (3, 5, N'Geschlecht Liste')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (4, 1, N'User Record Status')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (4, 2, N'Statutul userului')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (4, 3, N'Статус записи пользователя')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (4, 4, N'Fiche utilisateur État')
GO

INSERT INTO [dbo].[classifiertypetranslate] ([typeid], [language], [name])
VALUES 
  (4, 5, N'User Rekord-Status')
GO

--
-- Data for table dbo.users  (LIMIT 0,500)
--

SET IDENTITY_INSERT [dbo].[users] ON
GO

INSERT INTO [dbo].[users] ([userid], [firstname], [lastname], [login], [password], [passwordstatus], [recordstatus], [language], [email], [sysadmin])
VALUES 
  (1, N'Iastrebov', N'Sergiu', N'seriiiastreb', N'T8grJq7LR9KGjE7741gXMqPny8xsLvsyBiwIFwoF7rg=', 1, 5, 1, N'iastrebov.sergiu@mail.md', 1)
GO

SET IDENTITY_INSERT [dbo].[users] OFF
GO

--
-- Definition for indices : 
--

ALTER TABLE [dbo].[classifiers]
ADD CONSTRAINT [PK__classifi__AB8796A50EA330E9] 
PRIMARY KEY CLUSTERED ([code])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[classifierstranslate]
ADD CONSTRAINT [PK__classifi__B73AED3038996AB5] 
PRIMARY KEY CLUSTERED ([code], [lnguage])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[classifiertype]
ADD CONSTRAINT [PK__classifi__6EA8575F0AD2A005] 
PRIMARY KEY CLUSTERED ([typeid])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[classifiertypetranslate]
ADD CONSTRAINT [PK__classifi__6EA8575F31EC6D26] 
PRIMARY KEY CLUSTERED ([typeid], [language])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[st_domains]
ADD CONSTRAINT [PK__st_domai__C6891703778AC167] 
PRIMARY KEY CLUSTERED ([domain_id], [module_id])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[st_modules]
ADD CONSTRAINT [PK__st_modul__1A2D06531BFD2C07] 
PRIMARY KEY CLUSTERED ([module_id])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[st_roles]
ADD CONSTRAINT [PK__st_roles__760965CC1FCDBCEB] 
PRIMARY KEY CLUSTERED ([role_id])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[st_roles_permissions]
ADD CONSTRAINT [st_roles_permissions_pk] 
PRIMARY KEY CLUSTERED ([module_id], [role_id], [domain_id])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[st_users_roles]
ADD CONSTRAINT [PK__st_users__6EDEA1533587F3E0] 
PRIMARY KEY CLUSTERED ([user_id], [role_id])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[users]
ADD CONSTRAINT [PK__users__CBA1B257145C0A3F] 
PRIMARY KEY CLUSTERED ([userid])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[users]
ADD CONSTRAINT [UQ__users__7838F272173876EA] 
UNIQUE NONCLUSTERED ([login])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

--
-- Definition for foreign keys : 
--

ALTER TABLE [dbo].[classifiers]
ADD CONSTRAINT [classifiers_types_fk] FOREIGN KEY ([typeid]) 
  REFERENCES [dbo].[classifiertype] ([typeid]) 
  ON UPDATE CASCADE
  ON DELETE CASCADE
GO

ALTER TABLE [dbo].[classifierstranslate]
ADD CONSTRAINT [classifierstranslate_classifiers_fk] FOREIGN KEY ([code]) 
  REFERENCES [dbo].[classifiers] ([code]) 
  ON UPDATE CASCADE
  ON DELETE CASCADE
GO

ALTER TABLE [dbo].[classifiertypetranslate]
ADD CONSTRAINT [classifiertypetranslate_types_fk] FOREIGN KEY ([typeid]) 
  REFERENCES [dbo].[classifiertype] ([typeid]) 
  ON UPDATE CASCADE
  ON DELETE CASCADE
GO

ALTER TABLE [dbo].[st_domains]
ADD CONSTRAINT [st_domains_fk] FOREIGN KEY ([module_id]) 
  REFERENCES [dbo].[st_modules] ([module_id]) 
  ON UPDATE CASCADE
  ON DELETE CASCADE
GO

ALTER TABLE [dbo].[st_roles_permissions]
ADD CONSTRAINT [st_roles_permissions_domains_fk] FOREIGN KEY ([domain_id], [module_id]) 
  REFERENCES [dbo].[st_domains] ([domain_id], [module_id]) 
  ON UPDATE NO ACTION
  ON DELETE NO ACTION
GO

ALTER TABLE [dbo].[st_roles_permissions]
ADD CONSTRAINT [st_roles_permissions_fk] FOREIGN KEY ([role_id]) 
  REFERENCES [dbo].[st_roles] ([role_id]) 
  ON UPDATE CASCADE
  ON DELETE CASCADE
GO

ALTER TABLE [dbo].[st_roles_permissions]
ADD CONSTRAINT [st_roles_permissions_modules_fk] FOREIGN KEY ([module_id]) 
  REFERENCES [dbo].[st_modules] ([module_id]) 
  ON UPDATE CASCADE
  ON DELETE CASCADE
GO

ALTER TABLE [dbo].[st_users_roles]
ADD CONSTRAINT [st_users_roles_roles_fk] FOREIGN KEY ([role_id]) 
  REFERENCES [dbo].[st_roles] ([role_id]) 
  ON UPDATE CASCADE
  ON DELETE CASCADE
GO

ALTER TABLE [dbo].[st_users_roles]
ADD CONSTRAINT [st_users_roles_users_fk] FOREIGN KEY ([user_id]) 
  REFERENCES [dbo].[users] ([userid]) 
  ON UPDATE CASCADE
  ON DELETE CASCADE
GO

