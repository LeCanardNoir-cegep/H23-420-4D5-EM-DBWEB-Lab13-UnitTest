
	CREATE TABLE Produits.Produit(
		ProduitID int identity(1,1),
		Categorie nvarchar(50) NOT NULL,
		Nom nvarchar(100) NOT NULL,
		Prix money NOT NULL,
		QteStock int NOT NULL,
		EstDiscontinue bit NOT NULL,
		CONSTRAINT PK_Produit_ProduitID PRIMARY KEY (ProduitID)
	);
	
	ALTER TABLE Produits.Produit ADD CONSTRAINT DF_EstDiscontinue
	DEFAULT 0 FOR EstDiscontinue;
	GO
	
	ALTER TABLE Produits.Produit ADD CONSTRAINT DF_QteStock
	DEFAULT 0 FOR QteStock;
	GO