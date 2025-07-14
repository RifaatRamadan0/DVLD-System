using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace DVLD___DataAccessLayer
{
    public class clsTestAppointmentData
    {
        public static bool GetTestAppointmentInfoByID(int TestAppointmentID, ref int TestTypeID, ref int LocalLicenseApplicationID,
                ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool IsFound = false;

            string Query = @"SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                try
                {
                    Connection.Open();

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            TestTypeID = (int)Reader["TestTypeID"];
                            LocalLicenseApplicationID = (int)Reader["LocalDrivingLicenseApplicationID"];
                            AppointmentDate = (DateTime)Reader["AppointmentDate"];
                            PaidFees = float.Parse(Reader["PaidFees"].ToString());
                            CreatedByUserID = (int)Reader["CreatedByUserID"];
                            IsLocked = (bool)Reader["IsLocked"];
                            RetakeTestApplicationID = (Reader["RetakeTestApplicationID"] == DBNull.Value) ? -1 : (int)Reader["RetakeTestApplicationID"];
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

        public static DataTable GetAllTestAppointmentsOf(int LocalLicenseApplicationID, int TestTypeID)
        {
            DataTable dt = new DataTable();

            string Query = @"SELECT TestAppointmentID, AppointmentDate, PaidFees, IsLocked FROM TestAppointments
                WHERE TestTypeID = @TestTypeID AND LocalDrivingLicenseApplicationID = @LocalLicenseApplicationID
                    ORDER BY TestAppointmentID DESC";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                Command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);

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

        public static int AddNewTestAppointment(int TestTypeID, int LocalLicenseApplicationID,
                DateTime AppointmentDate, float PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            int TestAppointmentID = -1;

            string Query = @"INSERT INTO TestAppointments VALUES(@TestTypeID, @LocalLicenseApplicationID, @AppointmentDate, 
                            @PaidFees, @CreatedByUserID, @IsLocked, @RetakeTestApplicationID)
                                SELECT SCOPE_IDENTITY()";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                Command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);
                Command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                Command.Parameters.AddWithValue("@PaidFees", PaidFees);
                Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                Command.Parameters.AddWithValue("@IsLocked", IsLocked);

                if (RetakeTestApplicationID != -1)
                    Command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
                else
                    Command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);

                try
                {
                    Connection.Open();
                    object result = Command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    {
                        TestAppointmentID = InsertedID;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return TestAppointmentID;
        }

        public static bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID, int LocalLicenseApplicationID,
                DateTime AppointmentDate, float PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            int RowsAffected = 0;

            string Query = @"UPDATE TestAppointments SET TestTypeID = @TestTypeID,
                                LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,
                                AppointmentDate = @AppointmentDate,
                                PaidFees = @PaidFees,
                                CreatedByUserID = @CreatedByUserID,
                                IsLocked=@IsLocked,
                                RetakeTestApplicationID=@RetakeTestApplicationID
                        WHERE TestAppointmentID = @TestAppointmentID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                Command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalLicenseApplicationID);
                Command.Parameters.AddWithValue("@PaidFees", PaidFees);
                Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                Command.Parameters.AddWithValue("@IsLocked", IsLocked);

                if (RetakeTestApplicationID == -1)
                    Command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                else
                    Command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

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

        public static int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;

            string Query = @"SELECT TestID FROM Tests WHERE TestAppointmentID = @TestAppointmentID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                try
                {
                    Connection.Open();
                    object Result = Command.ExecuteScalar();

                    if (Result != null && int.TryParse(Result.ToString(), out int SelectedID))
                    {
                        TestID = SelectedID;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return TestID;
        }
    }
}
