CREATE PROCEDURE [dbo].[spRoomTypes_GetById]
	@id int
AS
BEGIN
	set nocount on;

	select [Id], [Title], [Description], [Price]
	from dbo.RoomTypes
	where id = @id;

END