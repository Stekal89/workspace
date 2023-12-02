--- Ich benötige für den Fall, das eine Abfragetool eingebaut werden soll so um die 20 Testdaten

/* vorhanden:
	- tblLand
	- tblOrt
	- tblTitel
	- tblSchulabschluss
	- tblFamilienstand
	- tblIdentifikationsArt
	- tblWohnart
	- tblBranche
	- tblBeschaeftigungsArt
*/


USE dbOnlineKredit
GO

/*
	To Do:
		- tblKunde
		- tblArbeitgeber
		- tblFinanzielleSituation
		- tblKontaktDaten
		- tblKontoDaten
		- tblKredit
*/

-- tblKunde:
					-- 1		2			3			4				5				6						7		8					9						10	
INSERT INTO tblKunde (Vorname, Nachname, Geschlecht, Geburtsdatum, FKFamilienstand, FKStaatsangehoerigkeit, FKWohnart, FKSchulabschluss, FKIdentifikationsArt, IdentifikationsNummer)
VALUES--1         2       3     4           5    6    7  8  9      10
	('Thomas', 'Wacker', 'm', '01.02.1988', 1, 'AUT', 1, 6, 4, 'ATU1234567890'),
	('Patrick', 'Mayer', 'm', '01.02.1989', 5, 'AUT', 4, 5, 3, 'ATU5684487890'),
	('Sascha', 'Merk', 'm', '01.05.1979', 2, 'AUT', 3, 1, 4, 'ATU75464690'),
	('Susanne', 'Wacker', 'w', '01.02.1979', 3, 'AUT', 2, 3, 2, 'ATU1123554890'),
	('Ingrid', 'Koch', 'w', '01.01.1989', 5, 'AUT', 4, 1, 2, 'ATU1234455890'),
	('Hari', 'Hirsch', 'm', '12.05.1989', 1, 'AUT', 2, 1, 4, 'ATU1234745890'),
	('Fred', 'Huber', 'm', '01.06.1989', 2, 'AUT', 1, 6, 3, 'ATU1234567566'),
	('Stephanie', 'Stankanova', 'w', '07.01.1988', 1, 'AUT', 1, 5, 2, 'ATU1234441890'),
	('Karl', 'Grüsel', 'm', '07.02.1989', 1, 'AUT', 1, 4, 1, 'ATU1234541890'),
	('Rene', 'Rudolf', 'm', '04.01.1970', 3, 'AUT', 3, 3, 4, 'ATU121111890'),
	('Lydia', 'Braugger', 'w', '01.04.1983', 4, 'AUT', 1, 2, 3, 'ATU121227890'),
	('Helga', 'Zelko', 'w', '01.01.1989', 4, 'AUT', 1, 1, 2, 'ATU1239999890'),
	('Marlene', 'Ullreich', 'w', '05.03.1989', 1, 'AUT', 1, 6, 1, 'ATU1231111290'),
	('Gerald', 'Bintinger', 'm', '13.05.1989', 2, 'AUT', 1, 2, 4, 'ATU1234578850'),
	('Mirko', 'Bertonski', 'm', '15.09.1978', 5, 'AUT', 2, 5, 4, 'ATU1234565678'),
	('Markus', 'Dorn', 'm', '07.04.1983', 3, 'AUT', 2, 4, 3, 'ATU1234568955'),
	('Jürgen', 'Schwaiger', 'm', '09.04.1982', 4, 'AUT', 1, 3, 3, 'ATU1234551152'),
	('Joseph', 'Olinger', 'm', '25.07.1984', 1, 'AUT', 1, 2, 3, 'ATU1234588444'),
	('Franz', 'Horvarth', 'm', '21.06.1981', 1, 'AUT', 3, 1, 3, 'ATU1234556585'),
	('Phillip', 'Omega', 'm', '22.05.1982', 2, 'AUT', 4, 5, 2, 'ATU1234564445')
GO

SELECT * FROM tblKunde
GO

