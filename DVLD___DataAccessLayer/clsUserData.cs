using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace DVLD___DataAccessLayer
{
    public class clsUserData
    {
        public static DataTable GetAllUsers()
        {
            DataTable Users = new DataTable();
            string Query = @"SELECT UserID, U.PersonID, (P.FirstName + ' '+ P.SecondName + ' ' + ISNULL(P.ThirdName + ' ', '') + P.LastName) AS FullName,
                            UserName, IsActive FROM Users U INNER JOIN People P ON U.PersonID = P.PersonID";

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
                            Users.Load(Reader);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return Users;
        }

        public static bool GetUserInfoByUsernameAndPassword(string UserName, string Password, ref int UserID,
            ref int PersonID, ref bool IsActive)
        {
            string Query = "SELECT UserID, PersonID, IsActive FROM Users WHERE UserName = @Username AND Password = @Password";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@Username", UserName);
                Command.Parameters.AddWithValue("@Password", Password);

                try
                {
                    Connection.Open();
                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            UserID = (int)Reader["UserID"];
                            PersonID = (int)Reader["PersonID"];
                            IsActive = (bool)Reader["IsActive"];
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }

            return false;
        }

        public static bool GetUserInfoByUserID(int UserID, ref int PersonID, ref string UserName,
            ref string Password, ref bool IsActive)
        {
            string Query = "SELECT * FROM Users WHERE UserID = @UserID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@UserID", UserID);

                try
                {
                    Connection.Open();
                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            PersonID = (int)Reader["PersonID"];
                            UserName = (string)Reader["UserName"];
                            Password = (string)Reader["Password"];
                            IsActive = (bool)Reader["IsActive"];

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

        public static bool GetUserInfoByPersonID(int PersonID, ref int UserID, ref string UserName, ref string Password, ref bool IsActive)
        {
            string Query = "SELECT * FROM Users WHERE PersonID = @PersonID";

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
                            UserID = (int)Reader["UserID"];
                            UserName = (string)Reader["UserName"];
                            Password = (string)Reader["Password"];
                            IsActive = (bool)Reader["IsActive"];

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

        public static bool IsUserExistForPersonID(int PersonID)
        {
            string Query = "SELECT Found = 1 FROM Users WHERE PersonID = @PersonID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@PersonID", PersonID);

                try
                {
                    Connection.Open();
                    object result = Command.ExecuteScalar();

                    if (result != null)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return false;
        }

        public static bool IsUserExist(int UserID)
        {
            string Query = "SELECT Found = 1 FROM Users WHERE UserID = @UserID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@UserID", UserID);

                try
                {
                    Connection.Open();
                    object Result = Command.ExecuteScalar();

                    if (Result != null)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return false;
        }

        public static bool IsUserExist(string UserName)
        {
            string Query = "SELECT Found = 1 FROM Users WHERE UserName = @UserName";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@UserName", UserName);

                try
                {
                    Connection.Open();
                    object Result = Command.ExecuteScalar();

                    if (Result != null)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return false;
        }

        public static int AddNewUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            int UserID = -1;
            string Query = @"INSERT INTO Users VALUES (@PersonID, @UserName, @Password, @IsActive)
                                    SELECT SCOPE_IDENTITY()";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@PersonID", PersonID);
                Command.Parameters.AddWithValue("@UserName", UserName);
                Command.Parameters.AddWithValue("@Password", Password);
                Command.Parameters.AddWithValue("@IsActive", IsActive);

                try
                {
                    Connection.Open();
                    object res = Command.ExecuteScalar();

                    if (res != null && int.TryParse(res.ToString(), out int InsertedID))
                    {
                        UserID = InsertedID;
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return UserID;
        }

        public static bool UpdateUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            int RowsAffected = 0;
            string Query = @"UPDATE Users SET PersonID = @PersonID, UserName = @UserName, Password = @Password,
                                IsActive = @IsActive WHERE UserID = @UserID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@UserID", UserID);
                Command.Parameters.AddWithValue("@PersonID", PersonID);
                Command.Parameters.AddWithValue("@UserName", UserName);
                Command.Parameters.AddWithValue("@Password", Password);
                Command.Parameters.AddWithValue("@IsActive", IsActive);

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

        public static bool DeleteUser(int UserID)
        {
            int RowsAffected = 0;
            string Query = @"DELETE FROM Users WHERE UserID = @UserID";

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Command.Parameters.AddWithValue("@UserID", UserID);

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
