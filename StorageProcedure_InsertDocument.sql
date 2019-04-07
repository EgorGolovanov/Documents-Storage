CREATE PROCEDURE [dbo].[sp_InsertDocument]
    @name varchar(MAX),
    @autor varchar(MAX),
	@date date,
	@binaryFile varchar(MAX)
AS
    INSERT INTO Document(name, autor, date, binaryFile)
    VALUES (@name, @autor, @date, @binaryFile)
  
    SELECT SCOPE_IDENTITY()

GO