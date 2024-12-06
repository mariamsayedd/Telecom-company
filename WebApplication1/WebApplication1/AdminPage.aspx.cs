
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
                case 5:
                    query = "SELECT * from CustomerWallet";
                    break;
                case 6:
                    query = "SELECT * from E_shopVouchers";
                    break;
                case 7:
                    query = "SELECT * from AccountPayments";
                    break;
                case 8:
                    query = "SELECT * from Num_of_cashback";
                    break;
            }


            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            if (dataTable.Rows.Count > 0)
            {
                GridView1.DataSource = dataTable;
                GridView1.DataBind();
            }
            else
            {
                ShowAlert("No records Found", "warning");
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

        protected void smsOffers_2_3a(object sender, EventArgs e)   
        {

            string query = "SELECT * FROM dbo.Account_SMS_Offers(@MobileNumber)";
            String inputMobileNumber = smsMobileNumber.Text;
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@MobileNumber", inputMobileNumber);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            GridView gv = new GridView();
   
            if (dataTable.Rows.Count > 0)
            {
                GridView2.DataSource = dataTable;
                GridView2.DataBind();
            }
            else
            {
                ShowAlert("No records found", "warning");
            }
        }

        

        protected void get_Accepted_Payment_Trans(object sender, EventArgs e)
        {
            string query = "SELECT COUNT(1) FROM Payment WHERE mobileNo = @Input";
            string inputMobileNumber = Accepted_Payment_Transactions_Mobile_Number.Text;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Input", inputMobileNumber);

                    connection.Open();

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count > 0)
                    {
                        string query2 = "exec  Account_Payment_Points @MobileNumber";

                        SqlCommand command2 = new SqlCommand(query2, connection);
                        command2.Parameters.AddWithValue("@MobileNumber", inputMobileNumber);

                        try
                        {
                            SqlDataAdapter adapter = new SqlDataAdapter(command2);
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            int acceptedPayment_count = int.Parse(dataTable.Rows[0][0].ToString());
                            outputText.InnerText = "Accepted payment :" + acceptedPayment_count;
                        }
                        catch (Exception ex)
                        {
                            ShowAlert($"An error occured , Please enter a valid mobile number", "none");
                        }

                    }
                    else
                    {
                        ShowAlert($"An error occured , Please enter a valid mobile number", "none");
                    }
                }
            }








        }

        protected void show_cashback_amount(object sender, EventArgs e)
        {

            string walletId = TextBox_WalletID.Text;
            string planID = TextBox_PlanID.Text;
            string query = "SELECT COUNT(*) FROM Cashback as c , plan_provides_benefits as pb WHERE c.walletID = @WalletId and pb.planID = @planID";
            string inputMobileNumber = Accepted_Payment_Transactions_Mobile_Number.Text;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@WalletID", walletId);
                    command.Parameters.AddWithValue("@planID", planID);

                    connection.Open();

                    try
                    {
                        int count = Convert.ToInt32(command.ExecuteScalar());


                        if (count > 0)
                        {

                            string query2 = "select dbo.Wallet_Cashback_Amount(@WalletId,@planId)";
                            SqlCommand command2 = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@WalletId ", walletId);
                            command.Parameters.AddWithValue("@planId", planID);
                            try
                            {
                                SqlDataAdapter adapter = new SqlDataAdapter(command);
                                DataTable dataTable = new DataTable();
                                adapter.Fill(dataTable);
                                int cashback_amount = int.Parse(dataTable.Rows[0][0].ToString());
                                cashback_H1.InnerText = "cashback amount : " + cashback_amount;
                            }
                            catch (Exception ex)
                            {
                                ShowAlert($"An error occured , Please enter a valid WalletID or PlanID", "none");
                            }

                        }
                        else
                        {
                            ShowAlert($"An error occured , Please enter a valid WalletID or PlanID", "none");
                        }
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        ShowAlert($"An error occured , Please enter a valid WalletID or PlanID", "none");
                    }
                }
            }
        }

        protected void getAvgSentTrans(object sender, EventArgs e)
        {
            string query = "select dbo.Wallet_Transfer_Amount(@WalletId,@start_date,@end_date)";
            string startDate = start_date_avgSentTrans.Text;
            string endDate = end_date_avgSentTrans.Text;
            string walletId = TextBox_WalletID_avgSentTrans.Text;
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@WalletId ", walletId);
            command.Parameters.AddWithValue("@start_date", startDate);
            command.Parameters.AddWithValue("@end_date", endDate);

            try
            {
                if (ExistsInDatabaseString("transfer_money", startDate, "transfer_date")
                && ExistsInDatabaseString("transfer_money", endDate, "transfer_date")
                && ExistsInDatabaseInt("transfer_money", Int32.Parse(walletId), "walletID1"))
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        int avgSentTrans = int.Parse(dataTable.Rows[0][0].ToString());
                        avg_Sent_Trans_h3.InnerText = "Average Sent Transaction : " + avgSentTrans;
                    }
                    catch (Exception ex)
                    {
                        ShowAlert($"An error occured , start date or end date or WalletID might be not valid", "none");
                    }

                }
                else
                {
                    ShowAlert($"An error occured , start date or end date or WalletID might be not valid", "none");
                }

            }
            catch (Exception ex)
            {
                ShowAlert($"An error occured , start date or end date or WalletID might be not valid", "none");
            }
        }

        protected void isMobileLinked(object sender, EventArgs e)
        {
            string query = "select dbo.Wallet_MobileNo(@MobileNo)";
            string mobilenumber_input = MobileNumber_isLinked.Text;
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@MobileNo", mobilenumber_input);
            try
            {
                if (!ExistsInDatabaseString("mobileNo", mobilenumber_input, "Wallet"))
                {
                    ShowAlert($"An error occured , Please enter a valid mobile number", "none");
                }
                else
                {

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    bool bit = bool.Parse(dataTable.Rows[0][0].ToString());
                    if (bit != null && bit == true)
                    {
                        isLinked.InnerText = "it is Linked to wallet";
                    }
                    else
                    {
                        isLinked.InnerText = "not linked";
                    }
                }


            }
            catch (Exception ex)
            {
                ShowAlert($"An error occured , Please enter a valid mobile number", "none");
            }


        }

        protected void updatePoints(object sender, EventArgs e)
        {

            string query = "exec Total_Points_Account @MobileNo";
            string mobileNo = MobileNo_update.Text;
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@MobileNo", mobileNo);

            try
            {
                if (ExistsInDatabaseString("mobileNo", mobileNo, "Payment"))
                    H1.InnerText = "points for " + mobileNo + " updated successfuly";
                else
                    ShowAlert($"An error occured , Please enter a valid mobile number , points did not update", "none");
            }
            catch (Exception ex)
            {
                ShowAlert($"An error occured , Please enter a valid mobile number , points did not update", "none");
            }
        }

        protected void deletingBenefits_2_3d(object sender, EventArgs e)   
        {
            string query = "exec Benefits_Account @MobileNumber, @PlanID";
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
                SqlCommand command2 = new SqlCommand("Select * from Benefits", connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command2);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                GridView gv = new GridView();
                if (dataTable.Rows.Count > 0)
                {
                    deletion_gridview.DataSource = dataTable;
                    deletion_gridview.DataBind();
                }
                else
                {
                    ShowAlert("No records found", "warning");
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"An error occured {ex.Message}", "none");
            }
            finally
            {
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
            else
            {
                string alertHtml = $@"
                 <div class='alert alert-{alertType} alert-dismissible fade show' role='alert'>
                     <strong>{(alertType == "success" ? "Success!" : "Warning!")}</strong> {message}
                     <button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>
                 </div>";

                AlertPlaceholder.Text = alertHtml;
            }
        }



        protected void Account_Plan_date_2_3_b(object sender, EventArgs e)
        {   string query = "select * from dbo.Account_Plan_date (@sub_date, @planId)";
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
                if (dataTable.Rows.Count > 0)
                {
                    Account_Plan_date_Grid_View.DataSource = dataTable;
                    Account_Plan_date_Grid_View.DataBind();
                }
                else
                {
                    ShowAlert("No records found", "warning");
                }

            }
            catch (Exception ex)
            {
                ShowAlert($"An error occured {ex.Message}", "none");
            }

        }


        protected void Account_Usage_Plan_2_3_c(object sender, EventArgs e)
        {    
            string query = "select * from dbo.Account_Usage_Plan(@mobileNum,@start_date )";
            String inputStartDate = Account_Usage_Plan_date.Text;
            String inputMobileNum = Account_Usage_Plan_mobileNum.Text;
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
                if (dataTable.Rows.Count > 0)
                {
                    Account_Usage_Plan_GridView.DataSource = dataTable;
                    Account_Usage_Plan_GridView.DataBind();
                }
                else
                {
                    ShowAlert("No records found", "warning");
                }

            }
            catch (Exception ex)
            {
                ShowAlert($"An error occured {ex.Message}", "none");
            }

        }

        protected void signOut(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }

}