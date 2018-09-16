use master
GO;

IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'eDnevnikRG')
	BEGIN
		DROP DATABASE eDnevnikRG
	END
GO;
 
CREATE DATABASE eDnevnikRG
GO;

USE eDnevnikRG
GO;
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'skola')
	BEGIN
    	EXEC ('CREATE SCHEMA skola;')
    END
GO;
    
    
--PREDMETI----------------------------------------------------------------------
CREATE TABLE skola.Predmeti
(
	PredmetID int IDENTITY NOT NULL			CONSTRAINT PK_Predmeti PRIMARY KEY (PredmetID),
	Redosled int NOT NULL,
	NazivPredmeta nvarchar(100) NOT NULL	CONSTRAINT UC_NazivPredmeta UNIQUE (NazivPredmeta),
	Izborni bit DEFAULT 0  NOT NULL,
	Fakultativni bit DEFAULT 0  NOT NULL,
	PrviRazred bit NOT NULL,
	DrugiRazred bit NOT NULL,
	TreciRazred bit NOT NULL,
	CetvrtiRazred bit NOT NULL,
)
GO;


INSERT INTO skola.Predmeti
(Redosled , NazivPredmeta , Izborni , Fakultativni , PrviRazred , DrugiRazred , TreciRazred , CetvrtiRazred )
VALUES
(100  ,N'Српски језик и књижевност'				,0,0,	1,1,1,1		),
(200  ,N'Енглески језик'						,0,0,	1,1,1,1		),
(300  ,N'Психологија'							,0,0,	0,1,0,0		),
(400  ,N'Филозофија'							,0,0,	0,0,0,1		),
(500  ,N'Историја'								,0,0,	1,1,0,0		),
(600  ,N'Физика'								,0,0,	1,1,1,1		),
(700  ,N'Географија'							,0,0,	1,1,0,0		),
(800  ,N'Хемија'								,0,0,	1,1,0,0		),
(900  ,N'Биологија'								,0,0,	0,0,1,1		),
(1000 ,N'Физичко васпитање'						,0,0,	1,1,1,1		),
(1100 ,N'Уметност'								,0,0,	1,0,0,0		),
(1200 ,N'Математика'							,0,0,	1,1,1,1		),
(1300 ,N'Дискретна математика'					,0,0,	0,0,1,0		),
(1400 ,N'Примена рачунара'						,0,0,	1,1,1,1		),
(1500 ,N'Рачунарски системи'					,0,0,	1,0,0,0		),
(1600 ,N'Програмитање и програмски језици'		,0,0,	1,1,1,0		),
(1700 ,N'Оперативни системи и рачунарске мреже'	,0,0,	0,1,0,0		),
(1800 ,N'Модели и базе података'				,0,0,	0,0,1,1		),
(1900 ,N'Напредне технике програмирања'			,0,0,	0,0,1,1		),
(2000 ,N'Рачунарство и друштво'					,0,0,	0,0,1,0		),
(2100 ,N'Грађанско васпитање'					,1,0,	1,1,1,1		),
(2200 ,N'Верска настава' 						,1,0,	1,1,1,1		),
(10000 ,N'Владање'								,0,0,	1,1,1,1		)
--SELECT * FROM skola.Predmeti WHERE PrviRazred = 1
GO;

CREATE TABLE skola.Profesori
(
	ProfesorID int IDENTITY NOT NULL CONSTRAINT PK_Profesor PRIMARY KEY,
	ImeProfesora nvarchar(50) NOT NULL,
	Email nvarchar(255) NOT NULL,
	KontaktTelefon nvarchar(50) NOT NULL,
	LoginSifra nvarchar(max) NOT NULL,
	Admin bit NOT NULL
)
GO;

CREATE TABLE skola.Odeljenja
(
	OdeljenjeID int IDENTITY NOT NULL CONSTRAINT PK_Odeljenja PRIMARY KEY,
	RedniBroj int NOT NULL CHECK (RedniBroj>0),
	GodinaUpisa int NOT NULL,
	Razred int NOT NULL CHECK (Razred>0 AND Razred<5)
)
GO;

--Napuni odeljenja
DECLARE @tgod int = YEAR(GETDATE())
DECLARE @god int = @tgod - 3
WHILE @god < 2100
BEGIN
	
	DECLARE @razred int = 1
	IF(@tgod > @god)
		BEGIN
			SET @razred = @tgod - @god +1
			--SELECT @razred
		END

	DECLARE @RedniBroj int = 1
	WHILE @RedniBroj < 10
	BEGIN
		INSERT INTO skola.Odeljenja
		(RedniBroj, GodinaUpisa, Razred)
		VALUES (@RedniBroj, @god, @razred  )
	SET @RedniBroj = @RedniBroj + 1
	END
SET @god = @god + 1
END
--SELECT * FROM skola.Odeljenja WHERE GodinaUpisa = 2019
GO;


