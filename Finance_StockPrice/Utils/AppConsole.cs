using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        public static List<string> Keys = new() { "YOUKEY", "YOUKEY", "YOUKEY" };

        public static readonly string ConnectionString = "server=YOUSERVER;database=YOUDB;user=YOUUSER;password=YOUPASS;Connection Timeout=1200";
    }
}
