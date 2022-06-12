using Microsoft.Extensions.Configuration;
using SignUpAUser.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SignUpAUser.Services
{
    public class UsersDAO : IUserDAO
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public UsersDAO(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        
        public bool RegisterUser(UserModel user)
        {
            bool success = false;
            var sqlStatement = @"INSERT INTO dbo.[User](FirstName, LastName, EmailAddress, Phone, [Address], [State], ActivateToken)
VALUES(@firstname, @lastName, @emailAddress, @phone, @address, @state, @activateToken)";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@firstname", System.Data.SqlDbType.VarChar, 40).Value = user.FirstName;
                command.Parameters.Add("@lastName", System.Data.SqlDbType.VarChar, 40).Value = user.LastName;
                command.Parameters.Add("@emailAddress", System.Data.SqlDbType.VarChar, 40).Value = user.EmailAddress;
                command.Parameters.Add("@phone", System.Data.SqlDbType.VarChar, 40).Value = user.Phone;
                command.Parameters.Add("@address", System.Data.SqlDbType.VarChar, 40).Value = user.Address;
                command.Parameters.Add("@state", System.Data.SqlDbType.VarChar, 40).Value = user.State;
                command.Parameters.Add("@activateToken", System.Data.SqlDbType.VarChar, 40).Value = user.ActivateToken.ToString();
                try
                {
                    connection.Open();
                    int reader = command.ExecuteNonQuery();
                    return true;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            };
            return success;
        }

        public int VerifiyUser(Guid token, string email)
        {
            var sqlStatement = @"SELECT Id FROM dbo.[User] WHERE EmailAddress = @email AND ActivateToken = @activateToken";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@activateToken", System.Data.SqlDbType.VarChar, 40).Value = token.ToString();
                command.Parameters.Add("@email", System.Data.SqlDbType.VarChar, 40).Value = email;
                try
                {
                    connection.Open();
                    int result = Convert.ToInt32(command.ExecuteScalar());
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return 0;
        }

        public bool UpdateStateUser(int userId)
        {
            var sqlStatement = @"UPDATE dbo.[User] 
                                SET State = 'Activated'
                                WHERE Id = @userId";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@userId", System.Data.SqlDbType.Int).Value = userId;
                try
                {
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return false;
        }

        public bool CheckIfExist(string email)
        {
            var sqlStatement = @"SELECT 1 FROM dbo.[User] WHERE EmailAddress = @email";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@email", System.Data.SqlDbType.VarChar, 40).Value = email;
                try
                {
                    connection.Open();
                    int result = Convert.ToInt32(command.ExecuteScalar());
                    if(result == 1)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return false;
        }

        public List<UserModel> GetListUser()
        {
            var users = new List<UserModel>();
            var sqlStatement = @"SELECT * FROM dbo.[User]";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var user = new UserModel()
                            {
                                Id = int.Parse(reader.GetValue(0).ToString()),
                                FirstName = reader.GetValue(1).ToString(),
                                LastName = reader.GetValue(2).ToString(),
                                EmailAddress = reader.GetValue(3).ToString(),
                                Phone = reader.GetValue(4).ToString(),
                                Address = reader.GetValue(5).ToString(),
                                State = reader.GetValue(6).ToString()
                            };
                            users.Add(user);
                        }
                        reader.Close();
                        return users;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return users;
        }
    }
}
