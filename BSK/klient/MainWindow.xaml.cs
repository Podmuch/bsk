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
                UpdateLoginRoleComboBox();

                Operacje = Baza.pobierzOperacje();

                WybraneOperacje = new ObservableCollection<Operacja>();
                SelectedOperationsListView.ItemsSource = WybraneOperacje;
                PozostaleOperacje = new ObservableCollection<Operacja>(Baza.pobierzOperacje());
                OperationsListView.ItemsSource = PozostaleOperacje;

                WybraneGrupy = new ObservableCollection<Grupa>();
                PozostaleGrupy = new ObservableCollection<Grupa>();
                SelectedGroupsListView.ItemsSource = WybraneGrupy;
                GroupsListView.ItemsSource = PozostaleGrupy;

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


        }

        private void Role_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateLoginRoleComboBox();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            string Login = LoginBox.Text.Trim();
            string Password = PobierzHaslo();
            string Rola = RoleBox.SelectedItem.ToString();
            if(CzyIstniejeUzytkownikODanymLoginie(Login)&&
               CzyHasloJestPrawidlowe(Login, Password))
            {
                Uzytkownik uzytkownik = Baza.pobierzUzytkownikow(" WHERE C_LOGIN = '" + Login + "'").First();
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
                        EdycjaRolColumn2Grid.Visibility = System.Windows.Visibility.Hidden;
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
            RoleNameTextBox.Clear();
            WybraneOperacje.Clear();
            PozostaleOperacje.Clear();
            foreach (Operacja o in Operacje)
            {
                if (!WybraneOperacje.Contains(o))
                {
                    PozostaleOperacje.Add(o);
                }
            }
            WybraneGrupy.Clear();
            PozostaleGrupy.Clear();
            foreach (Grupa g in Grupy)
            {
                {
                    PozostaleGrupy.Add(g);
                }
            }
            SaveRoleButton.Content = "Dodaj rolę";
            EdycjaRolColumn2Grid.Visibility = System.Windows.Visibility.Visible;
        }

        private void RolesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {         
            Rola wybranaRola = (Rola)RolesListView.SelectedItem;
            WybraneOperacje.Clear();
            WybraneOperacje = new ObservableCollection<Operacja>(Baza.pobierzOperacjeDlaDanejRoli(wybranaRola.Id));
            SelectedOperationsListView.ItemsSource = WybraneOperacje;
            PozostaleOperacje.Clear();
            foreach(Operacja o in Operacje)
            {
                if (!WybraneOperacje.Contains(o))
                {
                    PozostaleOperacje.Add(o);
                }
            }
            WybraneGrupy.Clear();
            PozostaleGrupy.Clear();
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
            RoleNameTextBox.Text = wybranaRola.Nazwa;
            SaveRoleButton.Content = "Zapisz zmiany";
            EdycjaRolColumn2Grid.Visibility = System.Windows.Visibility.Visible;
        }

        private void SaveRoleButton_Click(object sender, RoutedEventArgs e)
        {
            if(SaveRoleButton.Content.ToString() == "Dodaj rolę")
            {
                if(string.IsNullOrEmpty(RoleNameTextBox.Text))
                {
                    MessageBox.Show("Wprowadz nazwę dla nowej roli.");
                    return;
                }
                if (Baza.pobierzRole(" WHERE c_rola = '" + RoleNameTextBox.Text + "'").Count > 0)
                {
                    MessageBox.Show("Rola o takiej podanej już istnieje.");
                    return;
                }
                Baza.dodajNowaRole(RoleNameTextBox.Text, WybraneGrupy.ToList<Grupa>(), WybraneOperacje.ToList<Operacja>());
                Role = new ObservableCollection<Rola>(Baza.pobierzRole());
                RolesListView.ItemsSource = Role;
                UpdateLoginRoleComboBox();
                EdycjaRolColumn2Grid.Visibility = System.Windows.Visibility.Hidden;
                MessageBox.Show("Rola została dodana.");
            } 
            else
            {

            }        

        }

        private void GroupsSelect_Click(object sender, RoutedEventArgs e)
        {
            if(GroupsListView.SelectedIndex != -1)
            {
                Grupa g = (Grupa)GroupsListView.SelectedItem;
                PozostaleGrupy.Remove(g);
                WybraneGrupy.Add(g);
            }
        }

        private void GroupsSelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach(Grupa g in PozostaleGrupy.ToList<Grupa>())
            {
                PozostaleGrupy.Remove(g);
                WybraneGrupy.Add(g);
            }
        }

        private void GroupsDeselect_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedGroupsListView.SelectedIndex != -1)
            {
                Grupa g = (Grupa)SelectedGroupsListView.SelectedItem;
                WybraneGrupy.Remove(g);
                PozostaleGrupy.Add(g);
            }
        }

        private void GroupsDeselectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (Grupa g in WybraneGrupy.ToList<Grupa>())
            {
                WybraneGrupy.Remove(g);
                PozostaleGrupy.Add(g);
            }
        }

        private void OperationsSelect_Click(object sender, RoutedEventArgs e)
        {
            if (OperationsListView.SelectedIndex != -1)
            {
                Operacja o = (Operacja)OperationsListView.SelectedItem;
                PozostaleOperacje.Remove(o);
                WybraneOperacje.Add(o);
            }
        }

        private void OperationsSelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (Operacja o in PozostaleOperacje.ToList<Operacja>())
            {
                PozostaleOperacje.Remove(o);
                WybraneOperacje.Add(o);
            }
        }

        private void OperationsDeselect_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOperationsListView.SelectedIndex != -1)
            {
                Operacja o = (Operacja)SelectedOperationsListView.SelectedItem;
                WybraneOperacje.Remove(o);
                PozostaleOperacje.Add(o);
            }
        }

        private void OperationsDeselectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (Operacja o in WybraneOperacje.ToList<Operacja>())
            {
                WybraneOperacje.Remove(o);
                PozostaleOperacje.Add(o);
            }
        }

        private void UpdateLoginRoleComboBox()
        {
            RoleBox.Items.Clear();
            foreach (Rola r in Role)
            {
                RoleBox.Items.Add(r.Nazwa);
            }
            RoleBox.SelectedIndex = 0;
        }
    }
}
