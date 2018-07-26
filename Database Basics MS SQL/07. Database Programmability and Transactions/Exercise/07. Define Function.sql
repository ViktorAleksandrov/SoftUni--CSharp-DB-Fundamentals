CREATE FUNCTION ufn_IsWordComprised (@setOfLetters VARCHAR(MAX), @word VARCHAR(MAX))
RETURNS BIT
AS
BEGIN
	DECLARE @currentIndex INT = 1;
	DECLARE @currentChar CHAR;

	WHILE (@currentIndex <= LEN(@word))
	BEGIN
		SET @currentChar = SUBSTRING(@word, @currentIndex, 1);

		IF (CHARINDEX(@currentChar, @setOfLetters) = 0)
		BEGIN
			RETURN 0;
		END

		SET @currentIndex += 1;
	END

	RETURN 1;
END