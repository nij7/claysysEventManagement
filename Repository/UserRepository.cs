using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EventManagement.Repository
{
    public class UserRepository
    {
        private SqlConnection connection;

        /// <summary>
        /// Initializes the SQL connection using the connection string from the configuration file.
        /// </summary>
        private void Connection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["adoConnectionString"].ToString();
            connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Retrieves all employee responses from the database.
        /// </summary>
        /// <returns>List of EmployeeResponse objects.</returns>
        public IEnumerable<EmployeeResponse> GetAllResponses()
        {
            Connection();
            List<EmployeeResponse> responses = new List<EmployeeResponse>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("SPS_GetAllEmployeeResponses", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        EmployeeResponse response = new EmployeeResponse
                        {
                            ResponseID = Convert.ToInt32(reader["ResponseID"]),
                            EventName = reader["EventName"].ToString(),
                            Username = reader["Username"].ToString(),
                            ResponseDate = Convert.ToDateTime(reader["ResponseDate"]),
                            Status = reader["Status"].ToString()
                        };
                        responses.Add(response);
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return responses;
        }

        /// <summary>
        /// Retrieves a specific employee response for an event by event ID and user ID.
        /// </summary>
        /// <param name="eventId">The event ID.</param>
        /// <param name="userId">The user ID.</param>
        /// <returns>An EmployeeResponse object if found, otherwise null.</returns>
        public EmployeeResponse GetResponseForEvent(int eventId, int userId)
        {
            Connection();
            EmployeeResponse response = null;

            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SPS_GetEmployeeResponse", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EventID", eventId);
                    command.Parameters.AddWithValue("@UserID", userId);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        response = new EmployeeResponse
                        {
                            ResponseID = Convert.ToInt32(reader["ResponseID"]),
                            EventName = reader["EventName"].ToString(),
                            Username = reader["Username"].ToString(),
                            Status = reader["Status"].ToString()
                        };
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return response;
        }

        /// <summary>
        /// Adds a new employee response to the database.
        /// </summary>
        /// <param name="response">The EmployeeResponse object to add.</param>
        public void AddResponse(EmployeeResponse response)
        {
            Connection();

            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SPI_AddEmployeeResponse", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EventID", response.EventID);
                    command.Parameters.AddWithValue("@UserID", response.UserID);
                    command.Parameters.AddWithValue("@Status", response.Status);
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Retrieves all employee responses for a specific user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>List of EmployeeResponse objects.</returns>
        public IEnumerable<EmployeeResponse> GetResponsesByUser(int userId)
        {
            Connection();
            List<EmployeeResponse> responses = new List<EmployeeResponse>();

            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SPS_GetEmployeeResponsesByUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userId);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        responses.Add(new EmployeeResponse
                        {
                            ResponseID = Convert.ToInt32(reader["ResponseID"]),
                            EventID = Convert.ToInt32(reader["EventID"]),
                            EventName = reader["EventName"].ToString(),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Status = reader["Status"].ToString()
                        });
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return responses;
        }

        /// <summary>
        /// Approves an employee response by updating its status to "Approved".
        /// </summary>
        /// <param name="responseId">The response ID to approve.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public bool ApproveResponse(int responseId)
        {
            Connection();
            bool isSuccess = false;

            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("UPDATE EmployeeResponses SET Status = 'Approved' WHERE ResponseID = @ResponseID", connection))
                {
                    command.Parameters.AddWithValue("@ResponseID", responseId);
                    int rowsAffected = command.ExecuteNonQuery();
                    isSuccess = rowsAffected > 0;
                }
            }
            finally
            {
                connection.Close();
            }

            return isSuccess;
        }

        /// <summary>
        /// Rejects an employee response by updating its status to "Rejected".
        /// </summary>
        /// <param name="responseId">The response ID to reject.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public bool RejectResponse(int responseId)
        {
            Connection();
            bool isSuccess = false;

            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("UPDATE EmployeeResponses SET Status = 'Rejected' WHERE ResponseID = @ResponseID", connection))
                {
                    command.Parameters.AddWithValue("@ResponseID", responseId);
                    int rowsAffected = command.ExecuteNonQuery();
                    isSuccess = rowsAffected > 0;
                }
            }
            finally
            {
                connection.Close();
            }

            return isSuccess;
        }
    }
}
