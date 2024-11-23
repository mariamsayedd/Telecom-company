
----------2.1---------
--A
CREATE database Telecom_Team_12

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

Create TABLE Customer_Account (
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
      
CREATE TABLE Subscription (
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
    paymentID int PRIMARY KEY,
    planID int,
    remaining_balance AS dbo.Remaining_plan_amount_helper(paymentID,planID), 
    extra_amount AS dbo.Extra_plan_amount_helper(paymentID,planID),
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
    CONSTRAINT FK_Points_Group1 FOREIGN KEY (benefitID) references Benefits on delete cascade,
    CONSTRAINT FK_Points_Group2 FOREIGN KEY (PaymentID) references Payment
)

CREATE TABLE Exclusive_Offer(
    offerID int IDENTITY(1,1), 
    benefitID int, 
    internet_offered int, 
    SMS_offered int,
    minutes_offered int,
    PRIMARY KEY (offerID, benefitID),
    CONSTRAINT FK_Exclusive_Offer FOREIGN KEY (benefitID) references Benefits on delete cascade
)

CREATE TABLE Cashback (
    CashbackID int IDENTITY(1,1),
    benefitID int, 
    walletID int, 
    amount int DEFAULT (0),
    credit_date date,
    PRIMARY KEY (CashbackID, benefitID),
    CONSTRAINT FK_Cashback1 FOREIGN KEY (benefitID) references Benefits on delete cascade,
    CONSTRAINT FK_Cashback2 FOREIGN KEY (walletID) references Wallet
)

CREATE TABLE Plan_Provides_Benefits (
    benefitID int, 
    planID int,
    PRIMARY KEY (benefitID, planID),
    CONSTRAINT FK_Plan_Provides_Benefits1 FOREIGN KEY (benefitID) references Benefits on delete cascade,
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
    CREATE FUNCTION Remaining_plan_amount_helper(
    @PaymentID int,
    @PlanID int)
    RETURNS decimal(10,2)
    AS
    BEGIN
    DECLARE @MobileNo char(11)
    DECLARE @Plan_name varchar(50)
    DECLARE @Remaining_balance decimal(10,2)

    SET @MobileNo = (SELECT mobileNo FROM Payment Where Payment.paymentID = @PaymentID)
    SET @Plan_name = (SELECT name FROM Service_Plan WHERE Service_Plan.planID = @PlanID)
    SET @Remaining_balance = dbo.Remaining_plan_amount(@MobileNo,@Plan_name)

    RETURN @Remaining_balance
    END
    GO

    GO
    CREATE FUNCTION Extra_plan_amount_helper(
    @PaymentID int,
    @PlanID int)
    RETURNS decimal(10,2)
    AS
    BEGIN
    DECLARE @MobileNo char(11)
    DECLARE @Plan_name varchar(50)
    DECLARE @Extra_amount decimal(10,2)

    SET @MobileNo = (SELECT mobileNo FROM Payment Where Payment.paymentID = @PaymentID)
    SET @Plan_name = (SELECT name FROM Service_Plan WHERE Service_Plan.planID = @PlanID)
    SET @Extra_amount = dbo.Extra_plan_amount(@MobileNo,@Plan_name)

    RETURN @Extra_amount
    END
    GO
GO

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

--D
GO
CREATE procedure dropAllProceduresFunctionsViews
AS

--2.1--
DROP PROCEDURE createAllTables
DROP PROCEDURE dropAllTables
DROP PROCEDURE clearAllTables
DROP FUNCTION Remaining_plan_amount_helper
DROP FUNCTION Extra_plan_amount_helper

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

----------2.2---------
--A
GO 
CREATE VIEW allCustomerAccounts
AS
SELECT P.nationalID,
       P.first_name,
       P.last_name,
       P.email,
       P.address,
       P.date_of_birth,
       A.mobileNo,
       A.pass,
       A.balance,
       A.account_type,
       A.start_date,
       A.status,
       A.point
FROM Customer_profile P
INNER JOIN Customer_Account A
ON P.nationalID = A.nationalID 
WHERE A.status = 'active'
GO

--B
GO 
CREATE VIEW allServicePlans
AS
SELECT *
FROM Service_Plan
GO

--C
GO
CREATE VIEW allBenefits
AS
SELECT * FROM Benefits
WHERE status = 'active'
GO

--D
GO
CREATE VIEW AccountPayments
AS
SELECT 
    p.paymentID, 
    p.amount, 
    p.date_of_payment, 
    p.payment_method, 
    p.status AS PaymentStatus, 
    p.mobileNo, 
    
    c.pass, 
    c.balance, 
    c.account_type, 
    c.start_date, 
    c.status AS AccountStatus,
    c.point, 
    c.nationalID
FROM Payment p INNER JOIN Customer_Account c 
ON p.mobileNo = c.mobileNo;
GO        
        
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

--G
GO
CREATE VIEW CustomerWallet
AS
SELECT Wallet.* , Customer_profile.first_name , Customer_profile.last_name
FROM Wallet , Customer_profile
WHERE Wallet.nationalID = Customer_profile.nationalID
GO

--H
GO
CREATE VIEW E_shopVouchers
AS
SELECT E_shop.shopID, Voucher.voucherID, Voucher.value  
FROM E_shop 
LEFT OUTER JOIN Voucher ON (E_shop.shopID = Voucher.shopID)
GO

--I
GO
CREATE VIEW PhysicalStoreVouchers
AS
SELECT Physical_Shop.shopID ,Voucher.voucherID ,Voucher.value  
FROM Physical_Shop 
LEFT OUTER JOIN Voucher ON (Physical_Shop.shopID = Voucher.shopID)
GO

--J
GO
CREATE VIEW Num_of_cashback
AS
SELECT Cashback.walletID,  count(*) AS Count_of_Cashbacks
FROM Cashback 
GROUP BY walletID
GO

----------2.3---------
--A
GO
CREATE PROCEDURE Account_Plan
AS
SELECT A.*,P.*
FROM Customer_Account A
INNER JOIN Subscription S
ON A.mobileNo = S.mobileNo
INNER JOIN Service_Plan P
ON P.planID = S.planID
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
ON A.mobileNo = S.mobileNo
INNER JOIN Service_Plan P
ON P.planID = S.planID
WHERE P.planID = @Plan_id AND S.subscription_date = @Subscription_Date
)
GO

--C
GO
CREATE FUNCTION Account_Usage_Plan
(@MobileNo char(11), @from_date date)
RETURNS Table
AS
RETURN (
SELECT s.planID, sum(p.data_consumption) AS totalData, sum(p.minutes_used) AS totalMinutes, sum(p.SMS_sent) AS totalSMS
FROM Plan_Usage p INNER JOIN Subscription s
ON p.planID = s.planID AND p.mobileNo = s.mobileNo
WHERE p.mobileNo = @MobileNo AND p.start_date >= @from_date
GROUP BY s.planID)
GO

--D
GO
CREATE PROCEDURE Benefits_Account
@MobileNo char(11), @planID int
AS
DELETE FROM Benefits 
FROM Benefits b 
INNER JOIN Customer_Account c ON b.mobileNo = c.mobileNo 
INNER JOIN Subscription s ON c.mobileNo = s.mobileNo
WHERE b.mobileNo=@MobileNo AND s.planID = @planID

SELECT * FROM Benefits --added benefits as output
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
INNER JOIN Benefits b ON a.mobileNo = b.mobileNo
INNER JOIN Exclusive_Offer e ON b.benefitID = e.benefitID
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
INNER JOIN Payment p ON a.mobileNo = p.mobileNo 
WHERE a.mobileNo = @MobileNo AND p.status = 'successful'
AND  DATEDIFF(year, p.date_of_payment, GETDATE()) = 0

SELECT sum(pg.pointsAmount) 
FROM Customer_Account a ,Benefits b ,Points_Group pg
WHERE a.mobileNo = @MobileNo AND a.mobileNo = b.mobileNo AND b.benefitID = pg.benefitID 
GO

--G
GO
CREATE FUNCTION Wallet_Cashback_Amount
(
@WalletId int,
@planId int
)
RETURNS int
AS
BEGIN 
RETURN (SELECT sum(cb.amount) FROM Cashback AS cb 
INNER JOIN Plan_Provides_Benefits AS ppb ON cb.benefitID = ppb.benefitID
WHERE ppb.planID = @planID  AND cb.walletID = @WalletID)
END --assuming that several cashbacks can be returned as a sum and if it is one cashback it will be the same result
GO

--H
GO
create FUNCTION Wallet_Transfer_Amount 
(
    @Wallet_id int, 
    @start_date date, 
    @end_date date
)
RETURNS int 
AS
BEGIN
DECLARE @answer int
SELECT @answer = Avg(amount) 
FROM Transfer_money 
WHERE transfer_date >= @start_date AND transfer_date <= @end_date AND walletID1 = @Wallet_id
RETURN @answer
END
GO

--I
GO
CREATE FUNCTION Wallet_MobileNo(
@MobileNo char(11))
RETURNS bit
AS
BEGIN
DECLARE @answer BIT

IF EXISTS(
    SELECT *
    FROM Wallet w, Customer_Account ca
    WHERE ca.MobileNo = @MobileNo and ca.nationalID = w.nationalID )
    SET @answer = 1
ELSE SET @answer =0

RETURN @answer
END
GO


--J
GO
CREATE proc Total_Points_Account
@MobileNo char(11),
@allPoints int OUTPUT
AS

SELECT @allPoints = sum(p.pointsAmount)
FROM Benefits b, Points_Group p
WHERE b.mobileNo = @MobileNo and b.benefitID = p.benefitID

UPDATE Customer_Account
SET point = @allPoints
WHERE @MobileNo = mobileNo

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
  (SELECT * FROM Customer_Account WHERE mobileNo = @MobileNo AND pass = @password)
   SET @successBit = 1
 ELSE
   SET @successBit = 0
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
ON P.planID = S.planID
WHERE S.name= @Plan_name AND P.start_date>=@start_date AND P.end_date<=@end_date
)
GO
    
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

