INSERT INTO Country (Name)
VALUES ('Spain'),
       ('Italy'),
       ('France'),
       ('Greece'),
       ('Portugal'),
       ('Germany'),
       ('Croatia'),
       ('Turkey'),
       ('Egypt'),
       ('Thailand');

INSERT INTO Trip (Name, Description, DateFrom, DateTo, MaxPeople)
VALUES ('Mediterranean Cruise', 'Explore the beautiful Mediterranean coast', '2025-06-15', '2025-06-25', 100),
       ('Paris Weekend', 'Romantic weekend in the city of love', '2025-05-10', '2025-05-12', 20),
       ('Greek Island Hopping', 'Visit multiple Greek islands in one trip', '2025-07-01', '2025-07-12', 30),
       ('Tuscany Wine Tour', 'Taste the finest wines of Tuscany', '2025-09-05', '2025-09-12', 15),
       ('Egyptian Pyramids', 'Explore ancient Egyptian history', '2025-10-10', '2025-10-20', 25),
       ('Thai Beach Retreat', 'Relax on pristine Thai beaches', '2025-11-15', '2025-11-29', 40),
       ('Alpine Skiing', 'Ski trip to the Alps', '2026-01-10', '2026-01-17', 30),
       ('Spanish Fiesta', 'Experience Spanish culture and nightlife', '2025-08-01', '2025-08-10', 25),
       ('Adriatic Sailing', 'Sail along the Adriatic coastline', '2025-06-20', '2025-06-30', 12),
       ('Turkish Delight', 'Cultural tour of Turkey', '2025-05-20', '2025-05-30', 22);

INSERT INTO Country_Trip (IdCountry, IdTrip)
VALUES
-- Mediterranean Cruise (Spain, Italy, France)
(1, 1),
(2, 1),
(3, 1),
-- Paris Weekend (France)
(3, 2),
-- Greek Island Hopping (Greece)
(4, 3),
-- Tuscany Wine Tour (Italy)
(2, 4),
-- Egyptian Pyramids (Egypt)
(9, 5),
-- Thai Beach Retreat (Thailand)
(10, 6),
-- Alpine Skiing (Germany, France)
(6, 7),
(3, 7),
-- Spanish Fiesta (Spain)
(1, 8),
-- Adriatic Sailing (Croatia)
(7, 9),
-- Turkish Delight (Turkey)
(8, 10);

INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
VALUES ('Jan', 'Kowalski', 'jan.kowalski@example.com', '+48123456789', '90010112345'),
       ('Anna', 'Nowak', 'anna.nowak@example.com', '+48234567890', '92020223456'),
       ('Piotr', 'Wiśniewski', 'piotr.wisniewski@example.com', '+48345678901', '85030334567'),
       ('Katarzyna', 'Dąbrowska', 'katarzyna.dabrowska@example.com', '+48456789012', '88040445678'),
       ('Michał', 'Lewandowski', 'michal.lewandowski@example.com', '+48567890123', '87050556789'),
       ('Magdalena', 'Wójcik', 'magdalena.wojcik@example.com', '+48678901234', '91060667890'),
       ('Tomasz', 'Kamiński', 'tomasz.kaminski@example.com', '+48789012345', '86070778901'),
       ('Aleksandra', 'Kowalczyk', 'aleksandra.kowalczyk@example.com', '+48890123456', '94080889012'),
       ('Marcin', 'Zieliński', 'marcin.zielinski@example.com', '+48901234567', '89090990123'),
       ('Monika', 'Szymańska', 'monika.szymanska@example.com', '+48012345678', '93101001234');

INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt, PaymentDate)
VALUES (1, 1, '2025-05-01', '2025-05-05'),  -- Jan on Mediterranean Cruise
       (1, 3, '2025-05-10', '2025-05-15'),  -- Jan on Greek Island Hopping
       (2, 2, '2025-04-15', '2025-04-17'),  -- Anna on Paris Weekend
       (2, 8, '2025-06-20', NULL),          -- Anna on Spanish Fiesta (not paid yet)
       (3, 5, '2025-06-01', '2025-06-10'),  -- Piotr on Egyptian Pyramids
       (4, 6, '2025-07-15', '2025-07-20'),  -- Katarzyna on Thai Beach Retreat
       (5, 4, '2025-08-01', '2025-08-05'),  -- Michał on Tuscany Wine Tour
       (6, 7, '2025-11-01', '2025-11-10'),  -- Magdalena on Alpine Skiing
       (7, 9, '2025-05-01', '2025-05-10'),  -- Tomasz on Adriatic Sailing
       (8, 10, '2025-03-01', '2025-03-10'), -- Aleksandra on Turkish Delight
       (9, 1, '2025-05-02', '2025-05-12'),  -- Marcin on Mediterranean Cruise
       (10, 3, '2025-06-01', '2025-06-15'); -- Monika on Greek Island Hopping