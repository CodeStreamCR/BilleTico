/****** Object:  Table [dbo].[TBL_Commerce]    Script Date: 02/07/2025 06:49:41 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TBL_Commerce](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NULL,
	[Name] [varchar](30) NOT NULL,
	[Phone] [varchar](8) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Latitude] [decimal](9, 6) NOT NULL,
	[Longitude] [decimal](9, 6) NOT NULL,
	[Status] [varchar](10) NOT NULL,
	[IBAN] [varchar](22) NOT NULL,
	[CommissionRate] [decimal](5, 2) NOT NULL,
 CONSTRAINT [PK_TBL_Commerce] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

