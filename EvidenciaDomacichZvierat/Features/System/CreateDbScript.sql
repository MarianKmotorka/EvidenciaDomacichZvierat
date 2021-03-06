CREATE TABLE [dbo].[Majitel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Meno] [varchar](255) NOT NULL,
	[Priezvisko] [varchar](255) NOT NULL,
	[DatumNarodenia] [date] NOT NULL,
 CONSTRAINT [PK_Majitel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[MajitelZviera](
	[MajitelId] [int] NOT NULL,
	[ZvieraId] [int] NOT NULL,
 CONSTRAINT [PK_MajitelZviera] PRIMARY KEY CLUSTERED 
(
	[MajitelId] ASC,
	[ZvieraId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Zviera](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Meno] [nvarchar](255) NOT NULL,
	[PocetKrmeni] [int] NOT NULL,
	[DatumNarodenia] [date] NOT NULL,
	[UrovenVycviku] [int] NULL,
	[PredpokladanyVzrast] [int] NULL,
	[ChytaMysi] [bit] NULL,
	[Discriminator] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Zviera] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[MajitelZviera]  WITH CHECK ADD  CONSTRAINT [FK_MajitelZviera_Majitel] FOREIGN KEY([MajitelId])
REFERENCES [dbo].[Majitel] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE

ALTER TABLE [dbo].[MajitelZviera] CHECK CONSTRAINT [FK_MajitelZviera_Majitel]

ALTER TABLE [dbo].[MajitelZviera]  WITH CHECK ADD  CONSTRAINT [FK_MajitelZviera_Zviera] FOREIGN KEY([ZvieraId])
REFERENCES [dbo].[Zviera] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE

ALTER TABLE [dbo].[MajitelZviera] CHECK CONSTRAINT [FK_MajitelZviera_Zviera]


