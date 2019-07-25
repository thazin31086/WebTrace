using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

/// <summary>
/// https://www.youtube.com/watch?v=ajwGHb16sgk
/// </summary>
namespace WebTrace.DB
{
    public class WebTraceDB
    {
        MySqlConnection conn;
        string configString = "SERVER=192.185.7.163;PORT=3306;DATABASE=thazinaung_webtrace;UID=thazi_webtrace;PASSWORD=webtr@cedb123;";
        public WebTraceDB()
        {
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = configString;
                conn.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
