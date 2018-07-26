CREATE TRIGGER tr_UpdateOrdersDelivery
ON Orders
FOR UPDATE
AS
BEGIN
	DECLARE	@oldStatus BIT = (
		SELECT Delivered
		  FROM deleted
	)

	DECLARE	@newStatus BIT = (
		SELECT Delivered
		  FROM inserted
	)

	IF (@oldStatus = 0 AND @newStatus = 1)
	BEGIN
		UPDATE Parts
		 SET StockQty += op.Quantity
		FROM Parts AS p 
		JOIN OrderParts AS op
		  ON op.PartId = p.PartId
		JOIN Orders AS o
		  ON o.OrderId = op.OrderId
		JOIN inserted AS i
		  ON i.OrderId = o.OrderId
		JOIN deleted AS d
		  ON d.OrderId = i.OrderId
	END
END