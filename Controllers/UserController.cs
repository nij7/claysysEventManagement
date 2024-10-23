using EventManagement.Repository;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;
using EventManagement.Models;
using System.Data;
using System.Linq;

namespace EventManagement.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        // SqlConnection object to manage the database connection.
        private SqlConnection connection;

        // Repository instances for accessing database methods.
        AdminRepository data = new AdminRepository();
        UserRepository user = new UserRepository();

        // ErrorLog instance for logging errors, warnings, and info.
        private readonly ErrorLog errorLog = new ErrorLog();

        // Method to establish a database connection using the connection string from the config file.
        private void Connection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["adoConnectionString"].ToString();
            connection = new SqlConnection(connectionString);
        }

        // GET: User - Displays the employee response page.
        public ActionResult EmployeResponse()
        {
            return View();
        }

        // Displays the User Dashboard after checking if the user is logged in.
        public ActionResult UserDashboard()
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                errorLog.LogError($"Exception in UserDashboard: {ex.Message}");
                return RedirectToAction("Login", "Home");
            }

            errorLog.LogInfo("UserDashboard page accessed.");
            return View();
        }

        // Method to retrieve and set the UserID in session using the username.
        public void GetId(string username)
        {
            try
            {
                Connection();
                connection.Open();
                using (SqlCommand command = new SqlCommand("SPS_GetUserId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", username);

                    object result = command.ExecuteScalar();
                    int userId = Convert.ToInt32(result);

                    Session["user"] = userId;
                }
            }
            catch (Exception ex)
            {
                errorLog.LogError($"Exception in GetId: {ex.Message}");
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        // Displays profile information of the logged-in user.
        public ActionResult ProfileInfo()
        {
            try
            {
                errorLog.LogInfo("Profile info page accessed.");
                int employee = (int)Session["UserID"];
                var userInfo = data.GetUserByID(employee).FirstOrDefault();

                if (userInfo == null)
                {
                    return RedirectToAction("ProfileInfo");
                }

                return View(userInfo);
            }
            catch (Exception ex)
            {
                errorLog.LogError($"Exception in ProfileInfo: {ex.Message}");
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // Displays a list of upcoming events.
        public ActionResult UpcomingEvents()
        {
            try
            {
                errorLog.LogInfo("ViewEvents page GET accessed.");
                var eventsList = data.AllEvent();
                return View(eventsList);
            }
            catch (Exception ex)
            {
                errorLog.LogError($"Exception in UpcomingEvents: {ex.Message}");
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // Registers the user for a selected event and records their response.
        public ActionResult EventResponse(int eventId)
        {
            try
            {
                errorLog.LogInfo($"RegisterForEvent action triggered for EventID: {eventId}");
                int userId = (int)Session["UserID"];
                var existingResponse = user.GetResponseForEvent(eventId, userId);

                if (existingResponse != null)
                {
                    errorLog.LogWarn("Event registration failed. User already registered.");
                    TempData["ErrorMessage"] = "You have already applied for this event.";
                    return RedirectToAction("UpcomingEvents");
                }

                var response = new EmployeeResponse
                {
                    EventID = eventId,
                    UserID = userId,
                    Status = "Pending"
                };

                user.AddResponse(response);
                errorLog.LogInfo("User registered for event successfully.");
                TempData["SuccessMessage"] = "Your response has been recorded.";
                return RedirectToAction("UpcomingEvents");
            }
            catch (Exception ex)
            {
                errorLog.LogError($"Exception in EventResponse: {ex.Message}");
                TempData["ErrorMessage"] = "You have already applied for this event.";
                return RedirectToAction("UpcomingEvents");
            }
        }

        // Displays the user's responses to events.
        public ActionResult UserResponses()
        {
            try
            {
                errorLog.LogInfo("ViewUserRegistrations page accessed.");
                int userId = (int)Session["UserID"];
                var responses = user.GetResponsesByUser(userId);
                return View(responses);
            }
            catch (Exception ex)
            {
                errorLog.LogError($"Exception in UserResponses: {ex.Message}");
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // Displays the Edit Profile page for the logged-in user.
        public ActionResult EditProfile()
        {
            try
            {
                errorLog.LogInfo("Edit profile page accessed.");
                int employee = (int)Session["UserID"];
                var userProfile = data.GetUserByID(employee).FirstOrDefault();

                if (userProfile == null)
                {
                    return RedirectToAction("ProfileInfo");
                }

                return View(userProfile);
            }
            catch (Exception ex)
            {
                errorLog.LogError($"Exception in EditProfile (GET): {ex.Message}");
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ProfileInfo");
            }
        }

        // Handles the POST request to update the user's profile information.
        [HttpPost, ActionName("EditProfile")]
        public ActionResult EditProfile(Signup userProfile)
        {
            try
            {
                errorLog.LogInfo("Update profile action triggered.");

                if (ModelState.IsValid)
                {
                    bool isUpdated = data.UpdateUser(userProfile);

                    if (isUpdated)
                    {
                        TempData["SuccessMessage"] = "Details updated successfully.";
                        errorLog.LogInfo("User details updated successfully.");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to update the details.";
                        errorLog.LogWarn("User details update failed.");
                    }
                }

                return RedirectToAction("UserDashboard");
            }
            catch (Exception ex)
            {
                errorLog.LogError($"Exception in EditProfile (POST): {ex.Message}");
                TempData["ErrorMessage"] = ex.Message;
                return View(userProfile);
            }
        }

        // Displays the Change Password page.
        public ActionResult ChangePasswordUser()
        {
            errorLog.LogInfo("Change password user page accessed.");
            ViewBag.Message = "Change Password User";
            return View();
        }

        // Handles the POST request to change the user's password.
        [HttpPost]
        public ActionResult ChangePasswordUser(ChangePasswordViewModel model)
        {
            try
            {
                errorLog.LogInfo("Change password action triggered.");

                if (ModelState.IsValid)
                {
                    int userId = (int)Session["UserID"];
                    var user = data.GetUserByID(userId).FirstOrDefault();

                    if (user == null)
                    {
                        TempData["ErrorMessage"] = "User not found.";
                        errorLog.LogWarn("User not found during password change.");
                        return RedirectToAction("UserDashboard");
                    }

                    if (user.Password != model.CurrentPassword) // Ideally, hash the password before comparing
                    {
                        TempData["ErrorMessage"] = "Current password is incorrect.";
                        errorLog.LogWarn("Incorrect current password during password change.");
                        return View(model);
                    }

                    if (model.NewPassword != model.ConfirmNewPassword)
                    {
                        TempData["ErrorMessage"] = "New passwords do not match.";
                        errorLog.LogWarn("New password mismatch during change password.");
                        return View(model);
                    }

                    user.Password = model.NewPassword; // Ideally, hash the password before saving
                    bool isUpdated = data.UpdateUserPassword(user);

                    if (isUpdated)
                    {
                        TempData["SuccessMessage"] = "Password updated successfully.";
                        errorLog.LogInfo("Password updated successfully.");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to update the password.";
                        errorLog.LogWarn("Password update failed.");
                    }

                    return RedirectToAction("UserDashboard");
                }
            }
            catch (Exception ex)
            {
                errorLog.LogError($"Exception in ChangePasswordUser: {ex.Message}");
                TempData["ErrorMessage"] = ex.Message;
            }

            return View(model);
        }

        // Logs out the user and clears the session.
        public ActionResult SignOut()
        {
            try
            {
                errorLog.LogInfo("UserLogout action triggered.");
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("Login", "Home");
            }
            catch (Exception ex)
            {
                errorLog.LogError($"Exception in SignOut: {ex.Message}");
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Login", "Home");
            }
        }
    }
}
