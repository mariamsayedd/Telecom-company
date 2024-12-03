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
                    break;
                case 9:
                    break;
                case 10:
                    break;
                case 11:
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
        protected void callView(String query)
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            outputText.InnerText = "";
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
    }
}