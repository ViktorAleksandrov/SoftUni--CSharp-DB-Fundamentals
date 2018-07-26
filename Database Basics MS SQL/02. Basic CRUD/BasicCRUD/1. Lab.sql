--Employee Summary
SELECT FirstName + ' ' + LastName 
	AS [Full Name], 
	   JobTitle, 
	   Salary
  FROM Employees

--Highest Peak
CREATE VIEW v_HighestPeak 
AS
SELECT TOP (1) * 
	  FROM Peaks
  ORDER BY Elevation DESC

SELECT * FROM v_HighestPeak

--Update Projects
UPDATE Projects
   SET EndDate = GETDATE()
 WHERE EndDate IS NULL