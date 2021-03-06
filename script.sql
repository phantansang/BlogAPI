USE [BlogDB]
GO
/****** Object:  Table [dbo].[tblPOST]    Script Date: 13/05/2022 3:50:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPOST](
	[_PostID] [varchar](20) NOT NULL,
	[_Title] [nvarchar](200) NULL,
	[_MetaTitle] [nvarchar](200) NULL,
	[_Category] [varchar](20) NULL,
	[_Imagepath] [nvarchar](200) NULL,
	[_Description] [nvarchar](1000) NULL,
	[_Content] [nvarchar](max) NULL,
	[_Author] [varchar](20) NULL,
	[_Status] [int] NOT NULL,
	[_CreateTime] [datetime] NULL,
	[_LastUpdate] [datetime] NULL,
	[_Views] [int] NULL,
	[_Log] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblPOST] PRIMARY KEY CLUSTERED 
(
	[_PostID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[tblPOST] ([_PostID], [_Title], [_MetaTitle], [_Category], [_Imagepath], [_Description], [_Content], [_Author], [_Status], [_CreateTime], [_LastUpdate], [_Views], [_Log]) VALUES (N'actice0001', N'Post title 01', N'post-title-01', N'actice', N'/Content/image/post-title-01.webp', N'This is decription', N'<b>This is content of post 01</b>', N'ptsang', 1, CAST(N'2022-05-13T12:10:35.070' AS DateTime), CAST(N'2022-05-13T12:10:35.070' AS DateTime), 100, NULL)
INSERT [dbo].[tblPOST] ([_PostID], [_Title], [_MetaTitle], [_Category], [_Imagepath], [_Description], [_Content], [_Author], [_Status], [_CreateTime], [_LastUpdate], [_Views], [_Log]) VALUES (N'uncategorized0001', N'Post title 02', N'post-title-02', N'uncategorized', N'/Content/image/post-title-02.webp', N'This is decription', N'<b>This is content of post 02</b>', N'ptsang', 1, CAST(N'2022-05-13T12:10:35.070' AS DateTime), CAST(N'2022-05-13T12:10:35.070' AS DateTime), 100, NULL)
GO
