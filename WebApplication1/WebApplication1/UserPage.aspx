<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserPage.aspx.cs" Inherits="WebApplication1.UserPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Page</title>
    <!-- Add Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
       <style>
        /* Default Light Theme */
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
     

        #outputText{
            left: 50%;

        }
    </style>
</head>
<body id="bodyTag"  runat="server">
    <form id="form1" runat="server">
    <asp:Button ID="SignOut" runat="server" OnClick="signOut" type="button" text="Sign out" class="btn btn-primary" />
    <div class="container">
    <div class="centered-div text-center">
        <h2 class="mb-3">Choose an Action</h2>
        <!-- <p class="text-muted mb-4"></p> -->
        <div class="mb-3" >
           <asp:DropDownList ID="dropDown"  runat="server" CssClass="form-select">
                <asp:ListItem Value="0" Selected="True">Choose from the following:</asp:ListItem>
                <asp:ListItem Value="1">View details of all offered Service Plans.</asp:ListItem>       
                <asp:ListItem Value="2">View the total SMS, Minutes and Internet consumption.</asp:ListItem>
                <asp:ListItem Value="3">Display all offered plans currently not subscribed to.</asp:ListItem>   
                <asp:ListItem Value="4">Show the usage of active plans during this month.</asp:ListItem>
                <asp:ListItem Value="5">Show all the cashback transactions.</asp:ListItem>
                <asp:ListItem Value="6">View details for all active Benefits.</asp:ListItem>
                <asp:ListItem Value="7">Show unresolved technical support tickets for each account.</asp:ListItem>
                <asp:ListItem Value="8">Show the voucher with the highest value.</asp:ListItem>
                <asp:ListItem Value="9">Display the remaining amount for the last payment.</asp:ListItem>
                <asp:ListItem Value="10">Display the extra amount for the last payment.</asp:ListItem>
                <asp:ListItem Value="11">Show the top 10 successful payments with the highest value.</asp:ListItem>
                <asp:ListItem Value="12">View details for all shops.</asp:ListItem>
                <asp:ListItem Value="13">Show all service plans subscribed to in the past 5 months.</asp:ListItem>
                <asp:ListItem Value="14">Renew the subscription a plan.</asp:ListItem>
                <asp:ListItem Value="15">Get cashback amount for a specified payment benefit.</asp:ListItem>
                <asp:ListItem Value="16">Recharge balance.</asp:ListItem>
                <asp:ListItem Value="17">Redeem a certain voucher.</asp:ListItem>

            
           </asp:DropDownList>

        </div>
        <asp:Button ID="Button1" runat="server" OnClick="list" type="button" text="Submit" class="btn btn-primary" />
    </div>
</div>



         

                <!-- Retriving Remaining balance-->

      <div class="container mt-5 remaningBalance" id="remainingBalance" runat="server">
        <div class="row justify-content-center">
            <div class="col-md-4">
                <div class="card shadow">
                    <div class="card-header text-center bg-primary text-white">
                        <h5>Display the remaining amount for the last payment initiated.</h5>
                    </div>
                    <div class="card-body d-grid">
                            <div class="mb-3">
                                <asp:TextBox ID="servicePlanRem" runat="server" CssClass="form-control" placeholder="Enter Plan Name"></asp:TextBox>
                            </div>
                            <div class="d-grid col-md-3 mx-auto ">
                                <asp:Button ID="remainingBalanceButton" runat="server" OnClick="getRemainingBalanceClicked" Text="Query" CssClass="btn btn-primary btn-sm" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


                          <!-- Retriving Extra Amount-->

        <div class="container mt-5 extraAmount" id="extraAmount" runat="server">
          <div class="row justify-content-center">
              <div class="col-md-4">
                  <div class="card shadow">
                      <div class="card-header text-center bg-primary text-white">
                          <h5>Display the extra amount for the last payment initiated.</h5>
                      </div>
                      <div class="card-body d-grid">
                              <div class="mb-3">
                                  <asp:TextBox ID="servicePlanExtra" runat="server" CssClass="form-control" placeholder="Enter Plan Name"></asp:TextBox>
                              </div>
                              <div class="d-grid col-md-3 mx-auto ">
                                  <asp:Button ID="extraAmountButton" runat="server" OnClick="getExtraAmountClicked" Text="Query" CssClass="btn btn-primary btn-sm" />
                              </div>
                          </div>
                      </div>
                  </div>
              </div>
          </div>


         <!-- Consumption-->

                  <div class="container mt-5 getconsume" id="getconsume" runat="server">
    <div class="row justify-content-center">
        <div class="col-md-4">
            <div class="card shadow">
                <div class="card-header text-center bg-primary text-white">
                    <h5>View the total SMS, Minutes and Internet consumption for an input plan name within a certain input duration</h5>
                </div>
                <div class="card-body d-grid">
                        <div class="mb-3">
                            <asp:TextBox ID="consumption1" runat="server" CssClass="form-control" placeholder="Enter plan ID " ></asp:TextBox>
                        </div>
                    <div class="mb-1">
                        <asp:Label runat="server" Text="Enter Start Date" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="consumption2" runat="server" CssClass="form-control"  TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="mb-1">
                        <asp:Label runat="server" Text="Enter End Date" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="consumption3" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                        <div class="d-grid col-md-3 mx-auto">
                            <asp:Button ID="consumptionButton" runat="server" OnClick="getConsumptionClicked" Text="Submit" CssClass="btn btn-primary btn-sm" />
                        </div>
                </div>
            </div>
        </div>
    </div>
