using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAppLibrary.Data
{
    

    public class SqliteData : IDatabaseData
    {
        private readonly ISqliteDataAccess _db;
        private readonly string connectionStringName = "SqliteDb";

        public SqliteData(ISqliteDataAccess db)
        {
            _db = db;
        }
        public void BookGuest(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
        {
            string sql = "select 1 from Guests where FirstName = @firstName and LastName = @lastName";
            int results = _db.LoadData<dynamic, dynamic>(sql, new { firstName, lastName }, connectionStringName).Count();

            if (results == 0)
            {
                sql = @"INSERT INTO Guests (FirstName, LastName) values (@firstName, @lastName)";

                _db.SaveData(sql, new { firstName, lastName }, connectionStringName);
            }

            sql = @"select [Id], [FirstName], [LastName]
	                from Guests
	                where FirstName = @firstName and LastName = @lastName LIMIT 1;";
            GuestModel guest = _db.LoadData<GuestModel, dynamic>(sql,
                                                         new { firstName, lastName },
                                                         connectionStringName).First();

            sql = "SELECT * FROM RoomTypes where Id = @Id;";
            RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>(sql,
                                                                          new { Id = roomTypeId },
                                                                          connectionStringName).First();

            sql = @"select r.*
	                from Rooms r
	                inner join RoomTypes rt on r.RoomTypeId = rt.Id
	                where r.RoomTypeId = @roomTypeId
	                and r.Id not in (
	                select b.RoomId
	                from Bookings b
	                where (@startDate < b.StartDate and @endDate > b.EndDate)
		                or (b.StartDate <= @endDate and @endDate < b.EndDate)
		                or (b.StartDate <= @startDate and @startDate < b.EndDate)
	                );";
            List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>(sql,
                                                                              new { startDate, endDate, roomTypeId },
                                                                              connectionStringName);
            // [x] find out how many days guest is staying
            TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);

            // [x] book room.
            sql = @"INSERT INTO Bookings (GuestId, RoomId, StartDate, EndDate, TotalCost)
                    VALUES (@guestId, @roomId, @startDate, @endDate, @totalCost);";
            _db.SaveData(sql,
                         new
                         {
                             guestId = guest.Id,
                             roomId = availableRooms.First().Id,
                             startDate = startDate,
                             endDate = endDate,
                             totalCost = (timeStaying.Days * roomType.Price)
                         },
                          connectionStringName);
        }

        public void CheckInGuest(int bookingId)
        {          
            string sql = "UPDATE bookings " +
                "SET CheckedIn = 1 " +
                "WHERE Id = @Id";

            _db.SaveData(sql, new { Id = bookingId }, connectionStringName);
        }

        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            // @ symbol allows for multiple lines
            // whitespace will be sent over though.
            string sql = @"select rt.Id, rt.Title, rt.Description, rt.Price
	                        from Rooms r
	                        inner join RoomTypes rt on r.RoomTypeId = rt.Id
	                        where r.Id not in (
		                        select b.RoomId
		                        from Bookings b
		                        where (@startDate < b.StartDate and @endDate > b.EndDate)
			                        or (b.StartDate <= @endDate and @endDate < b.EndDate)
			                        or (b.StartDate <= @startDate and @startDate < b.EndDate)
	                        )
	                        group by rt.Id, rt.Title, rt.Description, rt.Price";

            var output = _db.LoadData<RoomTypeModel, dynamic>(sql,
                                                        new { startDate, endDate },
                                                        connectionStringName);

            output.ForEach(x => x.Price = x.Price / 100);

            return output;
        }

        public List<FullBookingModel> GetFilterBookings(string lastName)
        {
            string sql = @"select b.Id, b.StartDate, b.EndDate, g.FirstName, g.LastName, r.RoomNumber, b.CheckedIn, g.Id as GuestId, r.Id as RoomId, b.TotalCost, rt.Price, rt.Title, rt.Description
	                    from Guests g
	                    join Bookings b on g.Id = b.GuestId
	                    join Rooms r on b.RoomId = r.Id
	                    join RoomTypes rt on r.RoomTypeId = rt.Id
	                    where StartDate = @startDate AND LastName = @lastName;";

            List<FullBookingModel> output = _db.LoadData<FullBookingModel, dynamic>(sql,
                                           new { lastName, startDate = DateTime.Now.Date },
                                           connectionStringName);

            output.ForEach(x => x.TotalCost = x.TotalCost / 100);
            

            return output;
        }

        public RoomTypeModel RoomTypeIdLookUp(int roomTypeId)
        {
            string sql = "SELECT * FROM RoomTypes WHERE Id = @id";

            RoomTypeModel result = _db.LoadData<RoomTypeModel, dynamic>(sql, new { id = roomTypeId }, connectionStringName).First();

            return result;
        }
    }
}
