CREATE DATABASE Movies

CREATE TABLE Directors (
	Id INT PRIMARY KEY IDENTITY,
	DirectorName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Genres (
	Id INT PRIMARY KEY IDENTITY,
	GenreName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Categories (
	Id INT PRIMARY KEY IDENTITY,
	CategoryName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Movies (
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(50) NOT NULL,
	DirectorId INT FOREIGN KEY REFERENCES Directors(Id) NOT NULL,
	CopyrightYear INT NOT NULL,
	[Length] INT NOT NULL,
	GenreId INT FOREIGN KEY REFERENCES Genres(Id) NOT NULL,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	Rating DECIMAL(2, 1) NOT NULL,
	Notes NVARCHAR(MAX)
)

INSERT INTO Directors (DirectorName, Notes)
VALUES
('Steven Spielberg', NULL),
('Steven Soderbergh', NULL),
('James Cameron', NULL),
('Chris Nolan', NULL),
('George Lucas', NULL)

INSERT INTO Genres (GenreName, Notes)
VALUES
('thriller', NULL),
('sci-fi', NULL),
('fantasy', NULL),
('comedy', NULL),
('mistery', NULL)

INSERT INTO Categories(CategoryName, Notes)
VALUES
('Movie', NULL),
('TV Series', NULL),
('Documentary', NULL),
('Reality Show', NULL),
('Short film', NULL)

INSERT INTO Movies (Title, DirectorId, CopyrightYear, [Length], GenreId, CategoryId, Rating, Notes)
VALUES
('Terminator', 1, 1984, 110, 1, 1, 8.4, NULL),
('Conan', 2, 1982, 115, 2, 2, 7.4, NULL),
('Avatar', 3, 2009, 160, 3, 3, 7.8, NULL),
('Inception', 4, 2010, 150, 4, 4, 8.8, NULL),
('Hulk', 5, 2003, 130, 5, 5, 5.4, NULL)