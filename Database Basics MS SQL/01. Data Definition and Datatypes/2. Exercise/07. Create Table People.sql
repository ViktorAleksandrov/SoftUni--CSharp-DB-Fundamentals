CREATE TABLE People (
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(200) NOT NULL,
	Picture VARBINARY(MAX),
	Height DECIMAL(3, 2),
	[Weight] DECIMAL(5, 2),
	Gender CHAR(1) NOT NULL,
	Birthdate DATE NOT NULL,
	Biography NVARCHAR(MAX),

	CONSTRAINT CHK_Picture
	CHECK (DATALENGTH(Picture) <= 2 * 1024 *1024)
)

INSERT INTO People ([Name], Picture, Height, [Weight], Gender, Birthdate, Biography)
VALUES
('Pesho Petrov', NULL, 1.76, 78.2, 'm', CONVERT(DATE, '15-02-1985', 103), NULL),
('Gosho Georgiev', NULL, 1.83, 108.5, 'm', CONVERT(DATE, '15-05-1975', 103), NULL),
('Misho Dimov', NULL, 1.88, 98.4, 'm', CONVERT(DATE, '15-12-1995', 103), NULL),
('Ivan Ivanov', NULL, 1.72, 74.1, 'm', CONVERT(DATE, '25-02-1945', 103), NULL),
('Penka Dimitrova', NULL, 1.53, 56.3, 'f', CONVERT(DATE, '05-09-1993', 103), NULL)