
GO
/****** Object:  Table [dbo].[GGUser]    Script Date: 11/25/2019 10:39:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GGUser](
	[UserID] [varchar](50) NOT NULL,
	[PasswordMD5] [varchar](100) NOT NULL,
	[Phone] [varchar](20) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Friends] [nvarchar](4000) NOT NULL,
	[CommentNames] [nvarchar](max) NOT NULL,
	[OrgID] [nvarchar](20) NOT NULL,
	[Signature] [nvarchar](100) NOT NULL,
	[HeadImageIndex] [int] NOT NULL,
	[HeadImageData] [image] NULL,
	[Groups] [varchar](1000) NOT NULL,
	[UserState] [int] NOT NULL,
	[PcOfflineTime] [datetime] NOT NULL,
	[MobileOfflineTime] [datetime] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Version] [int] NOT NULL,
 CONSTRAINT [PK_GGUser] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0正常，1冻结，2禁言，3停用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GGUser', @level2type=N'COLUMN',@level2name=N'UserState'
GO

/****** Object:  Table [dbo].[GGGroup]    Script Date: 11/25/2019 10:39:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GGGroup](
	[GroupID] [varchar](20) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[CreatorID] [varchar](20) NOT NULL,
	[Announce] [nvarchar](200) NOT NULL,
	[Members] [nvarchar](4000) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[IsPrivate] [bit] NOT NULL,
	[Version] [int] NOT NULL,
 CONSTRAINT [PK_GGGroup] PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GGConfiguration]    Script Date: 11/25/2019 10:39:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GGConfiguration](
	[GGKey] [varchar](20) NOT NULL,
	[GGValue] [nvarchar](1000) NOT NULL,
 CONSTRAINT [PK_GGConfiguration] PRIMARY KEY CLUSTERED 
(
	[GGKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OfflineMessage]    Script Date: 11/25/2019 10:39:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OfflineMessage](
	[AutoID] [int] IDENTITY(1,1) NOT NULL,
	[SourceUserID] [varchar](50) NOT NULL,
	[DestUserID] [varchar](50) NOT NULL,
	[SourceType] [int] NOT NULL,
	[GroupID] [varchar](50) NOT NULL,
	[InformationType] [int] NOT NULL,
	[Information] [image] NULL,
	[Tag] [nvarchar](100) NOT NULL,
	[TimeTransfer] [datetime] NOT NULL,
 CONSTRAINT [PK_OfflineMessage] PRIMARY KEY CLUSTERED 
(
	[AutoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送离线消息的用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OfflineMessage', @level2type=N'COLUMN',@level2name=N'SourceUserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'接收离线消息的用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OfflineMessage', @level2type=N'COLUMN',@level2name=N'DestUserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送者设备' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OfflineMessage', @level2type=N'COLUMN',@level2name=N'SourceType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'该字段用于群离线消息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OfflineMessage', @level2type=N'COLUMN',@level2name=N'GroupID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'信息的类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OfflineMessage', @level2type=N'COLUMN',@level2name=N'InformationType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'信息内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OfflineMessage', @level2type=N'COLUMN',@level2name=N'Information'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'附带信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OfflineMessage', @level2type=N'COLUMN',@level2name=N'Tag'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务器接收到要转发离线消息的时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OfflineMessage', @level2type=N'COLUMN',@level2name=N'TimeTransfer'
GO
/****** Object:  Table [dbo].[OfflineFileItem]    Script Date: 11/25/2019 10:39:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OfflineFileItem](
	[AutoID] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](100) NOT NULL,
	[FileLength] [int] NOT NULL,
	[SenderID] [varchar](50) NOT NULL,
	[SenderType] [int] NOT NULL,
	[AccepterType] [int] NOT NULL,
	[AccepterID] [varchar](50) NOT NULL,
	[RelayFilePath] [nvarchar](300) NOT NULL,
 CONSTRAINT [PK_OfflineFileItem] PRIMARY KEY CLUSTERED 
(
	[AutoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'唯一编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OfflineFileItem', @level2type=N'COLUMN',@level2name=N'AutoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'离线文件的名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OfflineFileItem', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件的大小' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OfflineFileItem', @level2type=N'COLUMN',@level2name=N'FileLength'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OfflineFileItem', @level2type=N'COLUMN',@level2name=N'SenderID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送者设备' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OfflineFileItem', @level2type=N'COLUMN',@level2name=N'SenderType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'接收者设备。（用于多端助手的文件传送，如果是他人发给自己的离线文件，则忽略该字段）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OfflineFileItem', @level2type=N'COLUMN',@level2name=N'AccepterType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'接收者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OfflineFileItem', @level2type=N'COLUMN',@level2name=N'AccepterID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'在服务器上存储离线文件的临时路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OfflineFileItem', @level2type=N'COLUMN',@level2name=N'RelayFilePath'
GO
/****** Object:  Table [dbo].[GroupBan]    Script Date: 11/25/2019 10:39:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GroupBan](
	[AutoID] [int] IDENTITY(1,1) NOT NULL,
	[GroupID] [varchar](20) NOT NULL,
	[OperatorID] [varchar](20) NOT NULL,
	[UserID] [varchar](20) NOT NULL,
	[Comment2] [nvarchar](50) NOT NULL,
	[EnableTime] [datetime] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_GroupBan] PRIMARY KEY CLUSTERED 
(
	[AutoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'群组ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBan', @level2type=N'COLUMN',@level2name=N'GroupID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作人ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBan', @level2type=N'COLUMN',@level2name=N'OperatorID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被禁用者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBan', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBan', @level2type=N'COLUMN',@level2name=N'Comment2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'截至时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBan', @level2type=N'COLUMN',@level2name=N'EnableTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBan', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
/****** Object:  Table [dbo].[ChatMessageRecord]    Script Date: 11/25/2019 10:39:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChatMessageRecord](
	[AutoID] [bigint] IDENTITY(1,1) NOT NULL,
	[SpeakerID] [varchar](20) NOT NULL,
	[AudienceID] [varchar](20) NOT NULL,
	[IsGroupChat] [bit] NOT NULL,
	[Content] [image] NOT NULL,
	[OccureTime] [datetime] NOT NULL,
 CONSTRAINT [PK_ChatMessageRecord] PRIMARY KEY CLUSTERED 
(
	[AutoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AddGroupRequest]    Script Date: 11/25/2019 10:39:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AddGroupRequest](
	[AutoID] [int] IDENTITY(1,1) NOT NULL,
	[RequesterID] [varchar](20) NOT NULL,
	[GroupID] [varchar](20) NOT NULL,
	[AccepterID] [varchar](20) NOT NULL,
	[Comment2] [nvarchar](500) NOT NULL,
	[State] [int] NOT NULL,
	[Notified] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_AddGroupRequest] PRIMARY KEY CLUSTERED 
(
	[AutoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0:请求中 1：同意  2：拒绝' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AddGroupRequest', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0:申请未通知对方  1：申请已通知对方' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AddGroupRequest', @level2type=N'COLUMN',@level2name=N'Notified'
GO
/****** Object:  Table [dbo].[AddFriendRequest]    Script Date: 11/25/2019 10:39:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AddFriendRequest](
	[AutoID] [int] IDENTITY(1,1) NOT NULL,
	[RequesterID] [varchar](50) NOT NULL,
	[AccepterID] [varchar](50) NOT NULL,
	[RequesterCatalogName] [nvarchar](20) NOT NULL,
	[AccepterCatalogName] [nvarchar](20) NOT NULL,
	[Comment2] [nvarchar](500) NOT NULL,
	[State] [int] NOT NULL,
	[Notified] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_AddFriendRequest] PRIMARY KEY CLUSTERED 
(
	[AutoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0:请求中 1：同意  2：拒绝' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AddFriendRequest', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0:申请未通知对方  1：申请已通知对方' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AddFriendRequest', @level2type=N'COLUMN',@level2name=N'Notified'
GO
/****** Object:  Default [DF_AddFriendRequest_State]    Script Date: 11/25/2019 10:39:03 ******/
ALTER TABLE [dbo].[AddFriendRequest] ADD  CONSTRAINT [DF_AddFriendRequest_State]  DEFAULT ((0)) FOR [State]
GO
/****** Object:  Default [DF_AddFriendRequest_Notified]    Script Date: 11/25/2019 10:39:03 ******/
ALTER TABLE [dbo].[AddFriendRequest] ADD  CONSTRAINT [DF_AddFriendRequest_Notified]  DEFAULT ((0)) FOR [Notified]
GO
/****** Object:  Default [DF_AddGroupRequest_State]    Script Date: 11/25/2019 10:39:03 ******/
ALTER TABLE [dbo].[AddGroupRequest] ADD  CONSTRAINT [DF_AddGroupRequest_State]  DEFAULT ((0)) FOR [State]
GO
/****** Object:  Default [DF_AddGroupRequest_Notified]    Script Date: 11/25/2019 10:39:03 ******/
ALTER TABLE [dbo].[AddGroupRequest] ADD  CONSTRAINT [DF_AddGroupRequest_Notified]  DEFAULT ((0)) FOR [Notified]
GO
/****** Object:  Default [DF_GGGroup_Private]    Script Date: 11/25/2019 10:39:03 ******/
ALTER TABLE [dbo].[GGGroup] ADD  CONSTRAINT [DF_GGGroup_Private]  DEFAULT ((0)) FOR [IsPrivate]
GO
/****** Object:  Default [DF_GGUser_Phone]    Script Date: 11/25/2019 10:39:03 ******/
ALTER TABLE [dbo].[GGUser] ADD  CONSTRAINT [DF_GGUser_Phone]  DEFAULT ('') FOR [Phone]
GO
/****** Object:  Default [DF_GGUser_CommentNames]    Script Date: 11/25/2019 10:39:03 ******/
ALTER TABLE [dbo].[GGUser] ADD  CONSTRAINT [DF_GGUser_CommentNames]  DEFAULT ('') FOR [CommentNames]
GO
/****** Object:  Default [DF_GGUser_UserState]    Script Date: 11/25/2019 10:39:03 ******/
ALTER TABLE [dbo].[GGUser] ADD  CONSTRAINT [DF_GGUser_UserState]  DEFAULT ((0)) FOR [UserState]
GO


INSERT [dbo].[GGConfiguration] ([GGKey], [GGValue]) VALUES (N'GGVersion', N'1')