</div>


        <!-- initiate payment-->
          <div class="container mt-5 initiatePayment" id="initiatePayment" runat="server">
    <div class="row justify-content-center">
        <div class="col-md-4">
            <div class="card shadow">
                <div class="card-header text-center bg-primary text-white">
                    <h5>Renew the subscription of a plan.</h5>
                </div>
                <div class="card-body d-grid">
                        <div class="mb-3">
                            <asp:TextBox ID="Initiate_Payment_amount" runat="server" CssClass="form-control" placeholder="Enter amount"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <asp:TextBox ID="Initiate_Payment_method" runat="server" CssClass="form-control" placeholder="Enter the payment method you want(cash,credit)"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <asp:TextBox ID="Initiate_planid" runat="server" CssClass="form-control" placeholder="Enter plan ID"></asp:TextBox>
                        </div>
                        <div class="d-grid col-md-3 mx-auto">
                            <asp:Button ID="Initiate_Payment_submit" runat="server" OnClick="InitiatePaymentClicked" Text="Submit" CssClass="btn btn-primary btn-sm" />
                        </div>
                </div>
            </div>
        </div>
    </div>
</div>

                <!-- payment_wallet_cashback-->
          <div class="container mt-5 paymentwalletcashback" id="paymentwalletcashback" runat="server">
    <div class="row justify-content-center">
        <div class="col-md-4">
            <div class="card shadow">
                <div class="card-header text-center bg-primary text-white">
                    <h5>Get the amount of cashback that will be returned on the wallet of the customer of the input mobile number from a certain payment transaction of a specified benefit.</h5>
                </div>
                <div class="card-body d-grid">
                        <div class="mb-3">
                            <asp:TextBox ID="Payment_wallet_cashback_Payment_id" runat="server" CssClass="form-control" placeholder="Enter payment ID"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <asp:TextBox ID="Payment_wallet_cashback_Benefit_ID" runat="server" CssClass="form-control" placeholder="Enter benefit ID"></asp:TextBox>
                        </div>
                        <div class="d-grid col-md-3 mx-auto">
                            <asp:Button ID="Payment_wallet_cashback_submit" runat="server" OnClick="PaymentWalletCashbackClicked" Text="Submit" CssClass="btn btn-primary btn-sm" />
                        </div>
                </div>
            </div>
        </div>
    </div>
</div>
                        <!--  Initiate_balance_payment-->
          <div class="container mt-5  Initiate_balance_payment" id="Initiate_balance_payment" runat="server">
    <div class="row justify-content-center">
        <div class="col-md-4">
            <div class="card shadow">
                <div class="card-header text-center bg-primary text-white">
                    <h5>Recharge balance.</h5>
                </div>
                <div class="card-body d-grid">
                        <div class="mb-3">
                            <asp:TextBox ID="Initiate_balance_payment_amount" runat="server" CssClass="form-control" placeholder="Enter the amount"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <asp:TextBox ID="Initiate_balance_payment_paymentMethod" runat="server" CssClass="form-control" placeholder="Enter the payment method(cash,credit)"></asp:TextBox>
                        </div>
                        <div class="d-grid col-md-3 mx-auto">
                            <asp:Button ID="Initiate_balance_payment_submit" runat="server" OnClick="RechargeBalanceClicked" Text="Submit" CssClass="btn btn-primary btn-sm" />
                        </div>
                </div>
            </div>
        </div>
    </div>
</div>

                                <!-- Redeem voucher-->
          <div class="container mt-5   Redeem_voucher" id="Redeem_voucher" runat="server">
    <div class="row justify-content-center">
        <div class="col-md-4">
            <div class="card shadow">
                <div class="card-header text-center bg-primary text-white">
                    <h5>Redeem a certain voucher</h5>
                </div>
                <div class="card-body d-grid">
                        <div class="mb-3">
                            <asp:TextBox ID="Redeem_voucher_voucherID" runat="server" CssClass="form-control" placeholder="Enter the voucher ID"></asp:TextBox>
                        </div>
                        <div class="d-grid col-md-3 mx-auto">
                            <asp:Button ID="Redeem_voucher_submit" runat="server" OnClick="RedeemVoucherClicked" Text="Submit" CssClass="btn btn-primary btn-sm" />
                        </div>
                </div>
            </div>
        </div>
    </div>
</div>

    


         <div class="container mt-5">
        <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="true">
        </asp:GridView>
        </div>
        <div class="container mt-5 justify-content-center" runat="server">
        <h3 id="outputText" runat="server"></h3>
        </div>
    </form>
       <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
       
</body>
</html>