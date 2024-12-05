using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Diagnostics.Eventing.Reader;

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
            if (dropDown.SelectedValue != "13")
            {
                lastsubscribedfive.Visible = false;
            }
            if (dropDown.SelectedValue != "14")
            {
                initiatePayment.Visible = false;
            }
            if (dropDown.SelectedValue != "15")
            {
                paymentwalletcashback.Visible = false;
            }
            if (dropDown.SelectedValue != "16")
            {
               Initiate_balance_payment.Visible = false;
            }
            if (dropDown.SelectedValue != "17")
            {
                Redeem_voucher.Visible = false;
            }


        }
        protected void list(object sender, EventArgs e)
        {
            int value = Int32.Parse(dropDown.SelectedValue);
            String query = string.Empty;

            switch (value)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
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
                    query = "SELECT * FROM allShops";
                    callView(query);
                    break;
                case 13:
                    //lastsubscribedfive.Visible = true;
                    getSubscribedPlansFiveMonths(sender, e);
                    break;
                case 14:
                    InitiatePayment(sender, e);
                    break;
                case 15:
                    PaymentWalletCashback(sender, e);
                    break;
                case 16:
                    RechargeBalance(sender, e);
                    break;
                case 17:
                    RedeemVoucher(sender, e);
                    break;
            }
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

                    command.Parameters.AddWithValue("@mobile_num", mobileNo);

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
        protected void getSubscribedPlansFiveMonths(object sender, EventArgs e)
        {
            lastsubscribedfive.Visible = true;
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
            string query = "SELECT * FROM dbo.Subscribed_plans_5_Months(@MobileNo)";
            string inputMobileNo = Subscribed_plans_5_Months_MobileNo.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MobileNo", inputMobileNo);
                    command.CommandType = CommandType.Text;

                    try
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        if (!string.IsNullOrEmpty(inputMobileNo))
                        {
                            if (dataTable.Rows.Count > 0)
                            {
                                GridView1.DataSource = dataTable;
                                GridView1.DataBind();
                            }
                            else
                            {
                                outputText.InnerText = "No records found for this mobile number.Please enter a valid mobile number";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }


        protected void InitiatePayment(object sender, EventArgs e)
        {
            initiatePayment.Visible = true;
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
        }
        protected void InitiatePaymentClicked(object sender, EventArgs e)
        {
            decimal inputAmount = decimal.Parse(Initiate_Payment_amount.Text);
            string InputPaymentMethod = Initiate_Payment_method.Text;
            int InputPlanID = int.Parse(Initiate_planid.Text);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                if (InputPaymentMethod != "cash" && InputPaymentMethod != "credit")
                    outputText.InnerText = "The payment method can only be cash or credit.";
                else
                {
                    try
                    {
                        string storedProcedure = "Initiate_plan_payment";

                        using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                        {
                            // Specify that this command is a stored procedure
                            command.CommandType = CommandType.StoredProcedure;

                            // Add parameters to the stored procedure
                            command.Parameters.AddWithValue("@mobile_num", mobileNo);
                            command.Parameters.AddWithValue("@amount", inputAmount);
                            command.Parameters.AddWithValue("@payment_method", InputPaymentMethod);
                            command.Parameters.AddWithValue("@plan_id", InputPlanID);

                            SqlDataAdapter adapter = new SqlDataAdapter(command);
                            DataTable dataTable = new DataTable();
                            connection.Open();
                            adapter.Fill(dataTable);
                            outputText.InnerText = "Payment initiated successfully!";
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        outputText.InnerText = "Error: " + ex.Message;
                    }
                }
            }
        }





        protected void PaymentWalletCashback(object sender, EventArgs e)
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
            paymentwalletcashback.Visible = true;
            string InputMobileNo = Payment_wallet_cashback_Mobile_No.Text;
            string InputPaymentID = Payment_wallet_cashback_Payment_id.Text;
            string InputBenefitID = Payment_wallet_cashback_Benefit_ID.Text;


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                    string storedProcedure = "Payment_wallet_cashback";

                    // Create SqlCommand object to execute the stored procedure
                    using (SqlCommand command = new SqlCommand(storedProcedure, connection))

                    {
                        // Specify that this command is a stored procedure
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@mobile_num", InputMobileNo);
                        command.Parameters.AddWithValue("@payment_id", InputPaymentID);
                        command.Parameters.AddWithValue("@benefit_id", InputBenefitID);

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
        protected void RechargeBalance(object sender, EventArgs e)
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
            Initiate_balance_payment.Visible = true;
            string InputMobileNo = Initiate_balance_payment_MobileNo.Text;
            string InputAmount = Initiate_balance_payment_amount.Text;
            string InputPaymentMethod = Initiate_balance_payment_paymentMethod.Text;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
               
                    string storedProcedure = "Payment_wallet_cashback";

                    // Create SqlCommand object to execute the stored procedure
                    using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                    {
                        // Specify that this command is a stored procedure
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@mobile_num", InputMobileNo);
                        command.Parameters.AddWithValue("@amount", InputAmount);
                        command.Parameters.AddWithValue("@payment_method", InputPaymentMethod);

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

        protected void RedeemVoucher(object sender, EventArgs e)
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
            Redeem_voucher.Visible = true;
            string InputMobileNo = Redeem_voucher_MobileNo.Text;
            string InputVoucher = Redeem_voucher_voucherID.Text;
            string InputPaymentMethod = Initiate_balance_payment_paymentMethod.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string storedProcedure = " Redeem_voucher_points";

                // Create SqlCommand object to execute the stored procedure
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    // Specify that this command is a stored procedure
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@mobile_num", InputMobileNo);
                    command.Parameters.AddWithValue("@voucherID", InputVoucher);

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

        private bool ExistsInDatabase(string columnName, string value, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"SELECT COUNT(*) FROM {tableName} WHERE {columnName} = @value";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@value", value);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0; // If the count is greater than 0, the value exists
                }
            }
        }

    }
    } 


   