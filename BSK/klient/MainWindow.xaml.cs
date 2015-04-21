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
#if !DEBUG
            Baza = new DataBase("user", "pass", "localhost\\baza", "katalog");
            DataTable table = Baza.pobierz_dane("select nazwa from role");
            DataRowCollection rows = table.Rows;
            foreach(DataRow row in rows)
            {
                RoleBox.Items.Add(row[0]);
            }
#else
            RoleBox.Items.Add("Administrator");
            RoleBox.Items.Add("Student");
            RoleBox.Items.Add("Prowadzący");
            RoleBox.Items.Add("Planista");
            RoleBox.SelectedIndex = 0;
#endif  
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            string Login = LoginBox.Text;
            string Password = System.Security.Cryptography.SHA1.Create(PasswordBox.SecurePassword.ToString()).ToString();
            string Rola = RoleBox.SelectedItem.ToString();
#if !DEBUG
            DataTable table = Baza.pobierz_dane("select * from uzytkownicy where user = "+Login);
            if(table.Rows[0][5].ToString()==Password)
            {
                DataTable table2 = Baza.pobierz_dane("select * from posiadacze_rol where id_uzytkownika = " + table.Rows[0][0]);
                if (table2.Rows[0][2].ToString() == Rola)
                {
                    //zaloguj
                }
            }
#else
            
#endif
        }


    }
}
