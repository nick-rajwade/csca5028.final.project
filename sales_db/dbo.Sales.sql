CREATE TABLE [dbo].[Sales](
                    [saleId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	                [store_id] UNIQUEIDENTIFIER NOT NULL,
	                [loyalty_card] bit NOT NULL,
	                [payment_type] varchar(10) NOT NULL CHECK (payment_type IN ('Cash', 'Creditcard', 'Check')),
	                [total_items] INT NOT NULL,
	                [total_price] DECIMAL(10,2) NOT NULL,
	                [json_item_list] varchar(max) NOT NULL,
	                [created_at] DATETIME NOT NULL,
	                [updated_at] DATETIME NOT NULL DEFAULT GETDATE(),
	                [CC_AUTH] varchar(7) NULL CHECK (CC_AUTH IN ('AUTH','DECLINE')),
	                [CC_AUTH_CODE] varchar(10) NULL,
                    CONSTRAINT FK_Sales_Store FOREIGN KEY (store_id) REFERENCES Stores(Id));