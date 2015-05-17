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
        private Grid AktualnaTabela;
        private DataBase Baza;
        public ObservableCollection<Rola> Role;
        public ObservableCollection<Uzytkownik> Uzytkownicy;
        public List<Operacja> Operacje;
        public ObservableCollection<Operacja> PozostaleOperacje;
        public ObservableCollection<Operacja> WybraneOperacje;
        public ObservableCollection<Rola> WybraneRole;
        public ObservableCollection<Rola> PozostaleRole;
        private Rola aktualnaRola;
        private Uzytkownik zalogowanyUzytkownik;
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            Baza = new DataBase("user123", "haslo123", "localhost", "szkola");           
            try
            {
                Role = new ObservableCollection<Rola>(Baza.pobierzRole());
                RolesListView.ItemsSource = Role;
                UpdateLoginRoleComboBox();

                Operacje = Baza.pobierzOperacje();

                WybraneOperacje = new ObservableCollection<Operacja>();
                SelectedOperationsListView.ItemsSource = WybraneOperacje;
                PozostaleOperacje = new ObservableCollection<Operacja>(Baza.pobierzOperacje());
                OperationsListView.ItemsSource = PozostaleOperacje;

                WybraneRole = new ObservableCollection<Rola>();
                PozostaleRole = new ObservableCollection<Rola>();
                SelectedRolesListView.ItemsSource = WybraneRole;
                UserRolesListView.ItemsSource = PozostaleRole;
            }
            catch (Exception e )
            {
                MessageBox.Show("Błąd: " + e.Message);
            }
        }
        #region Logowanie
        private void Role_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateLoginRoleComboBox();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            string Login = LoginBox.Text.Trim();
            string Password = PobierzHaslo();
            string Rola = RoleBox.SelectedItem.ToString().ToLower();
            if(CzyIstniejeUzytkownikODanymLoginie(Login)&&
               CzyHasloJestPrawidlowe(Login, Password))
            {
                Uzytkownik uzytkownik = Baza.pobierzUzytkownikow(" WHERE C_LOGIN = '" + Login + "'").First();
                if ((uzytkownik.Grupa >> (RoleBox.SelectedIndex)) % 2 == 1)
                {
                    zalogowanyUzytkownik = uzytkownik;
                    aktualnaRola = Baza.pobierzRole(" WHERE C_ROLA = '" + Rola + "'").First();
                    LoginGrid.Visibility = System.Windows.Visibility.Hidden;
                    UserZalogowanyGrid.Visibility = System.Windows.Visibility.Visible;
                    EdycjaRolColumn2Grid.Visibility = System.Windows.Visibility.Hidden;
                    UstawStatus(Login, Rola);
                    if (Rola== "administrator")
                    {
                        Uzytkownicy = new ObservableCollection<Uzytkownik>(Baza.pobierzUzytkownikow());
                        UsersListView.ItemsSource = Uzytkownicy;
                        UserGrid.Visibility = System.Windows.Visibility.Hidden;
                        AdministratorGrid.Visibility = System.Windows.Visibility.Visible;
                        EdycjaRolColumn2Grid.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else
                    {
                        Operacje = Baza.pobierzOperacje(" join t_przywileje on c_Fk_id_operacji = c_id_operacji where c_Fk_id_roli = " + aktualnaRola.Id);
                        StudiaListView.ItemsSource = WybierzDostepneTabele();
                        UserGrid.Visibility = System.Windows.Visibility.Visible;
                        AdministratorGrid.Visibility = System.Windows.Visibility.Hidden;
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
            if (Uzytkownicy != null)
            {
                Uzytkownicy.Clear();
            }
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
        #endregion
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
            SaveRoleButton.Content = "Dodaj rolę";
            EdycjaRolColumn2Grid.Visibility = System.Windows.Visibility.Visible;
        }

        private void AddNewUserButton_Click(object sender, RoutedEventArgs e)
        {
            UserNameTextBox.Clear();
            WybraneRole.Clear();
            PozostaleRole.Clear();
            foreach (Operacja o in Operacje)
            {
                if (!WybraneOperacje.Contains(o))
                {
                    PozostaleOperacje.Add(o);
                }
            }
            WybraneRole.Clear();
            PozostaleRole.Clear();
            foreach (Rola r in Role)
            {
                {
                    PozostaleRole.Add(r);
                }
            }
            SaveUserButton.Content = "Dodaj uzytkownika";
            EdycjaRolColumn2Grid.Visibility = System.Windows.Visibility.Visible;
        }
        private void UsersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersListView.SelectedItem != null)
            {
                Uzytkownik wybranyUzytkownik = (Uzytkownik)UsersListView.SelectedItem;
                WybraneRole.Clear();
                WybraneRole = new ObservableCollection<Rola>(PobierzRoleUzytkownika(wybranyUzytkownik));
                SelectedRolesListView.ItemsSource = WybraneRole;
                PozostaleRole.Clear();
                foreach (Rola r in Role)
                {
                    if (!WybraneRole.Contains(r))
                    {
                        PozostaleRole.Add(r);
                    }
                }
                UserNameTextBox.Text = wybranyUzytkownik.NazwaUzytkownika;
                SaveUserButton.Content = "Zapisz zmiany";
                EdycjaUzytkownikowColumn2Grid.Visibility = System.Windows.Visibility.Visible;
            }
        }

        public List<Rola> PobierzRoleUzytkownika(Uzytkownik wybranyUzytkownik)
        {
            List<Rola> role = new List<Rola>();
            for (int i = 0; i < Role.Count;i++ )
            {
                if ((wybranyUzytkownik.Grupa >> i) % 2 == 1)
                {
                    role.Add(Role[i]);
                }
            }
            return role;
        }
        private void RolesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RolesListView.SelectedItem != null)
            {
                Rola wybranaRola = (Rola)RolesListView.SelectedItem;
                WybraneOperacje.Clear();
                WybraneOperacje = new ObservableCollection<Operacja>(Baza.pobierzOperacjeDlaDanejRoli(wybranaRola.Id));
                SelectedOperationsListView.ItemsSource = WybraneOperacje;
                PozostaleOperacje.Clear();
                foreach (Operacja o in Operacje)
                {
                    if (!WybraneOperacje.Contains(o))
                    {
                        PozostaleOperacje.Add(o);
                    }
                }
                RoleNameTextBox.Text = wybranaRola.Nazwa;
                SaveRoleButton.Content = "Zapisz zmiany";
                EdycjaRolColumn2Grid.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private void SaveUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveUserButton.Content.ToString() == "Dodaj uzytkownika")
            {
                if (string.IsNullOrEmpty(UserNameTextBox.Text))
                {
                    MessageBox.Show("Wprowadz nazwę dla nowego uzytkownika.");
                    return;
                }
                if (Baza.pobierzUzytkownikow(" WHERE c_login = '" + UserNameTextBox.Text + "'").Count > 0)
                {
                    MessageBox.Show("Uzytkownik o podanej nazwie już istnieje.");
                    return;
                }
                Baza.dodajNowegoUzytkownika(UserNameTextBox.Text, WybraneRole.ToList(), Role.ToList());
                Uzytkownicy = new ObservableCollection<Uzytkownik>(Baza.pobierzUzytkownikow());
                UsersListView.ItemsSource = Uzytkownicy;
                UpdateLoginRoleComboBox();
                EdycjaUzytkownikowColumn2Grid.Visibility = System.Windows.Visibility.Hidden;
                MessageBox.Show("Uzytkownik został dodany.");
            }
            else if(SaveUserButton.Content.ToString() =="Zapisz zmiany")
            {
                Baza.ModyfikujUyztkownika((Uzytkownik)UsersListView.SelectedItem, UserNameTextBox.Text, WybraneRole.ToList(), Role.ToList());
                Uzytkownicy = new ObservableCollection<Uzytkownik>(Baza.pobierzUzytkownikow());
                UsersListView.ItemsSource = Uzytkownicy;
                MessageBox.Show("Zmiany zostaly zapisane.");
            }        
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
                Baza.dodajNowaRole(RoleNameTextBox.Text, WybraneOperacje.ToList<Operacja>());
                Role = new ObservableCollection<Rola>(Baza.pobierzRole());
                RolesListView.ItemsSource = Role;
                UserRolesListView.ItemsSource = Role;
                UpdateLoginRoleComboBox();
                EdycjaRolColumn2Grid.Visibility = System.Windows.Visibility.Hidden;
                MessageBox.Show("Rola została dodana.");
            }
            else if (SaveRoleButton.Content.ToString() == "Zapisz zmiany")
            {
                Baza.ModyfikujRole(RoleNameTextBox.Text, (Rola)RolesListView.SelectedItem, WybraneOperacje.ToList());
                Role = new ObservableCollection<Rola>(Baza.pobierzRole());
                RolesListView.ItemsSource = Role;
                MessageBox.Show("Zmiany zostaly zapisane.");
            }          

        }

        private void RolesSelect_Click(object sender, RoutedEventArgs e)
        {
            if (UserRolesListView.SelectedIndex != -1)
            {
                Rola o = (Rola)UserRolesListView.SelectedItem;
                PozostaleRole.Remove(o);
                WybraneRole.Add(o);
            }
        }
        private void RolesSelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (Rola o in PozostaleRole.ToList<Rola>())
            {
                PozostaleRole.Remove(o);
                WybraneRole.Add(o);
            }
        }
        private void RolesDeselect_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRolesListView.SelectedIndex != -1)
            {
                Rola o = (Rola)SelectedRolesListView.SelectedItem;
                WybraneRole.Remove(o);
                PozostaleRole.Add(o);
            }
        }
        private void RolesDeselectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (Rola o in WybraneRole.ToList<Rola>())
            {
                WybraneRole.Remove(o);
                PozostaleRole.Add(o);
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

        private void StudiaListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StudiaListView.SelectedIndex != -1)
            {
                if (AktualnaTabela != null)
                    AktualnaTabela.Visibility = System.Windows.Visibility.Hidden;
                switch (((Tabela)StudiaListView.SelectedItem).NazwaTabeli)
                {
                    case "Studenci":
                        AktualnaTabela = StudentGrid;
                        WyswietlListeStudentow();
                        break;
                    case "Prowadzacy":
                        AktualnaTabela = ProwadzacyGrid;
                        WyswietlListeProwadzacych();
                        break;
                    case "Wyniki":
                        AktualnaTabela = WynikiGrid;
                        WyswietlListeWynikow();
                        break;
                    case "Przedmioty":
                        AktualnaTabela = PrzedmiotyGrid;
                        WyswietlListePrzedmiotow();
                        break;
                    case "ProwadzacySkladowych":
                        AktualnaTabela = ProwadzacySkladowychGrid;
                        WyswietlListeProwadzacychSkladowych();
                        break;
                    case "SkladowePrzedmiotow":
                        AktualnaTabela = SkladowePrzedmiotowGrid;
                        WyswietlListeSkladowychPrzedmiotow();
                        break;
                }
                AktualnaTabela.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private void WyswietlListeStudentow()
        {
            listaStudentowListView.ItemsSource = new ObservableCollection<Student>(Baza.pobierzStudentow());
        }

        private void WyswietlListeProwadzacych()
        {
            listaProwadzacychListView.ItemsSource = new ObservableCollection<Prowadzacy>(Baza.pobierzProwadzacych());
        }

        private void WyswietlListeWynikow()
        {
            listaWynikowListView.ItemsSource = new ObservableCollection<Wynik>(Baza.pobierzWyniki());
        }

        private void WyswietlListePrzedmiotow()
        {
            listaPrzedmiotowListView.ItemsSource = new ObservableCollection<Przedmiot>(Baza.pobierzPrzedmioty());
        }
        
        private void WyswietlListeProwadzacychSkladowych()
        {
            listaProwadzacychSkladowychListView.ItemsSource = new ObservableCollection<ProwadzacySkladowych>(Baza.pobierzProwadzacychSkladowych());
        }

        private void WyswietlListeSkladowychPrzedmiotow()
        {
            listaSkladowePrzedmiotowListView.ItemsSource = new ObservableCollection<SkladowaPrzedmiotu>(Baza.pobierzSkladowePrzedmiotow());
        }

        private List<Tabela> WybierzDostepneTabele()
        {
            List<Tabela> listaTabel = new List<Tabela>();
            var operacjeSelect = Operacje.FindAll((o)=>o.NazwaOperacji.Contains("select"));
            for (int i = 0; i < operacjeSelect.Count;i++ )
            {
                listaTabel.Add(new Tabela(operacjeSelect[i].NazwaOperacji.Contains("Prowadzacy_Skladowych") ? "ProwadzacySkladowych" :
                                          operacjeSelect[i].NazwaOperacji.Contains("Przedmioty") ? "Przedmioty" :
                                          operacjeSelect[i].NazwaOperacji.Contains("Studenci") ? "Studenci" :
                                          operacjeSelect[i].NazwaOperacji.Contains("Wyniki") ? "Wyniki" :
                                          operacjeSelect[i].NazwaOperacji.Contains("Prowadzacy") ? "Prowadzacy" : "SkladowePrzedmiotow"));
            }
            return listaTabel;
        }
    }
}
