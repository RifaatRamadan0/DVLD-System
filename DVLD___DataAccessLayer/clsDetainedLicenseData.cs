using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___DataAccessLayer
{
    public class clsDetainedLicenseData
    {

        public static bool IsDetainedLicense(int LicenseID)
        {
            bool IsDetained = false;
            string Query = @"SELECT IsDetained = 1 FROM DetainedLicenses WHERE LicenseID = @LicenseID AND IsReleased = 0";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@LicenseID", LicenseID);

                try
                {
                    Connection.Open();
                    object result = Command.ExecuteScalar();

                    if (result != null)
                    {
                        IsDetained = Convert.ToBoolean(result);
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return IsDetained;
        }

        public static int AddNewDetainedLicense(int LicenseID, DateTime DetainDate, float FineFees,
                int CreatedByUserID)
        {
            int DetainID = -1;
            string Query = @"INSERT INTO DetainedLicenses VALUES (@LicenseID, @DetainDate, @FineFees, 
                        @CreatedByUserID, 0) SELECT SCOPE_IDENTITY()";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@LicenseID", LicenseID);
                Command.Parameters.AddWithValue("@DetainDate", DetainDate);
                Command.Parameters.AddWithValue("@FineFees", FineFees);
                Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                try
                {
                    Connection.Open();
                    object result = Command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    {
                        DetainID = InsertedID;
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return DetainID;
        }

        public static bool UpdateDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate, float FineFees,
                int CreatedByUserID)
        {
            int RowsAffected = 0;
            string Query = @"UPDATE DetainedLicenses SET LiecnseID = @LicenseID, DetainDate = @DetainDate, 
                       FineFees = @FineFees, CreatedByUserID = @CreatedByUserID WHERE DetainID = @DetainID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@DetainID", DetainID);
                Command.Parameters.AddWithValue("@LicenseID", LicenseID);
                Command.Parameters.AddWithValue("@DetainDate", DetainDate);
                Command.Parameters.AddWithValue("@FineFees", FineFees);
                Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

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

        public static bool GetDetainedLicenseInfoByLicenseID(int LicenseID, ref int DetainID, ref DateTime DetainDate, ref float FineFees,
         ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate, ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool IsFound = false;
            string Query = @"SELECT TOP 1 * FROM DetainedLicenses WHERE LicenseID = @LicenseID ORDER BY DetainID DESC";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@LicenseID", LicenseID);

                try
                {
                    Connection.Open();
                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            DetainID = (int)Reader["DetainID"];
                            DetainDate = (DateTime)Reader["DetainDate"];
                            FineFees = Convert.ToSingle(Reader["FineFees"]);
                            CreatedByUserID = (int)Reader["CreatedByUserID"];
                            IsReleased = (bool)Reader["IsReleased"];
                            ReleaseDate = Reader["ReleaseDate"] == DBNull.Value ? default : (DateTime)Reader["ReleaseDate"];
                            ReleasedByUserID = Reader["ReleasedByUserID"] == DBNull.Value ? -1 : (int)Reader["ReleasedByUserID"];
                            ReleaseApplicationID = Reader["ReleaseApplicationID"] == DBNull.Value ? -1 : (int)Reader["ReleaseApplicationID"];

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

        public static bool GetDetainedLicenseInfoByDetainID(int DetainID, ref int LicenseID, ref DateTime DetainDate, ref float FineFees,
         ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate, ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool IsFound = false;
            string Query = @"SELECT TOP 1 * FROM DetainedLicenses WHERE DetainID = @DetainID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@DetainID", DetainID);

                try
                {
                    Connection.Open();
                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            LicenseID = (int)Reader["LicenseID"];
                            DetainDate = (DateTime)Reader["DetainDate"];
                            FineFees = Convert.ToSingle(Reader["FineFees"]);
                            CreatedByUserID = (int)Reader["CreatedByUserID"];
                            IsReleased = (bool)Reader["IsReleased"];
                            ReleaseDate = Reader["ReleaseDate"] == DBNull.Value ? default : (DateTime)Reader["ReleaseDate"];
                            ReleasedByUserID = Reader["ReleasedByUserID"] == DBNull.Value ? -1 : (int)Reader["ReleasedByUserID"];
                            ReleaseApplicationID = Reader["ReleaseApplicationID"] == DBNull.Value ? -1 : (int)Reader["ReleaseApplicationID"];

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

        public static bool ReleaseDetainedLicense(int DetainID, int ReleasedByUserID, int ReleaseApplicationID)
        {
            int RowsAffected = 0;
            string Query = @"UPDATE DetainedLicenses SET IsReleased = 1, ReleaseDate = @ReleaseDate, 
                    ReleasedByUserID = @ReleasedByUserID, ReleaseApplicationID = @ReleaseApplicationID WHERE DetainID = @DetainID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@DetainID", DetainID);
                Command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
                Command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
                Command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);

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

        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dt = new DataTable();
            string Query = @"SELECT * FROM DetainedLicenses_View ORDER BY IsReleased, DetainDate ASC";

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
    }
}
