CREATE PROCEDURE [dbo].[spBookings_GetFilterBookings]
	@startDate Date,
	@firstName nvarchar(50),
	@lastName nvarchar(50)
AS
begin
	set nocount on;

	select b.Id, b.StartDate, b.EndDate, g.FirstName, g.LastName, r.RoomNumber, b.CheckedIn, g.Id, r.Id
	from dbo.Guests g
	join dbo.Bookings b on g.Id = b.GuestId
	join dbo.Rooms r on b.RoomId = r.Id
	where StartDate = @startDate 
		AND FirstName = @firstName 
		AND LastName = @lastName;
end

