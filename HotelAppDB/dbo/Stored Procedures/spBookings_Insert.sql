CREATE PROCEDURE [dbo].[spBookings_Insert]
	@guestId int,
	@roomId int,
	@startDate date,
	@endDate date,
	@totalCost money
AS
	
begin
	set nocount on;

	insert into dbo.Bookings(guestId, roomId, startDate, endDate, totalCost)
	values (@guestId, @roomId, @startDate, @endDate, @totalCost);
end

