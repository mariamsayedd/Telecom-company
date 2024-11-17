drop database temp
----------2.1---------
create database Telecom_Team_12

GO

Use Telecom_Team_12

GO
CREATE PROCEDURE createAllTables
AS

CREATE TABLE Customer_Profile (
    nationalID int NOT NULL PRIMARY KEY ,
    first_name Varchar(50),
    last_name Varchar(50),
    email Varchar(50),
    address Varchar(50),
    date_of_birth date
)
Create table Customer_Account (
    mobileNo char(11) NOT NULL PRIMARY KEY,
    pass varchar(50),
    balance decimal(10,1),
    account_type Varchar(50),
    start_date date, 
    status Varchar(50), 
    point int DEFAULT 0, 
    nationalID int ,
    FOREIGN KEY (nationalID) REFERENCES Customer_Profile,
    check(account_type in ('Post Paid', 'Prepaid', 'Pay_as_you_go')),
    check(status in ('active', 'onhold'))
)
    
CREATE TABLE Service_Plan (
    planID int PRIMARY KEY identity(1,1),
    SMS_offered int,
    minutes_offered int,
    data_offered int,
    name Varchar(50),
    price int, 
    description Varchar(50)
)
      
Create table Subscription (
    mobileNo char(11) NOT NULL, 
    planID int ,
    subscription_date date,
    status Varchar(50),
    PRIMARY KEY (mobileNo, planID),
    FOREIGN KEY (mobileNo) REFERENCES Customer_Account,
    FOREIGN KEY (planID) REFERENCES Service_Plan,
    check(status in ('active', 'onhold')),
)

CREATE TABLE Plan_Usage (
    usageID int primary key identity(1,1),
    start_date date,
    end_date date,
    data_consumption int,
    minutes_used int, 
    SMS_sent int, 
    mobileNo char(11),
    planID int,
    foreign key(mobile_number) references Customer_Account,
    foreign key(planID) references Service_Plan
)

CREATE TABLE Payment(
    paymentID int primary key identity(1,1),
    amount decimal (10,1), 
    date_of_payment date, 
    payment_method Varchar(50), 
    status Varchar(50), 
    mobileNo char(11),
    foreign key(mobileNo) references Customer_Account,
    check(status in ('successful', 'pending', 'rejected')),
    check(payment_method in ('cash', 'credit'))
)

CREATE TABLE Process_Payment (
    paymentID int primary key identity(1,1),
    planID int,
    remaining_balance int, 
    extra_amount int,
    foreign key(paymentID) references Payment,
    foreign key(planID) references Service_Plan,
)

CREATE TABLE Wallet (
    walletID int primary key identity(1,1), 
    current_balance decimal(10,2),
    currency Varchar(50), 
    last_modified_date date, 
    nationalID int, 
    mobileNo char(11),
    foreign key(nationalID) references Customer_Profile
)

CREATE TABLE Transfer_money(
    walletID1 int,
    walletID2 int,
    transfer_id int identity(1,1),
    amount decimal(10,2),
    transfer_date date,
    primary key (walletID1,walletID2,transfer_id),
    foreign key (walletID1,walletID2) references Wallet
)

CREATE TABLE Benefits(
    benefitID int primary key identity(1,1), 
    description Varchar(50), 
    validity_date date, 
    status Varchar(50),
    mobileNo char(11),
    foreign key (mobileNo) references Customer_Account,
    check(status in ('active','expired'))
)

CREATE TABLE Points_Group(
    pointID int identity(1,1), 
    benefitID int, 
    pointsAmount int, 
    PaymentID int,
    primary key (pointID , benefitID),
    foreign key (benefitID) references Benefits,
    foreign key (PaymentID) references Payment
)

CREATE TABLE Exclusive_Offer(
    offerID int identity(1,1), 
    benefitID int, 
    internet_offered int, 
    SMS_offered int,
    minutes_offered int,
    primary key (offerID, benefitID),
    foreign key (benefitID) references Benefits
)

CREATE TABLE Cashback (
    CashbackID int identity(1,1),
    benefitID int, 
    walletID int, 
    amount int DEFAULT(dbo.getCashback(benefitID)),
    credit_date date,
    primary key (CashbackID, benefitID),
    foreign key (benefitID) references Benefits,
    foreign key (walletID) references Wallet
)

CREATE TABLE Plan_Provides_Benefits (
    benefitID int, 
    planID int,
    primary key (benefitID, planID),
    foreign key (benefitID) references Benefits,
    foreign key (planID) references Service_Plan
)

CREATE TABLE Shop (
    shopID int primary key identity(1,1), 
    name varchar(50), 
    category varchar(50)
)

CREATE TABLE Physical_Shop (
    shopID int primary key, 
    address varchar(50), 
    working_hours varchar(50),
    foreign key (shopID) references Shop
)

CREATE TABLE E_shop (
    shopID int primary key, 
    URL varchar(50),
    rating int,
    foreign key (shopID) references Shop
)

CREATE TABLE Voucher(
    voucherID int primary key identity(1,1),
    value int, 
    expiry_date date,
    points int, 
    mobileNo char(11), 
    shopID int,
    redeem_date date,
    foreign key (mobileNo) references Customer_Account,
    foreign key(shopID) references Shop
)

CREATE TABLE Technical_Support_Ticket(
    ticketID int identity(1,1),
    mobileNo char(11), 
    Issue_description Varchar(50),
    priority_level int,
    status Varchar(50),
    primary key (ticketID , mobileNo),
    foreign key (mobileNo) references Customer_Account,
    check (status in ('Open','In Progress','Resolved'))
)

GO

GO
create function getCashback
(@benifitID int)
returns int 
as
BEGIN

