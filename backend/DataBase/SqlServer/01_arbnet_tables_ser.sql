-- ===================================================================
-- PARTE 1: CREACIÓN DE ESTRUCTURAS DE TABLAS (SQL Server Nativo)
-- ===================================================================

CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL UNIQUE,
    PasswordHash VARCHAR(256) NOT NULL,
    RegisterDate DATETIME DEFAULT GETDATE(),
    Status VARCHAR(20) NOT NULL DEFAULT 'Active',
    ProfilePicture VARCHAR(250) NULL,
    Country VARCHAR(50) NULL,
    SubscriptionType VARCHAR(50) NULL
);

CREATE TABLE OrdersP2P (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    UnitPrice DECIMAL(18, 8) NOT NULL,
    Commission DECIMAL(18, 8) NOT NULL,
    TradeType VARCHAR(10) NOT NULL,
    Status VARCHAR(20) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    ExecutedAt DATETIME NULL,
    NetProfit DECIMAL(18, 2) NULL,
    OrderNumber VARCHAR(50) NULL,
    AdvNo VARCHAR(50) NULL,
    Asset VARCHAR(10) NULL,
    Fiat VARCHAR(10) NULL,
    FiatSymbol VARCHAR(5) NULL,
    TotalPrice DECIMAL(18, 2) NULL
);

CREATE TABLE Subscriptions (
    SubscriptionID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    PlanName VARCHAR(50) NOT NULL,
    StartDate DATETIME DEFAULT GETDATE(),
    EndDate DATETIME NULL,
    Status VARCHAR(20) DEFAULT 'Active',
    Price DECIMAL(18, 2) NULL
);

CREATE TABLE Wallets (
    WalletID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    WalletType VARCHAR(50) NOT NULL,
    Currency VARCHAR(10) NOT NULL,
    Balance DECIMAL(18, 2) NOT NULL DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    Status VARCHAR(20) DEFAULT 'Active'
);

CREATE TABLE Transactions (
    TransactionID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    WalletID INT NOT NULL,
    OrderID INT NULL,
    TransactionType VARCHAR(20) NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    Commission DECIMAL(18, 8) DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    Status VARCHAR(20) DEFAULT 'Pending',
    Asset VARCHAR(10) NULL,
    Fiat VARCHAR(10) NULL
);