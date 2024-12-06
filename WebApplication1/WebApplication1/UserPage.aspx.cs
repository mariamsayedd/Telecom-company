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
            mobileNo = Request.QueryString["mobileNo"];
            if (mobileNo == null)
            {
                bodyTag.InnerHtml = "<h1 class=\"mb-3\">ACCESS FORBIDDEN!!!!!</h1>";
            }
            else
            {
                string query = "SELECT nationalID FROM customer_account WHERE mobileNo = " + mobileNo;
                SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                int nationalID = int.Parse(dataTable.Rows[0][0].ToString());
                query = "SELECT first_name FROM customer_profile WHERE nationalID = " + nationalID;
                adapter = new SqlDataAdapter(query, connectionString);
                dataTable = new DataTable();
                adapter.Fill(dataTable);
                String name = dataTable.Rows[0][0].ToString();
                Response.Write($"Hello {name}");

            }

            //mobileNo = "01234567890";
            if (dropDown.SelectedValue != "2")
                getconsume.Visible = false;
            if (dropDown.SelectedValue != "9")
                remainingBalance.Visible = false;
            if (dropDown.SelectedValue != "10")
                extraAmount.Visible = false;
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
                    query = "SELECT * FROM allShops";
                    callView(query);
                    break;
                case 13:
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


        protected void getConsumption()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
            getconsume.Visible = true;
        }

        protected void getConsumptionClicked(object sender, EventArgs e)
        {

            string query = "SELECT * FROM dbo.Consumption(@Plan_name, @start_date, @end_date)";
            String startDate = consumption2.Text;
            String endDate = consumption3.Text;
            String serviceName = consumption1.Text;

            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(serviceName))
                ShowAlert("Please fill in the required fields", "warning");
            else
            {
                if ((!ExistsInDatabaseString("name", serviceName, "Service_plan")))
                    ShowAlert("The specified plan name does not exist.Please enter a valid name", "warning");

                else
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Plan_name", serviceName);
                    command.Parameters.AddWithValue("@start_date", startDate);
                    command.Parameters.AddWithValue("@end_date", endDate);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    GridView gv = new GridView();
                    GridView1.DataSource = dataTable;
                    GridView1.DataBind();
                    if (dataTable.Rows.Count > 0)
                    {
                        GridView1.DataSource = dataTable;
                        GridView1.DataBind();
                    }
                    else
                    {
                        ShowAlert("No consumption within specified duration", "warning");
                    }

                }
            }
        }

        protected void getOfferedPlans()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedure = "Unsubscribed_Plans";

                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@mobile_num", mobileNo);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    connection.Open();
                    adapter.Fill(dataTable);
                    GridView1.DataSource = dataTable;
                    GridView1.DataBind();
                    if (dataTable.Rows.Count > 0)
                    {
                        GridView1.DataSource = dataTable;
                        GridView1.DataBind();
                    }
                    else
                    {
                        ShowAlert("No unsubscribed plans", "warning");
                    }

                }
            }
        }

        protected void getUsagePlan()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
            string query = "SELECT * FROM dbo.Usage_Plan_CurrentMonth(@MobileNo)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@MobileNo", mobileNo);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            GridView gv = new GridView();
            GridView1.DataSource = dataTable;
            GridView1.DataBind();
            if (dataTable.Rows.Count > 0)
            {
                GridView1.DataSource = dataTable;
                GridView1.DataBind();
            }
            else
            {
                ShowAlert("No active plans in the current month", "warning");
            }
        }
        protected void getTransactions()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
            string query = "SELECT nationalID FROM customer_account WHERE mobileNo = " + mobileNo;
            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            int nationalID = int.Parse(dataTable.Rows[0][0].ToString());

            query = "SELECT * FROM dbo.Cashback_Wallet_Customer(@NationalID)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalID", nationalID);

            adapter = new SqlDataAdapter(command);
            dataTable = new DataTable();
            adapter.Fill(dataTable);
            GridView gv = new GridView();
            GridView1.DataSource = dataTable;
            GridView1.DataBind();
            if (dataTable.Rows.Count > 0)
            {
                GridView1.DataSource = dataTable;
                GridView1.DataBind();
            }
            else
            {
               ShowAlert("No cashback transactions for input wallet", "warning");
            }
        }

        protected void callView(String query)
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            GridView1.DataSource = dataTable;
            GridView1.DataBind();
            if (dataTable.Rows.Count > 0)
            {
                GridView1.DataSource = dataTable;
                GridView1.DataBind();
            }
            else
            {
                ShowAlert("No records in the table yet.", "warning");  }
        }
        protected void getUnresolvedTickets()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
            string query = "SELECT nationalID FROM customer_account WHERE mobileNo = " + mobileNo;
            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            int nationalID = int.Parse(dataTable.Rows[0][0].ToString());


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedure = "Ticket_Account_Customer";
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@NID", nationalID);

                    SqlDataAdapter adapter1 = new SqlDataAdapter(command);

                    DataTable dataTable1 = new DataTable();

                    try
                    {
                        connection.Open();
                        adapter1.Fill(dataTable1);
                        int count = int.Parse(dataTable1.Rows[0][0].ToString());
                        ShowAlert( "Number of unresolved tickets: " + count, "success");
                    }
                    catch (Exception ex)
                    {
                        ShowAlert("Error: " + ex.Message, "warning");
                    }
                }
            }
        }
        protected void getHighestVoucher()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";

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
                        ShowAlert("Error: " + ex.Message, "warning");
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
        }
        protected void getRemainingBalanceClicked(object sender, EventArgs e)
        {
            String serviceName = servicePlanRem.Text;
            if (string.IsNullOrEmpty(serviceName))
                ShowAlert("Please fill in the required field", "warning");
            else if ((!ExistsInDatabaseString("name", serviceName, "Service_plan")))
                ShowAlert("The specified plan name does not exist.Please enter a valid name","warning");
            else
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "SELECT dbo.Remaining_plan_amount(@mobile_num, @plan_name)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@mobile_num", mobileNo);
                            command.Parameters.AddWithValue("@plan_name", serviceName);

                            object result = command.ExecuteScalar();

                            if (result != null && result != DBNull.Value)
                            {
                                int remainingAmount = Convert.ToInt32(result);
                                ShowAlert($"Remaining plan amount: {remainingAmount}", "success");
                            }
                            else
                            {
                                ShowAlert("No result for the given plan name.", "warning");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowAlert($"Error: {ex.Message}", "warning");
                }
            }
        }

        protected void getExtraAmount()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
            extraAmount.Visible = true;
        }
        protected void getExtraAmountClicked(object sender, EventArgs e)
        {
            String serviceName = servicePlanExtra.Text;
            if (string.IsNullOrEmpty(serviceName))
                ShowAlert("Please fill in the required field", "warning");
            else if ((!ExistsInDatabaseString("name", serviceName, "Service_plan")))
                ShowAlert("The specified plan name does not exist.Please enter a valid name", "warning");
            else
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "SELECT dbo.Extra_plan_amount(@mobile_num, @plan_name)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@mobile_num", mobileNo);
                            command.Parameters.AddWithValue("@plan_name", serviceName);

                            object result = command.ExecuteScalar();

                            if (result != null && result != DBNull.Value)
                            {
                                int extraAmount = Convert.ToInt32(result);
                                ShowAlert($"Extra plan amount: {extraAmount}", "success");
                            }
                            else
                            {
                                ShowAlert("No result for the given plan name.", "warning")                  }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowAlert($"Error: {ex.Message}", "warning");
                }
            }
        }


        protected void getTop10()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedure = "Top_Successful_Payments";

                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
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
                        if (dataTable.Rows.Count > 0)
                        {
                            GridView1.DataSource = dataTable;
                            GridView1.DataBind();
                        }
                        else
                        {
                            ShowAlert("No payments on this account yet.", "warning");              }


                    }
                    catch (Exception ex)
                    {
                        ShowAlert("Error: " + ex.Message, "warning");
                    }
                }
            }
        }
        protected void getSubscribedPlansFiveMonths(object sender, EventArgs e)
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
            string query = "SELECT * FROM dbo.Subscribed_plans_5_Months(@mobile_num)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@mobile_num", mobileNo);
                    command.CommandType = CommandType.Text;

                    try
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count > 0)
                        {
                            GridView1.DataSource = dataTable;
                            GridView1.DataBind();
                        }
                        else
                        {
                            ShowAlert("No subscribed plans in the past 5 months", "warning");
                        }

                    }
                    catch (Exception ex)
                    {
                        ShowAlert("Error: " + ex.Message, "warning");
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
            string inputAmountText = Initiate_Payment_amount.Text;
            string InputPaymentMethod = Initiate_Payment_method.Text;
            string InputPlanIDText = Initiate_planid.Text;


            if (string.IsNullOrEmpty(inputAmountText) || string.IsNullOrEmpty(InputPaymentMethod) || string.IsNullOrEmpty(InputPlanIDText))
                ShowAlert("Please fill in all required fields", "warning");
            else
            {
                decimal inputAmount;
                int InputPlanID;
                try
                {
                    inputAmount = decimal.Parse(inputAmountText);
                }
                catch (FormatException)
                {
                    ShowAlert("Please enter a valid amount with at most 1 decimal place", "warning");
                    return;
                }

                try
                {
                    InputPlanID = int.Parse(InputPlanIDText);
                }
                catch (FormatException)
                {
                    ShowAlert("Please enter a valid plan ID.", "warning");
                    return;
                }

                if (InputPaymentMethod != "cash" && InputPaymentMethod != "credit")
                    ShowAlert("The payment method can only be cash or credit.", "warning");
                else if ((!ExistsInDatabaseInt("planID", InputPlanID, "Service_plan")))
                    ShowAlert("The specified plan ID does not exist.Please enter a valid ID", "warning");
                else
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            string storedProcedure = "Initiate_plan_payment";

                            using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;

                                command.Parameters.AddWithValue("@mobile_num", mobileNo);
                                command.Parameters.AddWithValue("@amount", inputAmount);
                                command.Parameters.AddWithValue("@payment_method", InputPaymentMethod);
                                command.Parameters.AddWithValue("@plan_id", InputPlanID);

                                SqlDataAdapter adapter = new SqlDataAdapter(command);
                                DataTable dataTable = new DataTable();
                                connection.Open();
                                adapter.Fill(dataTable);
                                ShowAlert("Subscription has been successfully renewed!", "success");

                            }
                        }
                        catch (Exception ex)
                        {
                            ShowAlert("Error: " + ex.Message, "warning");
                        }
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

        }
        protected void PaymentWalletCashbackClicked(object sender, EventArgs e)
        {

            string InputPaymentIDText = Payment_wallet_cashback_Payment_id.Text;
            string InputBenefitIDText = Payment_wallet_cashback_Benefit_ID.Text;

            if (string.IsNullOrEmpty(InputPaymentIDText) || string.IsNullOrEmpty(InputBenefitIDText))
                ShowAlert("Please fill in all required fields", "warning");
            else
            {

                int InputPaymentID;
                int InputBenefitID;
                try
                {
                    InputPaymentID = int.Parse(InputPaymentIDText);
                }
                catch (FormatException)
                {
                    ShowAlert("Please enter a valid payment ID", "warning");
                    return;
                }
                try
                {
                    InputBenefitID = int.Parse(InputBenefitIDText);
                }
                catch (FormatException)
                {
                    ShowAlert("Please enter a valid benefit ID.", "warning");
                    return;
                }

                if ((!ExistsInDatabaseInt("paymentID", InputPaymentID, "Payment")))
                    ShowAlert("The specified payment ID does not exist.Please enter a valid ID", "warning");
                else
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {

                        string storedProcedure = "Payment_wallet_cashback";

                        using (SqlCommand command = new SqlCommand(storedProcedure, connection))

                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@mobile_num", mobileNo);
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
                                ShowAlert("Cashback calculated and wallet's balance updated successfully!", "success");
                            }
                            catch (Exception ex)
                            {
                                ShowAlert("Error: " + ex.Message, "warning");
                            }
                        }
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


        }

        protected void RechargeBalanceClicked(object sender, EventArgs e)
        {
            string InputAmountText = Initiate_balance_payment_amount.Text;
            string InputPaymentMethod = Initiate_balance_payment_paymentMethod.Text;


            if (string.IsNullOrEmpty(InputAmountText) || string.IsNullOrEmpty(InputPaymentMethod))
                ShowAlert("Please fill in all required fields", "warning");
            else
            {
                decimal InputAmount;
                try
                {
                    InputAmount = decimal.Parse(InputAmountText);
                }
                catch (FormatException)
                {
                    ShowAlert("Please enter a valid amount.", "warning");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (InputPaymentMethod != "cash" && InputPaymentMethod != "credit")
                        ShowAlert("The payment method can only be cash or credit.", "warning");
                    else
                    {
                        string storedProcedure = "Initiate_balance_payment";

                        using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@mobile_num", mobileNo);
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
                                ShowAlert("Balance recharged successfully!", "success");
                            }
                            catch (Exception ex)
                            {
                                ShowAlert("Error: " + ex.Message, "warning");
                            }
                        }

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
        }


        protected void RedeemVoucherClicked(object sender, EventArgs a)
        {
            string InputVoucherText = Redeem_voucher_voucherID.Text;

            if (string.IsNullOrEmpty(InputVoucherText))
                ShowAlert("Please fill in the required field", "Wwarning");
            else
            {
                int InputVoucher;
                try
                {
                    InputVoucher = int.Parse(InputVoucherText);
                }
                catch (FormatException)
                {
                    ShowAlert("Please enter a valid voucher ID", "warning");
                    return;
                }

                if (!ExistsInDatabaseInt("voucherID", InputVoucher, "Voucher"))
                    ShowAlert("Please enter a valid voucher ID", "warning");
                else
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {

                        string storedProcedure = "Redeem_voucher_points";

                        using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@mobile_num", mobileNo);
                            command.Parameters.AddWithValue("@voucher_id", InputVoucher);

                            SqlDataAdapter adapter = new SqlDataAdapter(command);

                            DataTable dataTable = new DataTable();

                            try
                            {
                                connection.Open();
                                adapter.Fill(dataTable);
                                GridView1.DataSource = dataTable;
                                GridView1.DataBind();
                                ShowAlert("Voucher redeemed successfully!", "success");
                            }
                            catch (Exception ex)
                            {
                                ShowAlert("Error: " + ex.Message, "warning");
                            }
                        }
                    }
                }

            }
        }

        private bool ExistsInDatabaseString(string columnName, string value, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM " + tableName + " WHERE " + columnName + " = @value";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@value", value);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }

            }
        }
        private bool ExistsInDatabaseInt(string columnName, int value, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM " + tableName + " WHERE " + columnName + " = @value";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@value", value);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }


            }
        }
        protected void signOut(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
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




