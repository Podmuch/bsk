use szkola
GO
/* nale¿y zmieniæ œcie¿kê */
BULK INSERT dbo.t_Prowadzacy FROM 'D:/bulki/prowadzacy.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.t_Przedmioty FROM 'D:/bulki/przedmioty.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.t_Studenci FROM 'D:/bulki/studenci.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.t_Wyniki FROM 'D:/bulki/wyniki.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.t_Rodzaje_skladowych FROM 'D:/bulki/rodzaje_skladowych.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.t_Skladowe_przedmiotow FROM 'D:/bulki/skladowe_przedmiotow.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.t_Prowadzacy_skladowych_czesci FROM 'D:/bulki/prowadzacy_skladowych_czesci.bulk' WITH (FIELDTERMINATOR=';')

/*
	2 - user
	4 - student
	8 - prowadzacy
	16 - planista
	32 - admin
	
	2 - active
	4 - disabled
*/

/*
------------------------------------------------------------------------------------------------------------
INSERT INTO dbo.t_Uzytkownicy(c_Fk_nr_indeksu, c_Fk_id_pracownika, c_nazwa, c_grupa, c_status, c_haslo) 
	values (NULL, 0, 'student1', 6, 2, '04c72343945e2a6ef09221862164ac3a9e914373') -- haslo123
	
INSERT INTO dbo.t_Uzytkownicy(c_Fk_nr_indeksu, c_Fk_id_pracownika, c_nazwa, c_grupa, c_status, c_haslo) 
	values (550, NULL, 'prowadzacy1', 10, 2, '04c72343945e2a6ef09221862164ac3a9e914373') -- haslo123
	
INSERT INTO dbo.t_Uzytkownicy(c_Fk_nr_indeksu, c_Fk_id_pracownika, c_nazwa, c_grupa, c_status, c_haslo) 
	values (NULL, NULL, 'admin1', 62, 2, '04c72343945e2a6ef09221862164ac3a9e914373') -- haslo123
------------------------------------------------------------------------------------------------------------
INSERT INTO dbo.t_Akcje(c_akcja, c_tabela, c_status) values ('read', 't_Studenci', 2)
INSERT INTO dbo.t_Akcje(c_akcja, c_tabela, c_status) values ('write', 't_Studenci', 2)
INSERT INTO dbo.t_Akcje(c_akcja, c_tabela, c_status) values ('read', 't_Prowadzacy', 2)
INSERT INTO dbo.t_Akcje(c_akcja, c_tabela, c_status) values ('write', 't_Prowadzacy', 2)
INSERT INTO dbo.t_Akcje(c_akcja, c_tabela, c_status) values ('read', 't_Akcje', 2)
INSERT INTO dbo.t_Akcje(c_akcja, c_tabela, c_status) values ('write', 't_Akcje', 2)
INSERT INTO dbo.t_Akcje(c_akcja, c_tabela, c_status) values ('activate', 't_Akcje', 2)

*/

INSERT INTO dbo.t_Uzytkownicy(c_Fk_nr_indeksu, c_Fk_id_pracownika, c_nazwa, c_haslo) 
	values (550, NULL, 'student1', '04c72343945e2a6ef09221862164ac3a9e914373') -- haslo123
	
INSERT INTO dbo.t_Uzytkownicy(c_Fk_nr_indeksu, c_Fk_id_pracownika, c_nazwa, c_haslo) 
	values (NULL, 0, 'prowadzacy1', '04c72343945e2a6ef09221862164ac3a9e914373') -- haslo123
	
INSERT INTO dbo.t_Uzytkownicy(c_Fk_nr_indeksu, c_Fk_id_pracownika, c_nazwa, c_haslo) 
	values (NULL, NULL, 'admin1', '04c72343945e2a6ef09221862164ac3a9e914373') -- haslo123
	
INSERT INTO dbo.t_Role(c_rola) values ('Administrator')
INSERT INTO dbo.t_Role(c_rola) values ('Student')
INSERT INTO dbo.t_Role(c_rola) values ('Prowadzacy')
INSERT INTO dbo.t_Role(c_rola) values ('Planista')

INSERT INTO dbo.t_Przywileje(c_Fk_id_roli, c_Fk_id_uzytkownika) 
	values (2,1) 
	
INSERT INTO dbo.t_Przywileje(c_Fk_id_roli, c_Fk_id_uzytkownika) 
	values (3,2) 
	
INSERT INTO dbo.t_Przywileje(c_Fk_id_roli, c_Fk_id_uzytkownika) 
	values (1,3) 