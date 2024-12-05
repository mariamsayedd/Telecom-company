using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;

namespace WebApplication1
{
    public partial class UserPage : System.Web.UI.Page
    {
        static string connectionString = WebConfigurationManager.ConnectionStrings["master"].ToString();
        static SqlConnection connection = new SqlConnection(connectionString);
        static string mobileNo = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            //mobileNo = Request.QueryString["mobileNo"];
            mobileNo = "01234567890";
            if (dropDown.SelectedValue != "9")
                remainingBalance.Visible = false;
            if (dropDown.SelectedValue != "10")
                extraAmount.Visible = false;
        }
        protected void list(object sender, EventArgs e)
        {
            int value = Int32.Parse(dropDown.SelectedValue);
            String query = string.Empty;
            switch (value)
            {
                case 1:
                    query = "SELECT * FROM allServicePlans";
                    callView(query);
                    break;
                case 2:
                    getConsumption();
                    break;
                case 3:
                    getOfferedPlans();
                    break;
                case 4:
                    getUsagePlan();
                    break;
                case 5:
                    getTransactions();
                    break;
                case 6:
                    query = "SELECT * FROM allBenefits";
                    callView(query);
                    break;
                case 7:
                    getUnresolvedTickets();
                    break;
                case 8:
                    getHighestVoucher();
                    break;
                case 9:
                    getRemainingBalance();
                    break;
                case 10:
                    getExtraAmount();
                    break;
                case 11:
                    getTop10();
                    break;
                case 12:
                    break;
                case 13:
                    break;
                case 14:
                    break;
                case 15:
                    break;
                case 16:
                    break;
                case 17:
                    break;
            }
        }

        //visible to false?

        protected void getConsumption()   //protected void getConsumption(object sender, EventArgs e) why?  
        {

            string query = "SELECT * FROM dbo.Consumption(@Plan_name, @start_date, @end_date)";
            String serviceName = consumptionRem.Text;
            String serviceName2 = consumptionRem2.Text;
            String serviceName3 = consumptionRem3.Text;
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Plan_name", serviceName);
            command.Parameters.AddWithValue("@start_date", serviceName2);
            command.Parameters.AddWithValue("@end_date", serviceName3);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            GridView gv = new GridView();
            GridView1.DataSource = dataTable;
            GridView1.DataBind();

        }

