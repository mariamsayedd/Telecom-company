<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" Inherits="WebApplication1.AdminPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Page</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
       <style>
        body {
            background: url('https://source.unsplash.com/1600x900/?abstract,technology') no-repeat center center fixed;
            background-size: cover;
            color: #333;
        }
        .dark-theme {
            background: #121212 !important;
            color: #f1f1f1;
        }
        .dark-theme .card {
            background: #1f1f1f;
            color: #f1f1f1;
        }
           h2 {
           color: black;
           }
        #themeToggle {
            position: fixed; 
            top: 10px; 
            right: 10px; 
            width: 50px; 
            height: 50px; 
            padding: 0;
            z-index: 1000;
        }

        #themeToggle img {
            width: 30px;
            height: 30px;
        }
         .centered-div {
            background-color: white;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
            padding: 30px;
            width: 100%;
            max-width: 500px;
            margin: 100px auto; 
        }
       .alert-container {
        position: fixed;
        top: 25%;
        left: 50%;
        transform: translate(-50%, -50%);
        z-index: 1050;
        width: 50%; 
        max-width: 500px; 
        padding: 1rem;
        }
       
        .alert-container.show {
        display: block; 
    }
    </style>
</head>



<body class="bg-light">

    
    <button type="button" id="themeToggle" class="btn btn-outline-secondary">
        <img src="dark-theme.svg" alt="My Happy SVG" />
    </button>    
    <form runat="server">
                <asp:Button ID="SignOut" runat="server" OnClick="signOut" type="button" text="Sign out" class="btn btn-primary" />

        <div class="container">
    <div class="centered-div text-center">
        <h2 class="mb-3">Admin Viewing Options</h2>
        <div class="mb-3" >
           <asp:DropDownList ID="dropDown"  runat="server" CssClass="form-select">
                <asp:ListItem Value="0" Selected="True">Choose from the following:</asp:ListItem>
                <asp:ListItem Value="1">View details for all customer profiles along with their active accounts.</asp:ListItem> <%--  2.2 a takes no input --%>
                <asp:ListItem Value="2">View the list of all physical stores along with their redeemed voucher’s ids and values.</asp:ListItem> <%--  2.2 i takes no input --%>
                <asp:ListItem Value="3">View details for all resolved tickets.</asp:ListItem> <%--  2.2 f takes no input --%>
                <asp:ListItem Value="4">View all customers’ accounts along with their subscribed service plans.</asp:ListItem>
                <asp:ListItem Value="5"> View details of all wallets along with their customer names. </asp:ListItem> <%--  2.3 a  takes no input --%>
                <asp:ListItem Value="6"> View the list of all E-shops along with their redeemed voucher’s ids and values. </asp:ListItem> <%--  2.3 a  takes no input --%>
                <asp:ListItem Value="7"> View all payments’ transaction details along with their initiated Accounts. </asp:ListItem> <%--  2.3 a  takes no input --%>
                <asp:ListItem Value="8"> View the total number of cashback transactions per each wallet ID.</asp:ListItem> <%--  2.3 a  takes no input --%>
             
           </asp:DropDownList>

        </div>
        <asp:Button ID="Button1" runat="server" OnClick="list" type="button" text="Submit" class="btn btn-primary" />
    </div>
</div>



         <div class="container mt-5">
            <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="true">
            </asp:GridView>
        </div>

    



        <!-- Retriving SMS offers-->

  <div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-4">
            <div class="card shadow">
                <div class="card-header text-center bg-primary text-white">
                    <h5>Retrieve the list of gained offers of type ‘SMS’ for input account.</h5>
                </div>
                <div class="card-body d-grid">
                        <div class="mb-3">
                            <%-- <asp:Label runat="server" Text="Enter Mobile Number" CssClass="form-label"></asp:Label> --%>
                            <asp:TextBox ID="smsMobileNumber" runat="server" CssClass="form-control" placeholder="Enter mobile number "></asp:TextBox>
                        </div>
                        <div class="d-grid col-md-3 mx-auto ">
                            <asp:Button ID="smsOffersButton" runat="server" OnClick="smsOffers_2_3a" Text="Query" CssClass="btn btn-primary btn-sm" />
                        </div>
                </div>
            </div>
        </div>
    </div>
