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
        private List<Prowadzacy> Prowadzacy;
        private List<Przedmiot> Przedmioty;
        private List<Student> Studenci;
        private List<SkladowaPrzedmiotu> SkladowePrzedmiotow;

        private DataBase Baza;

        private List<Uzytkownik> Uzytkownicy;
        private List<Rola> Role;
        private List<Przywilej> Przywileje;
        private List<Operacja> Operacje;

        private Student ZalogowanyStudent;
        private Prowadzacy ZalogowanyProwadzacy;
        private Rola aktualnaRola;

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            LoginWindow.Visibility = Visibility.Visible;
            /*zalogowanaRola = null;
            Prowadzacy = null;
            Student = null;
            Wyniki = new List<Wynik>();
            Przedmioty = new List<Przedmiot>();
            SkladowePrzedmiotow = new List<SkladowaPrzedmiotu>();*/
            Baza = new DataBase("user123", "haslo123", "localhost", "szkola");
            DataTable table = Baza.pobierz_dane("select c_nazwa from t_Role");

            // REMOVE IT            
            try
            {
                Prowadzacy = Baza.pobierzProwadzacych();
                Przedmioty = Baza.pobierzPrzedmioty();
                Studenci = Baza.pobierzStudentow();
                SkladowePrzedmiotow = Baza.pobierzSkladowePrzedmiotow();
                Wyniki = Baza.pobierzWyniki();

                Uzytkownicy = Baza.pobierzUzytkownikow();
                Role = Baza.pobierzRole();
                Przywileje = Baza.pobierzPrzywileje();
                Operacje = Baza.pobierzOperacje();
            }
            catch (Exception e )
            {
                MessageBox.Show("COs sie zjebalo: " + e.Message);
            }
            // REMOVE IT

            DataRowCollection rows = table.Rows;
            foreach(DataRow row in rows)
            {
                RoleBox.Items.Add(row[0]);
            }
            RoleBox.SelectedIndex = 0;
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            string Login = LoginBox.Text;
            string Password = PobierzHaslo();
            string Rola = RoleBox.SelectedItem.ToString();
            if(CzyIstniejeUzytkownikODanymLoginie(Baza.pobierz_dane("SELECT C_NAZWA FROM T_UZYTKOWNICY"), Login)&&
               CzyHasloJestPrawidlowe(Baza.pobierz_dane("SELECT C_HASLO FROM T_UZYTKOWNICY WHERE C_NAZWA = '"+Login+"'"), Password))
            {
                Uzytkownik uzytkownik = Baza.pobierzUzytkownikow("C_NAZWA = '" + Login + "'").First();
                DataTable table2 = Baza.pobierz_dane("select * from t_przywileje join t_role on c_id_roli = c_Fk_id_roli "+
                                                     "where c_Fk_id_uzytkownika = '" + uzytkownik.IdUzytkownika + "' and c_rola = '" + Rola + "'");
                if (table2.Rows.Count > 0)
                {
                    if (Rola.ToLower() == "student")
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
                    MessageBox.Show("Użytkownik nie posiada wybranej roli");
                }
            }
            else
            {
                MessageBox.Show("Błędne hasło");
            }
        }

        private void StudentLogOutButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Clear();
            LoginWindow.Visibility = System.Windows.Visibility.Visible;
            StudentWindow.Visibility = System.Windows.Visibility.Hidden;
        }

        private string PobierzHaslo()
        {
            byte[] result = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(DataBase.SecureStringToString(PasswordBox.SecurePassword)));
            StringBuilder s = new StringBuilder();
            foreach (byte b in result)
                s.Append(b.ToString("x2").ToLower());
            return s.ToString();
        }

        private bool CzyIstniejeUzytkownikODanymLoginie(DataTable table, string Login)
        {
            bool czyIstnieje = false;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i][0].Equals(Login))
                    czyIstnieje = true;
            }
            return czyIstnieje;
        }

        private bool CzyHasloJestPrawidlowe(DataTable table, string Password)
        {
            bool czyIstnieje = false;
            if (table.Rows.Count>0)
            {
                czyIstnieje = table.Rows[0][0].Equals(Password);
            } 
            return czyIstnieje;
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveUser_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
