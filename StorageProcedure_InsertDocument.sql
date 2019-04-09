CREATE PROCEDURE [dbo].[sp_InsertDocument]
	@id int,
	@name nvarchar(255),
	@authorId int,
	@date datetime,
	@binaryFile nvarchar(MAX)
AS

INSERT INTO Document(id, NameDocument, AuthorId, Date, BinaryFile)
VALUES(@id, @name, @authorId, @date, @binaryFile)
SELECT SCOPE_IDENTITY() 
GO 