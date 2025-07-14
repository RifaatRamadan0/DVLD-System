using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Security.Policy;

namespace DVLD___DataAccessLayer
{
    public class clsPersonData
    {
        public static DataTable GetAllPeopleData()
        {
            DataTable dt = new DataTable();

            string Query = "SELECT PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, " +
                "CASE WHEN Gendor = 0 THEN 'Male' WHEN Gendor = 1 THEN 'Female' END AS Gendor, Address, Phone, Email," +
                " Countries.CountryName As Nationality, ImagePath" +
                " FROM People JOIN Countries ON Countries.CountryID = People.NationalityCountryID;";

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

        public static bool GetPersonByID(int PersonID, ref string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName,
           ref string LastName, ref DateTime DateOfBirth, ref short Gender, ref string Address, ref string Phone,
           ref string Email, ref int CountryID, ref string ImagePath)
        {
            bool IsFound = false;

            string Query = "SELECT * FROM People WHERE PersonID = @PersonID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@PersonID", PersonID);

                try
                {
                    Connection.Open();

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            FirstName = Reader["FirstName"].ToString();
                            NationalNo = Reader["NationalNo"].ToString();
                            SecondName = Reader["SecondName"].ToString();
                            ThirdName = Reader["ThirdName"] == DBNull.Value ? "" : Reader["ThirdName"].ToString();
                            LastName = Reader["LastName"].ToString();
                            DateOfBirth = DateTime.Parse(Reader["DateOfBirth"].ToString());
                            Gender = (byte)Reader["Gendor"];
                            Address = Reader["Address"].ToString();
                            Phone = Reader["Phone"].ToString();
                            Email = Reader["Email"] == DBNull.Value ? "" : Reader["Email"].ToString();
                            CountryID = (int)Reader["NationalityCountryID"];
                            ImagePath = Reader["ImagePath"] == DBNull.Value ? "" : Reader["ImagePath"].ToString();

                            IsFound = true;
                        }
                        else
                        {
                            IsFound = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    IsFound = false;
                }
            }

            return IsFound;
        }