--D
GO
CREATE FUNCTION Usage_Plan_CurrentMonth
(@MobileNo char(11))
RETURNS Table
AS
RETURN (
SELECT p.data_consumption, p.minutes_used, p.SMS_sent
FROM Plan_Usage p INNER JOIN Subscription s on p.planID = s.planID AND s.mobileNo = @MobileNo
WHERE p.mobileNo = @MobileNo 
      AND MONTH(start_date) = MONTH(current_timestamp) 
      AND YEAR(start_date) = YEAR(current_timestamp) 
      AND p.end_date >= current_timestamp --to make sure it's still active
      AND s.status = 'active'
      --add join to check active status in subscription (done)
)
GO

--E
GO
CREATE FUNCTION Cashback_Wallet_Customer
(@NationalID int)
RETURNS Table
AS
RETURN (
SELECT c.CashbackID, 
       c.benefitID, 
       c.walletID,    
       c.amount, 
       c.credit_date, 
   
       w.current_balance, 
       w.currency, 
       w.last_modified_date, 
       w.nationalID,
       w.mobileNo 
FROM Cashback c INNER JOIN Wallet w 
ON c.walletID = w.walletID
WHERE w.nationalID = @NationalID)
GO

--F
GO
CREATE PROCEDURE Ticket_Account_Customer
@NationalID int
AS
SELECT a.mobileNo,count(t.ticketID) 
FROM Customer_profile p 
INNER JOIN Customer_Account a ON p.nationalID=a.nationalID 
INNER JOIN Technical_Support_Ticket t ON a.mobileNo=t.mobileNo 
WHERE t.status <> 'resolved' AND p.nationalID = @NationalID
GROUP BY a.mobileNo
--Output a table or just the total number?
--chose to just output the table and remove the output variable
GO
    
