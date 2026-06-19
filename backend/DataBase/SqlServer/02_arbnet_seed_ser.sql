-- ===================================================================
-- PARTE 2: INSERCIÓN DE DATOS INICIALES (SQL Server Nativo)
-- ===================================================================

-- Habilitamos la inserción explícita de IDs en columnas de identidad
SET IDENTITY_INSERT Users ON;

INSERT INTO Users (UserID, FullName, Email, PasswordHash, Status, Country, SubscriptionType) 
VALUES (
    1, 
    'Usuario Demo', 
    'demo@arbnet.com', 
    '$2a$11$8GRXqTRdWpFe9hGu8lb53.Vpzjmi461O9zfTw9yWTdzPrCqumARpW', 
    'Active', 
    'Venezuela', 
    'Free'
);

SET IDENTITY_INSERT Users OFF;