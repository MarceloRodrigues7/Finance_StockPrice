# Finance_StockPrice
Application performs current information query about actions via API hgBrasil and save history in SqlAzure.

* Update main table information and perform history record (must be 1 hour after the last record of the same action)
* Queries made through the API [hgBrasil](https://console.hgbrasil.com)
* Database [SqlAzure](https://github.com/microsoft/sql-server-samples).
  * Table Finance_StockPrice: </br>
    ```Company_Name, Symbol,Market_Cap,Price,Change_Percent,Updated_At.```
  * Table History_Finance: </br>
    ```IdCompany,Market_Cap,Price,Change_Percent,Day,Hour.```
* Used [Refit](https://github.com/reactiveui/refit) for request the API.
* Used [Dapper](https://github.com/DapperLib/Dapper) and [SqlClient](https://github.com/dotnet/SqlClient) for transact in Database.