CREATE TABLE skola.DodeljeniProfesori
(
	OdeljenjeID int NOT NULL CONSTRAINT FK_Odeljenja_OdeljenjeID FOREIGN KEY (OdeljenjeID) REFERENCES skola.Odeljenja(OdeljenjeID),
	ProfesorID int NOT NULL CONSTRAINT FK_Profesori_ProfesorID FOREIGN KEY (ProfesorID) REFERENCES skola.Profesori(ProfesorID),		
	PredmetID int NOT NULL CONSTRAINT FK_Predmeti FOREIGN KEY (PredmetID) REFERENCES skola.Predmeti(PredmetID),	
	RazredniID bit NOT NULL
)
GO;


CREATE TABLE skola.SesijeKorisnika (
	SesijeID int NOT NULL IDENTITY CONSTRAINT PK_SesijeKorisnika PRIMARY KEY,
	VremeLogin datetime NOT NULL,
	VremeLogout datetime NULL,
	ProfesorID int NOT NULL,
	CONSTRAINT FK_SesijeKorisnika_Profesori FOREIGN KEY (ProfesorID) REFERENCES skola.Profesori(ProfesorID) 
)
GO;


CREATE TABLE skola.Ucenici (
	UcenikID int IDENTITY  CONSTRAINT PK_Ucenici PRIMARY KEY (UcenikID),
	MaticniBroj nvarchar(10) NOT NULL CONSTRAINT UC_MaticniBroj UNIQUE (MaticniBroj),
	Ime nvarchar(50) NOT NULL,
	Prezime nvarchar(50) NOT NULL,
	JMBG nvarchar(13) NOT NULL,
	OdeljenjeID int NOT NULL 			CONSTRAINT FK_Ucenici_OdeljenjeID_to_Odeljenja_OdeljenjeID FOREIGN KEY (OdeljenjeID) REFERENCES skola.Odeljenja(OdeljenjeID),
	DatumRodjenja date NOT NULL,
	MestoRodjenja nvarchar(50) NOT NULL,
	OpstinaRodjenja nvarchar(50) NOT NULL,
	DrzavaRodjenja nvarchar(50) NOT NULL,
	KontaktTelefonUcenika nvarchar(50),
	EmailUcenika nvarchar(255),
	ImeOca nvarchar(50) NOT NULL,
	PrezimeOca nvarchar(50) NOT NULL,
	KontaktTelefonOca nvarchar(50),
	EmailOca nvarchar(255),
	ImeMajke nvarchar(50) NOT NULL,
	PrezimeMajke nvarchar(50) NOT NULL,
	KontaktTelefonMajke nvarchar(50),
	EmailMajke nvarchar(255),
	LoginSifra nvarchar(max) NOT NULL
) 
GO;


CREATE TABLE skola.TipOcene
(
	TipOcene nvarchar(50) NOT NULL CONSTRAINT PK_TipOcene PRIMARY KEY 
)
GO;

INSERT INTO skola.TipOcene (TipOcene) VALUES (N'Усмени одговор'),(N'Писмени задатак'),(N'Контролна вежба'), (N'Активности на часу'), (N'Друго'), (N'Закључна оцена')
--SELECT * FROM skola.TipOcene
GO;

CREATE TABLE skola.Ocene 
(
	OcenaID int IDENTITY NOT NULL CONSTRAINT PK_ocene PRIMARY KEY,
	TipOcene nvarchar(50) NOT NULL CONSTRAINT FK_Ocene_TipOcene FOREIGN KEY (TipOcene) REFERENCES skola.TipOcene(TipOcene),
	Ocena int NOT NULL CHECK (Ocena>-1 AND Ocena<10), --0 neocenjen 1-5 , 6-7 veronauka , 7-8 gradjansko 
	OpisOcene nvarchar(50) NOT NULL,
	DatumOcene datetime NOT NULL,
	UcenikID int NOT NULL CONSTRAINT FK_Ocene_Ucenici FOREIGN KEY (UcenikID) REFERENCES skola.Ucenici(UcenikID),
	ProfesorID int NOT NULL CONSTRAINT FK_Ocene_Profesori FOREIGN KEY (ProfesorID) REFERENCES skola.Profesori(ProfesorID),
	PredmetID int NOT NULL CONSTRAINT FK_Ocene_Predmeti FOREIGN KEY (PredmetID) REFERENCES skola.Predmeti(PredmetID),
	Razred int NOT NULL
) 
GO;

CREATE TABLE skola.LogOcena
(
	LogID int IDENTITY NOT NULL CONSTRAINT PK_LogOcena PRIMARY KEY,
	OcenaID int NOT NULL CONSTRAINT FK_LogOcena_Ocene FOREIGN KEY (OcenaID) REFERENCES skola.Ocene(OcenaID),
	TipOcene nvarchar(50) NOT NULL CONSTRAINT LogOcena_TipOcene FOREIGN KEY (TipOcene) REFERENCES skola.TipOcene(TipOcene),
	Ocena int NOT NULL CHECK (Ocena>-1 AND Ocena<10), 
	OpisOcene nvarchar(50) NOT NULL,
	DatumOcene datetime NOT NULL,
	MaticniBroj nvarchar(10) NOT NULL,
	UcenikID int NOT NULL,
	ProfesorID int NOT NULL,
	PredmetID int NOT NULL,
	Razred int NOT NULL
) 
GO;


