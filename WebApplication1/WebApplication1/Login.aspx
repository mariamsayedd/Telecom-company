<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication1.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
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
        .dark-theme .btn-primary {
            background-color: #bb86fc;
            border-color: #bb86fc;
        }
        .dark-theme .btn-primary:hover {
            background-color: #9b69d9;
            border-color: #9b69d9;
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
    <div class="text-right mt-2">
    <button type="button" id="themeToggle" class="btn btn-outline-secondary">
        <img src="dark-theme.svg" alt="My Happy SVG" />
    </button>
         </div>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row justify-content-center align-items-center" style="height: 100vh;">
                <div class="col-md-4">
                    <div class="card shadow-sm">
                        <div class="card-body">
                            <h3 class="text-center mb-4">Please Login</h3>




                            <div class="mb-3">
                                <label for="username" class="form-label">Mobile Number</label>
                                <asp:TextBox ID="username" CssClass="form-control" runat="server" placeholder="Enter your mobile number"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="password" class="form-label">Password</label>
                                <asp:TextBox ID="password" CssClass="form-control" runat="server" placeholder="Enter your password" TextMode="Password"></asp:TextBox>
                            </div>
                            <div class="d-grid">
                                <asp:Button ID="Button1" runat="server" OnClick="login" Text="Sign In" CssClass="btn btn-primary btn-block" />
                            </div>
                        </div>
                        <div class="card-footer text-muted text-center">
                            <small>&copy;Telecom_Team_12</small>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <!-- Alert Placeholder -->
            <div class="alert-container mt-3">
            <asp:Literal ID="AlertPlaceholder" runat="server"></asp:Literal>
        </div>


           
    </form>
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
