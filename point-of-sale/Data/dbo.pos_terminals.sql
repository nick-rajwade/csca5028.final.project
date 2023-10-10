                      CREATE TABLE [dbo].[pos_terminals](
                      [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
                      pos_checkout_time INT NOT NULL,    
                      store_id UNIQUEIDENTIFIER NOT NULL,
                      CONSTRAINT FK_pos_terminals_Stores FOREIGN KEY (store_id) REFERENCES Stores(Id));