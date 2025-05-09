IF OBJECT_ID('Client', 'U') IS NULL
BEGIN
CREATE TABLE Client
(
    IdClient  INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(120) NOT NULL,
    LastName  NVARCHAR(120) NOT NULL,
    Email     NVARCHAR(120) NOT NULL,
    Telephone NVARCHAR(120) NULL,
    Pesel     NVARCHAR(120) NULL
);
END;

IF OBJECT_ID('Trip', 'U') IS NULL
BEGIN
CREATE TABLE Trip
(
    IdTrip      INT PRIMARY KEY IDENTITY(1,1),
    Name        NVARCHAR(120) NOT NULL,
    Description NVARCHAR(220) NULL,
    DateFrom    DATETIME NOT NULL,
    DateTo      DATETIME NOT NULL,
    MaxPeople   INT      NOT NULL
);
END;

IF OBJECT_ID('Country', 'U') IS NULL
BEGIN
CREATE TABLE Country
(
    IdCountry INT PRIMARY KEY IDENTITY(1,1),
    Name      NVARCHAR(120) NOT NULL
);
END;

IF OBJECT_ID('Country_Trip', 'U') IS NULL
BEGIN
CREATE TABLE Country_Trip
(
    IdCountry INT NOT NULL,
    IdTrip    INT NOT NULL,
    PRIMARY KEY (IdCountry, IdTrip),
    FOREIGN KEY (IdCountry) REFERENCES Country (IdCountry),
    FOREIGN KEY (IdTrip) REFERENCES Trip (IdTrip)
);
END;

IF OBJECT_ID('Client_Trip', 'U') IS NULL
BEGIN
CREATE TABLE Client_Trip
(
    IdClient     INT      NOT NULL,
    IdTrip       INT      NOT NULL,
    RegisteredAt DATETIME NOT NULL,
    PaymentDate  DATETIME NULL,
    PRIMARY KEY (IdClient, IdTrip),
    FOREIGN KEY (IdClient) REFERENCES Client (IdClient),
    FOREIGN KEY (IdTrip) REFERENCES Trip (IdTrip)
);
END;