USE BBS
DROP TABLE [e_Users]
DROP TABLE [e_BBS]
DROP TABLE [e_Topic]
DROP TABLE [e_Board]

--版区表（e_Board）
CREATE TABLE [dbo].[e_Board](
	[BoardId]		[INT] IDENTITY(1,1)	NOT NULL, --版区ID
	[Name]			[NVARCHAR](256)		NOT NULL, --版区名称
	[Depth]			[INT]				NOT NULL DEFAULT(0), --深度
	[ParentID]		[INT]				NOT NULL DEFAULT(0), --父版区ID
	[RootID]			[INT]				NOT NULL DEFAULT(0), --根版区ID
	[Order]			[INT]				NOT NULL DEFAULT(0), --排序
	[IsLeaf]		[INT]				NOT NULL DEFAULT(0), --叶子节点（0：不是；1：是，默认 0）
	[IsPublic]		[INT]				NOT NULL DEFAULT(0), --是否公共版区（0：不是；1：是，默认0）
	[BoardType]	[INT]				NOT NULL DEFAULT(0),--版区类型（0，无；1，问答；2，讨论；IsLeaf为0的一律为2，为0的不允许发帖）
	[Master]		[NVARCHAR](1000)	NOT NULL DEFAULT(''), --版主列表
	[AddTime]		[DateTime]			NOT NULL DEFAULT(GETDATE()),--添加时间
	[TopicNum]		[INT]				NOT NULL DEFAULT(0), --发帖总数
	[PostNum]		[INT]				NOT NULL DEFAULT(0), --回帖总数
	[ParentStr]		[VARCHAR](200)	NOT NULL DEFAULT(''),--父节点路径
	
CONSTRAINT [C_Board_PK]     
	PRIMARY KEY CLUSTERED    ( [BoardId] ASC ) 
ON [PRIMARY]) ON [PRIMARY]

--主帖表（e_Topic）:
CREATE TABLE [dbo].[e_Topic](
	[TopicId]		[INT] IDENTITY(1,1)	NOT NULL, --主帖ID
	[BoardId]		[INT]				NOT NULL, --版区ID
	[TopicType]		[INT]				NOT NULL DEFAULT(1), --帖子类型（1，问答，2，讨论；默认1）
	[Title]			[NVARCHAR](256)		NOT NULL, --帖子标题
	[Keywords]		[NVARCHAR](256)		NOT NULL, --关键字（用”,“区分）
	[PostUserId]		[INT]				NOT NULL, --发帖人ID
	[PostUserName]	[NVARCHAR](50)	NOT NULL, --发帖人昵称
	[PostTime]		[DateTime]			NOT NULL DEFAULT(GETDATE()), --发帖时间
	[PostIp]		[NVARCHAR](256)		NOT NULL, --发帖IP
	[LastPostUserId]		[INT]				 NULL, --最后发帖人ID
	[LastPostUserName]	[NVARCHAR](50)	 NULL, --最后发帖人昵称
	[LastPostTime]		[DateTime]			NULL DEFAULT(GETDATE()), --最后发帖时间
	[Status]		[INT]				NOT NULL DEFAULT(0), --状态（0：正常；1：删除）
	[IsFinish]		[INT]				NOT NULL DEFAULT(0), --是否结贴（0，否；1，是；默认 0）	
	[IsLock]		[INT]				NOT NULL DEFAULT(0), --是否锁定（0，否；1，是；默认 0）
	[IsDigest]		[INT]				NOT NULL DEFAULT(0), --是否精华（0，否；1，是；默认 0）
	[TopLevel]		[INT]				NOT NULL DEFAULT(0), --置顶等级（默认0，最高5，递增1）
	[Child]		[INT]				NOT NULL DEFAULT(0), --回帖数
	[Hits]			[INT]				NOT NULL DEFAULT(0), --点击数
	
CONSTRAINT [C_Topic_PK]     
	PRIMARY KEY CLUSTERED    ( [TopicId] ASC ) 
ON [PRIMARY]) ON [PRIMARY]

