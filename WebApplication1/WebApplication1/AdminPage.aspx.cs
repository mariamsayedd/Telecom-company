using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class AdminPage : System.Web.UI.Page
    {
        static string connectionString = WebConfigurationManager.ConnectionStrings["master"].ToString();
        static SqlConnection connection = new SqlConnection(connectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            ShowAlert("Welcome back Admin", "success");
        }

        protected void list(object sender, EventArgs e)
        {
            int value = Int32.Parse(dropDown.SelectedValue);
            String query = string.Empty;
            switch (value)
            {
                case 1:
                    query = "SELECT * from allCustomerAccounts";
                    break;
                case 2:
                    query = "SELECT * from PhysicalStoreVouchers";
                    break;
                case 3:
                    query = " select * from allResolvedTickets";
                    break;
                case 4:
                    query = "exec Account_Plan";
                    break;
            }

           // GridView1.Controls.Clear();

            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            // Bind the data to the GridView
            GridView1.DataSource = dataTable;
            GridView1.DataBind();

        }

        protected void smsOffers_2_3a(object sender, EventArgs e)   // try test data '01234567898' (offerID 10    benefitID 15)
        {

            string query = "SELECT * FROM dbo.Account_SMS_Offers(@MobileNumber)";
            //string query2 = "SELECT * from Customer_Account";
            String inputMobileNumber = smsMobileNumber.Text;
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@MobileNumber", inputMobileNumber);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            GridView gv = new GridView();
            GridView2.DataSource = dataTable;
            GridView2.DataBind();

        }


        protected void deletingBenefits_2_3d(object sender, EventArgs e)    // try test data '01234567890' and 37 (re-insert if needed)
        {
            string query = "exec Benefits_Account @MobileNumber, @PlanID";
            //string query2 = "SELECT * from Customer_Account";
            String inputMobileNumber = deleteMobileNum.Text;
            String inputPlanID = deletePlanID.Text;
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@MobileNumber", inputMobileNumber);
            command.Parameters.AddWithValue("@PlanID", inputPlanID);
            command.CommandType = CommandType.Text;
            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    ShowAlert("The benefits have been deleted successfully", "success");
                }
                else
                {
                    ShowAlert("No matching benefits found", "warning");
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"An error occured {ex.Message}", "none");
            }
            finally {
                connection.Close();
            }

        }

        private void ShowAlert(string message, string alertType)
        {
            if (message == "Welcome back Admin")
            {
                string alertHtml = $@"
                 <div class='alert alert-{alertType} alert-dismissible fade show' role='alert'>
                     <strong>Login Successful!</strong> {message}
                     <button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>
                 </div>";

                AlertPlaceholder.Text = alertHtml;
            }
            else {
                string alertHtml = $@"
                 <div class='alert alert-{alertType} alert-dismissible fade show' role='alert'>
                     <strong>{(alertType == "success" ? "Success!" : "Warning!")}</strong> {message}
                     <button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>
                 </div>";

                AlertPlaceholder.Text = alertHtml;
            }
        }



        protected void Account_Plan_date_2_3_b(object sender, EventArgs e) { //select * from dbo.Account_Plan_date ('2023-01-10', 41)
            string query = "select * from dbo.Account_Plan_date (@sub_date, @planId)";
            String inputSubscriptionDate = Account_Plan_date_subdate.Text;
            String inputPlanID = Account_Plan_date_plan_id.Text;
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@sub_date", inputSubscriptionDate);
            command.Parameters.AddWithValue("@PlanId", inputPlanID);
            command.CommandType = CommandType.Text;
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                GridView gv = new GridView();
                Account_Plan_date_Grid_View.DataSource = dataTable;
                Account_Plan_date_Grid_View.DataBind();

            }
            catch (Exception ex)
            {
                ShowAlert($"An error occured {ex.Message}", "none");
            }

        }

        //select* from dbo.Account_Usage_Plan('01234567890','2023-01-01' )

        protected void Account_Usage_Plan_2_3_c(object sender, EventArgs e){         //select* from dbo.Account_Usage_Plan('01234567890','2023-01-01' )
            string query = "select * from dbo.Account_Usage_Plan(@mobileNum,@start_date )";
            String inputStartDate = Account_Usage_Plan_date.Text;
            String inputMobileNum= Account_Usage_Plan_mobileNum.Text;
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@start_date", inputStartDate);
            command.Parameters.AddWithValue("@mobileNum", inputMobileNum);
            command.CommandType = CommandType.Text;
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                GridView gv = new GridView();
                Account_Usage_Plan_GridView.DataSource = dataTable;
                Account_Usage_Plan_GridView.DataBind();

            }
            catch (Exception ex)
            {
                ShowAlert($"An error occured {ex.Message}", "none");
            }

        }
    }

}