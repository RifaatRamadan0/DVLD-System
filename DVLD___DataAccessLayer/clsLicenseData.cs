using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD___DataAccessLayer
{
    public class clsLicenseData
    {
        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int ActiveLicenseID = -1;

            string Query = @"SELECT LicenseID FROM Licenses L INNER JOIN Drivers D ON L.DriverID = D.DriverID
                       WHERE D.PersonID = @PersonID AND L.LicenseClass = @LicenseClassID AND L.IsActive = 1";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@PersonID", PersonID);
                Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                try
                {
                    Connection.Open();
                    object result = Command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int SelectedLicenseID))
                    {
                        ActiveLicenseID = SelectedLicenseID;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return ActiveLicenseID;
        }

        public static bool GetLicenseInfoByLicenseID(int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClassID,
            ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes, ref float PaidFees, ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
        {
            bool IsFound = false;

            string Query = @"SELECT * FROM Licenses WHERE LicenseID = @LicenseID";

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
                            ApplicationID = (int)Reader["ApplicationID"];
                            DriverID = (int)Reader["DriverID"];
                            LicenseClassID = (int)Reader["LicenseClass"];
                            IssueDate = (DateTime)Reader["IssueDate"];
                            ExpirationDate = (DateTime)Reader["ExpirationDate"];
                            Notes = Reader["Notes"] == DBNull.Value ? "" : (string)Reader["Notes"];
                            PaidFees = Convert.ToSingle(Reader["PaidFees"]);
                            IsActive = (bool)Reader["IsActive"];
                            IssueReason = (byte)Reader["IssueReason"];
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

        public static int AddNewLicense(int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate,
                DateTime ExpirationDate, string Notes, float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int LicenseID = -1;

            string Query = @"INSERT INTO Licenses VALUES (@ApplicationID, @DriverID, @LicenseClassID, @IssueDate, 
                @ExpirationDate, @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID) SELECT SCOPE_IDENTITY()";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                Command.Parameters.AddWithValue("@DriverID", DriverID);
                Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                Command.Parameters.AddWithValue("@IssueDate", IssueDate);
                Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

                if (Notes == "")
                    Command.Parameters.AddWithValue("@Notes", DBNull.Value);
                else
                    Command.Parameters.AddWithValue("@Notes", Notes);

                Command.Parameters.AddWithValue("@PaidFees", PaidFees);
                Command.Parameters.AddWithValue("@IsActive", IsActive);
                Command.Parameters.AddWithValue("@IssueReason", IssueReason);
                Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                try
                {
                    Connection.Open();
                    object result = Command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    {
                        LicenseID = InsertedID;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return LicenseID;
        }

        public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate,
                DateTime ExpirationDate, string Notes, float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int RowsAffected = 0;

            string Query = @"UPDATE Licenses SET ApplicationID = @ApplicationID, DriverID = @DriverID, LicenseClassID = @LicenseClassID, 
                IssueDate = @IssueDate, ExpirationDate = @ExpirationDate, Notes = @Notes, PaidFees = @PaidFees, IsActive = @IsActive, 
                IssueReason = @IssueReason, CreatedByUserID = @CreatedByUserID WHERE LicenseID = @LicenseID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@LicenseID", LicenseID);
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                Command.Parameters.AddWithValue("@DriverID", DriverID);
                Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                Command.Parameters.AddWithValue("@IssueDate", IssueDate);
                Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

                if (Notes == "")
                    Command.Parameters.AddWithValue("@Notes", DBNull.Value);
                else
                    Command.Parameters.AddWithValue("@Notes", Notes);

                Command.Parameters.AddWithValue("@PaidFees", PaidFees);
                Command.Parameters.AddWithValue("@IsActive", IsActive);
                Command.Parameters.AddWithValue("@IssueReason", IssueReason);
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

        public static DataTable GetLocalLicensesByDriverID(int DriverID)
        {
            DataTable dt = new DataTable();

            string Query = @"SELECT * FROM Licenses INNER JOIN LicenseClasses ON Licenses.LicenseClass
                    = LicenseClasses.LicenseClassID WHERE DriverID = @DriverID ORDER BY IsActive DESC, ExpirationDate DESC";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@DriverID", DriverID);

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

        public static bool DeactivateOldLicense(int LicenseID)
        {
            int RowsAffected = 0;

            string Query = @"UPDATE Licenses SET IsActive = 0 WHERE LicenseID = @LicenseID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@LicenseID", LicenseID);

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
