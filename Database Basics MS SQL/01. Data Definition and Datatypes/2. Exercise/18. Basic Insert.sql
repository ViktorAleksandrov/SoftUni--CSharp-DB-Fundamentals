INSERT INTO Towns ([Name])
VALUES
('Sofia'),
('Plovdiv'),
('Varna'),
('Burgas')

INSERT INTO Departments ([Name])
VALUES
('Engineering'),
('Sales'),
('Marketing'),
('Software Development'),
('Quality Assurance')

INSERT INTO Employees (FirstName, MiddleName, LastName, JobTitle, DepartmentId, HireDate, Salary, AddressId)
VALUES
('Ivan', 'Ivanov', 'Ivanov', '.NET Developer', 4, CONVERT(DATE, '01/02/2013', 103), 3500, NULL),
('Petar', 'Petrov', 'Petrov', 'Senior Engineer', 1, CONVERT(DATE, '02/03/2004', 103), 4000, NULL),
('Maria', 'Petrova', 'Ivanova', 'Intern', 5, CONVERT(DATE, '28/08/2016', 103), 525.25, NULL),
('Georgi', 'Teziev', 'Ivanov', 'CEO', 2, CONVERT(DATE, '09/12/2007', 103), 3000, NULL),
('Peter', 'Pan', 'Pan', 'Intern', 3, CONVERT(DATE, '28/08/2016', 103), 599.88, NULL)