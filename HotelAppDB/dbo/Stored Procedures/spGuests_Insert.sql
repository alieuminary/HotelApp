CREATE PROCEDURE [dbo].[spGuests_Insert]
	@firstName nvarchar(50),
	@lastName nvarchar(50)
AS
begin
	set nocount on; -- when on, the 'rows count affected message' does not get displayed to client.

	-- check if guest is in the Guests table already
		-- if guest does not exist, add the guest to the Guests table
	if not exists (select 1 from dbo.Guests where FirstName = @firstName and LastName = @lastName)
	begin
		INSERT INTO dbo.Guests (FirstName, LastName)
		values (@firstName, @lastName)
	end

		-- if guest does exist, return guest info
	select top 1 [Id], [FirstName], [LastName]
	from dbo.Guests
	where FirstName = @firstName and LastName = @lastName;
end
