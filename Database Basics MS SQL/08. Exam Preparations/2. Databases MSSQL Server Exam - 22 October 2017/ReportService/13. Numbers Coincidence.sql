  SELECT DISTINCT u.Username
    FROM Users AS u
    JOIN Reports AS r
      ON r.UserId = u.Id
   WHERE (LEFT(u.Username, 1) LIKE '[0-9]'
		 AND CAST(r.CategoryId AS NVARCHAR) = LEFT(u.Username, 1))
	  OR (RIGHT(u.Username, 1) LIKE '[0-9]'
		 AND CAST(r.CategoryId AS NVARCHAR) = RIGHT(u.Username, 1))
ORDER BY u.Username