-- Agregar columnas para la foto de perfil a la tabla UsuariosRegistrados
ALTER TABLE UsuariosRegistrados
ADD FotoPerfil VARBINARY(MAX) NULL,
    TipoContenidoFoto NVARCHAR(100) NULL;

-- Agregar una restricción para los tipos de contenido permitidos
ALTER TABLE UsuariosRegistrados
ADD CONSTRAINT CK_UsuariosRegistrados_TipoContenidoFoto
CHECK (TipoContenidoFoto IN ('image/jpeg', 'image/png', 'image/gif') OR TipoContenidoFoto IS NULL);

GO
