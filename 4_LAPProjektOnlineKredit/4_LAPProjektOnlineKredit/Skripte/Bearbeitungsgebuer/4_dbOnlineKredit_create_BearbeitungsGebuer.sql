


-- L A P - P R O J E K T


/* Aufgabenstellung:
					 Es soll eine Bearbeitungsgebür von 4,56€ implementiert werden, die binnen
					 14 Tage (keine Werktage) zu bezahlen ist. 
					 Ist diese nicht bezahlt so wird eine Mail an den Kunden gesendet, die
					 besagt, dass er es versäumt hat die Gebühren zu bezahlen.
					 Sobald die Mail gesendet wurde bleiben Ihm noch 7 Tage (keine Werktage).
					 Also insgesammt 21 Tage.
*/

USE dbOnlineKredit
GO

CREATE TABLE tblBearbeitungsGebuer
(
	IDBearbeitungsGebuer INT NOT NULL PRIMARY KEY REFERENCES tblKunde,
	AntragErstelltAm DATETIME NOT NULL DEFAULT GETDATE(),
	GebuerBezahlt BIT NOT NULL,
	InBearbeitung BIT NULL 
)
GO


INSERT INTO tblBearbeitungsGebuer (IDBearbeitungsGebuer, AntragErstelltAm, GebuerBezahlt, InBearbeitung)
VALUES
	(1, '4.1.2017', 0, null),
	(2, '4.1.2017', 0, null),
	(3, '4.1.2017', 0, null),
	(4, '4.1.2017', 0, null),
	(5, '4.1.2017', 0, null),
	(6, '4.1.2017', 0, null),
	(7, '4.1.2017', 0, null),
	(8, '4.1.2017', 0, null),
	(9, '4.1.2017', 0, null),
	(10, '4.1.2017', 0, null),
	(11, '4.1.2017', 0, null),
	(12, '28.12.2016', 0, null),
	(13, '28.12.2016', 0, null),
	(14, '28.12.2016', 0, null),
	(15, '28.12.2016', 0, null),
	(16, '28.12.2016', 0, null),
	(17, '30.1.2016', 0, null),
	(18, '4.1.2016', 0, null),
	(19, '4.1.2017', 0, null),
	(20, '4.1.2017', 0, null)
GO


/*
DELETE FROM tblBearbeitungsGebuer
*/

USE master
GO