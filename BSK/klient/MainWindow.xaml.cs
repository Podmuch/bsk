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
            Baza = new DataBase("user123", "haslo123", "localhost", "szkola");
            DataTable table = Baza.pobierz_dane("select c_rola from t_Role");
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

            
            byte[] result = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(DataBase.SecureStringToString(PasswordBox.SecurePassword)));
            //byte[] result = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes("haslo123"));
            StringBuilder s = new StringBuilder();
            foreach (byte b in result)
                s.Append(b.ToString("x2").ToLower());
            string Password = s.ToString();

            /*
             * TO NIE DZIALA :/
             * 
             * string Password = System.Security.Cryptography.SHA1.Create(PasswordBox.SecurePassword.ToString()).ToString();
             */

            string Rola = RoleBox.SelectedItem.ToString();
#if !DEBUG

            DataTable table = Baza.pobierz_dane("select * from t_uzytkownicy where c_nazwa = '" + Login + "'");
            if(table.Rows[0][4].ToString()==Password)
            {
                DataTable table2 = Baza.pobierz_dane(
                    "select * from t_przywileje join t_role on c_id_roli = c_Fk_id_roli where c_Fk_id_uzytkownika = '" + table.Rows[0][0] + "'" +
                    "and c_rola = '" + Rola + "'"
                    );
                if (table2.Rows.Count > 0)
                {
                    LoginWindow.Visibility = System.Windows.Visibility.Hidden;
                    StudentWindow.Visibility = System.Windows.Visibility.Visible;
                    DataTable table3 = Baza.pobierz_dane(
                        "select * from t_studenci join t_uzytkownicy on c_Nr_indeksu = c_Fk_nr_indeksu where c_Id_uzytkownika = '" + table2.Rows[0][1] + "'"
                        );
                    StudentNameLabel.Content = "Imię: " + table3.Rows[0][2];
                    StudentFornameLabel.Content = "Nazwisko: " + table3.Rows[0][3];
                }
            }
            else
            {
                MessageBox.Show("Błędne hasło");
            }
#else
            
#endif
        }

        private void StudentLogOutButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Clear();
            LoginWindow.Visibility = System.Windows.Visibility.Visible;
            StudentWindow.Visibility = System.Windows.Visibility.Hidden;
        }


    }
}
