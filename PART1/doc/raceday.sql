CREATE DATABASE RACEDAY;
GO

USE RACEDAY;
GO

Create Table Users(
userID INT IDENTITY(1,1) PRIMARY KEY,
username VARCHAR (100) NOT NULL UNIQUE,
userEmail VARCHAR(150) UNIQUE CHECK(userEmail LIKE '%@%.%'),
userRole varchar(20) NOT NULL
  CHECK(userRole IN('Participant', 'Organiser')),
userPassword VARCHAR(150)
  CHECK(userPassword LIKE '%[A-Za-z]%'
  AND userPassword LIKE '%[0-9]%'
  AND userPassword LIKE '%[!@#$%^&*()_+=~-]%')
);

CREATE TABLE Participants (
    participantID INT IDENTITY(1,1) PRIMARY KEY,
    participantName VARCHAR(50) NOT NULL,
    participantEmail VARCHAR(100) UNIQUE NOT NULL,
    participantPhone VARCHAR(20) UNIQUE NOT NULL,
    userID INT NOT NULL FOREIGN KEY REFERENCES Users(userID)
);

CREATE TABLE Organiser (
    organiserID INT IDENTITY(1,1) PRIMARY KEY,
    organiserName VARCHAR(50) NOT NULL,
    organiserEmail VARCHAR(100) UNIQUE NOT NULL,
    organiserPhone VARCHAR(20) UNIQUE NOT NULL,
    userID INT NOT NULL FOREIGN KEY REFERENCES Users(userID)
);

--changed event to eventgig as event(s) is a command
CREATE TABLE EventGig (
    eventID INT IDENTITY(1,1) PRIMARY KEY,
    eventName VARCHAR(100) NOT NULL,
    eventDescription VARCHAR(255) NOT NULL,
    eventDate DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    eventLocation VARCHAR(100) NOT NULL,
    eventDistance VARCHAR(50) NOT NULL,
    eventType VARCHAR(20) NOT NULL CHECK (eventType IN ('Run', 'Walk', 'Cycle')),
    eventRoute VARCHAR(250) NULL,
    eventMap VARCHAR(MAX) NULL,
    organiserID INT NOT NULL FOREIGN KEY REFERENCES Organiser(organiserID)
);

CREATE TABLE Categories (
    categoryID INT IDENTITY(1,1) PRIMARY KEY,
    categoryAge INT NOT NULL,
    categoryDistance VARCHAR(5) NOT NULL,
    eventID int FOREIGN KEY REFERENCES EventGig(eventID)
);

CREATE TABLE EventEnrollment (
    enrollmentID INT IDENTITY(1,1) PRIMARY KEY,
    enrollmentDate DATETIME NOT NULL DEFAULT GETDATE(),
    eventID int FOREIGN KEY REFERENCES EventGig(eventID),
    participantID int FOREIGN KEY REFERENCES Participants(participantID),
    categoryID int FOREIGN KEY REFERENCES Categories(categoryID)
);

CREATE TABLE Results (
    resultID INT IDENTITY(1,1) PRIMARY KEY,
    eventID int FOREIGN KEY REFERENCES EventGig(eventID),
    participantTime TIME NOT NULL DEFAULT '0',
    participantID INT FOREIGN KEY REFERENCES Participants(participantID),
    participantPosition VARCHAR(5) NOT NULL UNIQUE
);

INSERT INTO Users(userName, userEmail, userRole, userPassword) VALUES
('John Smith', 'john.smith@raceday.com', 'Organiser', 'HashedPassword#486'),
('Sarah Williams', 'sarah.williams@raceday.com', 'Organiser', 'HashedPassword#752'),
('Michael Brown', 'michael.brown@email.com', 'Participant', 'HashedPassword#932'),
('Emily Jones', 'emily.jones@email.com', 'Participant', 'HashedPassword#021')

INSERT INTO Participants(userID, participantName, participantEmail, participantPhone) VALUES
(3, 'Michael Brown', 'michael.brown@email.com', '0835552001'),
(4, 'Emily Jones', 'emily.jones@email.com', '0835552002');

INSERT INTO Organiser (userID, organiserName, organiserEmail, organiserPhone) VALUES
(1, 'John Smith', 'john.smith@raceday.com', '0825551001'),
(2, 'Sarah Williams', 'sarah.williams@raceday.com', '0825551002');

INSERT INTO EventGig(eventName, eventDescription, eventDate, eventLocation, eventDistance, eventType, organiserID) VALUES
('Cape Town City Run', 'Annual city running event', '2026-09-12', 'Cape Town', '10KM', 'Run', 1),
('Johannesburg Fun Run','Community running event','2026-10-03','Johannesburg','5KM','Walk',2),
('Durban Beach Challenge','Beachside endurance event','2026-11-14','Durban','15KM','Cycle',1);

INSERT INTO Categories(categoryAge, categoryDistance, eventID) VALUES
(18, '10KM', 1),
(30, '10KM', 1),
(50, '10KM', 1),
(18, '5KM', 2),
(30, '5KM', 2),
(50, '5KM', 2),
(18, '15KM', 3),
(30, '15KM', 3),
(50, '15KM', 3);

INSERT INTO EventEnrollment (eventID, participantID, categoryID) VALUES
(1, 3, 1),
(1, 2, 2),
(2, 3, 4),
(2, 2, 5),
(3, 3, 7),
(3, 2, 8);

INSERT INTO Results(participantTime, participantID, participantPosition) VALUES
('00:52:35', 3, '1'),
('00:58:12', 2, '2');

  
SELECT * FROM Organiser;
SELECT * FROM Participants;
SELECT * FROM Users;
SELECT * FROM EventGig;
SELECT * FROM Categories;
SELECT * FROM EventEnrollment;
SELECT * FROM Results;

drop table results; 
drop table EventEnrollment; 
drop table EventGig; 
drop table Categories; 
drop table Users; 
drop table Participants; 
drop table Organiser; 