</div>

            
        <div class="container mt-5">
            <asp:GridView ID="GridView2" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="true">
            </asp:GridView>
        </div>





        <!-- deleting -->
        
  <div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-4">
            <div class="card shadow">
                <div class="card-header text-center bg-primary text-white">
                    <h5>Remove all benefits offered to the input account and plan ID.</h5>
                </div>
                <div class="card-body d-grid">
                        <div class="mb-3">
                            <%-- <asp:Label runat="server" Text="Enter Mobile Number" CssClass="form-label"></asp:Label> --%>
                            <asp:TextBox ID="deleteMobileNum" runat="server" CssClass="form-control" placeholder="Enter mobile number "></asp:TextBox>
                        </div>
                    <div class="mb-1">
                        <%-- <asp:Label runat="server" Text="Enter Mobile Number" CssClass="form-label"></asp:Label> --%>
                        <asp:TextBox ID="deletePlanID" runat="server" CssClass="form-control" placeholder="Enter Plan ID "></asp:TextBox>
                    </div>
                        <div class="d-grid col-md-3 mx-auto">
                            <asp:Button ID="Button2" runat="server" OnClick="deletingBenefits_2_3d" Text="Delete" CssClass="btn btn-danger btn-sm" />
                        </div>
                </div>
            </div>
        </div>
    </div>
</div>
        
        
        <div class="container mt-5">
            <asp:GridView ID="deletion_gridview" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="true">
            </asp:GridView>
        </div>


            <div class="alert-container mt-3">
            <!-- Alert Placeholder -->
            <asp:Literal ID="AlertPlaceholder" runat="server"></asp:Literal>
        </div>




        <!-- Account_Plan_date -->

          <div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-4">
            <div class="card shadow">
                <div class="card-header text-center bg-primary text-white">
                    <h5>Retrieve the list of accounts subscribed to the input plan on a certain date.</h5>
                </div>
                <div class="card-body d-grid">
                        <div class="mb-3">
                            <%-- <asp:Label runat="server" Text="Enter Mobile Number" CssClass="form-label"></asp:Label> --%>
                            <asp:TextBox ID="Account_Plan_date_subdate" runat="server" CssClass="form-control" placeholder="Enter mobile number " TextMode="Date"></asp:TextBox>
                        </div>
                    <div class="mb-1">
                        <%-- <asp:Label runat="server" Text="Enter Mobile Number" CssClass="form-label"></asp:Label> --%>
                        <asp:TextBox ID="Account_Plan_date_plan_id" runat="server" CssClass="form-control" placeholder="Enter Plan ID "></asp:TextBox>
                    </div>
                        <div class="d-grid col-md-3 mx-auto">
                            <asp:Button ID="Account_Plan_date_submit" runat="server" OnClick="Account_Plan_date_2_3_b" Text="Submit" CssClass="btn btn-primary btn-sm" />
                        </div>
                </div>
            </div>
        </div>
    </div>
</div>


     <div class="container mt-5">
        <asp:GridView ID="Account_Plan_date_Grid_View" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="true">
        </asp:GridView>
    </div>


        <!-- Account_Usage_Plan -->

        
          <div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-4">
            <div class="card shadow">
                <div class="card-header text-center bg-primary text-white">
                    <h5>Retrieve the list of accounts subscribed to the input plan on a certain date.</h5>
                </div>
                <div class="card-body d-grid">
                        <div class="mb-3">
                            <%-- <asp:Label runat="server" Text="Enter Mobile Number" CssClass="form-label"></asp:Label> --%>
                            <asp:TextBox ID="Account_Usage_Plan_mobileNum" runat="server" CssClass="form-control" placeholder="Enter mobile number " ></asp:TextBox>
                        </div>
                    <div class="mb-1">
                        <asp:TextBox ID="Account_Usage_Plan_date" runat="server" CssClass="form-control" placeholder="Enter Plan ID" TextMode="Date"></asp:TextBox>
                    </div>
                        <div class="d-grid col-md-3 mx-auto">
                            <asp:Button ID="Account_Usage_Plan_Button" runat="server" OnClick="Account_Usage_Plan_2_3_c" Text="Submit" CssClass="btn btn-primary btn-sm" />
                        </div>
                </div>
            </div>
        </div>
    </div>
</div>


     <div class="container mt-5">
        <asp:GridView ID="Account_Usage_Plan_GridView" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="true">
        </asp:GridView>
    </div>


                <!--  Show the number of accepted payment transactions initiated by the input account 
during the last year along with the total amount of earned points. -->

  <div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-4">
            <div class="card shadow">
                <div class="card-header text-center bg-primary text-white">
                    <h5> Show the number of accepted payment transactions initiated by the input account 
during the last year along with the total amount of earned points.</h5>
                </div>
                <div class="card-body d-grid">
                        <div class="mb-3">
                            <asp:TextBox ID="Accepted_Payment_Transactions_Mobile_Number" runat="server" CssClass="form-control" placeholder="Enter mobile number "></asp:TextBox>
                        </div>
                        <div class="d-grid col-md-3 mx-auto ">
                            <asp:Button ID="Button3" runat="server" OnClick="get_Accepted_Payment_Trans" Text="Query" CssClass="btn btn-primary btn-sm" />
                        </div>
                </div>
            </div>
        </div>
    </div>
</div>

            
         <div class="container mt-5 justify-content-center" runat="server">
 <h3 id="outputText" runat="server"></h3>
 </div>

                <!-- Show the amount of cashback returned on the input wallet id based on a certain 