CREATE TABLE skola.ArhivaOcena (
	MaticniBroj nvarchar(10) NOT NULL,
	Ime nvarchar(50) NOT NULL,
	Prezime nvarchar(50) NOT NULL,
	JMBG nvarchar(13) NOT NULL,
	MestoRodjenja nvarchar(50) NOT NULL,
	OpstinaRodjenja nvarchar(50) NOT NULL,
	DrzavaRodjenja nvarchar(50) NOT NULL,
	RedniBroj int NOT NULL,
	Razred int NOT NULL,
	SkolskaGodina int NOT NULL,
	NazivPredmeta nvarchar(100) NOT NULL,
	Ocena int NOT NULL
)
GO;

--TRIGERI ZA OCENE
CREATE TRIGGER skola.PromenaOceneLog
ON skola.Ocene
AFTER UPDATE
AS
BEGIN TRY 
	BEGIN TRANSACTION

	DECLARE @OcenaID as int,
			@TipOcene as nvarchar(50),
			@Ocena as int,
			@OpisOcene as nvarchar(50),
			@DatumOcene as datetime,
			@UcenikID as int,
			@ProfesorID as int,
			@PredmetID as int,
			@Razred as int
			
    SELECT
			@OcenaID = inserted.OcenaID,
			@TipOcene = inserted.TipOcene,
			@Ocena = inserted.Ocena,
			@OpisOcene = inserted.OpisOcene,
			@DatumOcene = inserted.DatumOcene,
			@UcenikID = inserted.UcenikID,
			@ProfesorID = inserted.ProfesorID,
			@PredmetID = inserted.PredmetID,
			@Razred = inserted.Razred
    FROM inserted

	INSERT INTO skola.LogOcena
	(OcenaID, TipOcene, Ocena, OpisOcene, DatumOcene, UcenikID, ProfesorID, PredmetID, Razred)
	SELECT OcenaID, TipOcene, Ocena, OpisOcene, DatumOcene, UcenikID, ProfesorID, PredmetID, Razred FROM deleted

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	RAISERROR ('Nije Proslo', 1, 1)
END CATCH
GO


CREATE TRIGGER skola.BrisanjeOceneLog
ON skola.Ocene
INSTEAD OF DELETE
AS
BEGIN TRY 
	BEGIN TRANSACTION

	INSERT INTO skola.LogOcena
	(OcenaID, TipOcene, Ocena, OpisOcene, DatumOcene, UcenikID, ProfesorID, PredmetID, Razred)
	SELECT OcenaID, TipOcene, Ocena, OpisOcene, DatumOcene, UcenikID, ProfesorID, PredmetID, Razred FROM deleted

	DELETE FROM skola.Ocene
	WHERE OcenaID IN (SELECT OcenaID FROM deleted)

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	RAISERROR ('Nije Proslo', 1, 1)
END CATCH
GO



/**************************** STORED PROCEDURES ****************************/


/***** STORE PROCEDURE ZA PREDMETE *****/

CREATE PROCEDURE skola.PredmetUpisNovog
(@Redosled int ,@NazivPredmeta nvarchar(100) ,@Izborni bit ,@Fakultativni bit ,@PrviRazred bit ,@DrugiRazred bit ,@TreciRazred bit, @CetvrtiRazred bit )
AS
BEGIN TRY
	IF EXISTS (SELECT 1 FROM skola.Predmeti WHERE NazivPredmeta = @NazivPredmeta)  OR  EXISTS (SELECT 1 FROM skola.Predmeti WHERE Redosled = @Redosled) 
		BEGIN
			RETURN -1
		END
	ELSE
		BEGIN
			INSERT INTO skola.Predmeti
			(Redosled , NazivPredmeta , Izborni , Fakultativni , PrviRazred , DrugiRazred , TreciRazred , CetvrtiRazred )
			VALUES
			(@Redosled , @NazivPredmeta , @Izborni , @Fakultativni , @PrviRazred , @DrugiRazred , @TreciRazred , @CetvrtiRazred )
			RETURN 0
		END
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
GO

CREATE PROCEDURE skola.PredmetMenjanje
(@PredmetID int, @Redosled int ,@NazivPredmeta nvarchar(100) ,@Izborni bit ,@Fakultativni bit ,@PrviRazred bit ,@DrugiRazred bit ,@TreciRazred bit, @CetvrtiRazred bit )
AS
BEGIN TRY
	IF EXISTS ( SELECT 1 FROM skola.Predmeti WHERE PredmetID = @PredmetID )
		BEGIN
			UPDATE skola.Predmeti
			SET Redosled = @Redosled , NazivPredmeta = @NazivPredmeta , Izborni = @Izborni , Fakultativni = @Fakultativni,
			PrviRazred = @PrviRazred , DrugiRazred = @DrugiRazred , TreciRazred = @TreciRazred , CetvrtiRazred = @CetvrtiRazred
			WHERE PredmetID = @PredmetID
		END
	ELSE
		BEGIN
			INSERT INTO skola.Predmeti
			(Redosled , NazivPredmeta , Izborni , Fakultativni , PrviRazred , DrugiRazred , TreciRazred , CetvrtiRazred )
			VALUES
			(@Redosled , @NazivPredmeta , @Izborni , @Fakultativni , @PrviRazred , @DrugiRazred , @TreciRazred , @CetvrtiRazred )
			RETURN -1
		END
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
GO

