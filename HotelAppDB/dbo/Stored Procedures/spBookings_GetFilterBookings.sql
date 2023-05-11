CREATE PROCEDURE [dbo].[spBookings_GetFilterBookings]
	@startDate Date,
	@lastName nvarchar(50)
AS
begin
	set nocount on;

	select b.Id, b.StartDate, b.EndDate, g.FirstName, g.LastName, r.RoomNumber, b.CheckedIn, g.Id as GuestId, r.Id as RoomId, b.TotalCost, rt.Price, rt.Title, rt.Description
	from dbo.Guests g
	join dbo.Bookings b on g.Id = b.GuestId
	join dbo.Rooms r on b.RoomId = r.Id
	join dbo.RoomTypes rt on r.RoomTypeId = rt.Id
	where StartDate = @startDate 
		AND LastName = @lastName;
end

