using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DVLD___DataAccessLayer
{
    public class clsApplicationTypeData
    {
        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();
            string Query = "SELECT * FROM ApplicationTypes";

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

        public static bool GetApplicationTypeInfoByID(int ID, ref string Title, ref float Fees)
        {
            bool IsFound = false;
            string Query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@ID", ID);

                try
                {
                    Connection.Open();
                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            Title = (string)Reader["ApplicationTypeTitle"];
                            Fees = float.Parse(Reader["ApplicationFees"].ToString());
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
            
        public static bool UpdateApplicationTypeInfo(int ID, string Title, float Fees)
        {
            int RowsAffected = 0;
            string Query = "UPDATE ApplicationTypes SET ApplicationTypeTitle = @Title, ApplicationFees = @Fees WHERE ApplicationTypeID = @ID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@ID", ID);
                Command.Parameters.AddWithValue("@Title", Title);
                Command.Parameters.AddWithValue("@Fees", Fees);

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

        public static int AddNewApplicationType(string Title, float Fees)
        {
            int ApplicationTypeID = -1;
            string Query = @"INSERT INTO ApplicationTypes VALUES(@Title, @Fees)
                                SELECT SCOPE_IDENTITY()";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@Title", Title);
                Command.Parameters.AddWithValue("@Fees", Fees);

                try
                {
                    Connection.Open();
                    object result = Command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    {
                        ApplicationTypeID = InsertedID;
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return ApplicationTypeID;
        }

    }
}
