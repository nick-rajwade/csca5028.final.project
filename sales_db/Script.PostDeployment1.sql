/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
INSERT INTO [dbo].[StoreLocation] (store_address, store_city, store_state, store_zip, store_country, lat, long)
            VALUES ('123 Main St', 'New York', 'NY', '10001', 'USA', 40.7128, -74.0060),
            ('456 Main St', 'Los Angeles', 'CA', '90001', 'USA', 34.0522, -118.2437),
            ('789 Main St', 'Chicago', 'IL', '60007', 'USA', 41.8781, -87.6298),
            ('101 Main St', 'Houston', 'TX', '77001', 'USA', 29.7604, -95.3698),
            ('112 Main St', 'Phoenix', 'AZ', '85001', 'USA', 33.4484, -112.0740),
            ('131 Main St', 'Philadelphia', 'PA', '19019', 'USA', 39.9526, -75.1652),
            ('415 Main St', 'San Antonio', 'TX', '78201', 'USA', 29.4241, -98.4936),
            ('161 Main St', 'San Diego', 'CA', '92101', 'USA', 32.7157, -117.1611),
            ('718 Main St', 'Dallas', 'TX', '75201', 'USA', 32.7767, -96.7970),
            ('919 Main St', 'San Jose', 'CA', '95101', 'USA', 37.3382, -121.8863);

INSERT INTO [dbo].[Stores] (store_name, healthcheckurl, healthcheckinterval, store_location_id)
            VALUES ('New York Store', 'new-york-pos', 5, 1),
            ('Los Angeles Store', 'la-pos', 5, 2),
            ('Chicago Store', 'chicago-pos', 5, 3),
            ('Houston Store', 'houston-pos', 5, 4),
            ('Phoenix Store', 'phoenix-pos', 5, 5),
            ('Philadelphia Store', 'philly-pos', 5, 6),
            ('San Antonio Store', 'sanantonio-pos', 5, 7),
            ('San Diego Store', 'sandiego-pos', 5, 8),
            ('Dallas Store', 'dallas-pos', 5, 9),
            ('San Jose Store', 'sanjose-pos', 5, 10);

-- Inserting 2 POS terminals for each store
DECLARE @i INT = 1;
DECLARE @storeId UNIQUEIDENTIFIER;
WHILE @i <= 2
BEGIN
	SET @storeId = (SELECT Id FROM Stores WHERE store_name = 'New York Store');
	INSERT INTO [dbo].[pos_terminals] (pos_checkout_time, store_id) VALUES (5, @storeId);
	SET @i = @i + 1;
END

SET @i = 1;
WHILE @i <= 2
BEGIN
	SET @storeId = (SELECT Id FROM Stores WHERE store_name = 'Los Angeles Store');
	INSERT INTO [dbo].[pos_terminals] (pos_checkout_time, store_id) VALUES (5, @storeId);
	SET @i = @i + 1;
END

SET @i = 1;
WHILE @i <= 2
BEGIN
	SET @storeId = (SELECT Id FROM Stores WHERE store_name = 'Chicago Store');
	INSERT INTO [dbo].[pos_terminals] (pos_checkout_time, store_id) VALUES (5, @storeId);
	SET @i = @i + 1;
END

SET @i = 1;
WHILE @i <= 2
BEGIN
	SET @storeId = (SELECT Id FROM Stores WHERE store_name = 'Houston Store');
	INSERT INTO [dbo].[pos_terminals] (pos_checkout_time, store_id) VALUES (5, @storeId);
	SET @i = @i + 1;
END

SET @i = 1;
WHILE @i <= 2
BEGIN
	SET @storeId = (SELECT Id FROM Stores WHERE store_name = 'Phoenix Store');
	INSERT INTO [dbo].[pos_terminals] (pos_checkout_time, store_id) VALUES (5, @storeId);
	SET @i = @i + 1;
END

SET @i = 1;
WHILE @i <= 2
BEGIN
	SET @storeId = (SELECT Id FROM Stores WHERE store_name = 'Philadelphia Store');
	INSERT INTO [dbo].[pos_terminals] (pos_checkout_time, store_id) VALUES (5, @storeId);
	SET @i = @i + 1;
END

SET @i = 1;
WHILE @i <= 2
BEGIN
	SET @storeId = (SELECT Id FROM Stores WHERE store_name = 'San Antonio Store');
	INSERT INTO [dbo].[pos_terminals] (pos_checkout_time, store_id) VALUES (5, @storeId);
	SET @i = @i + 1;
END

SET @i = 1;
WHILE @i <= 2
BEGIN
	SET @storeId = (SELECT Id FROM Stores WHERE store_name = 'San Diego Store');
	INSERT INTO [dbo].[pos_terminals] (pos_checkout_time, store_id) VALUES (5, @storeId);
	SET @i = @i + 1;
END

SET @i = 1;
WHILE @i <= 2
BEGIN
	SET @storeId = (SELECT Id FROM Stores WHERE store_name = 'Dallas Store');
	INSERT INTO [dbo].[pos_terminals] (pos_checkout_time, store_id) VALUES (5, @storeId);
	SET @i = @i + 1;
END

SET @i = 1;
WHILE @i <= 2
BEGIN
	SET @storeId = (SELECT Id FROM Stores WHERE store_name = 'San Jose Store');
	INSERT INTO [dbo].[pos_terminals] (pos_checkout_time, store_id) VALUES (5, @storeId);
	SET @i = @i + 1;
END