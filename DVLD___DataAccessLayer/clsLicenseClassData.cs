using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___DataAccessLayer
{
    public class clsLicenseClassData
    {

        public static DataTable GetAllLicenseClasses()
        {
            DataTable dt = new DataTable();

            string Query = "SELECT * FROM LicenseClasses";

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

        public static bool GetLicenseClassInfoByClassName(string ClassName, ref int LicenseClassID,
           ref string ClassDescription, ref byte MinimumAllowedAge, ref byte DefaultValidityLength, ref float ClassFees)
        {
            string Query = "SELECT * FROM LicenseClasses WHERE ClassName = @ClassName";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@ClassName", ClassName);

                try
                {
                    Connection.Open();

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            LicenseClassID = (int)Reader["LicenseClassID"];
                            ClassDescription = (string)Reader["ClassDescription"];
                            MinimumAllowedAge = (byte)Reader["MinimumAllowedAge"];
                            DefaultValidityLength = (byte)Reader["DefaultValidityLength"];
                            ClassFees = float.Parse(Reader["ClassFees"].ToString());

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

        public static bool GetLicenseClassInfoByLicenseClassID(int LicenseClassID, ref string ClassName,
           ref string ClassDescription, ref byte MinimumAllowedAge, ref byte DefaultValidityLength, ref float ClassFees)
        {
            string Query = "SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                try
                {
                    Connection.Open();

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            ClassName = (string)Reader["ClassName"];
                            ClassDescription = (string)Reader["ClassDescription"];
                            MinimumAllowedAge = (byte)Reader["MinimumAllowedAge"];
                            DefaultValidityLength = (byte)Reader["DefaultValidityLength"];
                            ClassFees = float.Parse(Reader["ClassFees"].ToString());

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

    }
}
