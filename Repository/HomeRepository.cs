using EventManagement.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EventManagement.Repository
{
    public class HomeRepository
    {
        private SqlConnection connection;

        // Initialize SQL connection using the connection string from the configuration file
        private void Connection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["adoConnectionString"].ToString();
            connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Inserts a new user into the database.
        /// </summary>
        /// <param name="signup">Signup object containing user details</param>
        public void InsertUser(Signup signup)
        {
            Connection();
            SqlCommand command = new SqlCommand("SPI_InsertUser", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Add parameters for the stored procedure
            command.Parameters.AddWithValue("@FirstName", signup.FirstName);
            command.Parameters.AddWithValue("@LastName", signup.LastName);
            command.Parameters.AddWithValue("@DateOfBirth", signup.DateOfBirth);
            command.Parameters.AddWithValue("@Gender", signup.Gender);
            command.Parameters.AddWithValue("@PhoneNumber", signup.PhoneNumber);
            command.Parameters.AddWithValue("@EmailAddress", signup.EmailAddress);
            command.Parameters.AddWithValue("@Address", signup.Address);
            command.Parameters.AddWithValue("@State", signup.State);
            command.Parameters.AddWithValue("@City", signup.City);
            command.Parameters.AddWithValue("@UserName", signup.Username);
            command.Parameters.AddWithValue("@Password", signup.Password);
            command.Parameters.AddWithValue("@ConfirmPassword", signup.ConfirmPassword);

            try
            {
                connection.Open();
                command.ExecuteNonQuery(); // Execute the query to insert user data
            }
            //catch (Exception ex)
            //{
            //    // Handle exception and throw it to the caller for further handling
            //    throw new Exception("Error inserting user: " + ex.Message);
            //}
            finally
            {
                connection.Close(); // Ensure the connection is closed even if an exception occurs
            }
        }

        /// <summary>
        /// Inserts contact information into the database.
        /// </summary>
        /// <param name="contact">ContactUs object containing contact details</param>
        /// <returns>true value</returns>
        public bool ContactUs(ContactUs contactUs)
        {
            Connection();
            SqlCommand command = new SqlCommand("SPI_Contactus", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Add parameters for the stored procedure
            command.Parameters.AddWithValue("@Name", contactUs.Name);
            command.Parameters.AddWithValue("@EmailAddress", contactUs.Email);
            command.Parameters.AddWithValue("@Subject", contactUs.Subject);

            try
            {
                connection.Open();
                command.ExecuteNonQuery(); // Execute the query to insert contact data
            }
            //catch (Exception ex)
            //{
            //    // Handle exception and throw it to the caller for further handling
            //    throw new Exception("Error inserting contact information: " + ex.Message);
            //}
            finally
            {
                connection.Close(); // Ensure the connection is closed even if an exception occurs
            }

            return true;
        }
    }
}
