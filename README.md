# Finance_StockPrice
Application performs current information query about actions via API twelvedata and save history in Database.

* Register the history of shares registered in StockList.
* Queries made through the API [twelvedata](https://twelvedata.com)
  * Change an API Key string in **Utils.Api Console**.<br/>
  ```public static List<string> Keys = new() { "YOUTOKEN" };```

* Database
  * Change the database connection string in **Utils.AppConsole**.<br/>
  ```public static readonly string ConnectionString = "server=YOUSERVER;database=YOUDATABASE;user=YOUUSER;password=YOUPASSWORD;Connection Timeout=1200";```
  
* Table StockList:<br/>
	```
	CREATE TABLE StockList(
		Id BIGINT IDENTITY(1,1) PRIMARY KEY,
		Symbol VARCHAR(15),
		Name VARCHAR(120),
		Currency VARCHAR(20),
		Exchange VARCHAR(50),
		Country VARCHAR(50),
		Type VARCHAR(50),
		CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP)
* Table HistoryPrice:<br/>
  	```
	CREATE TABLE HistoryPrice(
		Id BIGINT IDENTITY(1,1) PRIMARY KEY,
		IdStockList BIGINT FOREIGN KEY REFERENCES StockList(Id),
		DateRegister DATETIME,
		Opening FLOAT,
		HighestValue FLOAT,
		LowerValue FLOAT,
		CloseValue FLOAT,
		Volume INT)
* Used [SDK](https://github.com/pseudomarkets/TwelveDataSharp) for request the API.
* Used [Dapper](https://github.com/DapperLib/Dapper) and [SqlClient](https://github.com/dotnet/SqlClient) for transact in Database.
