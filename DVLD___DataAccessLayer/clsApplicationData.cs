using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD___DataAccessLayer
{
    public class clsApplicationData
    {
        public static int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
            byte ApplicationStatus, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            int ApplicationID = -1;
            string Query = @"INSERT INTO Applications VALUES(@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID, 
                @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID)
                              SELECT SCOPE_IDENTITY()";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                Command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                Command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                Command.Parameters.AddWithValue("@PaidFees", PaidFees);
                Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
        

                try
                {
                    Connection.Open();
                    object Result = Command.ExecuteScalar();

                    if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                    {
                        ApplicationID = InsertedID;
                    }

                }
                catch (Exception ex)
                {

                }


            }
            
            return ApplicationID;
        }

        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
            byte ApplicationStatus, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            int RowsAffected = 0;
            string Query = @"UPDATE Applications SET ApplicantPersonID = @ApplicantPersonID, ApplicationDate = @ApplicationDate,
            ApplicationTypeID = @ApplicationTypeID, ApplicationStatus = @ApplicationStatus, LastStatusDate = @LastStatusDate,
            PaidFees = @PaidFees, CreatedByUserID = @CreatedByUserID WHERE ApplicationID = @ApplicationID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                Command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                Command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                Command.Parameters.AddWithValue("@PaidFees", PaidFees);
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

            return (RowsAffected > 0);
        }


        public static bool GetApplicationInfoByApplicationID(int ApplicationID, ref int ApplicantPersonID, ref DateTime ApplicationDate,
            ref int ApplicationTypeID, ref byte ApplicationStatus, ref DateTime LastStatusDate, ref float PaidFees, ref int CreatedByUserID)
        {
            string Query = @"SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                try
                {
                    Connection.Open();
                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            ApplicantPersonID = (int)Reader["ApplicantPersonID"];
                            ApplicationDate = DateTime.Parse(Reader["ApplicationDate"].ToString());
                            ApplicationTypeID = (int)Reader["ApplicationTypeID"];
                            ApplicationStatus = (byte)Reader["ApplicationStatus"];
                            LastStatusDate = DateTime.Parse(Reader["LastStatusDate"].ToString());
                            PaidFees = float.Parse(Reader["PaidFees"].ToString());
                            CreatedByUserID = (int)Reader["CreatedByUserID"];

                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return false;
        }


        public static bool DeleteAppliation(int ApplicationID)
        {
            int RowsAffected = 0;
            string Query = @"DELETE FROM Applications WHERE ApplicationID = @ApplicationID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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

        public static bool UpdateStatus(int ApplicationID, byte ApplicationStatus)
        {
            int RowsAffected = 0;
            string Query = @"UPDATE Applications SET ApplicationStatus = @ApplicationStatus, LastStatusDate = @LastStatusDate 
                WHERE ApplicationID = @ApplicationID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                Command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);

                try
                {
                    Connection.Open();
                    RowsAffected = Command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
            }

            return (RowsAffected > 0);
        }

    }
}
