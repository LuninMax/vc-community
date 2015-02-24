﻿/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 2014-11-04 18:40:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
) 

GO

/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 2014-11-04 18:45:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO

/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 2014-11-04 18:45:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
) 

GO

/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 2014-11-04 18:46:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO

/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 2014-11-04 18:46:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO

INSERT [dbo].[__MigrationHistory] ([MigrationId], [ContextKey], [Model], [ProductVersion]) VALUES (N'201411101625303_InitialCreate', N'VirtoCommerce.Foundation.Data.Security.Identity.SecurityDbContext', 0x1F8B0800000000000400DD5CDB6EE436127D5F60FF41D05376E1B47CD919CC1ADD3370DAEEAC91F105D39E41DE066C89DD2646A21489726C04F9B23CE493F20B5BD45DBCE8D2ADBE380810B8A5E2A962F1902C968AF3D71F7F8E3F3C7BAEF184C388F874629E8C8E4D0353DB77085D4DCC982DBF7F677E78FFCF7F8CAF1CEFD9F892CB9D71396849A389F9C858706E5991FD883D148D3C62877EE42FD9C8F63D0B39BE757A7CFC5FEBE4C4C200610296618C3FC594110F273FE0E7D4A7360E588CDC1BDFC16E943D8737F304D5B8451E8E0264E389F98584CC9FFA9E87431B8F667E4C1DC4C0A8D125626834C7761C12F632BA7630A8602FA671E1120466CEB1BB340D44A9CF12F9F3CF119EB3D0A7AB79000F90FBF01260905B2237C259E7CE4BF1AEFD3C3EE5FDB4CA8639941D47CCF77A029E9C658EB3C4E66BB9DF2C1C0BAEBD4AFCC37B9DB87762E62EFBE4BBE00051E1F9D40DB9F0C4BC29545C44C12D6685AF4729E42C04B85FFDF0DBA88A7864746E775410ED7474CCFF3B32A6B1CBE2104F288E5988DC23E33E5EB8C4FE09BF3CF8DF309D9C9D2C9667EFDEBC45CED9DBFFE0B337D59E425F41AEF6001EDD877E8043B00D2F8BFE9B86556F67890D8B669536A957804B30674CE3063D7FC474C51E61369DBE338D1979C64EFE2423D7674A608A412316C6F0F336765DB47071F1DE6AD4C9FFDFA0F5F4CDDB41B4DEA227B24A865ED00F13278C4CE3137693B7D12309D2E9551BEFAF99D82CF43DFEBBCEAFF4EDD7B91FC32C86CEF85A910714AE30AB5B37B64AF276A234871A9ED639EAE1539B5B2AD35B29CA3BB4CE4CC855EC7A36E4F66E576F67C65D04010C5E422DEE9126C2F5DCC946023450A909A064DAC9E8E4AC1BD528B8E0EFBC725E7988B8032C9D1DB44048B324A1878B5EFEE0035111ED6DF33D8A2258399CFFA1E8B1C174F87300D373D2CD19F282AD6BBB7FF429BE8DBD059F27BBD335D8D03CFCEACF90CDFCF08AF2561BE37DF4ED6F7ECCAEA80313187F66760EC87F3E10AF3BC020E65CD8368EA21990193B53585C580E784DD9D9696F38BE66ED3B7499BA8878EAD845585DBFE6A265FCA2969062188D982A8E6932F5A3BF22B49BA9B9A8DED454A2D5D44CACAFA91CAC9BA599A4DED044A0D5CE546AB0C83019A1E143C304F6F063C3CD366FDD5A5071E31C5648FC23A6388465CCB9478CE1909623D065DDD847B0900C1F57BAF5BD29D1F405B9F1D0AAD69A0DC92230FC6C48600F7F362466C2E327E2F0A8A4C381291706F84EF2EAB358FB9C132CDBF574A87573D7CA77B306E8A6CB4514F936496681225596253AEAF6430C67B4673DD2DE889913E818109DF02D0F9E40DF4C915477F412BB9861E3C24E53895314D9C891DD081D727A1896EFA80AC3CA0C4ADDB87F4B3A81E938E48D103F044530530965F2B420D42601725BBD24B4ECB885F1BE173AC4379738C0942B6CF54417E5EA840937A0D0230C4A9B87C6568571CD44D444ADBA316F0B61CB7197F2183BE1644BECACE16516BF6D8598CD1EDB01399B5DD2C5006DF26F1F04CDCE2A5D09201E5C0E8DA0C2894943D02CA4DA0941EB1EDB0341EB2E7975044D8FA85DC75F38AF1E1A3DEB07E5DD6FEB8DEEDA03376BFE38306AA6B127B461D002874246F672C1DFE067A638998191D9E12CCAE25C911F1C798E593D5F5306BBCA20D46A061119D40458B2AC0534FB66280149B3A987717922AFD1BA2C84E8019B27DD1A61B3855F80AD1040C6AE7E3BAD08EABFB08ACCEC74F4287A56B0416278A793420547410871E5AA77BC8353744959D9315D02E13EA170A563D9603438A8256CD53829EFCCE05ECAA9D9EE255534D6271EDBC84B42ECA4F152DE99C1BD9471B4DD498A88A0474CB0918BEAFBF740932D4F73145B4DF16E6CA5F556D983B1A529CC1ADFA02020745529D4CA9E18F3B44A6BFAFDBC7F859297625876A428542AAC2D34313F442B2CBC05D560E98C8411E35FC117882779A68E2789C91BAB66EDCFF555F74E7904F34D2097E67FA72DFA7EE6AFEDC2728C92299841DF3D1EE824D9750533D4CD0D5E52875C142A12FA53DF8D3DAA8FBBF4ADD3CF7AD5F6E91319616C09F64B7195E44A29FAAD8F4BA75193A7CB2E46B0087BD61F453D846E2CF288B53A1ABA28568F9227B5AA28BA44D7DE465517FF0C379262DCD97F205B11B63323B34A972A40F6A82746A5584202ABBCEB8E5AAF67A962D6DF7447148A56AA90C2AB1E56564B536A46565FAC85A7F1A85AA2BB06B918A58A2EBFED8EAC284BA9422B5EAF81ADB0597CD71D5551B9520556BCEE8E5D96B1882BEC01EF79DAD3D07636BDF434BDD9AEA7C1D8CE7239CCA659A918A802551EF7C4CA6A0224B0ECF941524D7BA4DC0ED5D20CCB6654D360E8D7ACDA87F8FA92D5583DA0C7AC7D5DAF6D0B4DD5057ABC7E84DE2A6DA4E3A62852682F8E9DC2F1729C1DF5DA2F074967BF54C43472374248F01231EC65D4FAC59DBA04F30D2017B841942C71C4D28A12F3F4F8E454B8427438D779AC28725CC5515977A7A73E663B280EA34F28B41F5128976A6C70E5A5049512E1D7D4C1CF13F3B7A4D5799253E17F258F8F8CEBE83325BFC4F0E2218CB1F1BB5C7A3ACC1580E6B3DB815ED8E8EED5EB9FBFA64D8F8CBB1066CCB9712CF8729D11AE5FE3E8654DDA74036BD6BEDCF17A2754ED268412559810EB5F7C581036C8A587DCCAEF3CF4FCAFBEA6292F366C84A8B8BC3014DE202ED45D4E58074B7B3101E234CC928B09FD3AABBEA8B08E69DA4B0A84F60713AF28745F86F2967BDC6A1407A65D2C49899F5B4BBC37AAF7DCF7DE2455826F34D1E56AEF1E701B5474AFC18C57560C3DD8EEA8A8751E0C7B9FD4DE7A81F3A1D434970527FB2D65DE65F572C3D7A6BF55D1F20194D9292A87F65F9ABC6BAEE992BC075EDFD9AF00F9C0C896D593EDBFCC78D764D3A5790F9C6CBD8A890F8C6BFBDA3FF7CCB4CE5BE8DE4B83E54227CDC71A552EB8ADFA374D9CC3097FE10309D28832BDB1A92E37D3292BC9A255588AE895EAEBDC44C5D2C491F44A12CD6AFBF535DBF01B3B9BC934ABD5548736E9CED6FF46DD994CB36E4DCDE53EEA9695558FAA5AF29675ACA9B6EA35D529D77AD25216DF16B3367E797F4D65C98338A5367B34DF885F4F15F2202E1972EAF4A83A963FF7C2DE59F9172361FF8EC8AA84E0FF7E24C5766DD72C64AEE9D2CF376FC1A25C44C8D0DC60861CD8522F424696C866F09AE798932BE749DE8E7FE95860E79ADEC52C881974197B0BB796F0E2414093FEA4B4BA6EF3F82EE0BFA221BA0066129E9BBFA33FC4C4750ABB678A9C90068247175946978F25E399DDD54B8174EBD38E4099FB8AA0E8017B810B60D11D9DA327BC8E6D40BF8F7885EC973203A803691F88BADBC79704AD42E4451946D91E7E02871DEFF9FDFF0187F074A438550000, N'6.1.1-30610')
GO

INSERT INTO [AspNetUsers] ([Id],[Email],[EmailConfirmed],[PasswordHash],[SecurityStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEndDateUtc],[LockoutEnabled],[AccessFailedCount],[UserName]) VALUES (N'1',NULL,1,N'AHQSmKnSLYrzj9vtdDWWnUXojjpmuDW2cHvWloGL9UL3TC9UCfBmbIuR2YCyg4BpNg==',N'',NULL,0,0,NULL,1,0,N'admin');
GO
INSERT INTO [Account] ([AccountId],[StoreId],[MemberId],[UserName],[RegisterType],[AccountState],[LastModified],[Created],[Discriminator]) VALUES (N'1',NULL,N'1',N'admin',2,1,NULL,NULL,'Account');
GO