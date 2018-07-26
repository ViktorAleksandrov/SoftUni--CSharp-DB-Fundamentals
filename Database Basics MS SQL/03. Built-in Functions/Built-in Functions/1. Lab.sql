--Obfuscate CC Numbers
CREATE VIEW v_PublicPaymentInfo AS
SELECT CustomerID, 
	   FirstName, 
	   LastName, 
	   LEFT(PaymentNumber, 6) + REPLICATE('*', 10) AS PaymentNumber
  FROM Customers

SELECT *
  FROM v_PublicPaymentInfo

--Pallets
SELECT 
	CEILING(
		CEILING(
			CAST(Quantity AS FLOAT) / BoxCapacity) / 
		PalletCapacity) 
	AS [Number of pallets]
  FROM Products