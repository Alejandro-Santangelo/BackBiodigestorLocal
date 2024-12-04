-- Agregar columnas para la foto de perfil a la tabla UsuariosRegistrados
ALTER TABLE UsuariosRegistrados
ADD ProfilePicture VARBINARY(MAX) NULL,
    ProfilePictureContentType NVARCHAR(100) NULL;

-- Agregar una restricción para los tipos de contenido permitidos
ALTER TABLE UsuariosRegistrados
ADD CONSTRAINT CK_UsuariosRegistrados_ProfilePictureContentType
CHECK (ProfilePictureContentType IN ('image/jpeg', 'image/png', 'image/gif') OR ProfilePictureContentType IS NULL);

GO
