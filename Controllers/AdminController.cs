using System;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using EventManagement.Repository;
using EventManagement.Models;
using System.Web.Security;
using System.Linq;
using System.Collections.Generic;

namespace EventManagement.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly AdminRepository data = new AdminRepository();
        private readonly UserRepository user = new UserRepository();
        private readonly ErrorLog errorLog = new ErrorLog(); // Log service

        /// <summary>
        /// Initializes a new instance of the SqlConnection.
        /// </summary>
        private SqlConnection CreateConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["adoConnectionString"].ToString();
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Admin dashboard page.
        /// </summary>
        /// <returns>ViewResult</returns>
        public ActionResult AdminDashboard()
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                errorLog.LogInfo("AdminDashboard page accessed."); // Log
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while accessing the dashboard.";
                errorLog.LogError($"Exception in AdminDashboard: {ex.Message}"); // Log
                return RedirectToAction("Login", "Home");
            }
        }

        /// <summary>
        /// Displays the list of employees.
        /// </summary>
        /// <returns>ViewResult</returns>
        [HttpGet]
        public ActionResult EmployeeList()
        {
            try
            {
                errorLog.LogInfo("EmployeeList page GET accessed."); // Log
                var userList = data.AllEmployeeList();
                return View(userList);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving the employee list.";
                errorLog.LogError($"Exception in EmployeeList: {ex.Message}"); // Log
                return View();
            }
        }

        /// <summary>
        /// Displays details of a specific employee.
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>ViewResult</returns>
        public ActionResult Details(int id)
        {
            try
            {
                errorLog.LogInfo($"Details page accessed for UserID: {id}"); // Log
                var user = data.GetUserByID(id).FirstOrDefault();
                if (user == null)
                {
                    TempData["InfoMessage"] = "Currently, employee not available in the database.";
                    return RedirectToAction("EmployeeList");
                }
                return View(user);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving employee details.";
                errorLog.LogError($"Exception in Details: {ex.Message}"); // Log
                return View();
            }
        }

        // AddEvent page and action
        /// <summary>
        /// Display the AddEvent page.
        /// </summary>
        /// <returns>ViewResult</returns>
        public ActionResult AddEvent()
        {
            try
            {
                errorLog.LogInfo("AddEvent page accessed."); // Log
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while accessing the AddEvent page.";
                errorLog.LogError($"Exception in AddEvent page access: {ex.Message}"); // Log
                return View();
            }
        }

        /// <summary>
        /// Handles the POST request to add a new event.
        /// </summary>
        /// <param name="obj">Event object</param>
        /// <returns>RedirectResult</returns>
        [HttpPost]
        public ActionResult AddEvent(Event eventn)
        {
            try
            {
                errorLog.LogInfo("AddEvent action triggered."); // Log
                if (ModelState.IsValid)
                {
                    data.AddEvent(eventn);
                    TempData["SuccessMessage"] = "Event Added";
                    errorLog.LogInfo("Event added successfully."); // Log
                }
                else
                {
                    TempData["ErrorMessage"] = "Model state is invalid.";
                    errorLog.LogWarn("Model state invalid when adding event."); // Log
                }
                return RedirectToAction("AddEvent");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                errorLog.LogError($"Exception in AddEvent: {ex.Message}"); // Log
                return View();
            }
        }

        // UpdateEvent page
        /// <summary>
        /// Displays the list of events for updating.
        /// </summary>
        /// <returns>ViewResult</returns>
        [HttpGet]
        public ActionResult UpdateEvent()
        {
            try
            {
                errorLog.LogInfo("UpdateEvent page GET accessed."); // Log
                var eventList = data.AllEvent();
                return View(eventList);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving the event list.";
                errorLog.LogError($"Exception in UpdateEvent: {ex.Message}"); // Log
                return View();
            }
        }

        /// <summary>
        /// Displays details of a specific event.
        /// </summary>
        /// <param name="id">Event ID</param>
        /// <returns>ViewResult</returns>
        public ActionResult DetailsEvent(int id)
        {
            try
            {
                errorLog.LogInfo($"DetailsEvent page accessed for EventID: {id}"); // Log
                var details = data.GetEventByID(id).FirstOrDefault();
                if (details == null)
                {
                    TempData["InfoMessage"] = "Currently event not available in the database.";
                    return RedirectToAction("UpdateEvent");
                }
                return View(details);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving event details.";
                errorLog.LogError($"Exception in DetailsEvent: {ex.Message}"); // Log
                return View();
            }
        }

        // EditEvent page
        /// <summary>
        /// Displays the EditEvent page for a specific event.
        /// </summary>
        /// <param name="id">Event ID</param>
        /// <returns>ViewResult</returns>
        public ActionResult EditEvent(int id)
        {
            try
            {
                errorLog.LogInfo($"EditEvent page accessed for EventID: {id}"); // Log
                var details = data.GetEventByID(id).FirstOrDefault();
                if (details == null)
                {
                    TempData["InfoMessage"] = "Event not available with Id " + id.ToString();
                    return RedirectToAction("UpdateEvent");
                }
                return View(details);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while accessing the EditEvent page.";
                errorLog.LogError($"Exception in EditEvent: {ex.Message}"); // Log
                return View();
            }
        }

        /// <summary>
        /// Handles the POST request to update a specific event.
        /// </summary>
        /// <param name="details">Event object</param>
        /// <returns>RedirectResult</returns>
        [HttpPost, ActionName("EditEvent")]
        public ActionResult EditEvent(Event eventn)
        {
            try
            {
                errorLog.LogInfo($"EditEvent action triggered for EventID: {eventn.EventID}"); // Log
                if (ModelState.IsValid)
                {
                    bool isUpdated = data.UpdateEventByID(eventn);
                    if (isUpdated)
                    {
                        TempData["SuccessMessage"] = "Event details updated successfully...!";
                        errorLog.LogInfo("Event details updated successfully."); // Log
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Event is unable to update the details.";
                        errorLog.LogWarn("Event update failed."); // Log
                    }
                }
                return RedirectToAction("UpdateEvent");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                errorLog.LogError($"Exception in EditEvent: {ex.Message}"); // Log
                return View();
            }
        }

        // DeleteEvent page
        /// <summary>
        /// Displays the Delete page for a specific event.
        /// </summary>
        /// <param name="id">Event ID</param>
        /// <returns>ViewResult</returns>
        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        errorLog.LogInfo($"Delete page accessed for EventID: {id}"); // Log
        //        var obj = data.GetEventByID(id).FirstOrDefault();
        //        if (obj == null)
        //        {
        //            TempData["InfoMessage"] = "Event not available with Id " + id.ToString();
        //            return RedirectToAction("UpdateEvent");
        //        }
        //        return View(obj);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = "An error occurred while accessing the Delete page.";
        //        errorLog.LogError($"Exception in Delete: {ex.Message}"); // Log
        //        return View();
        //    }
        //}

        /// <summary>
        /// Handles the POST request to confirm the deletion of an event.
        /// </summary>
        /// <param name="id">Event ID</param>
        ///// <returns>RedirectResult</returns>
        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmation(int id)
        //{
        //    try
        //    {
        //        errorLog.LogInfo($"DeleteConfirmation action triggered for EventID: {id}"); // Log
        //        string result = data.DeleteEventByID(id);
        //        if (result.Contains("deleted"))
        //        {
        //            TempData["SuccessMessage"] = result;
        //            errorLog.LogInfo("Event deleted successfully."); // Log
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = result;
        //            errorLog.LogWarn("Event deletion failed."); // Log
        //        }
        //        return RedirectToAction("UpdateEvent");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = ex.Message;
        //        errorLog.LogError($"Exception in DeleteConfirmation: {ex.Message}"); // Log
        //        return View();
        //    }
        //}

        // User responses
        /// <summary>
        /// Displays the list of user responses.
        /// </summary>
        /// <returns>ViewResult</returns>
        public ActionResult UserResponses()
        {
            try
            {
                errorLog.LogInfo("UserResponses page accessed."); // Log
                var responses = user.GetAllResponses();
                return View(responses);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving user responses.";
                errorLog.LogError($"Exception in UserResponses: {ex.Message}"); // Log
                return View();
            }
        }

        /// <summary>
        /// Approves a user response.
        /// </summary>
        /// <param name="responseId">Response ID</param>
        /// <returns>RedirectResult</returns>
        [HttpPost]
        public ActionResult ApproveResponse(int responseId)
        {
            try
            {
                errorLog.LogInfo($"ApproveResponse action triggered for ResponseID: {responseId}"); // Log
                bool isApproved = user.ApproveResponse(responseId);
                if (isApproved)
                {
                    TempData["SuccessMessage"] = "Response approved successfully.";
                    errorLog.LogInfo("Response approved successfully."); // Log
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to approve response.";
                    errorLog.LogWarn("Response approval failed."); // Log
                }
                return RedirectToAction("UserResponses");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                errorLog.LogError($"Exception in ApproveResponse: {ex.Message}"); // Log
                return View();
            }
        }

        /// <summary>
        /// Rejects a user response.
        /// </summary>
        /// <param name="responseId">Response ID</param>
        /// <returns>RedirectResult</returns>
        [HttpPost]
        public ActionResult RejectResponse(int responseId)
        {
            try
            {
                errorLog.LogInfo($"RejectResponse action triggered for ResponseID: {responseId}"); // Log
                bool isRejected = user.RejectResponse(responseId);
                if (isRejected)
                {
                    TempData["SuccessMessage"] = "Response rejected successfully.";
                    errorLog.LogInfo("Response rejected successfully."); // Log
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to reject response.";
                    errorLog.LogWarn("Response rejection failed."); // Log
                }
                return RedirectToAction("UserResponses");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                errorLog.LogError($"Exception in RejectResponse: {ex.Message}"); // Log
                return View();
            }
        }

        // Contact list
        /// <summary>
        /// Displays the list of contact messages.
        /// </summary>
        /// <returns>ViewResult</returns>
        public ActionResult ContactList()
        {
            try
            {
                var contactList = data.ContactList();
                return View(contactList);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving the contact list.";
                errorLog.LogError($"Exception in ContactList: {ex.Message}"); // Log
                return View(new List<ContactUs>());
            }
        }

        // Signout
        /// <summary>
        /// Logs out the admin and redirects to the login page.
        /// </summary>
        /// <returns>RedirectResult</returns>
        public ActionResult Signout()
        {
            try
            {
                errorLog.LogInfo("AdminLogout action triggered."); // Log
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while logging out.";
                errorLog.LogError($"Exception in Signout: {ex.Message}"); // Log
                return RedirectToAction("Login", "Home");
            }
        }
    }
}
