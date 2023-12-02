-- Um sich als Admin einloggen zu können, muss ein extriger Login erstellt werden
-- der nicht mit den Kunden zusammenhängt. Da der Admin ja als Verwalter und nicht
-- als Kunde darauf Zugriff hat!!!



USE dbOnlineKredit
GO

CREATE TABLE tblAdminLogin
(
	ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Benutzername NVARCHAR(50) UNIQUE,
	Passwort VARBINARY(16)
)
GO

CREATE PROC pNeuenAdminEinfuegen @benutzer NVARCHAR(50), @passwort NVARCHAR(50)
AS
	DECLARE @pwd VARBINARY(16)
	SET @pwd = CONVERT(varbinary(16), HASHBYTES('SHA1', @passwort))

	INSERT INTO tblAdminLogin (Benutzername, Passwort)
	VALUES
		(@benutzer, @pwd)
GO


EXEC pNeuenAdminEinfuegen 'Stekal', '123Stekal!'
GO

EXEC pNeuenAdminEinfuegen 'Admin', '123Admin!'
GO

EXEC pNeuenAdminEinfuegen 'Administrator', '123Administrator!'
GO

SELECT * FROM tblAdminLogin
GO





USE master
GO








/*
-- Versuch 2 funktioniert leider nicht!!!
USE dbOnlineKredit
GO

DROP TABLE tblAdminLogin
GO

CREATE TABLE tblAdminLogin
(
	ID INT IDENTITY(1,1) NOT NULL,
	Benutzername NVARCHAR(50),
	Passwort NVARCHAR(50)
)
GO

-- drop proc pNeuenAdminEinfuegen

CREATE PROC pNeuenAdminEinfuegen @benutzer NVARCHAR(50), @passwort NVARCHAR(50)
AS
	DECLARE @pwd NVARCHAR(50)
	SET @pwd = CONVERT(NVARCHAR(50), HASHBYTES('SHA1', @passwort))

	INSERT INTO tblAdminLogin (Benutzername, Passwort)
	VALUES
		(@benutzer, @pwd)
GO


exec pNeuenAdminEinfuegen 'Stekal', '123Stekal!'
go

select * from tblAdminLogin
go


	DECLARE @passwort NVARCHAR(50)
	SET @passwort = '123Stekal!'
	DECLARE @pwd NVARCHAR(50)
	SET @pwd = CONVERT(NVARCHAR(50), HASHBYTES('SHA1', @passwort))
	SELECT @pwd




USE master
GO
*/