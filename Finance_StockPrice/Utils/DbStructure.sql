IF EXISTS(SELECT * FROM sys.tables WHERE SCHEMA_NAME(schema_id) LIKE 'dbo' AND name like 'StockList') 
	DROP TABLE [dbo].[StockList];
CREATE TABLE StockList(
	Id BIGINT IDENTITY(1,1) PRIMARY KEY,
	Symbol VARCHAR(15),
	Name VARCHAR(120),
	Currency VARCHAR(20),
	Exchange VARCHAR(50),
	Country VARCHAR(50),
	Type VARCHAR(50),
	CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP)
  
IF EXISTS(SELECT * FROM sys.tables WHERE SCHEMA_NAME(schema_id) LIKE 'dbo' AND name like 'HistoryPrice') 
	DROP TABLE [dbo].[HistoryPrice];  
CREATE TABLE HistoryPrice(
	Id BIGINT IDENTITY(1,1) PRIMARY KEY,
	IdStockList BIGINT FOREIGN KEY REFERENCES StockList(Id),
	DateRegister DATETIME,
	Opening FLOAT,
	HighestValue FLOAT,
	LowerValue FLOAT,
	CloseValue FLOAT,
	Volume INT)