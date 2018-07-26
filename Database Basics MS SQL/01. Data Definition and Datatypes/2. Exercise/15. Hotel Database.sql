CREATE DATABASE Hotel

CREATE TABLE Employees (
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Title NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Customers (
	AccountNumber INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	PhoneNumber NVARCHAR(20) NOT NULL,
	EmergencyName NVARCHAR(50),
	EmergencyNumber NVARCHAR(20) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE RoomStatus (
	RoomStatus NVARCHAR(50) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE RoomTypes (
	RoomType NVARCHAR(50) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE BedTypes (
	BedType NVARCHAR(50) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Rooms (
	RoomNumber INT PRIMARY KEY NOT NULL,
	RoomType NVARCHAR(50) FOREIGN KEY REFERENCES RoomTypes(Roomtype) NOT NULL,
	BedType NVARCHAR(50) FOREIGN KEY REFERENCES BedTypes (BedType) NOT NULL,
	Rate DECIMAL(10, 2) NOT NULL,
	RoomStatus NVARCHAR(50) FOREIGN KEY REFERENCES RoomStatus(RoomStatus) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Payments (
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	PaymentDate DATE NOT NULL,
	AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber) NOT NULL,
	FirstDateOccupied DATE NOT NULL,
	LastDateOccupied DATE NOT NULL,
	TotalDays AS DATEDIFF(DAY, FirstDateOccupied, LastDateOccupied),
	AmountCharged DECIMAL(10, 2) NOT NULL,
	TaxRate DECIMAL(10, 2),
	TaxAmount DECIMAL(10, 2),
	PaymentTotal DECIMAL(10, 2) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Occupancies (
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	DateOccupied DATE NOT NULL,
	AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber) NOT NULL,
	RoomNumber INT FOREIGN KEY REFERENCES Rooms(RoomNumber) NOT NULL,
	RateApplied DECIMAL(10, 2) NOT NULL,
	PhoneCharge DECIMAL(10, 2),
	Notes NVARCHAR(MAX)
)

INSERT INTO Employees (FirstName, LastName, Title, Notes) 
VALUES 
('Ivan', 'Ivanov', 'Receptionist', NULL),
('Martin', 'Martinov', 'Technical support', NULL),
('Maria', 'Marieva', 'Cleaner', NULL)

INSERT INTO Customers (FirstName, LastName, PhoneNumber, EmergencyName, EmergencyNumber, Notes) 
VALUES 
('Pesho', 'Peshov', '+395883333333', NULL, '123', NULL),
('Gosho', 'Goshev', '+395882222222', NULL, '123', NULL),
('Ivan', 'Ivanov', '+395888888888', NULL, '123', NULL)

INSERT INTO RoomStatus (RoomStatus, Notes)
VALUES
('Occupied', NULL),
('Vacant', 'Needs cleaning!'),
('Uninhabitable', 'Needs repairs')

INSERT INTO RoomTypes(RoomType, Notes)
VALUES
('Small', 'Room with one bed'),
('Medium', 'Room with two beds'),
('Large', 'Room with three beds')

INSERT INTO BedTypes(BedType, Notes)
VALUES
('Single', NULL),
('Double', NULL),
('VIP', NULL)

INSERT INTO Rooms(RoomNumber, RoomType, BedType, Rate, RoomStatus, Notes)
VALUES
(12, 'Small', 'Double', 50.00, 'Uninhabitable', 'Dirty'),
(13, 'Medium', 'Single', 70.99, 'Occupied', 'Clean'),
(333, 'Large', 'VIP', 100.00, 'Vacant', 'Repair')

INSERT INTO Payments (EmployeeId, PaymentDate, AccountNumber, FirstDateOccupied, LastDateOccupied, AmountCharged, TaxRate, TaxAmount, PaymentTotal, Notes)
VALUES
(1, CONVERT(DATE, '15-02-2017', 103), 3, CONVERT(DATE, '20-02-2017', 103), CONVERT(DATE, '25-02-2017', 103), 2568.36, NULL, NULL, 2763.48, NULL),
(3, CONVERT(DATE, '23-03-2017', 103), 2, CONVERT(DATE, '18-11-2017', 103), CONVERT(DATE, '18-12-2017', 103), 5218.00, NULL, NULL, 5586.39, NULL),
(2, CONVERT(DATE, '15-02-2017', 103), 1, CONVERT(DATE, '01-01-2018', 103), CONVERT(DATE, '20-01-2017', 103), 4158.45, NULL, NULL, 4395.80, NULL)

INSERT INTO Occupancies (EmployeeId, DateOccupied, AccountNumber, RoomNumber, RateApplied, PhoneCharge, Notes)
VALUES
(2, CONVERT(DATE, '18-12-2017', 103), 3, 12, 7.99, NULL, NULL),
(3, CONVERT(DATE, '09-09-2018', 103), 2, 333, 9.99, NULL, NULL),
(1, CONVERT(DATE, '10-06-2016', 103), 1, 13, 20.00, NULL, NULL)