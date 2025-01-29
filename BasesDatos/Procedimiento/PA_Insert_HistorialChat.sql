
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PA_Insert_HistorialChat]
    @HistorialChat NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO [dbo].[HistorialChat] (Id, Pregunta, Respuesta, Asesor, Categoria, SubCategoria)
    SELECT 
		JSON_VALUE(h.value, '$.IdChat') AS IdChat,
        JSON_VALUE(h.value, '$.Pregunta') AS Pregunta,
        ISNULL(JSON_VALUE(h.value, '$.respuesta'), '') AS Respuesta,
        JSON_VALUE(h.value, '$.Asesor') AS Asesor,
        ISNULL(JSON_VALUE(h.value, '$.categoria'), '') AS Categoria,
        ISNULL(JSON_VALUE(h.value, '$.subcategoria'), '') AS SubCategoria
    FROM OPENJSON(@HistorialChat) AS h;

		select 'COMPLETADO' as Message, 1 as Estatus
END
