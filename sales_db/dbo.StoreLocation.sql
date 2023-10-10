CREATE TABLE [dbo].[StoreLocation]
                            ([Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                            store_address VARCHAR(100) NOT NULL,
                            store_city VARCHAR(50) NOT NULL,
                            store_state VARCHAR(50) NOT NULL,
                            store_zip VARCHAR(10) NOT NULL,
                            store_country VARCHAR(50) NOT NULL,
                            lat DECIMAL(9,6) NOT NULL,
                            long DECIMAL(9,6) NOT NULL);