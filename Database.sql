﻿drop database Telecom_Team_12
----------2.1---------
--A
create database Telecom_Team_12

GO

Use Telecom_Team_12

--B
GO
CREATE PROCEDURE createAllTables
AS

CREATE TABLE Customer_profile (
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
    CONSTRAINT FK_Customer_Account FOREIGN KEY (nationalID) REFERENCES Customer_profile,
    CHECK(account_type in ('Post Paid', 'Prepaid', 'Pay_as_you_go')),
    CHECK(status in ('active', 'onhold'))
)
    
CREATE TABLE Service_Plan (
    planID int PRIMARY KEY IDENTITY(1,1),
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
    CONSTRAINT FK_Subscription1 FOREIGN KEY (mobileNo) REFERENCES Customer_Account,
    CONSTRAINT FK_Subscription2 FOREIGN KEY (planID) REFERENCES Service_Plan,
    CHECK(status in ('active', 'onhold')),
)

CREATE TABLE Plan_Usage (
    usageID int PRIMARY KEY IDENTITY(1,1),
    start_date date,
    end_date date,
    data_consumption int,
    minutes_used int, 
    SMS_sent int, 
    mobileNo char(11),
    planID int,
    CONSTRAINT FK_Plan_Usage1 FOREIGN KEY(mobileNo) references Customer_Account,
    CONSTRAINT FK_Plan_Usage2 FOREIGN KEY(planID) references Service_Plan
)

CREATE TABLE Payment(
    paymentID int PRIMARY KEY IDENTITY(1,1),
    amount decimal (10,1), 
    date_of_payment date, 
    payment_method Varchar(50), 
    status Varchar(50), 
    mobileNo char(11),
    CONSTRAINT FK_Payment FOREIGN KEY(mobileNo) references Customer_Account,
    CHECK(status in ('successful', 'pending', 'rejected')),
    CHECK(payment_method in ('cash', 'credit'))
)

CREATE TABLE Process_Payment (
    paymentID int PRIMARY KEY IDENTITY(1,1),
    planID int,
    remaining_balance int, 
    extra_amount int,
    CONSTRAINT FK_Process_Payment1 FOREIGN KEY(paymentID) references Payment,
    CONSTRAINT FK_Process_Payment2 FOREIGN KEY(planID) references Service_Plan,
)

CREATE TABLE Wallet (
    walletID int PRIMARY KEY IDENTITY(1,1), 
    current_balance decimal(10,2),
    currency Varchar(50), 
    last_modified_date date, 
    nationalID int, 
    mobileNo char(11),
    CONSTRAINT FK_Wallet FOREIGN KEY(nationalID) references Customer_profile
)

CREATE TABLE Transfer_money(
    walletID1 int,
    walletID2 int,
    transfer_id int IDENTITY(1,1),
    amount decimal(10,2),
    transfer_date date,
    PRIMARY KEY (walletID1,walletID2,transfer_id),
    CONSTRAINT FK_Transfer_money1 FOREIGN KEY (walletID1) references Wallet(walletID),
    CONSTRAINT FK_Transfer_money2 FOREIGN KEY (walletID2) references Wallet(walletID)
)

CREATE TABLE Benefits(
    benefitID int PRIMARY KEY IDENTITY(1,1), 
    description Varchar(50), 
    validity_date date, 
    status Varchar(50),
    mobileNo char(11),
    CONSTRAINT FK_Benefits FOREIGN KEY (mobileNo) references Customer_Account,
    CHECK(status in ('active','expired'))
)

CREATE TABLE Points_Group(
    pointID int IDENTITY(1,1), 
    benefitID int, 
    pointsAmount int, 
    PaymentID int,
    PRIMARY KEY (pointID , benefitID),
    CONSTRAINT FK_Points_Group1 FOREIGN KEY (benefitID) references Benefits,
    CONSTRAINT FK_Points_Group2 FOREIGN KEY (PaymentID) references Payment
)

CREATE TABLE Exclusive_Offer(
    offerID int IDENTITY(1,1), 
    benefitID int, 
    internet_offered int, 
    SMS_offered int,
    minutes_offered int,
    PRIMARY KEY (offerID, benefitID),
    CONSTRAINT FK_Exclusive_Offer FOREIGN KEY (benefitID) references Benefits
)

CREATE TABLE Cashback (
    CashbackID int IDENTITY(1,1),
    benefitID int, 
    walletID int, 
    amount int, --DEFAULT(dbo.getCashback(benefitID)),
    credit_date date,
    PRIMARY KEY (CashbackID, benefitID),
    CONSTRAINT FK_Cashback1 FOREIGN KEY (benefitID) references Benefits,
    CONSTRAINT FK_Cashback2 FOREIGN KEY (walletID) references Wallet
)

CREATE TABLE Plan_Provides_Benefits (
    benefitID int, 
    planID int,
    PRIMARY KEY (benefitID, planID),
    CONSTRAINT FK_Plan_Provides_Benefits1 FOREIGN KEY (benefitID) references Benefits,
    CONSTRAINT FK_Plan_Provides_Benefits2 FOREIGN KEY (planID) references Service_Plan
)

