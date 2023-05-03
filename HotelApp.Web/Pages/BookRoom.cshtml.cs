using HotelAppLibrary.Data;
using HotelAppLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Web.Pages
{
    public class BookRoomModel : PageModel
    {
        private readonly IDatabaseData _db;

        public BookRoomModel(IDatabaseData db)
        {
            _db = db;
        }

        [BindProperty(SupportsGet = true)]
        public int RoomTypeId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [BindProperty]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [BindProperty]
        public string LastName { get; set; }
        public RoomTypeModel RoomInfo { get; set; }

        public void OnGet()
        {
            if (RoomTypeId > 0)
            {
                RoomInfo = _db.RoomTypeIdLookUp(RoomTypeId);
            }

        }
        public IActionResult OnPost()
        {            
            _db.BookGuest(FirstName, LastName, StartDate, EndDate, RoomTypeId);
            return RedirectToPage("/Index");
        }
    }
}
