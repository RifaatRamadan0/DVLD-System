using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___DataAccessLayer
{
    public class clsCountryData
    {
        public static bool GetCountryByID(int CountryID, ref string CountryName)
        {
            string Query = "SELECT * FROM Countries WHERE CountryID = @CountryID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@CountryID", CountryID);

                try
                {
                    Connection.Open();
                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            CountryName = Reader["CountryName"].ToString();
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }

            return false;
        }

        public static bool GetCountryByName(string CountryName, ref int CountryID)
        {
            string Query = "SELECT * FROM Countries WHERE CountryName = @CountryName";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@CountryName", CountryName);

                try
                {
                    Connection.Open();
                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            CountryID = int.Parse(Reader["CountryID"].ToString());
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }

            return false;
        }

        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();
            string Query = "SELECT * FROM Countries";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                try
                {
                    Connection.Open();
                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.HasRows)
                        {
                            dt.Load(Reader);
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }

            return dt;
        }
    }
}
