# OTTMyPlatform
To push new project to new git repo
git init
git add .
git commit -m "Add existing project files to Git"
git remote add origin https://github.com/cameronmcnz/example-website.git
git push -u -f origin master


scaffold-DbContext 'Data Source=RPN;Initial Catalog=OTTPlatform; TrustServerCertificate=True;Integrated Security=True' Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models


****************************************************************************************************************************************************

CREATE TABLE Users (UserId int NOT NULL PRIMARY KEY);

INSERT INTO [dbo].[Users]([UserId],[UserName])VALUES
           (1,'rohan')

**************************************************************************

CREATE TABLE TVShow (
    ShowID int NOT NULL PRIMARY KEY,
    Title varchar(255) NOT NULL,
    Description varchar(255)
);

INSERT INTO [dbo].[TVShow]([ShowID],[Title],[Description])VALUES
           (1,'Game of	throwns','Dramatic web series')
INSERT INTO [dbo].[TVShow]([ShowID],[Title],[Description])VALUES
           (2,'URFI','Dramatic and horro web series')

**************************************************************************

CREATE TABLE Episode (
    EpisodeID int NOT NULL PRIMARY KEY,
	EpisodeNumber int,
	EpisodeTimeDuration int,
    ShowID int FOREIGN KEY REFERENCES TVShow(ShowID)
);

INSERT INTO [dbo].[Episode]([EpisodeID],[EpisodeNumber],[EpisodeTimeDuration],[ShowID])
     VALUES(1,1,40,1);
	 INSERT INTO [dbo].[Episode]([EpisodeID],[EpisodeNumber],[EpisodeTimeDuration],[ShowID])
     VALUES(2,2,36,1);
	  INSERT INTO [dbo].[Episode]([EpisodeID],[EpisodeNumber],[EpisodeTimeDuration],[ShowID])
     VALUES(3,1,26,2);
	  INSERT INTO [dbo].[Episode]([EpisodeID],[EpisodeNumber],[EpisodeTimeDuration],[ShowID])
     VALUES(4,2,56,2);


**************************************************************************

CREATE TABLE UserLoginDetail(
        UserName varchar(255),
        Password varchar(255),
        Email varchar(255),
        PhoneNumber int
);

INSERT INTO [dbo].[UserLoginDetail]([UserName],[Password],[Email],[PhoneNumber])VALUES
           ('rohan','myPassword','rohan@gmail.com',87877898);
INSERT INTO [dbo].[UserLoginDetail]([UserName],[Password],[Email],[PhoneNumber])VALUES
           ('admin','admin','admin@gmail.com',65877898);

**************************************************************************

CREATE TABLE UserShowWatchList(
        UserWatchListID int NOT NULL PRIMARY KEY,        
        ShowID int FOREIGN KEY REFERENCES TVShow(ShowID),
        UserId int FOREIGN KEY REFERENCES Users(UserId),
        EpisodeID int FOREIGN KEY REFERENCES Episode(EpisodeID)
);

INSERT INTO [dbo].[UserShowWatchList]
           ([UserWatchListID]
           ,[ShowID]
           ,[UserId]
           ,[EpisodeID])
     VALUES
           (1,1,1,2);

INSERT INTO [dbo].[UserShowWatchList]
           ([UserWatchListID]
           ,[ShowID]
           ,[UserId]
           ,[EpisodeID])
     VALUES
           (2,1,1,1);
**************************************************************************
