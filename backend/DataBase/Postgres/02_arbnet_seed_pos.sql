-- ===================================================================
-- PARTE 2: INSERCIÓN DE DATOS INICIALES (Data Seed)
-- ===================================================================

INSERT INTO "Users" ("UserID", "FullName", "Email", "PasswordHash", "Status", "Country", "SubscriptionType") 
OVERRIDING SYSTEM VALUE
VALUES (
    1, 
    'Usuario Demo', 
    'demo@arbnet.com', 
    '$2a$11$8GRXqTRdWpFe9hGu8lb53.Vpzjmi461O9zfTw9yWTdzPrCqumARpW', 
    'Active', 
    'Venezuela', 
    'Free'
);