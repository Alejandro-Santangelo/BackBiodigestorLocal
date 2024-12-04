-- Crear tabla para las fotos de perfil
CREATE TABLE ProfilePictures (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NumeroCliente INT NOT NULL,
    ProfilePicture VARBINARY(MAX) NOT NULL,
    ContentType NVARCHAR(100) NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_ProfilePictures_Cliente FOREIGN KEY (NumeroCliente) 
        REFERENCES Cliente(NumeroCliente) ON DELETE CASCADE,
    CONSTRAINT CK_ProfilePictures_ContentType 
        CHECK (ContentType IN ('image/jpeg', 'image/png', 'image/gif'))
);

-- Crear índice para mejorar el rendimiento
CREATE INDEX IX_ProfilePictures_NumeroCliente 
ON ProfilePictures(NumeroCliente);

GO