CREATE TABLE Shop (
    shopID int PRIMARY KEY IDENTITY(1,1), 
    name varchar(50), 
    category varchar(50)
)

CREATE TABLE Physical_Shop (
    shopID int PRIMARY KEY, 
    address varchar(50), 
    working_hours varchar(50),
    CONSTRAINT FK_Physical_Shop FOREIGN KEY (shopID) references Shop
)

CREATE TABLE E_shop (
    shopID int PRIMARY KEY, 
    URL varchar(50),
    rating int,
    CONSTRAINT FK_E_shop FOREIGN KEY (shopID) references Shop
)

CREATE TABLE Voucher(
    voucherID int PRIMARY KEY IDENTITY(1,1),
    value int, 
    expiry_date date,
    points int, 
    mobileNo char(11), 
    shopID int,
    redeem_date date,
    CONSTRAINT FK_Voucher1 FOREIGN KEY (mobileNo) references Customer_Account,
    CONSTRAINT FK_Voucher2 FOREIGN KEY(shopID) references Shop
)

CREATE TABLE Technical_Support_Ticket(
    ticketID int IDENTITY(1,1),
    mobileNo char(11), 
    Issue_description Varchar(50),
    priority_level int,
    status Varchar(50),
    PRIMARY KEY (ticketID , mobileNo),
    CONSTRAINT FK_Technical_Support_Ticket FOREIGN KEY (mobileNo) references Customer_Account,
    CHECK (status in ('Open','In Progress','Resolved'))
)
GO
EXEC createAllTables


--C
GO
CREATE PROCEDURE dropAllTables
AS
    --Dropping foreign key constraints--

    ALTER TABLE Customer_Account DROP CONSTRAINT FK_Customer_Account
    ALTER TABLE Subscription DROP CONSTRAINT FK_Subscription1
    ALTER TABLE Subscription DROP CONSTRAINT FK_Subscription2
    ALTER TABLE Plan_Usage DROP CONSTRAINT FK_Plan_Usage1
    ALTER TABLE Plan_Usage DROP CONSTRAINT FK_Plan_Usage2
    ALTER TABLE Payment DROP CONSTRAINT FK_Payment
    ALTER TABLE Process_Payment DROP CONSTRAINT FK_Process_Payment1
    ALTER TABLE Process_Payment DROP CONSTRAINT FK_Process_Payment2
    ALTER TABLE Wallet DROP CONSTRAINT FK_Wallet
    ALTER TABLE Transfer_money DROP CONSTRAINT FK_Transfer_money1
    ALTER TABLE Transfer_money DROP CONSTRAINT FK_Transfer_money2
    ALTER TABLE Benefits DROP CONSTRAINT FK_Benefits;
    ALTER TABLE Points_Group DROP CONSTRAINT FK_Points_Group1
    ALTER TABLE Points_Group DROP CONSTRAINT FK_Points_Group2
    ALTER TABLE Exclusive_Offer DROP CONSTRAINT FK_Exclusive_Offer
    ALTER TABLE Cashback DROP CONSTRAINT FK_Cashback1
    ALTER TABLE Cashback DROP CONSTRAINT FK_Cashback2
    ALTER TABLE Plan_Provides_Benefits DROP CONSTRAINT FK_Plan_Provides_Benefits1
    ALTER TABLE Plan_Provides_Benefits DROP CONSTRAINT FK_Plan_Provides_Benefits2
    ALTER TABLE Physical_Shop DROP CONSTRAINT FK_Physical_Shop
    ALTER TABLE E_shop DROP CONSTRAINT FK_E_shop
    ALTER TABLE Voucher DROP CONSTRAINT FK_Voucher1
    ALTER TABLE Voucher DROP CONSTRAINT FK_Voucher2
    ALTER TABLE Technical_Support_Ticket DROP CONSTRAINT FK_Technical_Support_Ticket

    --Dropping the tables--
    DROP TABLE Customer_profile
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
EXEC dropAllTables

--D
GO
CREATE procedure dropAllProceduresFunctionsViews
AS

--2.1--
DROP PROCEDURE createAllTables
DROP PROCEDURE dropAllTables
DROP PROCEDURE clearAllTables

--2.2--
DROP VIEW allCustomerAccounts
DROP VIEW allServicePlans
DROP VIEW allBenefits
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
DROP PROCEDURE Top_Successful_Payments
DROP FUNCTION Subscribed_plans_5_Months
DROP PROCEDURE Initiate_plan_payment
DROP PROCEDURE Payment_wallet_cashback
DROP PROCEDURE Initiate_balance_payment
DROP PROCEDURE Redeem_voucher_points

GO

--E
GO
CREATE PROCEDURE clearAllTables
AS
    EXEC dropAllTables
    EXEC createAllTables
GO
EXEC clearAllTables

----------2.2---------

--A
GO 
CREATE VIEW allCustomerAccounts
AS
SELECT P.nationalID AS Account_nationalID,P.first_name,P.last_name,P.email,P.address,P.date_of_birth,A.*
FROM Customer_profile P
INNER JOIN Customer_Account A
ON P.nationalID=A.nationalID 
WHERE A.status='active'
GO

