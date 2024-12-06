using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1

{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void login(object sender, EventArgs e)
        {
            // Get the connection string from the configuration file
            string connectionString = WebConfigurationManager.ConnectionStrings["master"].ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SELECT dbo.AccountLoginValidation(@MobileNo, @Password)", connection);

            command.CommandType = CommandType.Text;

            string mobileNo = username.Text; 
            string pass = password.Text;     

            command.Parameters.AddWithValue("@MobileNo", mobileNo);
            command.Parameters.AddWithValue("@Password", pass);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                bool isValid = result != null && Convert.ToBoolean(result);
                bool isAdmin = false;
                if (mobileNo == "admin" && pass == "admin") {
                    isValid = true;
                    isAdmin = true; 
                }   
                if (isValid)
                {
                    
                    if (isAdmin)
                    {
                        Session["bool"] = true;
                        Response.Redirect("AdminPage.aspx");
                    }
                    else {
                        Response.Redirect($"UserPage.aspx?mobileNo={mobileNo}");
                    }
                }
                else
                {
                    ShowAlert("Invalid mobile number or password.", "warning");
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"An error occurred: {ex.Data}", "warning" );
            }
        }

        private void ShowAlert(string message, string alertType)
        {
            string alertHtml = $@"
                <div class='alert alert-{alertType} alert-dismissible fade show' role='alert'>
                    <strong>{(alertType == "success" ? "Success!" : "Warning!")}</strong> {message}
                    <button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>
                </div>";

            AlertPlaceholder.Text = alertHtml;   
        }
    }
}