using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DVLD___DataAccessLayer
{
    public class clsTestTypeData
    {
        public static DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();

            string Query = "SELECT * FROM TestTypes";

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

        public static bool GetTestTypeInfoByID(int ID, ref string Title, ref string Description, ref float Fees)
        {
            bool IsFound = false;

            string Query = "SELECT * FROM TestTypes WHERE TestTypeID = @ID";

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
                            Title = (string)Reader["TestTypeTitle"];
                            Description = (string)Reader["TestTypeDescription"];
                            Fees = float.Parse(Reader["TestTypeFees"].ToString());

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

        public static bool UpdateTestType(int ID, string Title, string Description, float Fees)
        {
            int RowsAffected = 0;

            string Query = @"UPDATE TestTypes SET TestTypeTitle = @Title,
                            TestTypeDescription = @Description, TestTypeFees = @Fees
                            WHERE TestTypeID = @ID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@ID", ID);
                Command.Parameters.AddWithValue("@Title", Title);
                Command.Parameters.AddWithValue("@Description", Description);
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

            return (RowsAffected > 0);
        }
    }
}