        protected void getOfferedPlans()
        {
            // Set the DataSource to null and rebind the GridView
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedure = "Unsubscribed_Plans";

                // Create SqlCommand object to execute the stored procedure
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    // Specify that this command is a stored procedure
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@mobile_num", mobileNo);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    connection.Open();
                    adapter.Fill(dataTable);
                    GridView1.DataSource = dataTable;
                    GridView1.DataBind();
                }
            }
        }

        protected void getUsagePlan()   ////protected void getUsagePlan(object sender, EventArgs e) why?  
        {

            string query = "SELECT * FROM dbo.Usage_Plan_CurrentMonth(@MobileNo)"; ////mobileNo input?
            String serviceName = inputMobile.Text;
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@MobileNo", serviceName);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            GridView gv = new GridView();
            GridView1.DataSource = dataTable;
            GridView1.DataBind();

        }

        protected void getTransactions()   ////protected void getTransactions(object sender, EventArgs e) why?  
        {

            string query = "SELECT * FROM dbo.Cashback_Wallet_Customer(@NationalID)"; ////NationalID input?
            String serviceName = inputNational.Text;
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalID", serviceName);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            GridView gv = new GridView();
            GridView1.DataSource = dataTable;
            GridView1.DataBind();

        }

        protected void callView(String query)
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
            //remainingBalance.Visible = false;
            //extraAmount.Visible = false;
            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            // Bind the data to the GridView
            GridView1.DataSource = dataTable;
            GridView1.DataBind();
        }
        protected void getUnresolvedTickets()
        {
            // Set the DataSource to null and rebind the GridView
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
            //remainingBalance.Visible = false;
            //extraAmount.Visible = false;
            string query = "SELECT nationalID FROM customer_account WHERE mobileNo = " + mobileNo;
            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            int nationalID = int.Parse(dataTable.Rows[0][0].ToString());


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedure = "Ticket_Account_Customer";

                // Create SqlCommand object to execute the stored procedure
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    // Specify that this command is a stored procedure
                    command.CommandType = CommandType.StoredProcedure;

                    // Add the parameter @NID
                    command.Parameters.AddWithValue("@NID", nationalID);

                    // Create SqlDataAdapter to fill DataTable
                    SqlDataAdapter adapter1 = new SqlDataAdapter(command);

                    // Create DataTable to hold the result
                    DataTable dataTable1 = new DataTable();

                    try
                    {
                        connection.Open();
                        adapter1.Fill(dataTable1);
                        int count = int.Parse(dataTable1.Rows[0][0].ToString());
                        outputText.InnerText = "Number of unresolved tickets: " + count;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        protected void getHighestVoucher()
        {
            // Set the DataSource to null and rebind the GridView
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
            //remainingBalance.Visible = false;
            //extraAmount.Visible = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedure = "Account_Highest_Voucher";

                // Create SqlCommand object to execute the stored procedure
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    // Specify that this command is a stored procedure
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@mobile_num",  mobileNo);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    DataTable dataTable = new DataTable();

                    try
                    {
                        connection.Open();
                        adapter.Fill(dataTable);
                        int maxVoucher = int.Parse(dataTable.Rows[0][0].ToString());
                        outputText.InnerText = "ID of the Voucher with the highest value: " + maxVoucher;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }

        protected void getRemainingBalance()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
            remainingBalance.Visible = true;
            //extraAmount.Visible = false;
        }
        protected void getRemainingBalanceClicked(object sender, EventArgs e)
        {
            String serviceName = servicePlanRem.Text;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to call the function
                    string query = "SELECT dbo.Remaining_plan_amount(@mobile_num, @plan_name)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters for the function
                        command.Parameters.AddWithValue("@mobile_num", mobileNo);
                        command.Parameters.AddWithValue("@plan_name", serviceName);

                        // Execute the command and get the result
                        object result = command.ExecuteScalar();

                        // Check if result is not null and display
                        if (result != null && result != DBNull.Value)
                        {
                            int remainingAmount = Convert.ToInt32(result);
                            outputText.InnerText = $"Remaining plan amount: {remainingAmount}";
                        }
                        else
                        {
                            outputText.InnerText = "No result for the given plan name.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        protected void getExtraAmount()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
            //remainingBalance.Visible = false;
            extraAmount.Visible = true;
        }
        protected void getExtraAmountClicked(object sender, EventArgs e)
        {
            String serviceName = servicePlanExtra.Text;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to call the function
                    string query = "SELECT dbo.Extra_plan_amount(@mobile_num, @plan_name)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters for the function
                        command.Parameters.AddWithValue("@mobile_num", mobileNo);
                        command.Parameters.AddWithValue("@plan_name", serviceName);

                        // Execute the command and get the result
                        object result = command.ExecuteScalar();

                        // Check if result is not null and display
                        if (result != null && result != DBNull.Value)
                        {
                            int extraAmount = Convert.ToInt32(result);
                            outputText.InnerText = $"Extra plan amount: {extraAmount}";
                        }
                        else
                        {
                            outputText.InnerText = "No result for the given plan name.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        protected void getTop10()
        {
            // Set the DataSource to null and rebind the GridView
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
            //remainingBalance.Visible = false;
            //extraAmount.Visible = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedure = "Top_Successful_Payments";

                // Create SqlCommand object to execute the stored procedure
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    // Specify that this command is a stored procedure
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@mobile_num", mobileNo);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    DataTable dataTable = new DataTable();

                    try
                    {
                        connection.Open();
                        adapter.Fill(dataTable);
                        GridView1.DataSource = dataTable;
                        GridView1.DataBind();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
    }
}