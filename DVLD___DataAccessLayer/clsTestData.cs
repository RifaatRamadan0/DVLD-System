using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___DataAccessLayer
{
    public class clsTestData
    {
        public static int GetPassedTestsCount(int LocalLicenseApplicationID)
        {
            int PassedTestsCount = 0;

            string Query = @"SELECT COUNT(1) AS PassedTests FROM Tests T
                INNER JOIN TestAppointments TA ON T.TestAppointmentID = TA.TestAppointmentID
                    WHERE T.TestResult = 1 AND TA.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalLicenseApplicationID);

                try
                {
                    Connection.Open();
                    object result = Command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int SelectedPassedTests))
                    {
                        PassedTestsCount = SelectedPassedTests;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return PassedTestsCount;
        }

        public static bool GetLastTestInfoBy(int LocalLicenseApplicationID, int TestTypeID, ref int TestID, ref int TestAppointmentID,
            ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            bool IsFound = false;

            string Query = @"SELECT TOP 1 * FROM Tests INNER JOIN TestAppointments ON Tests.TestAppointmentID
            = TestAppointments.TestAppointmentID WHERE LocalDrivingLicenseApplicationID = @LocalLicenseApplicationID AND 
            TestTypeID = @TestTypeID ORDER BY Tests.TestAppointmentID DESC";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                try
                {
                    Connection.Open();

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            TestID = (int)Reader["TestID"];
                            TestAppointmentID = (int)Reader["TestAppointmentID"];
                            TestResult = (bool)Reader["TestResult"];
                            Notes = Reader["Notes"] == DBNull.Value ? "" : (string)Reader["Notes"];
                            CreatedByUserID = (int)Reader["CreatedByUserID"];

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

        public static bool GetTestInfoByTestID(int TestID, ref int TestAppointmentID, ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            bool IsFound = false;

            string Query = @"SELECT * FROM Tests WHERE TestID = @TestID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@TestID", TestID);

                try
                {
                    Connection.Open();

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            TestAppointmentID = (int)Reader["TestAppointmentID"];
                            TestResult = (bool)Reader["TestResult"];
                            Notes = Reader["Notes"] == DBNull.Value ? "" : (string)Reader["Notes"];
                            CreatedByUserID = (int)Reader["CreatedByUserID"];

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

        public static int AddNewTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int TestID = -1;

            string Query = @"INSERT INTO Tests VALUES(@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID)
                        UPDATE TestAppointments SET IsLocked = 1 WHERE TestAppointmentID = @TestAppointmentID
                                SELECT SCOPE_IDENTITY()";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                Command.Parameters.AddWithValue("@TestResult", TestResult);

                if (Notes == "")
                    Command.Parameters.AddWithValue("@Notes", DBNull.Value);
                else
                    Command.Parameters.AddWithValue("@Notes", Notes);

                Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                try
                {
                    Connection.Open();
                    object result = Command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    {
                        TestID = InsertedID;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return TestID;
        }

        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int RowsAffected = 0;

            string Query = @"UPDATE Tests SET TestAppointmentID = @TestAppointmentID, TestResult = @TestResult,
                Notes = @Notes, CreatedByUserID = @CreatedByUserID WHERE TestID = @TestID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@TestID", TestID);
                Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                Command.Parameters.AddWithValue("@TestResult", TestResult);

                if (Notes == "")
                    Command.Parameters.AddWithValue("@Notes", DBNull.Value);
                else
                    Command.Parameters.AddWithValue("@Notes", Notes);

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
    }
}
