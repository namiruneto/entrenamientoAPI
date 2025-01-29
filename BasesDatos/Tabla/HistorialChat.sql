CREATE TABLE [dbo].[HistorialChat](
	[Id] [uniqueidentifier] NOT NULL,
	[Pregunta] [varchar](max) NOT NULL,
	[Respuesta] [varchar](max) NOT NULL,
	[Asesor] [bit] NOT NULL,
	[Categoria] [varchar](50) NOT NULL,
	[SubCategoria] [varchar](50) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO