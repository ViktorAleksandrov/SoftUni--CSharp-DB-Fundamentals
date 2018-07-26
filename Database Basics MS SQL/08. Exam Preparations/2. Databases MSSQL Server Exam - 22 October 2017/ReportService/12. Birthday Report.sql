  SELECT DISTINCT c.[Name] AS [Category Name]
    FROM Categories AS c
    JOIN Reports AS r
      ON r.CategoryId = c.Id
    JOIN Users AS u
      ON u.Id = r.UserId
   WHERE MONTH(r.OpenDate) = MONTH(u.BirthDate)
     AND DAY(r.OpenDate) = DAY(u.BirthDate)
ORDER BY [Category Name]