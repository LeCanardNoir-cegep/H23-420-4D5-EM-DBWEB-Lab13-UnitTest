
	INSERT INTO Golf.Golfeur (Nom) VALUES
	('Tiger Woods'),
	('Patrick (Mii)'),
	('Arnold Palmer'),
	('Jack Nicklaus'),
	('Annika Sörenstam');
	GO
	
	INSERT INTO Golf.ScoreTrou (Score, Terme, DateTrou, GolfeurID) VALUES
	(-1, 'birdie', '20051005', 1),
	(-2, 'eagle', '20061201', 1),
	(1, 'bogey', '20040421', 1),
	(2, 'double bogey', '20080313', 1),
	(0, 'par', '20070707', 1),
	(-3, 'albatross', '20100102', 2),
	(-2, 'eagle', '20091021', 2),
	(-1, 'birdie', '20080714', 2),
	(-2, 'eagle', '20091123', 2),
	(-1, 'birdie', '19810304', 3),
	(0, 'par', '19840411', 3),
	(1, 'bogey', '19850819', 3),
	(1, 'bogey', '19910612', 4),
	(0, 'par', '19930928', 4),
	(-1, 'birdie', '20120730', 5);
	GO