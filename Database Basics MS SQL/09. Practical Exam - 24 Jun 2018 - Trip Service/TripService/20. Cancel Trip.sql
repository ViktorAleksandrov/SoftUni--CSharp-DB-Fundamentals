CREATE TRIGGER tr_CancelTrip
ON Trips
INSTEAD OF DELETE
AS
BEGIN
	UPDATE Trips
	   SET CancelDate = GETDATE()
	  FROM Trips AS t
	  JOIN deleted AS d
	    ON d.Id = t.Id
	 WHERE t.CancelDate IS NULL
END