--G
GO
CREATE PROCEDURE Account_Highest_Voucher
@MobileNo char(11),
@Voucher_id int OUTPUT
AS
SET @Voucher_id =
(SELECT top 1 v.voucherID
FROM Voucher v
INNER JOIN Customer_Account a ON v.mobileNo = a.mobileNo 
WHERE a.mobileNo = @MobileNo
ORDER BY v.value DESC)
GO

--H
GO
CREATE FUNCTION Remaining_plan_amount
(@MobileNo char(11),@plan_name varchar(50))
RETURNS decimal(10,2)
AS
BEGIN
DECLARE @Remaining_amount decimal(10,2)
set @Remaining_amount = (
CASE 
    WHEN        
        (SELECT price FROM Service_Plan WHERE name = @plan_name) > 
        (SELECT amount FROM Payment  WHERE mobileNo = @MobileNo AND date_of_payment = (select max(date_of_payment) from payment where mobileNo = @MobileNo )) 
    THEN  
		(SELECT price FROM Service_Plan WHERE name = @plan_name)-
		(SELECT amount FROM Payment  WHERE mobileNo = @MobileNo AND date_of_payment = (select max(date_of_payment) from payment where mobileNo = @MobileNo )) 
    ELSE 0
END
)
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
(CASE 
    WHEN        
        (SELECT price FROM Service_Plan WHERE name = @plan_name) < 
        (SELECT amount FROM Payment  WHERE mobileNo = @MobileNo AND date_of_payment = (select max(date_of_payment) from payment where mobileNo = @MobileNo )) 
    THEN  
		(SELECT amount FROM Payment  WHERE mobileNo = @MobileNo AND date_of_payment = (select max(date_of_payment) from payment where mobileNo = @MobileNo )) -
		(SELECT price FROM Service_Plan WHERE name = @plan_name)
	ELSE 0
END
)
RETURN @Extra_amount
END
GO
    