CREATE PROCEDURE skola.PredmetiBrisanje
(@PredmetID int)
AS
BEGIN TRY
IF EXISTS (SELECT 1 FROM skola.Predmeti WHERE PredmetID = @PredmetID)
	BEGIN
		DELETE FROM skola.Predmeti
		WHERE PredmetID = @PredmetID
		RETURN 0
	END
ELSE
	BEGIN
		RETURN -1
	END
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
GO

CREATE PROCEDURE skola.PredmetIzlistavanje --Htedoh da napisem ko KP al realno ima samo 20 predmeta max
AS
BEGIN TRY
	SELECT * FROM skola.Predmeti ORDER BY Redosled
	RETURN 0
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
GO




/***** STORE PROCEDURE ZA OCENE *****/

--- Procedura za unos nove ocene ---
CREATE PROCEDURE skola.OcenaUpisNove
(@TipOcene nvarchar(50), @Ocena int, @UcenikID int, @ProfesorID int, @PredmetID int , @Razred int)
AS
BEGIN TRY
	DECLARE @OpisOcene nvarchar(50)
	DECLARE @NazivPredmeta nvarchar(50)
	SET @NazivPredmeta = ( SELECT NazivPredmeta FROM skola.Predmeti WHERE PredmetID =  @PredmetID )
		
		SET @OpisOcene = 
			CASE
				WHEN (@NazivPredmeta=N'Верска настава' AND @Ocena=5) THEN 'истиче се'
				WHEN (@NazivPredmeta=N'Верска настава' AND @Ocena=4) THEN 'добар' 
				WHEN (@NazivPredmeta=N'Грађанско васпитање' AND @Ocena=5) THEN 'веома успешан'
				WHEN (@NazivPredmeta=N'Грађанско васпитање' AND @Ocena=4) THEN 'успешан'
				WHEN (@NazivPredmeta=N'Владање' AND @Ocena=5)  THEN N'примерно'
				WHEN (@NazivPredmeta=N'Владање' AND @Ocena=4)  THEN N'броло добро'
				WHEN (@NazivPredmeta=N'Владање' AND @Ocena=3)  THEN N'добро'
				WHEN (@NazivPredmeta=N'Владање' AND @Ocena=2)  THEN N'задовољавајуће'
				WHEN (@NazivPredmeta=N'Владање' AND @Ocena=1)  THEN N'незадовољавајуће'
				WHEN @Ocena=5 THEN N'одличан'      
				WHEN @Ocena=4 THEN N'врло добар'
				WHEN @Ocena=3 THEN N'добар'
				WHEN @Ocena=2 THEN N'довољан'
				WHEN @Ocena=1 THEN N'недовољан'
				WHEN @Ocena=0 THEN N'неоцењен'
			END

	INSERT INTO skola.Ocene
	(TipOcene, Ocena, OpisOcene ,DatumOcene, UcenikID, ProfesorID, PredmetID , Razred)
	VALUES
	(@TipOcene, @Ocena, @OpisOcene, GETDATE(), @UcenikID, @ProfesorID, @PredmetID , @Razred)
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
GO


--- Procedura za menjanje ocena ---
CREATE PROCEDURE skola.OceneMenjanje
(@OcenaID int, @TipOcene nvarchar(50), @Ocena int, @UcenikID int, @ProfesorID int, @PredmetID int, @Razred int) --- RAZRED JA MISLIM NE TREBA
AS
BEGIN TRY
IF EXISTS (SELECT 1 FROM skola.Ocene WHERE OcenaID = @OcenaID)
	BEGIN
		DECLARE @OpisOcene nvarchar(50)
		DECLARE @NazivPredmeta nvarchar(50)
		SET @NazivPredmeta = ( SELECT NazivPredmeta FROM skola.Predmeti WHERE PredmetID = @PredmetID )
			
			SET @OpisOcene = 
				CASE
					WHEN (@NazivPredmeta=N'Верска настава' AND @Ocena=5) THEN 'истиче се'
					WHEN (@NazivPredmeta=N'Верска настава' AND @Ocena=4) THEN 'добар' 
					WHEN (@NazivPredmeta=N'Грађанско васпитање' AND @Ocena=5) THEN 'веома успешан'
					WHEN (@NazivPredmeta=N'Грађанско васпитање' AND @Ocena=4) THEN 'успешан'
					WHEN (@NazivPredmeta=N'Владање' AND @Ocena=5)  THEN N'примерно'
					WHEN (@NazivPredmeta=N'Владање' AND @Ocena=4)  THEN N'броло добро'
					WHEN (@NazivPredmeta=N'Владање' AND @Ocena=3)  THEN N'добро'
					WHEN (@NazivPredmeta=N'Владање' AND @Ocena=2)  THEN N'задовољавајуће'
					WHEN (@NazivPredmeta=N'Владање' AND @Ocena=1)  THEN N'незадовољавајуће'
					WHEN @Ocena=5 THEN N'одличан'      
					WHEN @Ocena=4 THEN N'врло добар'
					WHEN @Ocena=3 THEN N'добар'
					WHEN @Ocena=2 THEN N'довољан'
					WHEN @Ocena=1 THEN N'недовољан'
					WHEN @Ocena=0 THEN N'неоцењен'
				END
		
		UPDATE skola.Ocene
		SET TipOcene = @TipOcene, Ocena = @Ocena, OpisOcene = @OpisOcene, DatumOcene = GETDATE(), UcenikID = @UcenikID, ProfesorID = @ProfesorID, PredmetID = @PredmetID, Razred = @Razred
		WHERE OcenaID = @OcenaID
		RETURN 0
	END