input plan id.  -->

        
          <div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-4">
            <div class="card shadow">
                <div class="card-header text-center bg-primary text-white">
                    <h5>Show the amount of cashback returned on the input wallet id based on a certain 
input plan id. </h5>
                </div>
                <div class="card-body d-grid">
                        <div class="mb-3">
                            <asp:TextBox ID="TextBox_WalletID" runat="server" CssClass="form-control" placeholder="Enter WalletID " ></asp:TextBox>
                        </div>
                    <div class="mb-1">
                        <asp:TextBox ID="TextBox_PlanID" runat="server" CssClass="form-control" placeholder="Enter Plan ID"></asp:TextBox>
                    </div>
                        <div class="d-grid col-md-3 mx-auto">
                            <asp:Button ID="Button4" runat="server" OnClick="show_cashback_amount" Text="Submit" CssClass="btn btn-primary btn-sm" />
                        </div>
                </div>
            </div>
        </div>
    </div>
</div>

      <div class="container mt-5 justify-content-center" runat="server">
<h3 id="cashback_H1" runat="server"></h3>
</div>


                      <!-- Show the average of the sent transaction amounts from the input wallet id within 
a certain input duration  -->

          <div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-4">
            <div class="card shadow">
                <div class="card-header text-center bg-primary text-white">
                    <h5>Show the average of the sent transaction amounts from the input wallet id within 
a certain input duration </h5>
                </div>
                <div class="card-body d-grid">
                        <div class="mb-3">
                            <asp:TextBox ID="start_date_avgSentTrans" runat="server" CssClass="form-control" placeholder="Enter startDate" TextMode="Date"></asp:TextBox>
                        </div>
                    <div class="mb-3">
    <asp:TextBox ID="end_date_avgSentTrans" runat="server" CssClass="form-control" placeholder="Enter endDate" TextMode="Date"></asp:TextBox>
</div>
                    <div class="mb-1">
                        <asp:TextBox ID="TextBox_WalletID_avgSentTrans" runat="server" CssClass="form-control" placeholder="Enter Wallet ID "></asp:TextBox>
                    </div>
                        <div class="d-grid col-md-3 mx-auto">
                            <asp:Button ID="Button5" runat="server" OnClick="getAvgSentTrans" Text="Submit" CssClass="btn btn-primary btn-sm" />
                        </div>
                </div>
            </div>
        </div>
    </div>
</div>


               <div class="container mt-5 justify-content-center" runat="server">
<h3 id="avg_Sent_Trans_h3" runat="server"></h3>
</div>


                <!--  Show if the input mobile Number is linked to a wallet, or not.  -->

          <div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-4">
            <div class="card shadow">
                <div class="card-header text-center bg-primary text-white">
                    <h5> Show if the input mobile Number is linked to a wallet, or not.</h5>
                </div>
                <div class="card-body d-grid">
                        <div class="mb-3">
                            <%-- <asp:Label runat="server" Text="Enter Mobile Number" CssClass="form-label"></asp:Label> --%>
                            <asp:TextBox ID="MobileNumber_isLinked" runat="server" CssClass="form-control" placeholder="Enter mobile number "></asp:TextBox>
                        </div>
                        <div class="d-grid col-md-3 mx-auto">
                            <asp:Button ID="Button6" runat="server" OnClick="isMobileLinked" Text="Submit" CssClass="btn btn-primary btn-sm" />
                        </div>
                </div>
            </div>
        </div>
    </div>
</div>

 <div class="container mt-5 justify-content-center" runat="server">
<h3 id="isLinked" runat="server"></h3>
</div>

        
                <!--  Take mobile number as an input then Update the total number of earned points 
that it should have. 
  -->

          <div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-4">
            <div class="card shadow">
                <div class="card-header text-center bg-primary text-white">
                    <h5> Take mobile number as an input then Update the total number of earned points 
that it should have. 
</h5>
                </div>
                <div class="card-body d-grid">
                        <div class="mb-3">
                            <%-- <asp:Label runat="server" Text="Enter Mobile Number" CssClass="form-label"></asp:Label> --%>
                            <asp:TextBox ID="MobileNo_update" runat="server" CssClass="form-control" placeholder="Enter mobile number "></asp:TextBox>
                        </div>
                        <div class="d-grid col-md-3 mx-auto">
                            <asp:Button ID="Button7" runat="server" OnClick="updatePoints" Text="Submit" CssClass="btn btn-primary btn-sm" />
                        </div>
                </div>
            </div>
        </div>
    </div>
</div>

<div class="container mt-5 justify-content-center" runat="server">
<h3 id="H1" runat="server"></h3>
</div>


        </form>
       <!-- bootstrap link  -->
   <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
       const themeToggle = document.getElementById('themeToggle');
       const body = document.body;

       themeToggle.addEventListener('click', () => {
           body.classList.toggle('dark-theme');
          
       });
    </script>
</body>
</html>
