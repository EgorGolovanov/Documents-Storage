IF NOT EXISTS (SELECT * FROM [Users] WHERE name = 'Antony')
BEGIN
    INSERT INTO [Users](name, password) VALUES ('Antony', 'FatTony')
END;
IF NOT EXISTS (SELECT * FROM [Users] WHERE name = 'Egor')
BEGIN
    INSERT INTO [Users](name, password) VALUES ('Egor', 'BadBoy')
END;
IF NOT EXISTS (SELECT * FROM [Users] WHERE name = 'Alex')
BEGIN
    INSERT INTO [Users](name, password) VALUES ('Alex', 'Orange')
END;
IF NOT EXISTS (SELECT * FROM [Users] WHERE name = 'Konstantin')
BEGIN
    INSERT INTO Users(name, password) VALUES ('Konstantin', 'Drugd1ller')
END;