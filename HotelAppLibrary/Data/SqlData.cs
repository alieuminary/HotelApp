using System;
using System.Collections.Generic;
using System.Text;
using HotelAppLibrary.Models;
using HotelAppLibrary.Databases;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;


namespace HotelAppLibrary.Data
{
    public class SqlData : IDatabaseData
    {
        private const string connectionStringName = "SqlDB";
        private readonly ISqlDataAccess _db;

        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }

        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            var output = _db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetAvailableTypes",
                                                 new { startDate = startDate, endDate = endDate },
                                                 connectionStringName,
                                                 true);
            return output;
        }

        public void BookGuest(string firstName,
                              string lastName,
                              DateTime startDate,
                              DateTime endDate,
                              int roomTypeId)
        {
            // [x] get guest info to get guest id
            // create a stored procedure that will get the guest's ID
            // logic: if the guest id is not available, insert it to table, then get it from table.
            // save guest id to be used later to insert to Booking table
            GuestModel guest = _db.LoadData<GuestModel, dynamic>("dbo.spGuests_Insert",
                                                         new { firstName, lastName },
                                                         connectionStringName,
                                                         true).First();

            // [] get roomtype price
            // create a sql statement inline that will get the pricing based on the room_type_id
            // this will be used to calculate the total cost spent
            // insert to TotalCost property
            RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>("select * from dbo.RoomTypes where Id = @Id",
                                                                          new { Id = roomTypeId },
                                                                          connectionStringName,
                                                                          false).First();

            // [x] get list of available rooms by roomTypeId
            // create a stored procedure that will get the room ID
            // logic: user will have already selected room_type_id when choosing a room initially.
            // With that knowledge in mind, we can find room_id based on the user's room_type_id chosen.
            // save room_id to be used later to insert to Booking table
            List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>("dbo.spRooms_GetAvailableRooms",
                                                                              new { startDate, endDate, roomTypeId },
                                                                              connectionStringName,
                                                                              true);
            // [x] find out how many days guest is staying
            TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);

            // [x] book room.
            _db.SaveData("dbo.spBookings_Insert",
                         new
                         {
                             guestId = guest.Id,
                             roomId = availableRooms.First().Id,
                             startDate = startDate,
                             endDate = endDate,
                             totalCost = (timeStaying.Days * roomType.Price)
                         },
                          connectionStringName,
                          true);
        }

        // gets the guest's booking information by their first and last name
        public List<FullBookingModel> GetFilterBookings(string firstName, string lastName)
        {
            var output = _db.LoadData<FullBookingModel, dynamic>("dbo.spBookings_GetFilterBookings",
                                                       new { firstName, lastName, startDate = DateTime.Now.Date },
                                                       connectionStringName,
                                                       true);
            return output;
        }

        // TC's solution
        public void CheckInGuest(int bookingId)
        {
            _db.SaveData("dbo.spBookings_CheckIn",
                         new { Id = bookingId },
                         connectionStringName,
                         true);
        }

        // my method - can delete
        public void GuestCheckIn(string firstName, string lastName)
        {
            FullBookingModel booking = GetFilterBookings(firstName, lastName).First();

            int guestId = booking.GuestId;
            int roomId = booking.RoomId;

            _db.SaveData("UPDATE dbo.bookings SET CheckedIn = 1 where GuestId = @guestId AND RoomId = @roomId;",
                         new { guestId, roomId },
                         connectionStringName,
                         false);
        }

        // my method
        public RoomTypeModel RoomTypeIdLookUp(int roomTypeId)
        {
            RoomTypeModel output = _db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetById",
                                                                        new { Id = roomTypeId },
                                                                        connectionStringName,
                                                                        true).First();
            return output;
        }
    }
}
