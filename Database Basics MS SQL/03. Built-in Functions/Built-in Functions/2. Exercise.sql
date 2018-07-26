--01. Find Names of All Employees by First Name
SELECT FirstName, LastName
  FROM Employees
 WHERE FirstName LIKE 'SA%'

--02. Find Names of All Employees by Last Name
SELECT FirstName, LastName
  FROM Employees
 WHERE LastName LIKE '%ei%'

--03. Find First Names of All Employess
SELECT FirstName
  FROM Employees
 WHERE DepartmentID IN(3, 10)
 AND YEAR(HireDate) BETWEEN 1995 AND 2005

--04. Find All Employees Except Engineers
SELECT FirstName, LastName
  FROM Employees
 WHERE JobTitle NOT LIKE '%engineer%'

--05. Find Towns with Name Length
SELECT [Name]
  FROM Towns
 WHERE LEN([Name]) IN(5, 6)
ORDER BY [Name]

--06. Find Towns Starting With
SELECT *
  FROM Towns
 WHERE LEFT([Name], 1) LIKE '[MKBE]'
ORDER BY [Name]

--07. Find Towns Not Starting With
SELECT *
  FROM Towns
 WHERE LEFT([Name], 1) NOT LIKE '[RBD]'
ORDER BY [Name]

--08. Create View Employees Hired After
CREATE VIEW V_EmployeesHiredAfter2000 AS
SELECT FirstName, LastName
  FROM Employees
 WHERE YEAR(HireDate) > 2000

--09. Length of Last Name
SELECT FirstName, LastName
  FROM Employees
 WHERE LEN(LastName) = 5

--10. Countries Holding 'A'
SELECT CountryName, IsoCode
  FROM Countries
 WHERE CountryName LIKE '%A%A%A%'
ORDER BY IsoCode

--11. Mix of Peak and River Names
SELECT PeakName, RiverName, 
	   LOWER(PeakName + RIGHT(RiverName, LEN(RiverName) - 1)) AS Mix
  FROM Peaks, Rivers
 WHERE RIGHT(PeakName, 1) = LEFT(RiverName, 1)
ORDER BY Mix

--12. Games From 2011 and 2012 Year
SELECT TOP (50) [Name], FORMAT([Start], 'yyyy-MM-dd') AS [Start]
  FROM Games
 WHERE YEAR([Start]) IN(2011, 2012)
ORDER BY [Start], [Name]

--13. User Email Providers
SELECT Username, 
	   RIGHT(Email, LEN(Email) - CHARINDEX('@', Email)) AS [Email Provider]
  FROM Users
ORDER BY [Email Provider], Username

--14. Get Users with IPAddress Like Pattern
SELECT Username, IpAddress
  FROM Users
WHERE IpAddress LIKE '___.1%.%.___'
ORDER BY Username

--15. Show All Games with Duration
SELECT [Name] AS Game, 
	   [Part of the Day] = 
		CASE
			WHEN DATEPART(HOUR, [Start]) < 12 THEN 'Morning'
			WHEN DATEPART(HOUR, [Start]) BETWEEN 12 AND 17 THEN 'Afternoon'
			ELSE 'Evening'
	    END,
		Duration = 
		CASE
			WHEN Duration <= 3 THEN 'Extra Short'
			WHEN Duration BETWEEN 4 AND 6 THEN 'Short'
			WHEN Duration > 6 THEN 'Long'
			ELSE 'Extra Long'
		END
  FROM Games
ORDER BY Game, Duration, [Part of the Day]

--16. Orders Table
SELECT ProductName, Orderdate, 
	   DATEADD(DAY, 3, Orderdate) AS [Pay Due], 
	   DATEADD(MONTH, 1, OrderDate) AS [Deliver Due]
  FROM Orders

--17. People Table
CREATE TABLE People (
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,
	Birthdate DATETIME NOT NULL
)

INSERT INTO People ([Name], Birthdate)
VALUES ('Victor', '2000/12/07'),
       ('Steven', '1992/09/10'),
	   ('Stephen', '1910/09/19'),
	   ('John', '2010/01/06')

SELECT [Name],
	   DATEDIFF(YEAR, Birthdate, GETDATE()) AS [Age in Years],
	   DATEDIFF(MONTH, Birthdate, GETDATE()) AS [Age in Months],
	   DATEDIFF(DAY, Birthdate, GETDATE()) AS [Age in Days],
	   DATEDIFF(MINUTE, Birthdate, GETDATE()) AS [Age in Minutes]
  FROM People