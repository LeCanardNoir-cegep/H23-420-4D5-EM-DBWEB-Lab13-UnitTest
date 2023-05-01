
	CREATE TABLE Golf.Golfeur(
		GolfeurID int IDENTITY(1,1),
		Nom nvarchar(100) NOT NULL,
		ScoreTotal int NOT NULL,
		NbTrous int NOT NULL,
		CONSTRAINT PK_Golf_GolfeurID PRIMARY KEY (GolfeurID)
	);
	GO
	
	CREATE TABLE Golf.ScoreTrou(
		ScoreTrouID int IDENTITY(1,1),
		Score int NOT NULL,
		Terme varchar(20) NOT NULL,
		DateTrou datetime NOT NULL,
		GolfeurID int NOT NULL,
		CONSTRAINT PK_ScorePartie_ScoreTrouID PRIMARY KEY (ScoreTrouID)
	);
	GO
	
	ALTER TABLE Golf.ScoreTrou ADD CONSTRAINT FK_ScorePartie_GolfeurID
	FOREIGN KEY (GolfeurID) REFERENCES Golf.Golfeur(GolfeurID);
	GO
	
	ALTER TABLE Golf.Golfeur ADD CONSTRAINT DF_Golfeur_ScoreTotal
	DEFAULT 0 FOR ScoreTotal;
	GO
	
	ALTER TABLE Golf.Golfeur ADD CONSTRAINT DF_Golfeur_NbTrous
	DEFAULT 0 FOR NbTrous;
	GO
	
	-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
	
	CREATE VIEW Golf.VW_DetailsScoreGolfeur
	AS
		SELECT G.GolfeurID, G.Nom, G.ScoreTotal, G.NbTrous, 
		AVG(ST.Score) AS 'ScoreMoyen', MIN(ST.DateTrou) AS 'DatePremierTrou', 
		MAX(ST.DateTrou) AS 'DateDernierTrou'
		FROM Golf.Golfeur G
		INNER JOIN Golf.ScoreTrou ST
		ON G.GolfeurID = ST.GolfeurID
		GROUP BY G.GolfeurID, G.Nom, G.ScoreTotal, G.NbTrous
	GO
	
	-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
	
	CREATE PROCEDURE Golf.USP_InsertScoreTrou
		@GolfeurID int,
		@Score int
	AS
	BEGIN
		DECLARE @Terme varchar(20);
		
		SET @Terme =
		CASE
			WHEN @Score < -3 THEN 'hacks on'
			WHEN @Score = -3 THEN 'albatross'
			WHEN @Score = -2 THEN 'eagle'
			WHEN @Score = -1 THEN 'birdie'
			WHEN @Score = 0 THEN 'par'
			WHEN @Score = 1 THEN 'bogey'
			WHEN @Score = 2 THEN 'double bogey'
			WHEN @Score = 3 THEN 'triple bogey'
			WHEN @Score > 3 THEN 'git gud'
		END
		
		INSERT INTO Golf.ScoreTrou (Score, Terme, GolfeurID, DateTrou) 
		VALUES (@Score, @Terme, @GolfeurID, GETDATE());
		
	END
	GO
	
	-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
	
	CREATE TRIGGER Golf.TR_ScoreTrou_iudValeursDerivees
	ON Golf.ScoreTrou AFTER INSERT, UPDATE, DELETE
	AS
	BEGIN
		UPDATE Golf.Golfeur
		SET ScoreTotal = (SELECT SUM(ST.Score) FROM Golf.ScoreTrou ST WHERE ST.GolfeurID = Golfeur.GolfeurID)
		WHERE GolfeurID IN (Select GolfeurID FROM inserted) OR GolfeurID IN (Select GolfeurID FROM deleted);
		
		UPDATE Golf.Golfeur
		SET NbTrous = (SELECT COUNT(ST.Score) FROM Golf.ScoreTrou ST WHERE ST.GolfeurID = Golfeur.GolfeurID)
		WHERE GolfeurID IN (Select GolfeurID FROM inserted) OR GolfeurID IN (Select GolfeurID FROM deleted);
	END
	GO
	
	
	
	
	
	
	
	
	