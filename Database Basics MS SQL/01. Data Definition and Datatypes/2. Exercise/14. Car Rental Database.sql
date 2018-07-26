CREATE DATABASE CarRental

CREATE TABLE Categories (
	Id INT PRIMARY KEY IDENTITY,
	CategoryName NVARCHAR(50) NOT NULL,
	DailyRate DECIMAL(15, 2),
	WeeklyRate DECIMAL(15, 2),
	MonthlyRate DECIMAL(15, 2),
	WeekendRate DECIMAL(15, 2)
)

CREATE TABLE Cars (
	Id INT PRIMARY KEY IDENTITY,
	PlateNumber NVARCHAR(10) NOT NULL,
	Manufacturer NVARCHAR(20) NOT NULL,
	Model NVARCHAR(20) NOT NULL,
	CarYear INT NOT NULL,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	Doors INT NOT NULL,
	Picture VARBINARY(MAX),
	Condition NVARCHAR(50),
	Available BIT NOT NULL
)

CREATE TABLE Employees (
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Title NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Customers (
	Id INT PRIMARY KEY IDENTITY,
	DriverLicenceNumber NVARCHAR(50) NOT NULL,
	FullName NVARCHAR(50) NOT NULL,
	[Address] NVARCHAR(50) NOT NULL,
	City NVARCHAR(50) NOT NULL,
	ZIPCode INT,
	Notes NVARCHAR(MAX)
)

CREATE TABLE RentalOrders (
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	CustomerId INT FOREIGN KEY REFERENCES Customers(Id) NOT NULL,
	CarId INT FOREIGN KEY REFERENCES Cars(Id) NOT NULL,
	TankLevel INT NOT NULL,
	KilometrageStart INT NOT NULL,
	KilometrageEnd INT NOT NULL,
	TotalKilometrage AS KilometrageEnd - KilometrageStart,
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL,
	TotalDays AS DATEDIFF(DAY, StartDate, EndDate),
	RateApplied DECIMAL(15, 2) NOT NULL,
	TaxRate DECIMAL(15, 2) NOT NULL,
	OrderStatus NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)

INSERT INTO Categories (CategoryName, DailyRate, WeeklyRate, MonthlyRate, WeekendRate)
VALUES
('Economy', 15.99, 50, 1000.00, 40.00),
('Standard', 80.99, 500, 2000, 200),
('Premium', 100.99, NULL, NULL, NULL)

INSERT INTO Cars (PlateNumber, Manufacturer, Model, CarYear, CategoryId, Doors, Picture, Condition, Available)
VALUES
('H2251BP', 'Honda', 'CR-V', 2008, 1, 5, NULL, 'Good', 1),
('T9816', 'Audi', '80', 1989, 1, 5, NULL, 'Excelent', 1),
('H0483BB', 'Ford', 'Mondeo', 2009, 1, 5, NULL, 'Average', 1)

INSERT INTO Employees (FirstName, LastName, Title, Notes)
VALUES
('Pesho', 'Petrov', 'Sales Manager', NULL),
('Georgi', 'Ivanov', 'Sales Person', NULL),
('Toncho', 'Tonchev', 'Accountant', NULL)

INSERT INTO Customers(DriverLicenceNumber, FullName, [Address], City, ZIPCode, Notes)
VALUES
('A111111w', 'Gogo Gogov', 'Balkanska str. 8', 'Ruse', 1233, 'Good driver'),
('B222222s', 'Maria Marieva', 'Ivan Vazov 14', 'Sofia', 5678, 'Bad driver'),
('C333333a', 'Strahil Strahilov', 'Viktor str. 10', 'Varna', 5689, NULL)

INSERT INTO RentalOrders (EmployeeId, CustomerId, CarId, TankLevel, KilometrageStart, KilometrageEnd, StartDate, EndDate, RateApplied, TaxRate, OrderStatus, Notes)
VALUES
(1, 1, 1, 54, 2189, 2456, CONVERT(DATE, '18-05-2018', 103), CONVERT(DATE, '19-05-2018', 103), 1.55, 0.20, 'Rented', NULL),
(2, 2, 2, 22, 13565, 14258, CONVERT(DATE, '18-06-2018', 103), CONVERT(DATE, '18-07-2018', 103), 2.99, 1.22, 'Pending', NULL),
(3, 3, 3, 180, 1202, 1964, CONVERT(DATE, '21-05-2018', 103), CONVERT(DATE, '28-05-2018', 103), 9.99, 0.01, 'Closed', NULL)