ELSE
	BEGIN
		RETURN -1
	END
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
GO

--- Procedura za brisanje ocena ---
CREATE PROCEDURE skola.OceneBrisanje
(@OcenaID int)
AS
BEGIN TRY
IF EXISTS (SELECT 1 FROM skola.Ocene WHERE OcenaID = @OcenaID)
	BEGIN
		DELETE FROM skola.Ocene
		WHERE OcenaID = @OcenaID
		RETURN 0
	END
ELSE
	BEGIN
		RETURN -1
	END
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
GO


CREATE PROCEDURE skola.OceneIzlistavanje
(@BrojPoStrani int = 20, @TrenutnaStrana int, @NazivPredmeta nvarchar(50), @NazivUcenika nvarchar(50), @NazivProfesora nvarchar(50), @Razred int, @RedniBroj int)
AS
BEGIN TRY
	SELECT U.Ime, U.Prezime, P.NazivPredmeta, O.Ocena, PR.ImeProfesora, O.TipOcene, O.DatumOcene 
	FROM skola.Ucenici AS U 
	INNER JOIN skola.Ocene AS O ON U.UcenikID = O.UcenikID
	INNER JOIN skola.Predmeti AS P ON O.PredmetID = P.PredmetID
	INNER JOIN skola.DodeljeniProfesori AS DP ON DP.PredmetID = P.PredmetID
	INNER JOIN skola.Profesori AS PR ON DP.ProfesorID = PR.ProfesorID
	INNER JOIN skola.Odeljenja AS OD ON OD.OdeljenjeID = DP.OdeljenjeID
	WHERE
	( (@NazivPredmeta IS NULL) OR (P.NazivPredmeta = @NazivPredmeta ) ) AND
	( (@NazivUcenika IS NULL) OR (CONCAT('%', U.Ime, ' ',  U.Prezime ,'%') LIKE @NazivUcenika) ) AND
	( (@NazivProfesora IS NULL) OR (PR.ImeProfesora LIKE @NazivProfesora) ) AND
	( (@Razred IS NULL) OR (OD.Razred = @Razred ) ) AND
	( (@RedniBroj IS NULL) OR (OD.RedniBroj = @RedniBroj) )
	
	ORDER BY P.NazivPredmeta
	OFFSET (@TrenutnaStrana * @BrojPoStrani) ROWS
         FETCH NEXT @BrojPoStrani ROWS ONLY
	RETURN 0
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
GO


/***** STORE PROCEDURE ZA PROFESORE *****/

--- Procedura za dodavanje profesora ---
CREATE PROCEDURE skola.ProfesoriUpisNovih
(@ImeProfesora nvarchar(50), @Email nvarchar(255), @KontaktTelefon nvarchar(50), @LoginSifra nvarchar(max), @Admin bit)
AS
BEGIN TRY
	INSERT INTO skola.Profesori
	(ImeProfesora, Email, KontaktTelefon, LoginSifra, Admin)
	VALUES
	(@ImeProfesora, @Email, @KontaktTelefon, @LoginSifra, @Admin)
	RETURN 0
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
GO

--- Procedura za menjanje profesora ---
CREATE PROCEDURE skola.ProfesoriMenjanje
(@ProfesorID int, @ImeProfesora nvarchar(50), @Email nvarchar(255), @KontaktTelefon nvarchar(50), @LoginSifra nvarchar(max), @Admin bit)
AS
BEGIN TRY
IF EXISTS (SELECT 1 FROM skola.Profesori WHERE ProfesorID = @ProfesorID)
	BEGIN
		UPDATE skola.Profesori
		SET ImeProfesora = @ImeProfesora, Email = @Email, KontaktTelefon = @KontaktTelefon, LoginSifra = @LoginSifra, Admin = @Admin
		WHERE ProfesorID = @ProfesorID
		RETURN 0
	END