--J 
GO
CREATE PROCEDURE Top_Successful_Payments
@MobileNo char(11)
AS
Select top 10 amount  from Payment AS p 
where p.mobileNo = @MobileNo and p.status = 'successful'
order by amount desc
GO

--K
GO
CREATE FUNCTION Subscribed_plans_5_Months (
    @MobileNo char(11)
)
RETURNS TABLE  
AS
RETURN (SELECT sp.* 
FROM
Service_Plan AS sp , Subscription AS s 
where sp.planID = s.planID and s.mobileNo = @MobileNo and s.subscription_date >= DATEADD(MONTH, -5, GETDATE()))
GO


--L
GO
CREATE PROCEDURE Initiate_plan_payment
@MobileNo char(11) ,
@amount decimal(10,1),
@payment_method varchar(50),
@plan_id int
AS

-- in the description of the procedure it is mentioned that the payment is accepted so i guess we should not look at whether the payment is suffecient or not and straight away update and insert the values

INSERT INTO Payment (amount, date_of_payment, payment_method, status, mobileNo) Values(@amount, GETDATE(), @payment_method, 'successful', @MobileNo )
update Subscription 
set status = 'active' 
WHERE mobileNo = @MobileNo and planID = @plan_id
GO

--M
-- Cashback is calculated AS 10% of the payment amount.

GO 
CREATE proc Payment_wallet_cashback
@MobileNo char(11),
@payment_id int,
@benefit_id int
AS

if exists (Select *
    FROM Benefits b
    WHERE b.mobileNo = @MobileNo and b.benefitID = @benefit_id and b.validity_date >=  GETDATE())

    begin

    declare @paymentAmount int
    declare @walletID int

    SELECT @paymentAmount = Payment.amount
    FROM Payment
    WHERE Payment.paymentID = @payment_id


    SELECT @walletID = Wallet.walletID
    FROM Customer_Account, Wallet 
    WHERE Customer_Account.mobileNo = @MobileNo and Wallet.nationalID = Customer_Account.nationalID


    UPDATE Cashback
    SET amount = CAST(@paymentAmount * 0.1 AS INT)
    WHERE walletID = @walletID AND benefitID = @benefit_id
    
    UPDATE Wallet
    SET current_balance =   current_balance + (CAST(@paymentAmount * 0.1 AS INT))
    WHERE walletID = @walletID 

    END
else 
    print 'Couldnt find cashback'
GO
--change from insert to update(done)

--N
GO
CREATE PROCEDURE Initiate_balance_payment
@MobileNo char(11) ,
@amount decimal(10,1),
@payment_method varchar(50)
AS
INSERT INTO Payment(amount, date_of_payment, payment_method, status,MobileNo )
     VALUES(@amount , GETDATE() , @payment_method , 'successful', @MobileNo)

GO

--O
GO
CREATE PROCEDURE Redeem_voucher_points
@MobileNo char(11), @voucher_id int
AS
DECLARE @VoucherPoints INT
DECLARE @AccountPoints int

SELECT @VoucherPoints =  voucher.points
FROM Voucher 
WHERE Voucher.voucherID = @voucher_id


SELECT @AccountPoints =  Customer_Account.point
FROM Customer_Account
WHERE Customer_Account.MobileNo = @MobileNo


UPDATE Customer_Account
SET point = @AccountPoints + @VoucherPoints
WHERE mobileNo = @MobileNo


UPDATE Voucher
SET redeem_date = getDate()
where voucherID =  @voucher_id 
GO
