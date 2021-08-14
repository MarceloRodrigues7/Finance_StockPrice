using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace Finance_StockPrice.Utils
{
    public class AppConsole
    {

        public static string CurrentVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public static string ValidConectionDb()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.State.ToString();
            };
        }

        public static string InitText = @"
@@@@@ @@@@@ @@@@@ @@@@@ @   @ @@@@@ @@@@@ @ @@@@@ @@@@@
@@      @   @   @ @@    @  @  @   @ @   @ @ @@    @@  
@@@@@   @   @   @ @@    @@@@  @@@@@ @@@@@ @ @@    @@@@@
   @@   @   @   @ @@    @  @  @     @ @   @ @@    @@
@@@@@   @   @@@@@ @@@@@ @   @ @     @  @  @ @@@@@ @@@@@";

        public static List<string> Keys = new() { "YOUTOKEN" };

        public static readonly string ConnectionString = "server=YOUSERVER;database=YOUDATABASE;user=YOUUSER;password=YOUPASSWORD;Connection Timeout=1200";
    }
}