ELSE
	BEGIN 
		RETURN -1
	END
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
GO

--- Procedura za brisanje profesora ---
CREATE PROCEDURE skola.ProfesoriBrisanje
(@ProfesorID int)
AS
BEGIN TRY
IF EXISTS (SELECT 1 FROM skola.Profesori WHERE ProfesorID = @ProfesorID)
	BEGIN
		DELETE FROM skola.Profesori
		WHERE ProfesorID = @ProfesorID
		RETURN 0
	END
ELSE
	BEGIN
		RETURN -1
	END
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
GO

CREATE PROCEDURE skola.ProfesoriIzlistavanje
AS
BEGIN TRY
	SELECT * FROM skola.Profesori ORDER BY ImeProfesora
	RETURN 0
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
GO



/***** STORE PROCEDURE ZA UCENIKE *****/


--- Procesura za dodavanje ucenika ---

CREATE PROCEDURE skola.UceniciUpisNovih
(@MaticniBroj nvarchar(10), @Ime nvarchar(50), @Prezime nvarchar(50), @JMBG nvarchar(50), @OdeljenjeID int, @DatumRodjenja date, @MestoRodjenja nvarchar(50), @OpstinaRodjenja nvarchar(50), @DrzavaRodjenja nvarchar(50), @KontaktTelefonUcenika nvarchar(50), @EmailUcenika nvarchar(255), @ImeOca nvarchar(50), @PrezimeOca nvarchar(50), @KontaktTelefonOca nvarchar(50), @EmailOca nvarchar(255), @ImeMajke nvarchar(50), @PrezimeMajke nvarchar (50), @KontaktTelefonMajke nvarchar(50), @EmailMajke nvarchar(255), @LoginSifra nvarchar(max))
AS
BEGIN TRY
	INSERT INTO skola.Ucenici
	(MaticniBroj, Ime, Prezime, JMBG, OdeljenjeID, DatumRodjenja, MestoRodjenja, OpstinaRodjenja, DrzavaRodjenja, KontaktTelefonUcenika, EmailUcenika, ImeOca, PrezimeOca, KontaktTelefonOca, EmailOca, ImeMajke, PrezimeMajke, KontaktTelefonMajke, EmailMajke, LoginSifra)
	VALUES
	(@MaticniBroj, @Ime, @Prezime, @JMBG, @OdeljenjeID, @DatumRodjenja, @MestoRodjenja, @OpstinaRodjenja, @DrzavaRodjenja, @KontaktTelefonUcenika, @EmailUcenika, @ImeOca, @PrezimeOca, @KontaktTelefonOca, @EmailOca, @imemajke, @prezimemajke, @KontaktTelefonMajke, @EmailMajke, @LoginSifra)
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
GO

--- Procedura za menjanje ucenika ---

CREATE PROCEDURE skola.UceniciMenjanje
(@UcenikID int ,@MaticniBroj nvarchar(10), @Ime nvarchar(50), @Prezime nvarchar(50), @JMBG nvarchar(50), @OdeljenjeID int, @DatumRodjenja date, @MestoRodjenja nvarchar(50), @OpstinaRodjenja nvarchar(50), @DrzavaRodjenja nvarchar(50), @KontaktTelefonUcenika nvarchar(50), @EmailUcenika nvarchar(255), @ImeOca nvarchar(50), @PrezimeOca nvarchar(50), @KontaktTelefonOca nvarchar(50), @EmailOca nvarchar(255), @ImeMajke nvarchar(50), @PrezimeMajke nvarchar (50), @KontaktTelefonMajke nvarchar(50), @EmailMajke nvarchar(255), @LoginSifra nvarchar(max))
AS
BEGIN TRY
IF EXISTS (SELECT 1 FROM skola..Ucenici WHERE MaticniBroj = @MaticniBroj)
	BEGIN
		UPDATE skola.Ucenici
		SET MaticniBroj = @MaticniBroj, Ime = @Ime, Prezime = @Prezime, JMBG = @JMBG, OdeljenjeID = @OdeljenjeID, DatumRodjenja = @DatumRodjenja, MestoRodjenja = @MestoRodjenja, OpstinaRodjenja = @OpstinaRodjenja, DrzavaRodjenja = @DrzavaRodjenja, ImeOca = @ImeOca, PrezimeOca = @PrezimeOca, KontaktTelefonOca = @KontaktTelefonOca, EmailOca = @EmailOca, ImeMajke = @ImeMajke, PrezimeMajke = @PrezimeMajke, KontaktTelefonMajke = @KontaktTelefonMajke, EmailMajke = @EmailMajke, LoginSifra = @LoginSifra
		WHERE UcenikID = @UcenikID
		RETURN 0
	END
ELSE
	BEGIN
		RETURN -1
	END
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
GO

--- Procedura za brisanje ucenika ---

CREATE PROCEDURE skola.UceniciBrisanje
(@UcenikID int)
AS
BEGIN TRY
IF EXISTS (SELECT 1 FROM skola.Ucenici WHERE UcenikID = @UcenikID)
	BEGIN
		DELETE FROM skola.Ucenici
		WHERE UcenikID = @UcenikID
		RETURN 0
	END
