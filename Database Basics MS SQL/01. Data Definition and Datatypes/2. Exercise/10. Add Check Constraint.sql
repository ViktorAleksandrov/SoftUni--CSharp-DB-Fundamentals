ALTER TABLE Users
ADD CONSTRAINT CHK_PasswordLength
CHECK (LEN([Password]) >= 5)