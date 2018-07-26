CREATE TABLE Passports (
	PassportID INT CONSTRAINT PK_Passports PRIMARY KEY IDENTITY (101, 1),
	PassportNumber CHAR(8) NOT NULL
)

CREATE TABLE Persons (
	PersonID INT NOT NULL IDENTITY,
	FirstName VARCHAR(20) NOT NULL,
	Salary DECIMAL(15, 2) NOT NULL,
	PassportID INT NOT NULL UNIQUE
)

INSERT INTO Passports (PassportNumber)
VALUES ('N34FG21B'),
	   ('K65LO4R7'),	   
	   ('ZE657QP2')

INSERT INTO Persons (FirstName, Salary, PassportID)
VALUES ('Roberto', 43300.00, 102),
	   ('Tom', 56100.00, 103),	   
	   ('Yana', 60200.00, 101)

ALTER TABLE Persons
ADD CONSTRAINT PK_Persons
PRIMARY KEY (PersonID)

ALTER TABLE Persons
ADD CONSTRAINT FK_Persons_Passports
FOREIGN KEY (PassportID)
REFERENCES Passports(PassportID)