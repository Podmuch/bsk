using klient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public ObservableCollection<Rola> Role;
        private List<Przywilej> Przywileje;
        public List<Operacja> Operacje;
        public ObservableCollection<Operacja> PozostaleOperacje;
        public ObservableCollection<Operacja> WybraneOperacje;

        public List<Grupa> Grupy;
        public ObservableCollection<Grupa> PozostaleGrupy;
        public ObservableCollection<Grupa> WybraneGrupy;


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
            //LoginWindow.Visibility = Visibility.Visible;

            /*zalogowanaRola = null;
            Prowadzacy = null;
            Student = null;
            Wyniki = new List<Wynik>();
            Przedmioty = new List<Przedmiot>();
            SkladowePrzedmiotow = new List<SkladowaPrzedmiotu>();*/
            Baza = new DataBase("user123", "haslo123", "localhost", "szkola");
            DataTable table = Baza.pobierz_dane("select c_rola from t_Role");

            // REMOVE IT            
            try
            {
                Grupy = new List<Grupa>();
                Grupy.Add(new Grupa(1, "Uzytkownik"));
                Grupy.Add(new Grupa(2, "Student"));
                Grupy.Add(new Grupa(4, "Prowadzacy"));
                Grupy.Add(new Grupa(8, "Planista"));
                Grupy.Add(new Grupa(16, "Administrator"));


                Role = new ObservableCollection<Rola>(Baza.pobierzRole());
                RolesListView.ItemsSource = Role;

                Operacje = Baza.pobierzOperacje();
                PozostaleOperacje = new ObservableCollection<Operacja>(Operacje.ToList<Operacja>());
                OperationsListView.ItemsSource = PozostaleOperacje;

               /* Prowadzacy = Baza.pobierzProwadzacych();
                Przedmioty = Baza.pobierzPrzedmioty();
                Studenci = Baza.pobierzStudentow();
                SkladowePrzedmiotow = Baza.pobierzSkladowePrzedmiotow();
                Wyniki = Baza.pobierzWyniki();

                Uzytkownicy = Baza.pobierzUzytkownikow();
                Role = Baza.pobierzRole();
                Przywileje = Baza.pobierzPrzywileje();
                Operacje = Baza.pobierzOperacje();*/
            }
            catch (Exception e )
            {
                MessageBox.Show("Błąd: " + e.Message);
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
            string Login = LoginBox.Text.Trim();
            string Password = PobierzHaslo();
            string Rola = RoleBox.SelectedItem.ToString();
            if(CzyIstniejeUzytkownikODanymLoginie(Login)&&
               CzyHasloJestPrawidlowe(Login, Password))
            {
                Uzytkownik uzytkownik = Baza.pobierzUzytkownikow("C_LOGIN = '" + Login + "'").First();
                DataTable table2 = Baza.pobierz_dane("select * from t_uzytkownicy join t_role on c_grupa & c_grupy_ktorych_dotyczy > 0 " +
                                                     "where c_Id_uzytkownika = " + uzytkownik.IdUzytkownika + " and c_rola = '" + Rola + "'");
                if (table2.Rows.Count > 0)
                {
                    LoginGrid.Visibility = System.Windows.Visibility.Hidden;
                    UserZalogowanyGrid.Visibility = System.Windows.Visibility.Visible;
                    UstawStatus(Login, Rola);
                    if (Rola.ToLower() == "administrator")
                    {
                        UserGrid.Visibility = System.Windows.Visibility.Hidden;
                        AdministratorGrid.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        UserGrid.Visibility = System.Windows.Visibility.Visible;
                        AdministratorGrid.Visibility = System.Windows.Visibility.Hidden;
                    }

                    /*if (Rola.ToLower() == "student")
                    {
                        LoginGrid.Visibility = System.Windows.Visibility.Hidden;
                        UserGrid.Visibility = System.Windows.Visibility.Visible;
                        DataTable table3 = Baza.pobierz_dane(
                            "select * from t_studenci join t_uzytkownicy on c_Nr_indeksu = c_Fk_nr_indeksu where c_Id_uzytkownika = '" + table2.Rows[0][1] + "'"
                            );
                        UserNameLabel.Content = "Imię: " + table3.Rows[0][2];
                        UserFornameLabel.Content = "Nazwisko: " + table3.Rows[0][3];
                    }*/
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
            LoginGrid.Visibility = System.Windows.Visibility.Visible;
            UserGrid.Visibility = System.Windows.Visibility.Hidden;
            AdministratorGrid.Visibility = System.Windows.Visibility.Hidden;
            UserZalogowanyGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private string PobierzHaslo()
        {
            byte[] result = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(DataBase.SecureStringToString(PasswordBox.SecurePassword)));
            StringBuilder s = new StringBuilder();
            foreach (byte b in result)
                s.Append(b.ToString("x2").ToLower());
            return s.ToString();
        }

        private bool CzyIstniejeUzytkownikODanymLoginie(string Login)
        {
            DataTable table = Baza.pobierz_dane("SELECT C_LOGIN FROM T_UZYTKOWNICY");
            bool czyIstnieje = false;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i][0].Equals(Login))
                    czyIstnieje = true;
            }
            return czyIstnieje;
        }

        private bool CzyHasloJestPrawidlowe(string Login, string Password)
        {
            DataTable table = Baza.pobierz_dane("SELECT C_HASLO FROM T_UZYTKOWNICY WHERE C_LOGIN = '" + Login + "'");
            bool czyIstnieje = false;
            if (table.Rows.Count>0)
            {
                czyIstnieje = table.Rows[0][0].Equals(Password);
            } 
            return czyIstnieje;
        }

        private void UstawStatus(string login, string rola)
        {
            UserLoginStatus.Content = "Login:   " + login;
            UserRoleStatus.Content = "Role:   " + rola;            
        }

        private void LoginGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                LogIn_Click(sender, e);
            }
        }

        private void AddNewRoleButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RolesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Rola wybranaRola = (Rola)RolesListView.SelectedItem;
            WybraneOperacje = new ObservableCollection<Operacja>(Baza.pobierzOperacjeDlaDanejRoli(wybranaRola.Id));
            SelectedOperationsListView.ItemsSource = WybraneOperacje;
            PozostaleOperacje = new ObservableCollection<Operacja>();
            foreach(Operacja o in Operacje)
            {
                if (!WybraneOperacje.Contains(o))
                {
                    PozostaleOperacje.Add(o);
                }
            }
                
            OperationsListView.ItemsSource = PozostaleOperacje;
            WybraneGrupy = new ObservableCollection<Grupa>();
            PozostaleGrupy = new ObservableCollection<Grupa>();
            foreach(Grupa g in Grupy)
            {
                if((g.Id & wybranaRola.Grupy_ktorych_dotyczy) > 0)
                {
                    WybraneGrupy.Add(g);
                }
                else
                {
                    PozostaleGrupy.Add(g);
                }
            }
            SelectedGroupsListView.ItemsSource = WybraneGrupy;
            GroupsListView.ItemsSource = PozostaleGrupy;
            RoleNameTextBox.Text = wybranaRola.Nazwa;
        }


    }
}
