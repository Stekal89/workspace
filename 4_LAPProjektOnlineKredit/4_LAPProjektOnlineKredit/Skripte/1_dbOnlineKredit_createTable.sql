USE master
GO

DROP DATABASE dbOnlineKredit
GO

CREATE DATABASE dbOnlineKredit
GO

USE dbOnlineKredit
GO

-------------------------------------------------------------------
								--	Lookup-Tabellen
-------------------------------------------------------------------



CREATE TABLE tblLand
(
	IDLand CHAR(3) PRIMARY KEY NOT NULL,
	Land VARCHAR(100) NOT NULL
);
GO

CREATE TABLE tblOrt
(
	IDOrt INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	PLZ VARCHAR(10) NOT NULL,
	FKLand CHAR(3) FOREIGN KEY REFERENCES tblLand NOT NULL,
	Ort VARCHAR(100) NOT NULL
);
Go

CREATE TABLE tblTitel
(
	IDTitel INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Titel VARCHAR(50) NOT NULL unique
);
GO


CREATE TABLE tblFamilienstand
(
	IDFamilienstand INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Familienstand VARCHAR(25) NOT NULL unique
);
GO

CREATE TABLE tblIdentifikationsArt
(
	IDIdentifikationsArt INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	IdentifikationsArt VARCHAR(20) NOT NULL unique
);
GO

CREATE TABLE tblWohnart
(
	IDWohnart INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Wohnart VARCHAR(40) NOT NULL unique
);
GO

CREATE TABLE tblSchulabschluss
(
	IDSchulabschluss INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Schulabschluss VARCHAR(40) NOT NULL unique
);
GO
--------------------------------------------------------------------
									--	HAUPTTABELLE
--------------------------------------------------------------------

CREATE TABLE tblKunde
(
	IDKunde INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Vorname NVARCHAR(50) NOT NULL,
	Nachname NVARCHAR(50) NOT NULL,
	Geschlecht CHAR(1) NOT NULL CHECK(Geschlecht in ('m','w')),
	Geburtsdatum DATE NOT NULL CHECK(Geburtsdatum <= DATEADD(YEAR,-18,GETDATE())),
	AnzahlKinder INT,
	FKTitel INT FOREIGN KEY REFERENCES tblTitel,
	FKFamilienstand INT FOREIGN KEY REFERENCES tblFamilienstand,
	FKStaatsangehoerigkeit CHAR(3) FOREIGN KEY REFERENCES tblLand,
	FKWohnart INT FOREIGN KEY REFERENCES tblWohnart,
	FKSchulabschluss INT FOREIGN KEY REFERENCES tblSchulabschluss,
	IdentifikationsNummer VARCHAR(30),
	FKIdentifikationsArt INT FOREIGN KEY REFERENCES tblIdentifikationsArt
);                               
GO



-------------------------------------------------------------------------------------------------------------------
--						/|	    /|
--					   / |  o  / |
--						 | 	     |
--						 |  o    |
--						 |	     |
-------------------------------------------------------------------------------------------------------------------

CREATE TABLE tblKontaktDaten
(
	IDKontaktDaten INT PRIMARY KEY REFERENCES tblKunde,
	FKOrt INT FOREIGN KEY REFERENCES tblOrt,
	Strasse NVARCHAR(50) NOT NULL,
	Hausnummer NVARCHAR(20) NOT NULL,
	Stiege NVARCHAR(20),
	Tür NVARCHAR(20),
	EMail NVARCHAR(100),
	Telefonnummer NVARCHAR(100)
)
GO

------------------------------------------
-- Arbeitgeber

CREATE TABLE tblBeschaeftigungsart
(
	IDBeschaeftigungsart INT identity(1,1) PRIMARY KEY,
	Beschaeftigungsart VARCHAR(50) NOT NULL unique
);
GO

CREATE TABLE tblBranche
(
	IDBranche INT identity(1,1) PRIMARY KEY,
	Branche VARCHAR(100) NOT NULL	
);
GO

CREATE TABLE tblArbeitgeber
(
	IDArbeitgeber INT PRIMARY KEY REFERENCES tblKunde,
	Firma NVARCHAR(100) NOT NULL, 
	FKBeschaeftigungsArt INT NOT NULL FOREIGN KEY REFERENCES tblBeschaeftigungsart, 
	FKBranche INT FOREIGN KEY REFERENCES tblBranche,
	BeschaeftigtSeit DATE
);
GO

------------------------------------------


CREATE TABLE tblKontoDaten
(
	IDKontoDaten INT PRIMARY KEY REFERENCES tblKunde NOT NULL,		-- 1:1 Beziehung
	BIC VARCHAR(30) NOT NULL,	
	IBAN VARCHAR(30) NOT NULL,	
	HatKonto BIT NOT NULL, 
	Bank NVARCHAR(100) NOT NULL           
);
GO

CREATE TABLE tblLogin
(	IDLogin INT PRIMARY KEY REFERENCES tblKunde,
	Username NVARCHAR(100) NOT NULL UNIQUE,
	Passwort VARCHAR(100)
)
GO

CREATE TABLE tblKredit
(
	IDKredit INT PRIMARY KEY REFERENCES tblKunde,
	GewuenschterKredit DECIMAL(10,2) NOT NULL,
	GewuenschteLaufzeit smallint NOT NULL CHECK(GewuenschteLaufzeit > 0),
	KreditBewilligt BIT NOT NULL DEFAULT(0)
);
GO

CREATE TABLE tblFinanzielleSituation
(
	IDFinanzielleSituation INT PRIMARY KEY REFERENCES tblKunde,
	MonatsEinkommenNetto DECIMAL(10,2),
	Wohnkosten DECIMAL(10,2),
	SonstigeEinkommen DECIMAL(10,2), 
	Unterhalt  DECIMAL(10,2), 
	Raten  DECIMAL(10,2)
);
GO

CREATE TABLE tblEinstellungen
(
	IDEinstellungen INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	NormalZins DECIMAL(4,2) NOT NULL,
	EffektiverZins DECIMAL(4,2) NOT NULL
);
GO



