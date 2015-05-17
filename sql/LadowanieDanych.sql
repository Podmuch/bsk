use szkola
GO
/* nale¿y zmieniæ œcie¿kê */
BULK INSERT dbo.t_Prowadzacy FROM 'C:/bulki/prowadzacy.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.t_Przedmioty FROM 'C:/bulki/przedmioty.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.t_Studenci FROM 'C:/bulki/studenci.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.t_Wyniki FROM 'C:/bulki/wyniki.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.t_Rodzaje_skladowych FROM 'C:/bulki/rodzaje_skladowych.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.t_Skladowe_przedmiotow FROM 'C:/bulki/skladowe_przedmiotow.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.t_Prowadzacy_skladowych_czesci FROM 'C:/bulki/prowadzacy_skladowych_czesci.bulk' WITH (FIELDTERMINATOR=';')

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
	1 - user
	2 - student
	4 - prowadzacy
	8 - planista
	16 - admin
	17 - admin + user itd
*/



INSERT INTO dbo.t_Uzytkownicy(c_Fk_nr_indeksu, c_Fk_id_pracownika, c_login, c_haslo, c_grupa) 
	values (600, NULL, 'student1', '04c72343945e2a6ef09221862164ac3a9e914373', 3) -- haslo123
	
INSERT INTO dbo.t_Uzytkownicy(c_Fk_nr_indeksu, c_Fk_id_pracownika, c_login, c_haslo, c_grupa) 
	values (NULL, 0, 'prowadzacy1', '04c72343945e2a6ef09221862164ac3a9e914373', 5) -- haslo123
	
INSERT INTO dbo.t_Uzytkownicy(c_Fk_nr_indeksu, c_Fk_id_pracownika, c_login, c_haslo, c_grupa) 
	values (NULL, NULL, 'admin1', '04c72343945e2a6ef09221862164ac3a9e914373', 31) -- haslo123


INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Prowadzacy_select')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Prowadzacy_update')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Prowadzacy_insert')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Prowadzacy_delete')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Prowadzacy_Skladowych_select')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Prowadzacy_Skladowych_update')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Prowadzacy_Skladowych_insert')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Prowadzacy_Skladowych_delete')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Przedmioty_select')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Przedmioty_update')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Przedmioty_insert')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Przedmioty_delete')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Skladowe_Przedmiotow_select')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Skladowe_Przedmiotow_update')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Skladowe_Przedmiotow_insert')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Skladowe_Przedmiotow_delete')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Studenci_select')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Studenci_update')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Studenci_insert')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Studenci_delete')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Wyniki_select')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Wyniki_update')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Wyniki_insert')

INSERT INTO dbo.t_Operacje(c_active, c_operacja) 
	values (1, 't_Wyniki_delete')

	
INSERT INTO dbo.t_Role(c_rola, c_grupy_ktorych_dotyczy) 
	values ('Administrator', 16)
INSERT INTO dbo.t_Role(c_rola, c_grupy_ktorych_dotyczy) 
	values ('Student', 2)
INSERT INTO dbo.t_Role(c_rola, c_grupy_ktorych_dotyczy) 
	values ('Prowadzacy', 4)
INSERT INTO dbo.t_Role(c_rola, c_grupy_ktorych_dotyczy) 
	values ('Planista', 8)

INSERT INTO dbo.t_Przywileje(c_Fk_id_roli, c_Fk_id_operacji) 
	values (1,1) 
	
INSERT INTO dbo.t_Przywileje(c_Fk_id_roli, c_Fk_id_operacji) 
	values (1,2) 
	
INSERT INTO dbo.t_Przywileje(c_Fk_id_roli, c_Fk_id_operacji) 
	values (1,3) 

INSERT INTO dbo.t_Przywileje(c_Fk_id_roli, c_Fk_id_operacji) 
	values (1,4) 

INSERT INTO dbo.t_Przywileje(c_Fk_id_roli, c_Fk_id_operacji) 
	values (1,5) 