INSERT INTO tblArbeitgeber (IDArbeitgeber, Firma, FKBeschaeftigungsArt, FKBranche, BeschaeftigtSeit)
VALUES
	(1, 'AMS', 5, 10, '01.01.2010'),
	(2, 'BBRZ', 4, 9, '01.01.2010'),
	(3, 'Zum Wirten', 3, 8, '01.01.2010'),
	(4, '4You', 2, 7, '01.01.2010'),
	(5, 'Billa', 1, 6, '01.01.2010'),
	(6, 'REWE', 5, 5, '01.01.2010'),
	(7, 'Penny Markt', 4, 4, '01.01.2010'),
	(8,'BDA', 3, 3, '01.01.2010'),
	(9, 'MVA', 2, 2, '01.01.2010'),
	(10, 'HEX', 1, 1, '01.01.2010'),
	(11, 'RGB', 5, 10, '01.01.2010'),
	(12, 'IOJ', 4, 9, '01.01.2010'),
	(13, 'JOY', 3, 8, '01.01.2010'),
	(14, 'WES', 2, 7, '01.01.2010'),
	(15, 'Coca Cola', 1, 6, '01.01.2010'),
	(16, 'Freedom', 4, 5, '01.01.2010'),
	(17, 'XYZ', 5, 4, '01.01.2010'),
	(18, 'ABC', 3, 3, '01.01.2010'),
	(19, '789', 1, 2, '01.01.2010'),
	(20, 'PIM', 2, 1, '01.01.2010')
GO

SELECT * FROM tblArbeitgeber
GO

INSERT INTO tblFinanzielleSituation (IDFinanzielleSituation, MonatsEinkommenNetto, Wohnkosten, SonstigeEinkommen, Unterhalt, Raten)
VALUES
	(1, 2000, 0, 0, 0, 50),
	(2, 1200, 500, 0, 200, 20),
	(3, 1200, 300, 300, 0, 10),
	(4, 4500, 250, 0, 100, 0),
	(5, 3000, 700, 100, 0, 0),
	(6, 1800, 0, 0, 0, 0),
	(7, 1850, 1200, 0, 0, 0),
	(8, 1250, 750, 0, 0, 30),
	(9, 1400, 600, 0, 0, 0),
	(10, 2000, 800, 0, 100, 0),
	(11, 1400, 150, 0, 0, 0),
	(12, 1300, 0, 0, 0, 0),
	(13, 1200, 0, 0, 0, 10),
	(14, 1100, 0, 0, 0, 0),
	(15, 1100, 0, 0, 150, 0),
	(16, 1500, 0, 200, 0, 50),
	(17, 1600, 0, 1500, 0, 0),
	(18, 1800, 0, 0, 0, 0),
	(19, 1200, 0, 0, 0, 30),
	(20, 1200, 0, 0, 0, 0)
GO

SELECT * FROM tblFinanzielleSituation
GO

INSERT INTO tblKontaktDaten(IDKontaktDaten, FKOrt, Strasse, Hausnummer, EMail, Telefonnummer)
VALUES
	(1, 1, 'Strasse', '1', 'max.muster@gmail.com', '06641234567'),
	(2, 1, 'Hauptstrasse', '21', 'max.muster@hotmail.com', '06641212467'),
	(3, 1, 'Nebenstrasse', '12', 'dibo@gmail.com', '06641231167'),
	(4, 1, 'Oberstrasse', '13', 'john@gmx.at', '06641234887'),
	(5, 1, 'Unterstrasse', '31', 'girly@gmail.com', '06641234599'),
	(6, 1, 'Hilfswerkstrasse', '3', 'muster@gmail.com', '06641444567'),
	(7, 1, 'Strasse', '4', 'program@gmail.com', '06641234596'),
	(8, 1, 'Hauergasse', '10', 'fido@gmail.com', '06641214567'),
	(9, 1, 'Ungarplatz', '7', 'max@gmail.com', '06641774567'),
	(10, 1, 'Platz', '1', 'max.muster456@gmail.com', '06641238867'),
	(11, 1, 'Gasse', '8', 'ingo@gmail.com', '06641237767'),
	(12, 1, 'Dr. Adolf Schärfgasse', '19', 'ppp89@gmail.com', '06641234997'),
	(13, 1, 'Dr. Theodor Körnergasse', '20', 'xxx55@gmail.com', '06641233567'),
	(14, 1, 'Franz Jonassgasse', '7', 'terr88@gmail.com', '06641234567'),
	(15, 1, 'Johnstrasse', '4', 'dss9@gmail.com', '06641234567'),
	(16, 1, 'Simmeringer Hauptstrasse', '5', 'r8955@gmail.com', '06641212567'),
	(17, 1, 'Enkplatz', '1', 'max.muster333@gmx.com', '06641234747'),
	(18, 1, 'Hauptplatz', '1', 'max322@gmail.com', '06641234877'),
	(19, 1, 'Friedrich Strasse', '1', 'ollo44@gmail.com', '06641664567'),
	(20, 1, 'Schubert Strasse', '1', 'frizi25@gmail.com', '06645234567')
