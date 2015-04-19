using klient.Model;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace klient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Rola zalogowanaRola;
        private List<Wynik> Wyniki;
        private Prowadzacy Prowadzacy;
        private List<Przedmiot> Przedmioty;
        private Student Student;
        private List<SkladowaPrzedmiotu> SkladowePrzedmiotow;
        private DataBase Baza;
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            LoginWindow.Visibility = Visibility.Visible;
            zalogowanaRola = null;
            Prowadzacy = null;
            Student = null;
            Wyniki = new List<Wynik>();
            Przedmioty = new List<Przedmiot>();
            SkladowePrzedmiotow = new List<SkladowaPrzedmiotu>();
            Baza = new DataBase("user", "pass", "localhost\\baza", "katalog");
            DataTable table = Baza.pobierz_dane("select * from klienci");
            //RoleBox.
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