        public static bool GetPersonByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName, ref string ThirdName,
           ref string LastName, ref DateTime DateOfBirth, ref short Gender, ref string Address, ref string Phone,
           ref string Email, ref int CountryID, ref string ImagePath)
        {
            bool IsFound = false;

            string Query = "SELECT * FROM People WHERE NationalNo = @NationalNo";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@NationalNo", NationalNo);

                try
                {
                    Connection.Open();

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            PersonID = int.Parse(Reader["PersonID"].ToString());
                            FirstName = Reader["FirstName"].ToString();
                            SecondName = Reader["SecondName"].ToString();
                            ThirdName = Reader["ThirdName"].ToString();
                            LastName = Reader["LastName"].ToString();
                            DateOfBirth = DateTime.Parse(Reader["DateOfBirth"].ToString());
                            Gender = byte.Parse(Reader["Gendor"].ToString());
                            Address = Reader["Address"].ToString();
                            Phone = Reader["Phone"].ToString();
                            Email = Reader["Email"].ToString();
                            CountryID = int.Parse(Reader["NationalityCountryID"].ToString());
                            ImagePath = Reader["ImagePath"].ToString();

                            IsFound = true;
                        }
                        else
                        {
                            IsFound = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    IsFound = false;
                }
            }

            return IsFound;
        }

        public static int AddNewPerson(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth,
            short Gender, string Address, string Phone, string Email, int CountryID, string ImagePath)
        {
            int PersonID = -1;

            string Query = "INSERT INTO People VALUES (@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth, " +
                "@Gender, @Address, @Phone, @Email, @CountryID, @ImagePath) " +
                "SELECT SCOPE_IDENTITY()";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@NationalNo", NationalNo);
                Command.Parameters.AddWithValue("@FirstName", FirstName);
                Command.Parameters.AddWithValue("@SecondName", SecondName);

                if (string.IsNullOrEmpty(ThirdName))
                    Command.Parameters.AddWithValue("@ThirdName", DBNull.Value);
                else
                    Command.Parameters.AddWithValue("@ThirdName", ThirdName);

                Command.Parameters.AddWithValue("@LastName", LastName);
                Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                Command.Parameters.AddWithValue("@Gender", Gender);
                Command.Parameters.AddWithValue("@Address", Address);
                Command.Parameters.AddWithValue("@Phone", Phone);

                if (string.IsNullOrEmpty(Email))
                    Command.Parameters.AddWithValue("@Email", DBNull.Value);
                else
                    Command.Parameters.AddWithValue("@Email", Email);

                Command.Parameters.AddWithValue("@CountryID", CountryID);

                if (string.IsNullOrEmpty(ImagePath))
                    Command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
                else
                    Command.Parameters.AddWithValue("@ImagePath", ImagePath);

                try
                {
                    Connection.Open();
                    object result = Command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    {
                        PersonID = InsertedID;
                    }
                }
                catch (Exception e)
                {
                }
            }

            return PersonID;
        }

        public static bool UpdatePerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName,
            DateTime DateOfBirth, short Gender, string Address, string Phone, string Email, int CountryID, string ImagePath)
        {
            int RowsAffected = 0;

            string Query = "UPDATE People SET FirstName = @FirstName, SecondName = @SecondName, " +
                "ThirdName = @ThirdName, LastName = @LastName, DateOfBirth = @DateOfBirth, Gendor = @Gender, " +
                "Address = @Address, Phone = @Phone, Email = @Email, NationalityCountryID = @CountryID, ImagePath = @ImagePath " +
                "WHERE PersonID = @PersonID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@PersonID", PersonID);
                Command.Parameters.AddWithValue("@NationalNo", NationalNo);
                Command.Parameters.AddWithValue("@FirstName", FirstName);
                Command.Parameters.AddWithValue("@SecondName", SecondName);

                if (string.IsNullOrEmpty(ThirdName))
                    Command.Parameters.AddWithValue("@ThirdName", DBNull.Value);
                else
                    Command.Parameters.AddWithValue("@ThirdName", ThirdName);

                Command.Parameters.AddWithValue("@LastName", LastName);
                Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                Command.Parameters.AddWithValue("@Gender", Gender);
                Command.Parameters.AddWithValue("@Address", Address);
                Command.Parameters.AddWithValue("@Phone", Phone);

                if (string.IsNullOrEmpty(Email))
                    Command.Parameters.AddWithValue("@Email", DBNull.Value);
                else
                    Command.Parameters.AddWithValue("@Email", Email);

                Command.Parameters.AddWithValue("@CountryID", CountryID);

                if (string.IsNullOrEmpty(ImagePath))
                    Command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
                else
                    Command.Parameters.AddWithValue("@ImagePath", ImagePath);

                try
                {
                    Connection.Open();
                    RowsAffected = Command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return RowsAffected > 0;
        }

        public static bool DeletePerson(int PersonID)
        {
            int RowsAffected = 0;

            string Query = "DELETE FROM People WHERE PersonID = @PersonID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static bool IsPersonExist(string NationalNo)
        {
            bool IsExist = false;

            string Query = "SELECT Found = 1 FROM People WHERE NationalNo = @NationalNo";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@NationalNo", NationalNo);

                try
                {
                    Connection.Open();

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        IsExist = Reader.HasRows;
                    }
                }
                catch (Exception e)
                {
                    IsExist = false;
                }
            }

            return IsExist;
        }

        public static bool IsPersonExist(int PersonID)
        {
            bool IsExist = false;

            string Query = "SELECT Found = 1 FROM People WHERE PersonID = @PersonID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@PersonID", PersonID);

                try
                {
                    Connection.Open();

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        IsExist = Reader.HasRows;
                    }
                }
                catch (Exception e)
                {
                    IsExist = false;
                }
            }

            return IsExist;
        }
    }
}
