using EventManagement;
using EventManagement.Models;
using EventManagement.Repository;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;

public class HomeController : Controller
{
    private SqlConnection connection;
    private readonly ErrorLog errorLog = new ErrorLog();
    private readonly HomeRepository homeData = new HomeRepository();

    /// <summary>
    /// Initializes the SQL connection using the connection string from the configuration file.
    /// </summary>
    private void InitializeConnection()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["adoConnectionString"].ToString();
        connection = new SqlConnection(connectionString);
    }

    /// <summary>
    /// Action method for the Home page (Index).
    /// </summary>
    /// <returns>Returns the Index view</returns>
    public ActionResult Index()
    {
        errorLog.LogInfo("Index page has been accessed.");
        return View();
    }

    /// <summary>
    /// Action method for the About page.
    /// </summary>
    /// <returns>Returns the About view</returns>
    public ActionResult About()
    {
        errorLog.LogInfo("About page has been accessed.");
        ViewBag.Message = "Your application description page.";
        return View();
    }

    /// <summary>
    /// GET method for the Login page.
    /// </summary>
    /// <returns>Returns the Login view</returns>
    [HttpGet]
    public ActionResult Login()
    {
        errorLog.LogInfo("Login page GET accessed.");
        return View();
    }

    /// <summary>
    /// POST method for user login.
    /// </summary>
    /// <param name="login">Signin object containing login credentials</param>
    /// <returns>Redirects to the appropriate dashboard based on user role</returns>
    [HttpPost]
    public ActionResult Login(Signin signin)
    {
        try
        {
            if (ModelState.IsValid)
            {
                InitializeConnection(); // Initialize SQL connection

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Username = @Username AND Password = @Password", connection))
                {
                    cmd.Parameters.AddWithValue("@Username", signin.Username);
                    cmd.Parameters.AddWithValue("@Password", signin.Password);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        Session["UserID"] = reader["UserID"];
                        Session["Username"] = reader["Username"];
                        errorLog.LogInfo($"User {signin.Username} logged in.");

                        // Redirect to Admin or User dashboard based on the role
                        if (signin.Username.ToLower() == "admin")
                        {
                            errorLog.LogInfo("Admin page has been accessed.");
                            FormsAuthentication.SetAuthCookie(signin.Username, false);
                            return RedirectToAction("AdminDashboard", "Admin");
                        }
                        else
                        {
                            errorLog.LogInfo("User page has been accessed.");
                            FormsAuthentication.SetAuthCookie(signin.Username, false);
                            return RedirectToAction("UserDashboard", "User");
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid login attempt.";
                        errorLog.LogWarn("Invalid login attempt.");
                    }
                    connection.Close();
                }
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            errorLog.LogError($"Exception during login: {ex.Message}");
        }

        return View(signin);
    }

    /// <summary>
    /// GET method for the Signup page.
    /// </summary>
    /// <returns>Returns the Signup view</returns>
    public ActionResult Signup()
    {
        errorLog.LogInfo("Signup page accessed.");
        return View();
    }

    /// <summary>
    /// POST method for user registration.
    /// </summary>
    /// <param name="signup">Signup object containing user registration details</param>
    /// <returns>Redirects to the Signup page</returns>
    [HttpPost]
    public ActionResult Signup(Signup signup)
    {
        try
        {
            if (ModelState.IsValid)
            {
                try
                {
                    homeData.InsertUser(signup); // Insert user data into the database
                    TempData["SuccessMessage"] = "Registered successfully";
                    errorLog.LogInfo("User registered successfully.");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Failed to register user. Please try again.";
                    errorLog.LogError($"Exception during signup: {ex.Message}");
                }
            }
            return RedirectToAction("Signup");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An unexpected error occurred. Please try again.";
            errorLog.LogError($"Unexpected exception during signup: {ex.Message}");
            return View();
        }
    }

    /// <summary>
    /// GET method for the Contact page.
    /// </summary>
    /// <returns>Returns the Contact view</returns>
    public ActionResult Contact()
    {
        errorLog.LogInfo("Contact page accessed.");
        return View();
    }

    /// <summary>
    /// POST method to submit contact information.
    /// </summary>
    /// <param name="contact">ContactUs object containing contact details</param>
    /// <returns>Redirects to the Contact page</returns>
    [HttpPost]
    public ActionResult Contact(ContactUs contact)
    {
        try
        {
            homeData.ContactUs(contact); // Insert contact data into the database
            TempData["SuccessMessage"] = "Message sent. We will reach out to you..!";
            errorLog.LogInfo("Contact message sent.");
            return RedirectToAction("Contact");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            errorLog.LogError($"Exception during contact submission: {ex.Message}");
            return View();
        }
    }
}
