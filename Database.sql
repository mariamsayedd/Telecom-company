create database Telecom_Team_12

GO

Use Telecom_Team_12

GO
create procedure createAllTables
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

create table Plan_Usage (
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

create table Payment(
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

create table Process_Payment (
    paymentID int primary key identity(1,1),
    planID int,
    remaining_balance int, 
    extra_amount int,
    foreign key(paymentID) references Payment,
    foreign key(planID) references Service_Plan,
    CHECK (
        remaining_balance = 
        CASE 
            WHEN (SELECT amount FROM Payment p WHERE p.paymentID = Process_Payment.paymentID) < 
                    (SELECT price FROM Service_Plan WHERE Service_Plan.planID = Process_Payment.planID)
            THEN 
                (SELECT price FROM Service_Plan WHERE Service_Plan.planID = Process_Payment.planID) - 
                (SELECT amount FROM Payment WHERE Payment.paymentID = Process_Payment.paymentID)
            ELSE 0
        END
    ),
    CHECK (
        additional_amounts = 
        CASE 
            WHEN (SELECT amount FROM Payment WHERE Payment.paymentID = Process_Payment.paymentID) > 
                    (SELECT price FROM Service_Plan WHERE Service_Plan.planID = Process_Payment.planID)
            THEN 
                (SELECT amount FROM Payment WHERE Payment.paymentID = Process_Payment.paymentID) - 
                (SELECT price FROM Service_Plan WHERE Service_Plan.planID = Process_Payment.planID)
            ELSE 0
        END
    )
)

create table Wallet (
    walletID int primary key identity(1,1), 
    current_balance decimal(10,2),
    currency Varchar(50), 
    last_modified_date date, 
    nationalID int, 
    mobileNo char(11),
    foreign key(nationalID) references Customer_Profile
)

create table Transfer_money(
    walletID1 int,
    walletID2 int,
    transfer_id int identity(1,1),
    amount decimal(10,2),
    transfer_date date,
    primary key (walletID1,walletID2,transfer_id),
    foreign key (walletID1,walletID2) references Wallet
)

create table Benefits(
    benefitID int primary key identity(1,1), 
    description Varchar(50), 
    validity_date date, 
    status Varchar(50),
    mobileNo char(11),
    foreign key (mobileNo) references Customer_Account,
    check(status in ('active','expired'))
)

create table Points_Group(
    pointID int identity(1,1), 
    benefitID int, 
    pointsAmount int, 
    PaymentID int,
    primary key (pointID , benefitID),
    foreign key (benefitID) references Benefits,
    foreign key (PaymentID) references Payment
)

create table Exclusive_Offer(
    offerID int identity(1,1), 
    benefitID int, 
    internet_offered int, 
    SMS_offered int,
    minutes_offered int,
    primary key (offerID, benefitID),
    foreign key (benefitID) references Benefits
)

create table Cashback (
    CashbackID int identity(1,1),
    benefitID int, 
    walletID int, 
    amount int,
    credit_date date,
    primary key (CashbackID, benefitID),
    foreign key (benefitID) references Benefits,
    foreign key (walletID) references Wallet
)

create table Plan_Provides_Benefits (
    benefitID int, 
    planID int,
    primary key (benefitID, planID),
    foreign key (benefitID) references Benefits,
    foreign key (planID) references Service_Plan
)

create table Shop (
    shopID int primary key identity(1,1), 
    name varchar(50), 
    category varchar(50)
)

create table Physical_Shop (
    shopID int primary key, 
    address varchar(50), 
    working_hours varchar(50),
    foreign key (shopID) references Shop
)

create table E_shop (
    shopID int primary key, 
    URL varchar(50),
    rating int,
    foreign key (shopID) references Shop
)

create table Voucher(
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

create table Technical_Support_Ticket(
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
create procedure dropAllTables
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

DROP PROCEDURE createAllTables
DROP PROCEDURE dropAllTables
DROP PROCEDURE clearAllTables

GO

GO
create procedure clearAllTables
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