GO

SELECT * FROM tblKontaktDaten
GO

INSERT INTO tblKontoDaten(IDKontoDaten, BIC, IBAN, HatKonto, Bank)
VALUES
	(1, 'BARHEX', 'AT21 3324 5422 3212 15', 1, 'Deutsche Bank AG' ),
	(2, 'BARHEX', 'AT21 3324 5422 3212 16', 1, 'Deutsche Bank AG' ),
	(3, 'BARHEX', 'AT21 3324 5422 3212 17', 1, 'Deutsche Bank AG' ),
	(4, 'BARHEX', 'AT21 3324 5422 3212 18', 1, 'Deutsche Bank AG' ),
	(5, 'BARHEX', 'AT21 3324 5422 3212 19', 1, 'Deutsche Bank AG' ),
	(6, 'BARHEX', 'AT21 3324 5422 3212 20', 1, 'Deutsche Bank AG' ),
	(7, 'BARHEX', 'AT21 3324 5422 3212 21', 1, 'Deutsche Bank AG' ),
	(8, 'BARHEX', 'AT21 3324 5422 3212 22', 1, 'Deutsche Bank AG' ),
	(9, 'BARHEX', 'AT21 3324 5422 3212 23', 1, 'Deutsche Bank AG' ),
	(10, 'BARHEX', 'AT21 3324 5422 3212 24', 1, 'Deutsche Bank AG' ),
	(11, 'BARBIN', 'AT21 3324 5422 3212 25', 0, 'Sparkasse' ),
	(12, 'HEXBAR', 'AT21 3324 5422 3212 26', 0, 'Raiffaisenbank' ),
	(13, 'BINBAR', 'AT21 3324 5422 3212 27', 0, 'Bank Austria' ),
	(14, 'BICBIC', 'AT21 3324 5422 3212 28', 0, 'EasyBank' ),
	(15, 'AZEDEE', 'AT21 3324 5422 3212 29', 0, 'Sparkasse' ),
	(16, 'ASSEED', 'AT21 3324 5422 3212 30', 0, 'BAWAG' ),
	(17, 'AEEWSS', 'AT21 3324 5422 3212 31', 0, 'Stekal Bank' ),
	(18, 'GGREDD', 'AT21 3324 5422 3212 32', 0, 'Grübel Bank' ),
	(19, 'BBDEEE', 'AT21 3324 5422 3212 33', 0, 'Jakubetz Bank' ),
	(20, 'AWEDDF', 'AT21 3324 5422 3212 34', 0, 'Sven Bank' )
GO

SELECT * FROM tblKontoDaten
GO

INSERT INTO tblKredit (IDKredit, GewuenschterKredit, GewuenschteLaufzeit, KreditBewilligt)
VALUES
	(20, 10000, 36, 1),
	(19, 100000, 36, 0),
	(18, 50000, 36, 1),
	(17, 70000, 36, 0),
	(16, 7000, 36, 1),
	(15, 10000, 36, 0),
	(14, 15000, 36, 1),
	(13, 13000, 36, 0),
	(12, 14000, 36, 1),
	(11, 16000, 36, 0),
	(10, 17000, 36, 1),
	(9, 18000, 36, 0),
	(8, 20000, 36, 1),
	(7, 25000, 36, 1),
	(6, 35000, 36, 0),
	(5, 80000, 36, 1),
	(4, 75000, 36, 1),
	(3, 79000, 36, 0),
	(2, 90000, 36, 1),
	(1, 65000, 36, 0)
GO

SELECT * FROM tblKredit
GO


/*

DELETE FROM tblKunde
DELETE FROM tblArbeitgeber
DELETE FROM tblFinanzielleSituation
DELETE FROM tblKontaktDaten
DELETE FROM tblKontoDaten
DELETE FROM tblKredit


*/
USE master
GO