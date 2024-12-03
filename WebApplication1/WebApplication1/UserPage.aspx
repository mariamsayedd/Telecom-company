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
            position: fixed; /* Stays in place even when you scroll */
            top: 10px; /* Distance from the top of the page */
            right: 10px; /* Distance from the right edge of the page */
            width: 50px; /* Adjust the size of the button */
            height: 50px; /* Adjust the size of the button */
            padding: 0; /* Remove extra padding */
            z-index: 1000; /* Ensures it appears above other elements */
        }

        #themeToggle img {
            width: 30px; /* Adjust the size of the image */
            height: 30px;
        }
         .centered-div {
            background-color: white;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
            padding: 30px;
            width: 100%;
            max-width: 500px;
            margin: 100px auto; /* Centers the div */
        }
       .alert-container {
        position: fixed;
        top: 25%;
        left: 50%;
        transform: translate(-50%, -50%);
        z-index: 1050; /* Ensures it's above most elements */
        width: 50%; /* Adjust width as needed */
        max-width: 500px; /* Limit the maximum width */
        padding: 1rem;
        }

        .alert-container.show {
        display: block; /* Make visible when needed */
    }
        #outputText{
            left: 50%;

        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
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
                <asp:ListItem Value="7">Shows the number of technical support tickets that are NOT ‘Resolved’ for each account of the input customer.</asp:ListItem>
                <asp:ListItem Value="8">Show the voucher with the highest value.</asp:ListItem>
                <asp:ListItem Value="9">Display the remaining amount for the last payment.</asp:ListItem>
                <asp:ListItem Value="10">Display the extra amount for the last payment.</asp:ListItem>
                <asp:ListItem Value="11">Show the top 10 successful payments with the highest value.</asp:ListItem>
                <asp:ListItem Value="12">View details for all shops.</asp:ListItem>
                <asp:ListItem Value="13">Show all service plans subscribed to in the past 5 months.</asp:ListItem>
                <asp:ListItem Value="14">Renew the subscription a plan.</asp:ListItem>
                <asp:ListItem Value="15">Get the amount of cashback that will be returned on the wallet of the customer of the input mobile number from a certain payment transaction of a specified benefit.</asp:ListItem>
                <asp:ListItem Value="16">Recharge balance.</asp:ListItem>
                <asp:ListItem Value="17">Redeem a certain voucher.</asp:ListItem>

            
           </asp:DropDownList>

        </div>
        <asp:Button ID="Button1" runat="server" OnClick="list" type="button" text="Submit" class="btn btn-primary" />
    </div>
</div>



         <div class="container mt-5">
            <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="true">
            </asp:GridView>
        </div>
        <div class="container mt-5" runat="server">
        <h3 id="outputText" runat="server"></h3>
        </div>

    </form>
</body>
</html>
