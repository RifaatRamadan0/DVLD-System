using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD___DataAccessLayer
{
    public class clsDriverData
    {
        public static DataTable GetAllDrivers()
        {
            DataTable dt = new DataTable();

            string Query = "SELECT * FROM Drivers_View";

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
                catch (Exception ex)
                {
                }
            }

            return dt;
        }

        public static bool GetDriverInfoByDriverID(int DriverID, ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool IsFound = false;

            string Query = "SELECT * FROM Drivers WHERE DriverID = @DriverID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@DriverID", DriverID);

                try
                {
                    Connection.Open();

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            PersonID = (int)Reader["PersonID"];
                            CreatedByUserID = (int)Reader["CreatedByUserID"];
                            CreatedDate = (DateTime)Reader["CreatedDate"];

                            IsFound = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return IsFound;
        }

        public static int AddNewDriver(int PersonID, int CreatedByUserID)
        {
            int DriverID = -1;

            string Query = @"INSERT INTO Drivers VALUES (@PersonID, @CreatedByUserID, @CreatedDate) SELECT SCOPE_IDENTITY()";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@PersonID", PersonID);
                Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                Command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                try
                {
                    Connection.Open();
                    object result = Command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    {
                        DriverID = InsertedID;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return DriverID;
        }

        public static bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            int RowsAffected = 0;

            string Query = @"UPDATE Drivers SET PersonID = @PersonID, CreatedByUserID = @CreatedByUserID, 
                        CreatedDate = @CreatedDate WHERE DriverID = @DriverID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@DriverID", DriverID);
                Command.Parameters.AddWithValue("@PersonID", PersonID);
                Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                Command.Parameters.AddWithValue("@CreatedDate", CreatedDate);

                try
                {
                    Connection.Open();
                    RowsAffected = Command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
            }

            return RowsAffected > 0;
        }

        public static bool GetDriverInfoByPersonID(int PersonID, ref int DriverID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool IsFound = false;

            string Query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@PersonID", PersonID);

                try
                {
                    Connection.Open();

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            DriverID = (int)Reader["DriverID"];
                            CreatedByUserID = (int)Reader["CreatedByUserID"];
                            CreatedDate = (DateTime)Reader["CreatedDate"];

                            IsFound = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return IsFound;
        }
    }
}