--帖子表（e_BBS）:
CREATE TABLE [dbo].[e_BBS](
	[BBSId]			[INT] IDENTITY(1,1)	NOT NULL, --帖子ID
	[TopicId]		[INT]				NOT NULL, --主帖ID
	[BoardId]		[INT]				NOT NULL, --版区ID
	[Content]		[Text]				NOT NULL, --帖子内容
	[PostUserId]		[INT]				NOT NULL, --发帖人ID
	[PostUserName]	[NVARCHAR](50)	NOT NULL, --发帖人昵称
	[PostTime]		[DateTime]			NOT NULL DEFAULT(GETDATE()), --发帖时间
	[PostIp]		[NVARCHAR](256)		NOT NULL, --发帖IP
	[IsTopic]		[INT]				NOT NULL DEFAULT(0), --是否主贴（0，否；1，是；默认 0）
	[IsAnswer]		[INT]				NOT NULL DEFAULT(0), --是否被设定为答案（0，否；1，是；默认 0）
	[DisplayMode]	[INT]				NOT NULL DEFAULT(0), --显示模式（0，公开，1，私有；默认0，用于处理不公开显示帖子）
	[AnswerMode]	[INT]				NOT NULL DEFAULT(0), --答案模式（0，公开，1，私有；默认0，用于处理只针对提问人显示的答案）
	[Status]		[INT]				NOT NULL DEFAULT(0), --状态（0：正常；1：删除）
	
CONSTRAINT [C_BBS_PK]     
	PRIMARY KEY CLUSTERED    ( [BBSId] ASC ) 
ON [PRIMARY]) ON [PRIMARY]


--用户表（e_Users）:
CREATE TABLE [dbo].[e_Users](
	[UserId]		[INT] IDENTITY(1,1)	NOT NULL, --用户ID
	[Level]			[INT]				NOT NULL DEFAULT(0), --等级（0,注册用户；1，学员；2，教师；9，管理员）
	[LoginName]		[NVARCHAR](50)		NOT NULL, --用户名
	[Password]		[NVARCHAR](50)		NOT NULL, --密码
	[NickName]		[NVARCHAR](50)		NOT NULL DEFAULT(''), --昵称
	[RealName]		[NVARCHAR](50)		NOT NULL DEFAULT(''), --真实姓名
	[Gender]		[NVARCHAR](10)		NOT NULL, --性别
	[UserEmail]		[NVARCHAR](255)		NULL, --电子信箱
	[UserSign]		[NTEXT]								NULL, --签名
	[UserQuesion]		[NVARCHAR](50)		NOT NULL DEFAULT(''), --密码保护问题 
	[UserAnswer]		[NVARCHAR](50)		NOT NULL DEFAULT(''), --密码保护问题答案 
	[AddTime]		[DateTime]			NOT NULL DEFAULT(GETDATE()), --注册时间
	[AddIp]			[NVARCHAR](255) NOT NULL,--注册IP
	[TopicCnt]		[INT]				NOT NULL DEFAULT(0), --发帖总数
	[PostCnt]		[INT]				NOT NULL DEFAULT(0), --回帖总数
	[DelCnt]			[INT]				NOT NULL DEFAULT(0), --被删帖子数
	[LoginCnt]		[INT]				NOT NULL DEFAULT(0), --登录次数
	[Grade]			[INT]				NOT NULL DEFAULT(0), --积分
	[LastLoginTime]	[DateTime]			NOT NULL DEFAULT(GETDATE()), --最后登录日期
	[Status]		[INT]				NOT NULL DEFAULT(0), --状态（0：正常；1：删除）
	
CONSTRAINT [C_Users_PK]     
	PRIMARY KEY CLUSTERED    ( [UserId] ASC ) 
ON [PRIMARY]) ON [PRIMARY]

ALTER TABLE [e_Topic]  WITH CHECK ADD  CONSTRAINT [FK_Topic_User]   
	FOREIGN KEY([UserId])
REFERENCES [e_Users] ([UserId])

ALTER TABLE [e_BBS]  WITH CHECK ADD  CONSTRAINT [FK_BBS_Topic]   
	FOREIGN KEY([TopicId])
REFERENCES [e_Topic] ([TopicId])

ALTER TABLE [e_BBS]  WITH CHECK ADD  CONSTRAINT [FK_BBS_User]   
	FOREIGN KEY([UserId])
REFERENCES [e_Users] ([UserId])

ALTER TABLE [dbo].[e_Topic]  WITH CHECK ADD  CONSTRAINT [FK_Topic_Product] 
	FOREIGN KEY([BoardId])
	REFERENCES [dbo].[e_Board] ([BoardId])
GO

--添加应用记录    
INSERT INTO [dbo].[e_Board]
		   ([Name],[BoardType],[Path])
 VALUES
		   ('默认版区',2,'-1-')

INSERT INTO [dbo].[e_Board]
		   ([Name],[Depth],[ParentID],[Order],[IsLeaf],[BoardType],[Path])
VALUES
		   ('默认版区子版区一',1,1,1,1,1,'-1-2-')

INSERT INTO [dbo].[e_Board]
		   ([Name],[Depth],[ParentID],[Order],[IsLeaf],[BoardType],[Path])
VALUES
		   ('默认版区子版区二',1,1,2,1,1,'-1-3-')
		   
INSERT INTO [dbo].[e_Users]
		   ([LoginName],[Password],[Level],[AddIp])
VALUES
		   ('admin','welcome',9,'')
		   
 INSERT INTO [dbo].[e_Users]
		   ([LoginName],[Password],[AddIp])
VALUES
		   ('test','test','')
GO