ELSE
	BEGIN
		RETURN -1
	END
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
GO

CREATE PROCEDURE skola.UceniciIzlistavanje 
(@TrenutnaStrana int, @BrojPoStrani int, @Razred int, @RedniBroj int, @NazivUcenika nvarchar(50))
AS
BEGIN TRY
	SELECT * FROM skola.Ucenici
	INNER JOIN skola.Odeljenja ON Ucenici.OdeljenjeID = Odeljenja.OdeljenjeID
	WHERE
	( (@Razred IS NULL) OR (Odeljenja.Razred = @Razred) ) AND
	( (@RedniBroj IS NULL) OR (Odeljenja.RedniBroj = @RedniBroj) ) AND
	( (@NazivUcenika IS NULL) OR ( Ime LIKE ('%' + replace(@NazivUcenika, '%', '[%]') + '%' )) ) 
	ORDER BY Prezime
	OFFSET (@TrenutnaStrana * @BrojPoStrani) ROWS
    FETCH NEXT @BrojPoStrani ROWS ONLY
	RETURN 0
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
GO



/***** STORE PROCEDURE ZA ODELJENJA *****/


--- SP za dodavanje razrednog u odeljenja --- BRISEMO

--CREATE PROCEDURE skola.OdeljenjaMenjanjeRazrednog
--(@OdeljenjeID int, @RazredniID int)
--AS
--BEGIN TRY
--IF EXISTS (SELECT 1 FROM skola.Odeljenja WHERE OdeljenjeID = @OdeljenjeID)
--	BEGIN
--		UPDATE skola.Odeljenja
--		SET RazredniID = @RazredniID
--		WHERE OdeljenjeID = @OdeljenjeID
--		RETURN 0
--	END
--ELSE
--	BEGIN 
--		RETURN -1
--	END
--END TRY
--BEGIN CATCH
--	RETURN @@ERROR
--END CATCH
--GO

--- SP za selektovanje odeljenja ---

CREATE PROCEDURE skola.OdeljenjaIzlistaj
(@BrojPoStrani int = 20, @TrenutnaStrana int, @RedniBroj int, @Razred int, @GodinaUpisa int)
AS
BEGIN TRY
	SELECT *
	FROM skola.Odeljenja  
	--INNER JOIN skola.Godine ON dbo.Odeljenja.GodinaID = dbo.Godine.GodinaID
	WHERE 
  		( (@RedniBroj IS NULL) OR (RedniBroj = @RedniBroj) )  AND
  		( (@Razred IS NULL) OR (Razred = @Razred) )  AND
  		( (@GodinaUpisa IS NULL) OR (GodinaUpisa = @GodinaUpisa) )

	ORDER BY skola.Odeljenja.RedniBroj
	OFFSET (@TrenutnaStrana * @BrojPoStrani) ROWS
         FETCH NEXT @BrojPoStrani ROWS ONLY

	RETURN 0
END TRY
BEGIN CATCH
	RETURN @@Error
END CATCH
GO;


/******OSTALO********/

 --- Procedura za login ---
CREATE PROCEDURE skola.LoginKorisnika
 (
	@Korisnik nvarchar(255),
	@LoginSifra nvarchar(max),
	@ProfesorID int OUTPUT,
	@Admin int OUTPUT,
	@UcenikID int OUTPUT
 )
 AS
 BEGIN TRY
	IF EXISTS (SELECT 1 FROM skola.Profesori WHERE Email = @Korisnik AND LoginSifra = @LoginSifra)
	BEGIN 
--		SELECT  @ProfesorID = ProfesorID ,@Admin = Admin
--		FROM skola.Profesori
--		WHERE Email = @Korisnik AND LoginSifra = @LoginSifra
		SELECT @ProfesorID = ProfesorID , @Admin = Admin
		FROM skola.Profesori
		WHERE Email = @Korisnik AND LoginSifra = @LoginSifra
		
		INSERT INTO skola.SesijeKorisnika
		(VremeLogin, ProfesorID)
		VALUES 
		(GETDATE(), @ProfesorID)
		RETURN 0
	END
	ELSE
	IF EXISTS (SELECT 1 FROM skola.Ucenici WHERE JMBG = @Korisnik AND LoginSifra = @LoginSifra)
	BEGIN
		SELECT @UcenikID = UcenikID
		FROM skola.Ucenici
		WHERE JMBG = @Korisnik AND LoginSifra = @LoginSifra
		RETURN 0
	END
	ELSE
		BEGIN
			RETURN -1
		END
END TRY
BEGIN CATCH
	RETURN @@Error
END CATCH
GO;


--USE eDnevnikRG
--SELECT * FROM skola.Profesori
--GO;

INSERT INTO eDnevnikRG.skola.Profesori
(ImeProfesora, Email, KontaktTelefon, LoginSifra, Admin)
VALUES
(N'Neki Profa', 'neki@profa.com', '911', 'nekasifra', 0),
(N'Neki Admin', 'neki@admin.com', '911', 'nekasifra', 1);
GO;

