/*USE SZKOLA
GO
CREATE LOGIN user123 
    WITH PASSWORD = 'haslo123';
GO
*/


CREATE DATABASE szkola
GO

USE szkola
GO

CREATE TABLE t_Prowadzacy
(
	c_Id_pracownika integer PRIMARY KEY,
	c_Pesel varchar (20) NOT NULL,
	c_Tytul varchar(20) NOT NULL,
	c_Staz integer NOT NULL,
	c_Wiek integer NOT NULL,
	c_Imie varchar(20) NOT NULL,
	c_Nazwisko varchar(30) NOT NULL,
	c_Katedra varchar(100) NOT NULL,
	c_Wydzial varchar(100) NOT NULL,	
);
GO

CREATE TABLE t_Przedmioty
(
	c_Nazwa varchar(100) PRIMARY KEY,
	c_Semestr integer NOT NULL,
	c_Ilosc_godzin integer NOT NULL,
	c_Ilosc_ects integer NOT NULL,
	c_Fk_gl_prowadz integer NOT NULL REFERENCES t_Prowadzacy(c_Id_pracownika),
);
GO

CREATE TABLE t_Studenci
(
	c_Nr_indeksu integer PRIMARY KEY,
	c_Pesel varchar (20) NOT NULL,
	c_Imie varchar(20) NOT NULL,
	c_Nazwisko varchar(30) NOT NULL,
	c_Rok integer NOT NULL,
	c_Semestr integer NOT NULL,
	c_Dlug_ects integer NOT NULL,
);
GO

CREATE TABLE t_Wyniki
(
	c_Fk_Student integer NOT NULL REFERENCES t_Studenci(c_Nr_indeksu),
	c_Fk_Przedmiot varchar(100) NOT NULL REFERENCES t_Przedmioty(c_Nazwa),
	c_Wynik float NOT NULL,
);
GO

CREATE TABLE t_Rodzaje_skladowych
(
	c_Nazwa varchar(100) PRIMARY KEY,
);
GO

CREATE TABLE t_Skladowe_przedmiotow
(
	c_Id_skladowej integer PRIMARY KEY,
	c_Fk_Przedmiot varchar(100) NOT NULL REFERENCES t_Przedmioty(c_Nazwa),
	c_Fk_skladowa varchar(100) NOT NULL REFERENCES t_Rodzaje_skladowych(c_Nazwa),
	c_Ilosc_godzin integer NOT NULL,
	c_Fk_osoby_odp integer NOT NULL REFERENCES t_Prowadzacy(c_Id_pracownika),
);
GO

CREATE TABLE t_Prowadzacy_skladowych_czesci
(
	c_Fk_id_skladowej integer NOT NULL REFERENCES t_Skladowe_przedmiotow(c_Id_skladowej),
	c_Fk_id_pracownika integer NOT NULL REFERENCES t_Prowadzacy(c_Id_pracownika),
);
GO

CREATE TABLE t_Uzytkownicy
(
	c_id_uzytkownika integer IDENTITY(1,1) PRIMARY KEY, 
	c_Fk_nr_indeksu integer REFERENCES t_Studenci(c_Nr_indeksu),
	c_Fk_id_pracownika integer REFERENCES t_Prowadzacy(c_Id_pracownika),
	c_login varchar(30) not null,
	c_grupa integer not null default 1,  -- do jakiej grupy nalezy uzytkownik
	--c_status integer not null default 1, -- czy user jest aktywny/usuniety/wylaczony itd
	c_haslo varchar(50) not null, -- sha1			
);

CREATE TABLE t_Operacje
(
	c_id_operacji integer IDENTITY(1,1) PRIMARY KEY, 
	c_active bit not null default 1,
	c_operacja varchar(50) not null, -- zmiana has³a, edycja danych itd		
);

CREATE TABLE t_Role
(
	c_id_roli integer IDENTITY(1,1) PRIMARY KEY,
	c_rola varchar(50) not null,
	c_aktywna bit not null default 1,
	c_grupy_ktorych_dotyczy integer not null default 1, -- wielokrotnosci dwójki
)

CREATE TABLE t_Przywileje
(
	c_Fk_id_roli integer REFERENCES t_Role(c_id_roli),
	c_Fk_id_operacji integer REFERENCES t_Operacje(c_id_operacji),
	c_aktywny bit not null default 1,
)

CREATE TABLE t_Sesje
(
	c_Fk_id_uzytkownika integer not null,	
	c_Fk_id_roli integer not null,
	c_dataWygasniecia datetime not null,
	PRIMARY KEY(c_Fk_id_uzytkownika, c_Fk_id_roli)
)


/*CREATE TABLE t_Uzytkownicy
(
	c_Id_uzytkownika integer IDENTITY(1,1) PRIMARY KEY, 
	c_Fk_nr_indeksu integer REFERENCES t_Studenci(c_Nr_indeksu),
	c_Fk_id_pracownika integer REFERENCES t_Prowadzacy(c_Id_pracownika),
	c_nazwa varchar(30) not null,
	c_haslo varchar(50) not null, -- sha1			
);

CREATE TABLE t_Role
(
	c_id_roli integer IDENTITY(1,1) PRIMARY KEY,
	c_rola varchar(20) not null,
);

CREATE TABLE t_Przywileje
(
	c_Fk_id_roli integer NOT NULL REFERENCES t_Role(c_id_roli),
	c_Fk_id_uzytkownika integer NOT NULL REFERENCES t_Uzytkownicy(c_Id_uzytkownika),
)*/