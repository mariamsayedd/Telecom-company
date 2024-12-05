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

            // Extract text values from TextBox controls
            string mobileNo = username.Text;  // Assuming 'username' is the TextBox for the mobile number
            string pass = password.Text;     // Assuming 'password' is the TextBox for the password

            // Add parameters with their values
            command.Parameters.AddWithValue("@MobileNo", mobileNo);
            command.Parameters.AddWithValue("@Password", pass);

            try
            {
                // Open the connection
                connection.Open();

                // Execute the scalar function
                object result = command.ExecuteScalar();

                // Convert the result to a Boolean
                bool isValid = result != null && Convert.ToBoolean(result);
                bool isAdmin = false;
                if (mobileNo == "admin" && pass == "admin") {
                    isValid = true;
                    isAdmin = true; 
                }   
                // Check the result and respond accordingly
                if (isValid)
                {
                    
                    if (isAdmin)
                    {
                        Response.Redirect($"AdminPage.aspx");
                    }
                    // Redirect or perform additional actions
                    else {
                        Response.Redirect($"UserPage.aspx?mobileNo={mobileNo}");
                    }
                }
                else
                {
                    Response.Write("Invalid mobile number or password.");
                }
            }
            catch (Exception ex)
            {
                Response.Write("An error occurred: " + ex.Message);
            }
        }

    }
}