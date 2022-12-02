CREATE TABLE [Users] (
  [User_id] int,
  [FirstName] nvarchar(255),
  [LastName] nvarchar(255),
  [email] nvarchar(255),
  [phone] nvarchar(255),
  [password] nvarchar(255),
  [is_admin] boolean,
  [is_student] boolean,
  [is_elder] boolean,
  PRIMARY KEY ([User_id])
)
GO

CREATE TABLE [Station] (
  [Station_id] int,
  [Name] nvarchar(255),
  [Desctiption] nvarchar(255),
  PRIMARY KEY ([Station_id])
)
GO

CREATE TABLE [Line] (
  [Line_id] int,
  [Name] nvarchar(255),
  PRIMARY KEY ([Line_id])
)
GO

CREATE TABLE [LineStations] (
  [LineStations_id] int,
  [Line_id] int,
  [Station_id] int,
  [Distance] int,
  [DepartureTime] time,
  [ArrivalTime] time,
  PRIMARY KEY ([LineStations_id])
)
GO

CREATE TABLE [Train] (
  [Train_id] int,
  [Line_id] int,
  PRIMARY KEY ([Train_id])
)
GO

CREATE TABLE [Wagons] (
  [Wagon_id] int,
  [Train_id] int,
  [Capacity] int,
  PRIMARY KEY ([Wagon_id])
)
GO

CREATE TABLE [Ticket] (
  [Ticket_id] int,
  [Price] double,
  [User_id] int,
  [Wagon_id] int,
  [DayAndTime] datetime,
  [DepartureStation] int,
  [ArrivalStation] int,
  [NumberOfSeat] int,
  PRIMARY KEY ([Ticket_id])
)
GO

ALTER TABLE [LineStations] ADD FOREIGN KEY ([Line_id]) REFERENCES [Line] ([Line_id])
GO

ALTER TABLE [LineStations] ADD FOREIGN KEY ([Station_id]) REFERENCES [Station] ([Station_id])
GO

ALTER TABLE [Train] ADD FOREIGN KEY ([Line_id]) REFERENCES [Line] ([Line_id])
GO

ALTER TABLE [Wagons] ADD FOREIGN KEY ([Train_id]) REFERENCES [Train] ([Train_id])
GO

ALTER TABLE [Ticket] ADD FOREIGN KEY ([User_id]) REFERENCES [Users] ([User_id])
GO

ALTER TABLE [Ticket] ADD FOREIGN KEY ([Wagon_id]) REFERENCES [Wagons] ([Wagon_id])
GO

ALTER TABLE [Ticket] ADD FOREIGN KEY ([DepartureStation]) REFERENCES [LineStations] ([LineStations_id])
GO

ALTER TABLE [Ticket] ADD FOREIGN KEY ([ArrivalStation]) REFERENCES [LineStations] ([LineStations_id])
GO
