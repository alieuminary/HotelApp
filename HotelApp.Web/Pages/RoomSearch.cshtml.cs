using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HotelAppLibrary.Models;
using HotelAppLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Web.Pages
{
    public class RoomSearchModel : PageModel
    {
        private readonly IDatabaseData _db;

        [BindProperty(SupportsGet = true)]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [BindProperty(SupportsGet = true)]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);

        [BindProperty(SupportsGet = true)]
        public bool SearchEnabled { get; set; } = false;

        [BindProperty]
        public List<RoomTypeModel> AvailableRoomTypes { get; set; } = new List<RoomTypeModel>();

        public RoomSearchModel(IDatabaseData db)
        {
            _db = db;
        }

        public void OnGet()
        {
            if (SearchEnabled == true)
            {
                AvailableRoomTypes = _db.GetAvailableRoomTypes(StartDate, EndDate);
            }
            
        }

        public IActionResult OnPost()
        {
            return RedirectToPage(new 
            { 
                SearchEnabled = true,
                StartDate = StartDate.ToString("yyyy-MM-dd"), 
                EndDate = EndDate.ToString("yyyy-MM-dd")
            });
        }
    }
}
