using HotelAppLibrary.Data;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelApp.Desktop
{
    /// <summary>
    /// Interaction logic for CheckInForm.xaml
    /// </summary>
    public partial class CheckInForm : Window
    {
        private readonly IDatabaseData _db;
        private FullBookingModel _data;

        public CheckInForm(IDatabaseData db)
        {
            InitializeComponent();
            _db = db;
        }

        public void PopulateCheckInInfo(FullBookingModel data)
        {
            _data = data;
            firstNameText.Text = _data.FirstName;
            lasttNameText.Text = _data.LastName;
            roomTitleText.Text = _data.Title;
            roomNumberText.Text = _data.RoomNumber;
            totalCostText.Text = String.Format("{0:C}", _data.TotalCost);
        }

        private void checkInUser_Click(object sender, RoutedEventArgs e)
        {
            _db.CheckInGuest(_data.Id); // booking id
            this.Close();
        }
    }
}
