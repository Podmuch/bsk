use szkola
GO
/* nale¿y zmieniæ œcie¿kê */
BULK INSERT dbo.t_Prowadzacy FROM 'C:/Users/Dominik/Documents/GitHub/bsk/bulki/prowadzacy.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.t_Przedmioty FROM 'C:/Users/Dominik/Documents/GitHub/bsk/bulki/przedmioty.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.t_Studenci FROM 'C:/Users/Dominik/Documents/GitHub/bsk/bulki/studenci.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.t_Wyniki FROM 'C:/Users/Dominik/Documents/GitHub/bsk/bulki/wyniki.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.t_Rodzaje_skladowych FROM 'C:/Users/Dominik/Documents/GitHub/bsk/bulki/rodzaje_skladowych.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.t_Skladowe_przedmiotow FROM 'C:/Users/Dominik/Documents/GitHub/bsk/bulki/skladowe_przedmiotow.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.t_Prowadzacy_skladowych_czesci FROM 'C:/Users/Dominik/Documents/GitHub/bsk/bulki/prowadzacy_skladowych_czesci.bulk' WITH (FIELDTERMINATOR=';')

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
	values (600, NULL, 'student1', '04c72343945e2a6ef09221862164ac3a9e914373') -- haslo123
	
INSERT INTO dbo.t_Uzytkownicy(c_Fk_nr_indeksu, c_Fk_id_pracownika, c_nazwa, c_haslo) 
	values (NULL, 0, 'prowadzacy1', '04c72343945e2a6ef09221862164ac3a9e914373') -- haslo123
	
INSERT INTO dbo.t_Uzytkownicy(c_Fk_nr_indeksu, c_Fk_id_pracownika, c_nazwa, c_haslo) 
	values (NULL, NULL, 'admin1', '04c72343945e2a6ef09221862164ac3a9e914373') -- haslo123


INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 'zmiana hasla')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 'edycja imienia')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 'edycja nazwiska')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 'wyswietlenie listy studentów')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 'zmiana grupy studenta')

	
INSERT INTO dbo.t_Role(c_nazwa, c_grupy_ktorych_dotyczy) 
	values ('Administrator', 16)
INSERT INTO dbo.t_Role(c_nazwa, c_grupy_ktorych_dotyczy) 
	values ('Student', 2)
INSERT INTO dbo.t_Role(c_nazwa, c_grupy_ktorych_dotyczy) 
	values ('Prowadzacy', 4)
INSERT INTO dbo.t_Role(c_nazwa, c_grupy_ktorych_dotyczy) 
	values ('Planista', 8)

INSERT INTO dbo.t_Przywileje(c_Fk_id_roli, c_id_operacji) 
	values (1,1) 
	
INSERT INTO dbo.t_Przywileje(c_Fk_id_roli, c_id_operacji) 
	values (1,2) 
	
INSERT INTO dbo.t_Przywileje(c_Fk_id_roli, c_id_operacji) 
	values (1,3) 

INSERT INTO dbo.t_Przywileje(c_Fk_id_roli, c_id_operacji) 
	values (1,4) 

INSERT INTO dbo.t_Przywileje(c_Fk_id_roli, c_id_operacji) 
	values (1,5) 