using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System;
using EventManagement.Models;
using System.Web;
using System.IO;

namespace EventManagement.Repository
{
    public class AdminRepository
    {
        
        string connectionString = ConfigurationManager.ConnectionStrings["adoConnectionString"].ToString();

        /// <summary>
        /// all employee list
        /// </summary>
        /// <returns>all empoloyees</returns>
            public List<Signup> AllEmployeeList()
            {
                List<Signup> SignupList = new List<Signup>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SPS_GetUsers";
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();

                    connection.Open();
                    adapter.Fill(dataTable);
                    connection.Close();
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        SignupList.Add(new Signup
                        {
                            UserID = Convert.ToInt32(dataRow["UserID"]),
                            FirstName = dataRow["FirstName"].ToString(),
                            LastName = dataRow["LastName"].ToString(),
                            DateOfBirth = dataRow["DateOfBirth"].ToString(),
                            Gender = dataRow["Gender"].ToString(),
                            PhoneNumber = dataRow["PhoneNumber"].ToString(),
                            EmailAddress = dataRow["EmailAddress"].ToString(),
                            Address = dataRow["Address"].ToString(),
                            State = dataRow["State"].ToString(),
                            City = dataRow["City"].ToString(),
                            Username = dataRow["Username"].ToString(),
                            Password = dataRow["Password"].ToString(),
                            ConfirmPassword = dataRow["ConfirmPassword"].ToString()


                        });
                    }
                }
                    return SignupList;
                
            }
        /// <summary>
        /// employee by id
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns>employee details</returns>
        public List<Signup> GetUserByID(int UserID)
        {
            List<Signup> SignupList = new List<Signup>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SPS_GetUserById";
                command.Parameters.AddWithValue("@UserID", UserID);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dataUsers = new DataTable();

                connection.Open();
                sqlDA.Fill(dataUsers);
                connection.Close();

                foreach (DataRow dr in dataUsers.Rows)
                {
                    SignupList.Add(new Signup
                    {
                        UserID = Convert.ToInt32(dr["UserID"]),
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        DateOfBirth = dr["DateOfBirth"].ToString(),
                        Gender = dr["Gender"].ToString(),
                        PhoneNumber = dr["PhoneNumber"].ToString(),
                        EmailAddress = dr["EmailAddress"].ToString(),
                        Address = dr["Address"].ToString(),
                        State = dr["State"].ToString(),
                        City = dr["City"].ToString(),
                        Username = dr["Username"].ToString(),
                        Password = dr["Password"].ToString(),
                        ConfirmPassword = dr["ConfirmPassword"].ToString()
                    });
                }
            }

            return SignupList;
        }
        /// <summary>
        /// update employee
        /// </summary>
        /// <param name="signup"></param>
        /// <returns>edit details by id</returns>
        public bool UpdateUser(Signup signup)
        {
            int i= 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SPU_UpdateUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", signup.UserID);
                command.Parameters.AddWithValue("@FirstName", signup.FirstName);
                command.Parameters.AddWithValue("@LastName", signup.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", signup.DateOfBirth);
                command.Parameters.AddWithValue("@Gender", signup.Gender);
                command.Parameters.AddWithValue("@PhoneNumber", signup.PhoneNumber);
                command.Parameters.AddWithValue("@EmailAddress", signup.EmailAddress);
                command.Parameters.AddWithValue("@Address", signup.Address);
                command.Parameters.AddWithValue("@State", signup.State);
                command.Parameters.AddWithValue("@City", signup.City);
                command.Parameters.AddWithValue("@Username", signup.Username);
                
                
                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();

            }
            if (i > 0)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// add event
        /// </summary>
        /// <param name="eventn"></param>
        /// <return>adding event</return>
        public void AddEvent(Event eventn)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SPI_AddEvent", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EventName", eventn.EventName);
                command.Parameters.AddWithValue("@DateOfEvent", eventn.DateOfEvent);
                command.Parameters.AddWithValue("@Description", eventn.Description);
                command.Parameters.AddWithValue("@Subevent1", eventn.Subevent1);
                command.Parameters.AddWithValue("@Subdesc1", eventn.Subdesc1);
                command.Parameters.AddWithValue("@Subevent2", eventn.Subevent2);
                command.Parameters.AddWithValue("@Subdesc2", eventn.Subdesc2);
                command.Parameters.AddWithValue("@Subevent3", eventn.Subevent3);
                command.Parameters.AddWithValue("@Subdesc3", eventn.Subdesc3);
                command.Parameters.AddWithValue("@Subevent4", eventn.Subevent4);
                command.Parameters.AddWithValue("@Subdesc4", eventn.Subdesc4);
                command.Parameters.AddWithValue("@Subevent5", eventn.Subevent5);
                command.Parameters.AddWithValue("@Subdesc5", eventn.Subdesc5);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        /// <summary>
        /// all event list
        /// </summary>
        /// <returns>event table</returns>
        public List<Event> AllEvent()
        {
            List<Event> AllEventList = new List<Event>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SPS_AllEvents";
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                connection.Open();
                adapter.Fill(dataTable);
                connection.Close();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AllEventList.Add(new Event
                    {
                        EventID = Convert.ToInt32(dataRow["EventID"]),
                        EventName = dataRow["EventName"].ToString(),
                        DateOfEvent = dataRow["DateOfEvent"].ToString(),
                        Description = dataRow["Description"].ToString(),
                        Subevent1= dataRow["Subevent1"].ToString(),
                        Subdesc1 = dataRow["Subdesc1"].ToString(),
                        Subevent2 = dataRow["Subevent2"].ToString(),
                        Subdesc2 = dataRow["Subdesc2"].ToString(),
                        Subevent3 = dataRow["Subevent3"].ToString(),
                        Subdesc3 = dataRow["Subdesc3"].ToString(),
                        Subevent4 = dataRow["Subevent4"].ToString(),
                        Subdesc4 = dataRow["Subdesc4"].ToString(),
                        Subevent5 = dataRow["Subevent5"].ToString(),
                        Subdesc5 = dataRow["Subdesc5"].ToString(),


                    });
                }
            }
            return AllEventList;

        }
        /// <summary>
        /// return event by id
        /// </summary>
        /// <param name="EventID"></param>
        /// <returns>event details by id</returns>
        public List<Event> GetEventByID(int EventID)
        {
            List<Event> AllEventList = new List<Event>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SPS_GetEventById";
                command.Parameters.AddWithValue("@EventID", EventID);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dataUsers = new DataTable();

                connection.Open();
                sqlDA.Fill(dataUsers);
                connection.Close();

                foreach (DataRow dr in dataUsers.Rows)
                {
                    AllEventList.Add(new Event
                    {
                        EventID = Convert.ToInt32(dr["eventID"]),
                        EventName = dr["EventName"].ToString(),
                        DateOfEvent = dr["DateOfEvent"].ToString(),
                        Description = dr["Description"].ToString(),
                        Subevent1 = dr["Subevent1"].ToString(),
                        Subdesc1 = dr["Subdesc1"].ToString(),
                        Subevent2 = dr["Subevent2"].ToString(),
                        Subdesc2 = dr["Subdesc2"].ToString(),
                        Subevent3 = dr["Subevent3"].ToString(),
                        Subdesc3 = dr["Subdesc3"].ToString(),
                        Subevent4 = dr["Subevent4"].ToString(),
                        Subdesc4 = dr["Subdesc4"].ToString(),
                        Subevent5 = dr["Subevent5"].ToString(),
                        Subdesc5 = dr["Subdesc5"].ToString(),
                    });
                }
            }

            return AllEventList;
        }
        /// <summary>
        /// edit event
        /// </summary>
        /// <param name="eventn"></param>
        /// <returns>edit eventby id</returns>
        public bool UpdateEventByID(Event eventn)
        {
            int i = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SPU_UpdateEvent", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EventID", eventn.EventID);
                command.Parameters.AddWithValue("@EventName", eventn.EventName);
                command.Parameters.AddWithValue("@DateOfEvent", eventn.DateOfEvent);
                command.Parameters.AddWithValue("@Description", eventn.Description);
                command.Parameters.AddWithValue("@Subevent1", eventn.Subevent1);
                command.Parameters.AddWithValue("@Subdesc1", eventn.Subdesc1);
                command.Parameters.AddWithValue("@Subevent2", eventn.Subevent2);
                command.Parameters.AddWithValue("@Subdesc2", eventn.Subdesc2);
                command.Parameters.AddWithValue("@Subevent3", eventn.Subevent3);
                command.Parameters.AddWithValue("@Subdesc3", eventn.Subdesc3);
                command.Parameters.AddWithValue("@Subevent4", eventn.Subevent4);
                command.Parameters.AddWithValue("@Subdesc4", eventn.Subdesc4);
                command.Parameters.AddWithValue("@Subevent5", eventn.Subevent5);
                command.Parameters.AddWithValue("@Subdesc5", eventn.Subdesc5);

                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();

            }
            if (i > 0)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        ////delete event
        //public String DeleteEventByID(int EventID)
        //{
        //    string result = "";
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand command = new SqlCommand("sp_DeleteEvent", connection);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@EventID", EventID);
        //        // Correct parameter name to match stored procedure definition
        //        command.Parameters.Add("@OUTPUTMESSAGE", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

        //        connection.Open();
        //        command.ExecuteNonQuery();
        //        // Correct parameter name to match stored procedure definition
        //        result = command.Parameters["@OUTPUTMESSAGE"].Value.ToString();
        //        connection.Close();
        //    }
        //    return result;
        //}



        /// <summary>
        /// feedback list
        /// </summary>
        /// <returns>contact us table</returns>
        public List<ContactUs> ContactList()
        {
            List<ContactUs> ContactList = new List<ContactUs>();
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SPS_GetContact";
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                connection.Open();
                adapter.Fill(dataTable);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ContactList.Add(new ContactUs
                    {
                        ContactId = Convert.ToInt32(dataRow["ContactID"]),
                        Name = dataRow["Name"].ToString(),
                        Email = dataRow["EmailAddress"].ToString(),
                        Subject = dataRow["Subject"].ToString(),
                    });
                }
            }
            finally
            {
                connection.Close();
            }

            return ContactList;
        }

        public bool ValidateCurrentPassword(int userId, string currentPassword)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SPS_ValidateUserPassword", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@Password", currentPassword);

                connection.Open();
                var result = cmd.ExecuteScalar();
                connection.Close();

                return result != null && (int)result == 1;
            }
        }


        /// <summary>
        /// change password
        /// </summary>
        /// <param name="signup"></param>
        /// <returns>changing password table</returns>
        public bool UpdateUserPassword(Signup signup)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SPU_UpdateUserPassword", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Password", signup.Password);
                cmd.Parameters.AddWithValue("@UserID", signup.UserID);

                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                connection.Close();

                return rowsAffected > 0;
            }
        }






    }
}
