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
    public class clsLocalLicenseApplicationData
    {
        public static int AddNewLocalLicenseApplication(int ApplicationID, int LicenseClassID)
        {
            int LocalLicenseApplicationID = -1;

            string Query = @"INSERT INTO LocalDrivingLicenseApplications VALUES(@ApplicationID, @LicenseClassID)
                              SELECT SCOPE_IDENTITY()";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                try
                {
                    Connection.Open();
                    object Result = Command.ExecuteScalar();

                    if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                    {
                        LocalLicenseApplicationID = InsertedID;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return LocalLicenseApplicationID;
        }

        public static bool UpdateLocalLicenseApplication(int LocalLicenseApplicationID, int ApplicationID, int LicenseClassID)
        {
            int RowsAffected = 0;

            string Query = @"UPDATE LocalDrivingLicenseApplications SET ApplicationID = @ApplicationID, 
            LicenseClassID = @LicenseClassID WHERE LocalDrivingLicenseApplicationID = @LocalLicenseApplicationID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

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

        public static bool GetLocalLicenseApplicationInfoByLocalLicenseID(int LocalLicenseApplicationID,
            ref int ApplicationID, ref int LicenseClassID)
        {
            string Query = @"SELECT * FROM LocalDrivingLicenseApplications L WHERE 
                    L.LocalDrivingLicenseApplicationID = @LocalLicenseApplicationID;";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);

                try
                {
                    Connection.Open();

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            ApplicationID = (int)Reader["ApplicationID"];
                            LicenseClassID = (int)Reader["LicenseClassID"];

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

        public static int GetActiveApplicationIDForLicenseClass(int ApplicantPersonID, int ApplicationType, int LicenseClassID)
        {
            int ActiveApplicationID = -1;

            string Query = @"SELECT ActiveApplicationID = A.ApplicationID FROM LocalDrivingLicenseApplications L INNER JOIN Applications A ON 
            A.ApplicationID = L.ApplicationID WHERE A.ApplicationStatus = 1 AND A.ApplicantPersonID = @PersonID AND
            A.ApplicationTypeID = @ApplicationType AND L.LicenseClassID = @ClassID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@PersonID", ApplicantPersonID);
                Command.Parameters.AddWithValue("@ClassID", LicenseClassID);
                Command.Parameters.AddWithValue("@ApplicationType", ApplicationType);

                try
                {
                    Connection.Open();
                    object result = Command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int SelectedApplicationID))
                    {
                        ActiveApplicationID = SelectedApplicationID;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return ActiveApplicationID;
        }

        public static DataTable GetAllLocalLicenseApplications()
        {
            DataTable dt = new DataTable();

            string Query = @"SELECT * FROM LocalDrivingLicenseApplications_View ORDER BY LocalDrivingLicenseApplicationID DESC";

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

        public static bool DoesPassTestType(int LocalLicenseApplicationID, int TestTypeID)
        {
            bool TestResult = false;

            string Query = @"SELECT TestResult FROM Tests WHERE TestAppointmentID = (SELECT TOP 1 TestAppointmentID FROM TestAppointments
                        WHERE LocalDrivingLicenseApplicationID = @LocalLicenseApplicationID AND TestTypeID = @TestTypeID 
                            ORDER BY TestAppointmentID DESC)";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                try
                {
                    Connection.Open();
                    object Result = Command.ExecuteScalar();

                    if (Result != null && bool.TryParse(Result.ToString(), out bool SelectedResult))
                    {
                        TestResult = SelectedResult;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return TestResult;
        }

        public static bool HasActiveTestAppointment(int LocalLicenseApplicationID, int TestTypeID)
        {
            bool IsFound = false;

            string Query = @"SELECT TOP 1 Found = 1 FROM TestAppointments WHERE LocalDrivingLicenseApplicationID = 
                @LocalLicenseApplicationID AND TestTypeID = @TestTypeID AND IsLocked = 0 ORDER BY TestAppointmentID DESC";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                try
                {
                    Connection.Open();
                    object Result = Command.ExecuteScalar();

                    if (Result != null)
                    {
                        IsFound = true;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return IsFound;
        }

        public static int GetNumberOfTrials(int LocalLicenseApplicationID, int TestTypeID)
        {
            int NumberOfTrials = 0;

            string Query = @"SELECT COUNT(TestID) AS NumberOfTrials FROM Tests INNER JOIN TestAppointments ON TestAppointments.TestAppointmentID
            = Tests.TestAppointmentID WHERE TestTypeID = @TestTypeID AND LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalLicenseApplicationID);
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                try
                {
                    Connection.Open();
                    object result = Command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int SelectedTrials))
                    {
                        NumberOfTrials = SelectedTrials;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return NumberOfTrials;
        }

        public static bool DeleteLocalLicenseApplication(int LocalLicenseApplicationID)
        {
            int RowsAffected = 0;

            string Query = @"DELETE FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LocalLicenseApplicationID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);

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
    }
}