--B
GO 
CREATE VIEW allServicePlans
AS
SELECT *
FROM Service_Plan
/*SELECT     
    S.planID,
    S.SMS_offered,
    S.minutes_offered,
    S.data_offered,
    S.name,
    S.price,
    S.description,
    PB.benefitID,
    E.offerID,
    E.internet_offered,
    E.SMS_offered AS ExtraSMSOffered,
    E.minutes_offered AS ExtraMinutesOffered 
FROM Service_Plan S
INNER JOIN Plan_Provides_Benefits PB
ON S.planID=PB.planID
INNER JOIN Exclusive_Offer E
ON E.benefitID=PB.benefitID*/
GO

--need to change common column names for each col to be unique

--E
GO
CREATE VIEW allShops
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
--A
GO
CREATE PROCEDURE Account_Plan
AS
SELECT A.*,P.*
FROM Customer_Account A
INNER JOIN Subscription S
ON A.mobileNo=S.mobileNo
INNER JOIN Service_Plan P
ON P.planID=S.planID
GO
--B
GO
CREATE FUNCTION Account_Plan_date
(@Subscription_Date date,
 @Plan_id int)
RETURNS TABLE
AS
RETURN(
SELECT A.mobileNo,P.planID,P.name
FROM Customer_Account A
INNER JOIN Subscription S
ON A.mobileNo=S.mobileNo
INNER JOIN Service_Plan P
ON P.planID=S.planID
WHERE P.planID=@Plan_id AND S.subscription_date=@Subscription_Date
)
GO

--E
GO
CREATE FUNCTION Account_SMS_Offers
(@MobileNo char(11))
RETURNS Table
AS
RETURN (
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
WHERE a.mobileNo = @MobileNo AND p.status = 'successful'
AND  DATEDIFF(year, p.date_of_payment, GETDATE()) = 0

SELECT @Total_amount = a.point 
FROM Customer_Account a
WHERE a.mobileNo = @MobileNo
GO

----------2.4---------
--A
GO
CREATE FUNCTION AccountLoginValidation
(@MobileNo char(11), @password varchar(50))
RETURNS BIT
AS
BEGIN
 declare @successBit bit
 IF EXISTS 
  (SELECT * FROM Customer_Account WHERE mobileNo=@MobileNo AND pass=@password)
   SET @successBit=1
 ELSE
   SET @successBit=0
RETURN @successBit
END
GO
--B
GO
CREATE FUNCTION Consumption
(@Plan_name varchar(50),@start_date date,@end_date date)
RETURNS TABLE
AS
RETURN (
SELECT sum(P.SMS_sent)AS TotalSMSSent,sum(P.minutes_used) AS TotalMinutesUsed, sum(P.data_consumption) AS TotalDataConsumption
FROM Plan_Usage P
INNER JOIN Service_Plan S
ON P.planID=S.planID
WHERE S.name= @Plan_name AND P.start_date>=@start_date AND P.end_date<=@end_date
)
GO

--total of each?
--C
GO
CREATE PROCEDURE Unsubscribed_Plans
@MobileNo char(11)
AS
SELECT P.*
FROM Service_Plan P
LEFT OUTER JOIN Subscription S
ON P.planID=S.planID AND  S.mobileNo=@MobileNo
WHERE S.planID IS NULL
GO
--offered service plan so should i join with exclusive offers?
--G
GO
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
GO

--H
GO
CREATE FUNCTION Remaining_plan_amount
(@MobileNo char(11),@plan_name varchar(50))
RETURNS INT
AS
BEGIN
DECLARE @Remaining_amount INT
SET @Remaining_amount =       
CASE 
    WHEN 
        (SELECT p.amount FROM Payment p , Process_Payment pp WHERE p.paymentID = pp.paymentID) < 
        (SELECT price FROM Service_Plan WHERE Service_Plan.name = @plan_name)
    THEN 
        (SELECT price FROM Service_Plan WHERE Service_Plan.name = @plan_name) - 
        (SELECT p.amount FROM Payment p , Process_Payment pp WHERE p.paymentID = pp.paymentID)
    ELSE 0
END
RETURN @Remaining_amount
END
GO

--I
GO
CREATE FUNCTION Extra_plan_amount
(@MobileNo char(11),@plan_name varchar(50))
RETURNS INT
AS
BEGIN
DECLARE @Extra_amount INT
SET @Extra_amount =       
CASE 
    WHEN 
        (SELECT p.amount FROM Payment p , Process_Payment pp WHERE p.paymentID = pp.paymentID) > 
        (SELECT price FROM Service_Plan WHERE Service_Plan.name = @plan_name)
    THEN 
        (SELECT p.amount FROM Payment p , Process_Payment pp WHERE p.paymentID = pp.paymentID) -
        (SELECT price FROM Service_Plan WHERE Service_Plan.name = @plan_name)
    ELSE 0
END
RETURN @Extra_amount
END
GO
