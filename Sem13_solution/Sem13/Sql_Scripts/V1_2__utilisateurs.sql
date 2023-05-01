	
	CREATE TABLE Utilisateurs.Utilisateur(
		UtilisateurID int IDENTITY(1,1),
		Pseudonyme nvarchar(50) NOT NULL,
		MotDePasseHache varbinary(32) NOT NULL,
		MdpSel varbinary(16) NOT NULL,
		Email nvarchar(256) NOT NULL,
		CONSTRAINT PK_Utilisateur_UtilisateurID PRIMARY KEY (UtilisateurID)
	);
	GO
	
	ALTER TABLE Utilisateurs.Utilisateur ADD CONSTRAINT UC_Utilisateur_Pseudonyme
	UNIQUE (Pseudonyme);
	GO
	
	-- Procédure pour créer un utilisateur
	CREATE PROCEDURE Utilisateurs.USP_CreerUtilisateur
		@Pseudonyme nvarchar(50),
		@MotDePasse nvarchar(100),
		@Email nvarchar(256)
	AS
	BEGIN
		-- Sels aléatoires
		DECLARE @MdpSel varbinary(16) = CRYPT_GEN_RANDOM(16);
		
		-- Concaténation de données et sel
		DECLARE @MdpEtSel nvarchar(116) = CONCAT(@MotDePasse, @MdpSel);
		
		-- Hachage du mot de passe
		DECLARE @MdpHachage varbinary(32) = HASHBYTES('SHA2_256', @MdpEtSel);
		
		-- Insertion
		INSERT INTO Utilisateurs.Utilisateur (Pseudonyme, MotDePasseHache, MdpSel, Email)
		VALUES
		(@Pseudonyme, @MdpHachage, @MdpSel, @Email);
		
	END
	GO
	

    -- Ajouter un utilisateur de test
	EXEC Utilisateurs.USP_CreerUtilisateur
		@Pseudonyme = 'max',
		@MotDePasse = 'Salut1!',
		@Email = 'm@m.m'
	GO
	
	-- Procédure d'authentification
	CREATE PROCEDURE Utilisateurs.USP_AuthUtilisateur
		@Pseudonyme nvarchar(50),
		@MotDePasse nvarchar(100)
	AS
	BEGIN
		DECLARE @Sel varbinary(16);
		DECLARE @MdpHache varbinary(32);
		SELECT @Sel = MdpSel, @MdpHache = MotDePasseHache 
		FROM Utilisateurs.Utilisateur
		WHERE Pseudonyme = @Pseudonyme; -- Si les pseudos sont uniques !
		
		IF HASHBYTES('SHA2_256', CONCAT(@MotDePasse, @Sel)) = @MdpHache
		BEGIN
			-- On retourne l'utilisateur si le mot de passe est valide
			SELECT * FROM Utilisateurs.Utilisateur WHERE Pseudonyme = @Pseudonyme;
		END
		ELSE
		BEGIN
			SELECT TOP 0 * FROM Utilisateurs.Utilisateur; -- On retourne rien si mot de passe invalide
		END
	END
	GO