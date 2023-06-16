--create database ProductNews
--use ProductNews

CREATE TABLE Accounts (
	AccountID int identity PRIMARY KEY,
	Username nvarchar(50),
	[Password] nvarchar(50),
	FirstName nvarchar(50),
	LastName nvarchar(50),
	Gender bit,
	DateOfBirth datetime,
	PhoneNumber char(10),
	Avatar nvarchar(max),
	[Status] bit
)

CREATE TABLE NewsGroup (
	NewsGroupID int identity PRIMARY KEY,
	NewsGroupName nvarchar(100),
	CreatedDate datetime,
	ModifiedDate datetime,
	CreatedBy int FOREIGN KEY REFERENCES Accounts(AccountID),
	ModifiedBy int FOREIGN KEY REFERENCES Accounts(AccountID),
	ModifiedHistory ntext, 
	[Status] bit
)

CREATE TABLE News (
	[NewID] int identity PRIMARY KEY,
	NewsGroupID int FOREIGN KEY REFERENCES NewsGroup(NewsGroupID),
	NewTitle nvarchar(max),
	NewsHeading nvarchar(max),
	NewsPreviewImage nvarchar(max),
	NewsContent ntext,
	[View] int,
	CreatedDate datetime,
	ModifiedDate datetime,
	CreatedBy int FOREIGN KEY REFERENCES Accounts(AccountID),
	ModifiedBy int FOREIGN KEY REFERENCES Accounts(AccountID),
	ModifiedHistory ntext,
	[Status] bit
)

CREATE TABLE Customers (
	CustomerID int identity PRIMARY KEY,
	FirstName nvarchar(50),
	LastName nvarchar(50),
	Gender bit,
	Email nvarchar(100),
	UserName nvarchar(100),
	[Password] nvarchar(100),
	[Address] nvarchar(100),
	Phone nvarchar(15),
	CreatedDate datetime,
	Avatar nvarchar(max),
	ModifiedHistory ntext,
	[Status] bit
)