declare @amount int 
select @amount = p.amount 
from Benefit b
inner join customer_account acc on b.mobileNo = acc.mobileNo 
inner join payment p on acc.mobileNo = p.mobileNo
where b.benefitID = @benifitID
declare @cashback int
set @cashback = @amount * 0.1 
return @cashback
END
GO


GO
CREATE PROCEDURE dropAllTables
AS

DROP TABLE Customer_Profile
DROP TABLE Customer_Account
DROP TABLE Service_Plan
DROP TABLE Subscription
DROP TABLE Plan_Usage
DROP TABLE Payment
DROP TABLE Process_Payment
DROP TABLE Wallet
DROP TABLE Transfer_money
DROP TABLE Benefits
DROP TABLE Points_Group
DROP TABLE Exclusive_Offer
DROP TABLE Cashback
DROP TABLE Plan_Provides_Benefits
DROP TABLE Shop
DROP TABLE Physical_Shop
DROP TABLE E_shop
DROP TABLE Voucher
DROP TABLE Technical_Support_Ticket

GO

GO
CREATE procedure dropAllProceduresFunctionsViews
AS

--2.1--
DROP PROCEDURE createAllTables
DROP PROCEDURE dropAllTables
DROP PROCEDURE clearAllTables
DROP FUNCTION getCashback

--2.2--
DROP VIEW allCustomerAccounts
DROP VIEW allServicePlans
DROP VIEW AllBenefits
DROP VIEW AccountPayments
DROP VIEW allShops
DROP VIEW allResolvedTickets
DROP VIEW CustomerWallet
DROP VIEW E_shopVouchers
DROP VIEW PhysicalStoreVouchers
DROP VIEW Num_of_cashback

--2.3--
DROP PROCEDURE Account_Plan
DROP FUNCTION Account_Plan_date
DROP FUNCTION Account_Usage_Plan
DROP PROCEDURE Benefits_Account
DROP FUNCTION Account_SMS_Offers
DROP PROCEDURE Account_Payment_Points
DROP FUNCTION Wallet_Cashback_Amount
DROP FUNCTION Wallet_Transfer_Amount
DROP FUNCTION Wallet_MobileNo
DROP PROCEDURE Total_Points_Account

--2.4--
DROP FUNCTION AccountLoginValidation
DROP FUNCTION Consumption
DROP PROCEDURE Unsubscribed_Plans
DROP FUNCTION Usage_Plan_CurrentMonth
DROP FUNCTION Cashback_Wallet_Customer
DROP PROCEDURE Ticket_Account_Customer
DROP PROCEDURE Account_Highest_Voucher
DROP FUNCTION Remaining_plan_amount
DROP FUNCTION Extra_plan_amount
DROP PROCEDURE Top_Successfl_Payments
DROP FUNCTION Subscribed_plans_5_Months
DROP PROCEDURE Initiate_plan_payment
DROP PROCEDURE Payment_wallet_chasback
DROP PROCEDURE Initiate_balance_payment
DROP PROCEDURE Redeem_voucher_points

GO

GO
CREATE PROCEDURE clearAllTables
AS

TRUNCATE TABLE Customer_Profile
TRUNCATE TABLE Customer_Account
TRUNCATE TABLE Service_Plan
TRUNCATE TABLE Subscription
TRUNCATE TABLE Plan_Usage
TRUNCATE TABLE Payment
TRUNCATE TABLE Process_Payment
TRUNCATE TABLE Wallet
TRUNCATE TABLE Transfer_money
TRUNCATE TABLE Benefits
TRUNCATE TABLE Points_Group
TRUNCATE TABLE Exclusive_Offer
TRUNCATE TABLE Cashback
TRUNCATE TABLE Plan_Provides_Benefits
TRUNCATE TABLE Shop
TRUNCATE TABLE Physical_Shop
TRUNCATE TABLE E_shop
TRUNCATE TABLE Voucher
TRUNCATE TABLE Technical_Support_Ticket

GO

----------2.2---------
--E
GO
CREATE VIEW AllShops
AS
SELECT * FROM Shop
GO

--F
GO
CREATE VIEW allResolvedTickets
AS
SELECT * FROM Technical_Support_Ticket
WHERE status = 'Resolved'
GO

----------2.3---------
--E
GO
CREATE FUNCTION Account_SMS_Offers
(@MobileNo char(11))
returns Table
AS
return (
SELECT a.mobileNo , e.SMS_offered
FROM Customer_Account a 
inner join Benefits b on a.mobileNo = b.mobileNo
inner join Exclusive_Offer e on b.benefitID = e.benefitID
WHERE a.mobileNo = @MobileNo)
GO

--F
GO
CREATE PROCEDURE Account_Payment_Points
@MobileNo char(11),
@Total_Transactions int OUTPUT,
@Total_Amount int OUTPUT
AS
SELECT @Total_Transactions = count(*)
FROM Customer_Account a 
inner join Payment p on a.mobileNo = p.mobileNo 
WHERE a.mobileNo = @MobileNo AND p.status = 'succesful'
AND  DATEDIFF(year, '2021/08/25', GETDATE()) = 0

SELECT @Total_amount = a.point 
FROM Customer_Account a
WHERE a.mobileNo = @MobileNo
GO

----------2.4---------
--G
CREATE PROCEDURE Account_Highest_Voucher
@MobileNo char(11),
@Voucher_id int OUTPUT
AS
SELECT @Voucher_id = v.voucherID 
FROM Voucher v
WHERE v.value = 
(SELECT MAX(v1.value) 
from Voucher v1 
inner join Customer_Account a on v1.mobileNo = a.mobileNo 
WHERE a.mobileNo = @MobileNo)










