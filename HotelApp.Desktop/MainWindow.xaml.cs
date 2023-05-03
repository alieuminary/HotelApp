using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using HotelAppLibrary.Data;
using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;

namespace HotelApp.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDatabaseData _db;
        List<FullBookingModel> results = new List<FullBookingModel>();

        public MainWindow(IDatabaseData db)
        {
            _db = db;
            InitializeComponent();

            bookingsList.ItemsSource = results;
        }

        private void SearchBookings_Click(object sender, RoutedEventArgs e)
        {
            results = _db.GetFilterBookings(firstName.Text, lastName.Text);
        }
    }
}
