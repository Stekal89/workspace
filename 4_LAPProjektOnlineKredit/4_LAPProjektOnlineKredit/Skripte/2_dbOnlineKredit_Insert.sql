USE dbOnlineKredit
GO

/*
SELECT * FROM LandOrt.dbo.Länderneu
GO
*/

INSERT INTO tblLand
SELECT [Spalte 0], [Spalte 1] 
FROM LandOrt.dbo.Länderneu
WHERE [Spalte 0] <> ''
GO

/*
SELECT * FROM tblLand
GO
*/

INSERT INTO tblOrt(PLZ, Ort, FKLand)
SELECT PLZ, Ort , ('AUT')
FROM LandOrt.dbo.tblOrt
GO

/*
SELECT * FROM LandOrt.dbo.tblOrt
GO
*/



INSERT INTO tblTitel
VALUES
	('Ing.'),
	('Dr.'),
	('Mag.'),
	('DI Dipl.-Ing.'),
	('Mag. med. vet.'),
	('Dr. med. univ.'),
	('Dr. med. dent.'),
	('Mag. arch.'),
	('Mag. rer. nat.'),
	('Mag. pharm.'),
	('Mag. phil.'),
	('Mag. iur.'),
	('Mag. rer. soc. oec.'),
	('Mag. theol.'),
	('M.A.I.S'),
	('MA M.A.'),
	('LLM LL.M.'),
	('MSc M.Sc.'),
	('BA B.A.'),
	('BEng B.Eng.'),
	('BSc B.Sc.'),
	('Bakk. rer. soc. oec'),
	('PhD'),
	('Dr. nat. techn.'),
	('Dr. med. univ. et scient. med.'),
	('Dr. scient. med.'),
	('Dr. mont.'),
	('Dr. rer. nat.'),
	('Dr. phil.'),
	('Dr. phil. fac. theol.'),
	('Dr. iur.'),
	('Dr. rer. soc. oec.'),
	('Dr. techn.'),
	('Dr. theol.'),
	('Dr. med. vet.'),
	('Dr. med. dent.et scient. med.'),
	('M.E.S'),
	('MAS'),
	('MA'),
	('MBA'),
	('M.B.L.'),
	('MIB'),
	('LL.M.'),
	('MPOS'),
	('MP'),
	('MSc')
GO


INSERT INTO tblSchulabschluss
VALUES
	('Lehre'),
	('Hauptschule'),
	('Universität'),
	('Matura'),
	('Kein Abschluss'),
	('Sonstige...')
GO

INSERT INTO tblFamilienstand
VALUES
	('ledig'),
	('verheiratet'),
	('geschieden'),
	('in Partnerschaft'),
	('verwitwet')
GO


INSERT INTO tblIdentifikationsArt
VALUES
	('Reisepass'),
	('Personalausweis'),
	('Führerschein'),
	('Sonstige...')
GO

INSERT INTO tblWohnart
VALUES
	('Miete'),
	('Eigentum'),
	('Wohngemeinschaft'),
	('Genossenschaft')
GO

--------------------------------------------------------------------
--			Lookup für tblArbeitgeber
--------------------------------------------------------------------

INSERT INTO tblBranche
VALUES
	('Gastronomie'),
	('Metallbau'),
	('Einzelhandel'),
	('Informationstechnologie'),
	('Amt'),
	('Logistik'),
	('Handel/Büro'),
	('Elektronik'),
	('Baustellen'),
	('Sonstige...')
GO

INSERT INTO tblBeschaeftigungsart
VALUES
	('Vollzeit'),
	('Teilzeit'),
	('Selbstständig'),
	('Geringfügig'),
	('Freier Dienstnehmer')
GO


