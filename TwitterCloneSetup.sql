--/*CREATE Database, then run rest of script on TwitterClone */
--CREATE DATABASE TwitterClone;
--GO

/* CREATE Tables */

CREATE TABLE Users
(
UserID int NOT NULL PRIMARY KEY IDENTITY(1,1),
UserName varchar(15),
PWord varchar(30)
)
GO


CREATE TABLE Tweets
(
TweetID int NOT NULL PRIMARY KEY IDENTITY(1,1),
UserID int NOT NULL FOREIGN KEY REFERENCES Users(UserID),
TweetText varchar(140),
TweetDate datetime
)
GO


CREATE TABLE UserFollowingXref
(
UserFollowingXrefID int NOT NULL PRIMARY KEY IDENTITY(1,1),
UserID int NOT NULL FOREIGN KEY REFERENCES Users(UserID),
FollowingID int NOT NULL FOREIGN KEY REFERENCES Users(UserID)
)
GO


/* CREATE PROCEDURES */



CREATE PROCEDURE AddUser
@UserName varchar(15),
@PWord varchar(30)
AS
BEGIN
DECLARE @NewUserID int
	Insert Into Users (UserName, PWord) Values (@UserName, @PWord)
	Set @NewUserID = (SELECT TOP 1 UserID FROM Users ORDER BY UserID DESC)
	----When creating a new user, they follow themselves, so their content shows up in their timeline
	Insert Into UserFollowingXref (UserID, FollowingID) Values (@NewUserID, @NewUserID)
	----New Users always follow HockeyFan63, and can see all other profiles from there
	Insert Into UserFollowingXref (UserID, FollowingID) Values (@NewUserID, 1)
END
GO

CREATE PROCEDURE AddTweet
@UserID int,
@TweetText varchar(140)
AS
BEGIN
	Insert Into Tweets (UserID, TweetText, TweetDate) Values (@UserID, @TweetText, GETDATE())
END
GO

CREATE PROCEDURE FollowUser
@UserID int,
@FollowingID int
AS
BEGIN
	Insert Into UserFollowingXref (UserID, FollowingID) Values (@UserID, @FollowingID)
END
GO

CREATE PROCEDURE GetTimeline
@UserID int
AS
BEGIN
SELECT        Users_1.UserName AS FollowerName, dbo.Tweets.TweetText, Users_1.UserID AS FollowerID, dbo.Tweets.TweetDate, dbo.Users.UserName
FROM            dbo.Users LEFT OUTER JOIN
                         dbo.UserFollowingXref ON dbo.Users.UserID = dbo.UserFollowingXref.UserID RIGHT OUTER JOIN
                         dbo.Tweets LEFT OUTER JOIN
                         dbo.Users AS Users_1 ON dbo.Tweets.UserID = Users_1.UserID ON dbo.UserFollowingXref.FollowingID = Users_1.UserID WHERE dbo.UserFollowingXref.UserID = @UserID ORDER BY dbo.Tweets.TweetDate DESC
END
GO

CREATE PROCEDURE GetProfileTweets
@UserID int
AS
BEGIN
SELECT        dbo.Tweets.TweetText, dbo.Tweets.TweetDate, dbo.Users.UserName
FROM            dbo.Tweets RIGHT OUTER JOIN
                         dbo.Users ON dbo.Tweets.UserID = dbo.Users.UserID WHERE dbo.Users.UserID = @UserID ORDER BY dbo.Tweets.TweetDate DESC
END
GO

CREATE PROCEDURE GetProfileFollowers
@UserID int
AS
BEGIN
SELECT        dbo.Users.UserID, dbo.Users.UserName
FROM            dbo.Users LEFT OUTER JOIN
                         dbo.UserFollowingXref ON dbo.Users.UserID = dbo.UserFollowingXref.UserID WHERE dbo.UserFollowingXref.FollowingID = @UserID
END
GO

CREATE PROCEDURE AuthenticateUser
@UserName varchar(15),
@PWord varchar(30),
@UserID int OUTPUT
AS
BEGIN
SELECT @UserID = UserID FROM Users Where UserName = @UserName AND PWord = @PWord
END
GO

----Populate Database with some initial Data
Insert Into Users (UserName, PWord) Values ('HockeyFan63', 'slapshot')
GO
Insert Into Users (UserName, PWord) Values ('ComicsFan25', 'hero')
GO
Insert Into Users (UserName, PWord) Values ('PitMaster', 'bbq')
GO
Insert Into Users (UserName, PWord) Values ('Programmer', 'badpw')
GO

Insert Into UserFollowingXref (UserID, FollowingID) Values (1,1)
GO
Insert Into UserFollowingXref (UserID, FollowingID) Values (2,2)
GO
Insert Into UserFollowingXref (UserID, FollowingID) Values (3,3)
GO
Insert Into UserFollowingXref (UserID, FollowingID) Values (4,4)
GO
Insert Into UserFollowingXref (UserID, FollowingID) Values (1,2)
GO
Insert Into UserFollowingXref (UserID, FollowingID) Values (1,3)
GO
Insert Into UserFollowingXref (UserID, FollowingID) Values (2,3)
GO
Insert Into UserFollowingXref (UserID, FollowingID) Values (3,4)
GO
Insert Into UserFollowingXref (UserID, FollowingID) Values (4,2)
GO
Insert Into UserFollowingXref (UserID, FollowingID) Values (2,1)
GO
Insert Into UserFollowingXref (UserID, FollowingID) Values (3,1)
GO
Insert Into UserFollowingXref (UserID, FollowingID) Values (4,1)
GO

Insert Into Tweets (UserID, TweetText, TweetDate) Values (1, 'Can''t wait for hockey season to start!', '9/1/2015')
GO
Insert Into Tweets (UserID, TweetText, TweetDate) Values (1, 'Pastrnak is a beast!', '9/22/2015')
GO
Insert Into Tweets (UserID, TweetText, TweetDate) Values (1, 'Let''s see how Malcolm Subban does this year', '9/23/2015')
GO

Insert Into Tweets (UserID, TweetText, TweetDate) Values (2, 'Can''t wait for Secret Wars to end', '9/19/2015')
GO
Insert Into Tweets (UserID, TweetText, TweetDate) Values (2, 'Rico Renzi had a spotlight on ComicsAlliance!', '9/25/2015')
GO
Insert Into Tweets (UserID, TweetText, TweetDate) Values (3, 'My sauce is the boss', '9/21/2015')
GO
Insert Into Tweets (UserID, TweetText, TweetDate) Values (4, 'You should really encrypt your passwords', '9/20/2015')
GO
Insert Into Tweets (UserID, TweetText, TweetDate) Values (4, 'Finished my project!', '9/25/2015')
GO