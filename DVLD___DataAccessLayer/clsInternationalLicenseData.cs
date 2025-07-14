using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD___DataAccessLayer
{
    public class clsInternationalLicenseData
    {
        public static bool GetInternationalLicenseInfoByID(int InternationalLicenseID, ref int ApplicationID,
              ref int DriverID, ref int IssuedUsingLocalLicenseID, ref int CreatedByUserID, ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive)
        {
            bool IsFound = false;

            string Query = @"SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

                try
                {
                    Connection.Open();

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            ApplicationID = (int)Reader["ApplicationID"];
                            DriverID = (int)Reader["DriverID"];
                            IssuedUsingLocalLicenseID = (int)Reader["IssuedUsingLocalLicenseID"];
                            IssueDate = (DateTime)Reader["IssueDate"];
                            ExpirationDate = (DateTime)Reader["ExpirationDate"];
                            IsActive = (bool)Reader["IsActive"];
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

        public static DataTable GetInternationalLicensesByDriverID(int DriverID)
        {
            DataTable dt = new DataTable();

            string Query = @"SELECT * FROM InternationalLicenses WHERE DriverID = @DriverID ORDER BY ExpirationDate DESC";

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

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            int ActiveInternationaLicenseID = -1;

            string Query = @" UPDATE InternationalLicenses SET IsActive = 0 WHERE DriverID = @DriverID AND 
            GETDATE() NOT BETWEEN IssueDate and ExpirationDate;
            SELECT InternationalLicenseID FROM InternationalLicenses WHERE IsActive = 1 AND DriverID = @DriverID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@DriverID", DriverID);

                try
                {
                    Connection.Open();
                    object result = Command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int SelectedID))
                    {
                        ActiveInternationaLicenseID = SelectedID;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return ActiveInternationaLicenseID;
        }

        public static int AddNewInternationalLicense(int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID,
                DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            int InternationalLicenseID = -1;

            string Query = @"UPDATE InternationalLicenses SET IsActive = 0 WHERE DriverID = @DriverID;
                INSERT INTO InternationalLicenses VALUES (@ApplicationID, @DriverID, @IssuedUsingLocalLicenseID, @IssueDate,
                @ExpirationDate, @IsActive, @CreatedByUserID) SELECT SCOPE_IDENTITY()";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                Command.Parameters.AddWithValue("@DriverID", DriverID);
                Command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
                Command.Parameters.AddWithValue("@IssueDate", IssueDate);
                Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                Command.Parameters.AddWithValue("@IsActive", IsActive);
                Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                try
                {
                    Connection.Open();
                    object result = Command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    {
                        InternationalLicenseID = InsertedID;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return InternationalLicenseID;
        }

        public static DataTable GetAllInternationalLicenses()
        {
            DataTable dt = new DataTable();

            string Query = @"SELECT InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, 
                ExpirationDate, IsActive FROM InternationalLicenses ORDER BY IsActive, ExpirationDate DESC";

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
