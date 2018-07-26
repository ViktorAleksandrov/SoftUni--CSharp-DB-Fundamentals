CREATE DATABASE WMS

CREATE TABLE Clients (
	ClientId INT IDENTITY NOT NULL,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	Phone CHAR(12) NOT NULL

	CONSTRAINT PK_Clients 
	PRIMARY KEY (ClientId)
)

CREATE TABLE Mechanics (
	MechanicId INT IDENTITY NOT NULL,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	[Address] VARCHAR(255) NOT NULL

	CONSTRAINT PK_Mechanics 
	PRIMARY KEY (MechanicId)
)

CREATE TABLE Models (
	ModelId INT IDENTITY NOT NULL,
	[Name] VARCHAR(50) NOT NULL

	CONSTRAINT PK_Models 
	PRIMARY KEY (ModelId),

	CONSTRAINT UQ_Models_Name 
	UNIQUE ([Name])
)

CREATE TABLE Jobs (
	JobId INT IDENTITY NOT NULL,
	ModelId INT NOT NULL,
	[Status] VARCHAR(11) CONSTRAINT DF_Jobs_Status DEFAULT 'Pending',
	ClientId INT NOT NULL,
	MechanicId INT,
	IssueDate DATE NOT NULL,
	FinishDate DATE

	CONSTRAINT PK_Jobs 
	PRIMARY KEY (JobId),

	CONSTRAINT FK_Jobs_Models
	FOREIGN KEY (ModelId)
    REFERENCES Models (ModelId),

	CONSTRAINT FK_Jobs_Clients
	FOREIGN KEY (ClientId)
	REFERENCES Clients (ClientId),

	CONSTRAINT FK_Jobs_Mechanics
	FOREIGN KEY (MechanicId)
	REFERENCES Mechanics (MechanicId),

	CONSTRAINT CHK_Jobs_Status
	CHECK ([Status] IN ('Pending', 'In Progress', 'Finished'))
)

CREATE TABLE Orders (
	OrderId INT IDENTITY NOT NULL,
	JobId INT NOT NULL,
	IssueDate DATE,
	Delivered BIT CONSTRAINT DF_Orders_Delivered DEFAULT 0

	CONSTRAINT PK_Orders 
	PRIMARY KEY (OrderId),

	CONSTRAINT FK_Orders_Jobs
	FOREIGN KEY (JobId)
    REFERENCES Jobs (JobId)
)

CREATE TABLE Vendors (
	VendorId INT IDENTITY NOT NULL,
	[Name] VARCHAR(50) NOT NULL

	CONSTRAINT PK_Vendors 
	PRIMARY KEY (VendorId),

	CONSTRAINT UQ_Vendors_Name
	UNIQUE ([Name])
)

CREATE TABLE Parts (
	PartId INT IDENTITY NOT NULL,
	SerialNumber VARCHAR(50) NOT NULL,
	[Description] VARCHAR(255),
	Price DECIMAL(6, 2) NOT NULL,
	VendorId INT NOT NULL,
	StockQty INT CONSTRAINT DF_Parts_StockQty DEFAULT 0

	CONSTRAINT PK_Parts 
	PRIMARY KEY (PartId),

	CONSTRAINT FK_Parts_Vendors
	FOREIGN KEY (VendorId)
    REFERENCES Vendors (VendorId),

	CONSTRAINT UQ_Parts_SerialNumber
	UNIQUE (SerialNumber),

	CONSTRAINT CHK_Parts_Price
	CHECK (Price > 0),

	CONSTRAINT CHK_Parts_StockQty
	CHECK (StockQty >= 0)
)

CREATE TABLE OrderParts (
	OrderId INT NOT NULL,
	PartId INT NOT NULL,
	Quantity INT CONSTRAINT DF_OrderParts_Quantity DEFAULT 1

	CONSTRAINT PK_OrderParts 
	PRIMARY KEY (OrderId, PartId),

	CONSTRAINT FK_OrderParts_Orders
	FOREIGN KEY (OrderId)
    REFERENCES Orders (OrderId),

	CONSTRAINT FK_OrderParts_Parts
	FOREIGN KEY (PartId)
    REFERENCES Parts (PartId),

	CONSTRAINT CHK_OrderParts_Quantity
	CHECK (Quantity > 0)
)

CREATE TABLE PartsNeeded (
	JobId INT NOT NULL,
	PartId INT NOT NULL,
	Quantity INT CONSTRAINT DF_PartsNeeded_Quantity DEFAULT 1

	CONSTRAINT PK_PartsNeeded 
	PRIMARY KEY (JobId, PartId),

	CONSTRAINT FK_PartsNeeded_Jobs
	FOREIGN KEY (JobId)
    REFERENCES Jobs (JobId),

	CONSTRAINT FK_PartsNeeded_Parts
	FOREIGN KEY (PartId)
    REFERENCES Parts (PartId),

	CONSTRAINT CHK_PartsNeeded_Quantity
	CHECK (Quantity > 0)
)