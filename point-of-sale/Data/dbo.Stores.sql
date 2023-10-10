CREATE TABLE [dbo].[Stores](
                            [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	                        store_name NVARCHAR(50) NOT NULL,
	                        healthcheckurl NVARCHAR(200) NOT NULL,
	                        healthcheckinterval INT NOT NULL,
	                        store_location_id INT NOT NULL, -- FK to StoreLocation.Id
	                        CONSTRAINT FK_Stores_StoreLocation FOREIGN KEY (store_location_id) REFERENCES StoreLocation(Id));