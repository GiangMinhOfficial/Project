--create database ProductNews
--use ProductNews
--drop database ProductNews

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
	IsDelete bit
)

CREATE TABLE NewsGroup (
	NewsGroupID int identity PRIMARY KEY,
	NewsGroupName nvarchar(100),
	CreatedDate datetime,
	ModifiedDate datetime,
	CreatedBy int FOREIGN KEY REFERENCES Accounts(AccountID),
	ModifiedBy int FOREIGN KEY REFERENCES Accounts(AccountID),
	ModifiedHistory ntext, 
	IsDelete bit
)

CREATE TABLE News (
	[NewsID] int identity PRIMARY KEY,
	NewsGroupID int FOREIGN KEY REFERENCES NewsGroup(NewsGroupID),
	NewsTitle nvarchar(max),
	NewsHeading nvarchar(max),
	NewsPreviewImage nvarchar(max),
	NewsContent ntext,
	[View] int,
	CreatedDate datetime,
	ModifiedDate datetime,
	CreatedBy int FOREIGN KEY REFERENCES Accounts(AccountID),
	ModifiedBy int FOREIGN KEY REFERENCES Accounts(AccountID),
	ModifiedHistory ntext,
	IsDelete bit
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
	IsDelete bit
)

CREATE TABLE Comments (
	CommentId int identity primary key,
	[NewsID] int foreign key references News([NewsID]),
	CustomerId int foreign key references Customers(CustomerID),
	Content ntext,
	CreatedDate datetime,
	UpdatedDate datetime,
	[Status] int, -- trạng thái cmt, được duyệt hoặc đang duyệt hoặc từ chối duyệt
	IsDelete bit default 0
)

CREATE TABLE Evaluation (
	EvaluationId int identity primary key,
	[NewsID] int foreign key references News([NewsID]),
	CustomerId int foreign key references Customers(CustomerID),
	Content ntext,
	CreatedDate datetime,
	UpdatedDate datetime,
	Rating int check (rating <= 5 and rating >= 1),
	[Status] int, -- trạng thái cmt, được duyệt hoặc đang duyệt hoặc từ chối duyệt
	IsDelete bit default 0
)