--Matični broj ima 7 “kućica”, odnosno 7 cifara:
---prva dva mesta su redni broj učenika u dnevniku;
---treće mesto je razred koji upisuje
---četvrto i peto su broj odeljenja
---šesto i sedmo godina upisa (poslednje dve cifre)

INSERT INTO eDnevnikRG.skola.Ucenici
(MaticniBroj, Ime, Prezime, JMBG, OdeljenjeID, DatumRodjenja, MestoRodjenja, OpstinaRodjenja, DrzavaRodjenja, KontaktTelefonUcenika, EmailUcenika, ImeOca, PrezimeOca, KontaktTelefonOca, EmailOca, ImeMajke, PrezimeMajke, KontaktTelefonMajke, EmailMajke, LoginSifra)
VALUES
('0110118', N'Стефан', N'Јовановић', '792371296', 28, '1912-10-25' , N'Београд', N'Савски Венац', N'Србија', '011-232-222', N'стефо@маил.ком', N'Предраг', N'Пијанчић', N'022-232333', '', N'Јасна', N'Пијанчић-Лакић', N'054-2323-22', '', N'opasnasifra1'),
('0210118', N'Мирко', N'Јовановић', '792651296', 28,  '1912-10-25' , N'Београд', N'Савски Венац', N'Србија', '011-232-222', N'стефо@маил.ком', N'Предраг', N'Пијанчић', N'022-232333', '', N'Јасна', N'Пијанчић-Лакић', N'054-2323-22', '', N'opasnasifra2'),
('0310118', N'Јелица', N'Јовановић', '792999296', 28, '1912-10-25' , N'Београд', N'Савски Венац', N'Србија', '011-232-222', N'стефо@маил.ком', N'Предраг', N'Пијанчић', N'022-232333', '', N'Јасна', N'Пијанчић-Лакић', N'054-2323-22', '', N'opasnasifra3'),
('0410118', N'Милијана', N'Кркиц', '792299296', (SELECT OdeljenjeID FROM skola.Odeljenja WHERE RedniBroj=1 AND Razred=1 AND GodinaUpisa=2018  ), '1912-10-25' , N'Београд', N'Савски Венац', N'Србија', '011-232-222', N'ремина@маил.ком', N'Предраг', N'Пијанчић', N'022-232333', '', N'Јасна', N'Пијанчић-Лакић', N'054-2323-22', '', N'opasnasifra4')
GO;
SELECT * FROM skola.Ucenici
--
--EXEC skola.UceniciIzlistavanje 0, 20, 1, 1, N'и'
								--0,20,1,1,""
								
--IZLISTAVANJE UCENIKA PROBA
--DECLARE @Razred int = 1
--DECLARE @RedniBroj int = 1
--DECLARE @NazivUcenika nvarchar(50) = N'и'
--SELECT * FROM skola.Ucenici WHERE Ime LIKE ('%' + replace(@NazivUcenika, '%', '[%]') + '%' )
--
--
--SELECT * FROM skola.Ucenici
--	INNER JOIN skola.Odeljenja ON Ucenici.OdeljenjeID = Odeljenja.OdeljenjeID
--	WHERE
--	( (@Razred IS NULL) OR (Odeljenja.Razred = @Razred) ) AND
--	( (@RedniBroj IS NULL) OR (Odeljenja.RedniBroj = @RedniBroj) ) AND
--	( (@NazivUcenika IS NULL) OR ( ('%' + Ime  + ' ' +  Prezime + '%') LIKE @NazivUcenika) ) 
--	ORDER BY Prezime
--	OFFSET (@TrenutnaStrana * @BrojPoStrani) ROWS
--    FETCH NEXT @BrojPoStrani ROWS ONLY


--TESTIRANJA

--DECLARE @pid int 
--DECLARE @mb nvarchar(10)
--DECLARE @a bit
--Exec skola.LoginKorisnika N'neki@admin.com', N'nekasifra' , @pid out ,@a out, @mb out 
--SELECT @pid , @a
--select * from skola.Predmeti

--SELECT * FROM skola.Odeljenja WHERE GodinaUpisa = 2018

--SELECT * FROM skola.Ucenici

--
--CREATE proc skola.Test
--(
--	@mleko int OUTPUT 
--)
--AS
--BEGIN
--	SET @mleko = 5
--	--SELECT @mleko = 44
--	RETURN 2
--END
--
--DECLARE @nekiint int
--EXEC skola.Test @nekiint OUT
--SELECT @nekiint
--DECLARE @Ree int
--DECLARE @pid int 
--DECLARE @mb nvarchar(10)
--DECLARE @a bit
--Exec skola.LoginKorisnika N'neki@admin.com', N'nekasifra' , @pid out ,@a out, @mb out 

--use eDnevnikRG
--SELECT * FROM skola.SesijeKorisnika


exec skola.ProfesoriIzlistavanje