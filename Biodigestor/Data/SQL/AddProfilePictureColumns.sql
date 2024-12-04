-- Verificar si las columnas ya existen antes de agregarlas
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'ProfilePicture')
BEGIN
    ALTER TABLE [dbo].[AspNetUsers]
    ADD ProfilePicture VARBINARY(MAX) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'ProfilePictureContentType')
BEGIN
    ALTER TABLE [dbo].[AspNetUsers]
    ADD ProfilePictureContentType NVARCHAR(100) NULL;
END

-- Crear un índice para mejorar el rendimiento de las búsquedas por usuario
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AspNetUsers_ProfilePicture' AND object_id = OBJECT_ID('AspNetUsers'))
BEGIN
    CREATE INDEX [IX_AspNetUsers_ProfilePicture] ON [dbo].[AspNetUsers] (Id)
    INCLUDE (ProfilePicture, ProfilePictureContentType);
END

-- Agregar una restricción de tamaño máximo para el tipo de contenido
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_AspNetUsers_ProfilePictureContentType')
BEGIN
    ALTER TABLE [dbo].[AspNetUsers]
    ADD CONSTRAINT [CK_AspNetUsers_ProfilePictureContentType]
    CHECK (ProfilePictureContentType IN ('image/jpeg', 'image/png', 'image/gif') OR ProfilePictureContentType IS NULL);
END

GO
