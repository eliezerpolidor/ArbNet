-- ===================================================================
-- PARTE 3: RESTRICCIONES Y LLAVES FORÁNEAS (SQL Server Nativo)
-- ===================================================================

ALTER TABLE OrdersP2P 
    ADD CONSTRAINT FK_OrdersP2P_Users 
    FOREIGN KEY (UserID) REFERENCES Users (UserID);

ALTER TABLE Subscriptions 
    ADD CONSTRAINT FK_Subscriptions_Users 
    FOREIGN KEY (UserID) REFERENCES Users (UserID);

ALTER TABLE Wallets 
    ADD CONSTRAINT FK_Wallets_Users 
    FOREIGN KEY (UserID) REFERENCES Users (UserID);

ALTER TABLE Transactions 
    ADD CONSTRAINT FK_Transactions_Users 
    FOREIGN KEY (UserID) REFERENCES Users (UserID);

ALTER TABLE Transactions 
    ADD CONSTRAINT FK_Transactions_Wallets 
    FOREIGN KEY (WalletID) REFERENCES Wallets (WalletID);

ALTER TABLE Transactions 
    ADD CONSTRAINT FK_Transactions_OrdersP2P 
    FOREIGN KEY (OrderID) REFERENCES OrdersP2P (OrderID);