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
                outputText.InnerText = "Please fill in the required fields";
            else
            {
                if ((!ExistsInDatabaseString("name", serviceName, "Service_plan")))
                    outputText.InnerText = "The specified plan name does not exist.Please enter a valid name";
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
                        outputText.InnerText = "No consumption within specified duration";
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
                        outputText.InnerText = "No unsubscribed plans";
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
                outputText.InnerText = "No active plans in the current month";
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
                outputText.InnerText = "No cashback transactions for input wallet";
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
                outputText.InnerText = "No records in the table yet.";
            }
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
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedure = "Account_Highest_Voucher";

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
        }
        protected void getRemainingBalanceClicked(object sender, EventArgs e)
        {
            String serviceName = servicePlanRem.Text;
            if (string.IsNullOrEmpty(serviceName))
                outputText.InnerText = "Please fill in the required field";
            else if ((!ExistsInDatabaseString("name", serviceName, "Service_plan")))
                outputText.InnerText = "The specified plan name does not exist.Please enter a valid name";
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
                outputText.InnerText = "Please fill in the required field";
            else if ((!ExistsInDatabaseString("name", serviceName, "Service_plan")))
                outputText.InnerText = "The specified plan name does not exist.Please enter a valid name";
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
                            outputText.InnerText = "No payments on this account yet.";
                        }


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
                            outputText.InnerText = "No subscribed plans in the past 5 months";
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
            string inputAmountText = Initiate_Payment_amount.Text;
            string InputPaymentMethod = Initiate_Payment_method.Text;
            string InputPlanIDText = Initiate_planid.Text;


            if (string.IsNullOrEmpty(inputAmountText) || string.IsNullOrEmpty(InputPaymentMethod) || string.IsNullOrEmpty(InputPlanIDText))
                outputText.InnerText = "Please fill in all required fields";
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
                    outputText.InnerText = "Please enter a valid amount with at most 1 decimal place";
                    return;
                }

                try
                {
                    InputPlanID = int.Parse(InputPlanIDText);
                }
                catch (FormatException)
                {
                    outputText.InnerText = "Please enter a valid plan ID.";
                    return;
                }

                if (InputPaymentMethod != "cash" && InputPaymentMethod != "credit")
                    outputText.InnerText = "The payment method can only be cash or credit.";
                else if ((!ExistsInDatabaseInt("planID", InputPlanID, "Service_plan")))
                    outputText.InnerText = "The specified plan ID does not exist.Please enter a valid ID";
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
                                outputText.InnerText = "Subscription has been successfully renewed!";

                            }
                        }
                        catch (Exception ex)
                        {
                            outputText.InnerText = "Error: " + ex.Message;
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
                outputText.InnerText = "Please fill in all required fields";
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
                    outputText.InnerText = "Please enter a valid payment ID";
                    return;
                }
                try
                {
                    InputBenefitID = int.Parse(InputBenefitIDText);
                }
                catch (FormatException)
                {
                    outputText.InnerText = "Please enter a valid benefit ID.";
                    return;
                }

                if ((!ExistsInDatabaseInt("paymentID", InputPaymentID, "Payment")))
                    outputText.InnerText = "The specified payment ID does not exist.Please enter a valid ID";
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
                                outputText.InnerText = "Cashback calculated and wallet's balance updated successfully!";
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
                outputText.InnerText = "Please fill in all required fields";
            else
            {
                decimal InputAmount;
                try
                {
                    InputAmount = decimal.Parse(InputAmountText);
                }
                catch (FormatException)
                {
                    outputText.InnerText = "Please enter a valid amount.";
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (InputPaymentMethod != "cash" && InputPaymentMethod != "credit")
                        outputText.InnerText = "The payment method can only be cash or credit.";
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
                                outputText.InnerText = "Balance recharged successfully!";

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
                outputText.InnerText = "Please fill in the required field";
            else
            {
                int InputVoucher;
                try
                {
                    InputVoucher = int.Parse(InputVoucherText);
                }
                catch (FormatException)
                {
                    outputText.InnerText = "Please enter a valid voucher ID";
                    return;
                }

                if (!ExistsInDatabaseInt("voucherID", InputVoucher, "Voucher"))
                    outputText.InnerText = "Please enter a valid voucher ID";
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
                                outputText.InnerText = "Voucher redeemed successfully!";
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
    }
}




