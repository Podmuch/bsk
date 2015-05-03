using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
namespace generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker worker;
        private List<string> imiona;
        private List<string> nazwiska;
        private List<string> przedmioty = new List<string>();
        private List<KeyValuePair<string, List<string>>> katedry = new List<KeyValuePair<string, List<string>>>();

        private List<KeyValuePair<string, List<KeyValuePair<string, List<string>>>>> wydzialy =
            new List<KeyValuePair<string, List<KeyValuePair<string, List<string>>>>>();

        private Random rand = new Random((int) DateTime.Now.Ticks);
        private int iloscProwadzacych = 5;
        private int id = 0;
        private List<Prowadzacy> wylosowaniProwadzacy = new List<Prowadzacy>();
        private List<Przedmiot> wylosowanePrzedmioty = new List<Przedmiot>();
        private List<SkladowaPrzedmiotu> wylosowaneSkladowePrzedmiotu = new List<SkladowaPrzedmiotu>();
        private List<ProwadzacySkladowych> wylosowaniProwadzacySkladowych = new List<ProwadzacySkladowych>();
        private List<Wyniki> wylosowaneWyniki = new List<Wyniki>();
        private List<Studenci> wylosowaniStudenci = new List<Studenci>();
        private List<RodzajSkladowych> wylosowaneRodzajeSkladowych;
        private List<Arkusz1> wylosowaneWierszeArkuszu1 = new List<Arkusz1>();
        private List<Arkusz2> wylosowaneWierszeArkuszu2 = new List<Arkusz2>();

        private void initwydzialy()
        {
            string tmp = System.IO.File.ReadAllText("przedmioty.txt", Encoding.GetEncoding("windows-1250"));
            string[] kat = tmp.Split('&');
            foreach (string k in kat)
            {
                if (k != "")
                {
                    string[] przed = k.Split(new char[] {'\n', '\r', '\t'});
                    katedry.Add(new KeyValuePair<string, List<string>>(przed[0],
                        przed.ToList().Where((x) => !x.Contains("Katedra") && x != "").ToList()));
                    przedmioty.AddRange(przed.ToList().Where((x) => !x.Contains("Katedra") && x != ""));
                }
            }
            wydzialy.Add(new KeyValuePair<string, List<KeyValuePair<string, List<string>>>>("Wydział Architektury",
                new List<KeyValuePair<string, List<string>>>(katedry.GetRange(0, 4))));
            wydzialy.Add(
                new KeyValuePair<string, List<KeyValuePair<string, List<string>>>>(
                    "Wydział Elektroniki, Telekomunikacji i Informatyki",
                    new List<KeyValuePair<string, List<string>>>(katedry.GetRange(4, 4))));
            wydzialy.Add(
                new KeyValuePair<string, List<KeyValuePair<string, List<string>>>>("Wydział Zarządzania i Ekonomii",
                    new List<KeyValuePair<string, List<string>>>(katedry.GetRange(8, 3))));
        }

        private void initImionaNazwiska()
        {
            string tmp = System.IO.File.ReadAllText("imiona.txt", Encoding.GetEncoding("windows-1250"));
            imiona = tmp.Split(new char[] {'\n', '\r', '\t'}).Where((x) => x != "").ToList();
            tmp = System.IO.File.ReadAllText("nazwiska.txt", Encoding.GetEncoding("windows-1250"));
            nazwiska = tmp.Split(new char[] {'\n', '\r', '\t'}).Where((x) => x != "").ToList();
        }

        private BackgroundWorker wczytywanieWorker;
        public MainWindow()
        {
            InitializeComponent();
            initwydzialy();
            initImionaNazwiska();
            losujRodzajeSkladowych();
            wczytywanieWorker = new BackgroundWorker();
            wczytywanieWorker.WorkerReportsProgress = true;
            wczytywanieWorker.WorkerSupportsCancellation = false;
            wczytywanieWorker.DoWork += losujWszystko;
            wczytywanieWorker.ProgressChanged += (x, y) =>
            {
                bar1.Value = y.ProgressPercentage;
            };
            wczytywanieWorker.RunWorkerCompleted += (x, y) =>
            {
                button.IsEnabled = true;
            };
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = false;
            worker.DoWork += zapisz;
            worker.ProgressChanged += (x, y) =>
            {
                bar1.Value = y.ProgressPercentage;
            };
            button.Content = "Generuj";
        }

        private void losujProwadzacych()
        {
            string[] tytuly = new string[3] {"mgr", "dr", "prof"};
            for (int i = 0; i < iloscProwadzacych; i++)
            {
                int wiek = rand.Next(30, 60);
                var tmp = wydzialy[rand.Next(0, wydzialy.Count)];
                string wydzial = tmp.Key;
                string katedra = tmp.Value[rand.Next(0, tmp.Value.Count)].Key;
                string pesel = rand.Next(10, 99) + "0" + rand.Next(1, 10) + "" + rand.Next(10, 28) + "" + rand.Next(10000, 99999);
                wylosowaniProwadzacy.Add(new Prowadzacy(id, pesel, tytuly[rand.Next(0, 3)], wiek - 30, wiek,
                    imiona[rand.Next(0, imiona.Count)], nazwiska[rand.Next(0, nazwiska.Count)],
                    katedra, wydzial));
                id++;
            }
        }

        private void losujPrzedmioty()
        {
            foreach (var przed in przedmioty)
            {
                if (!wylosowanePrzedmioty.Exists((x) => x.nazwa == przed))
                {
                    wylosowanePrzedmioty.Add(new Przedmiot(id, przed, rand.Next(1, 11), 15*rand.Next(1, 5),
                        rand.Next(1, 7),
                        wylosowaniProwadzacy[rand.Next(0, wylosowaniProwadzacy.Count)].id));
                    id++;
                }
            }
        }

        private void losujRodzajeSkladowych()
        {
            wylosowaneRodzajeSkladowych = new List<RodzajSkladowych>();
            wylosowaneRodzajeSkladowych.Add(new RodzajSkladowych("projekt"));
            wylosowaneRodzajeSkladowych.Add(new RodzajSkladowych("laboratorium"));
            wylosowaneRodzajeSkladowych.Add(new RodzajSkladowych("ćwiczenia"));
            wylosowaneRodzajeSkladowych.Add(new RodzajSkladowych("wykład"));
        }

        private void losujSkladowePrzedmiotu()
        {
            foreach (var przedmiot in wylosowanePrzedmioty)
            {
                int iloscgodzin = przedmiot.godziny;
                for (int i = 0; i < 4 && iloscgodzin != 0; i++)
                {
                    int tmp = (iloscgodzin > 15) ? rand.Next(1,1+ iloscgodzin/15)*15 : iloscgodzin;
                    iloscgodzin -= tmp;
                    wylosowaneSkladowePrzedmiotu.Add(new SkladowaPrzedmiotu(id, przedmiot.nazwa,
                        wylosowaneRodzajeSkladowych[rand.Next(0, 4)].nazwa, tmp,
                        wylosowaniProwadzacy[rand.Next(0, wylosowaniProwadzacy.Count)].id));
                    id++;
                }
            }
        }

        private void losujProwadzacychSkladowych()
        {
            foreach (var prowadzacy in wylosowaniProwadzacy)
            {
                for (int i = 0; i < 10; i++)
                {
                    var tmp=new ProwadzacySkladowych(wylosowaneSkladowePrzedmiotu[rand.Next(0, wylosowaneSkladowePrzedmiotu.Count)].idskladowej, prowadzacy.id);
                    if(!wylosowaniProwadzacySkladowych.Exists((x)=>x.idprowadzacego==tmp.idprowadzacego&&x.idskladowej==tmp.idskladowej))
                        wylosowaniProwadzacySkladowych.Add(tmp);
                }
            }
        }

        private void losujStudentow()
        {
            for (int i = 0; i < 100*iloscProwadzacych; i++)
            {
                int rok = rand.Next(1, 5);
                int semestr = (rok - 1)*2 + rand.Next(1, 2);
                int dlug = rand.Next(0, 20);
                string pesel = rand.Next(10, 99)+"0" + rand.Next(1, 10)+"" + rand.Next(10, 28)+""+ rand.Next(10000, 99999).ToString();
                wylosowaniStudenci.Add(new Studenci(id, pesel, imiona[rand.Next(0, imiona.Count)],
                    nazwiska[rand.Next(0, nazwiska.Count)], rok, semestr, dlug));
                id++;
            }
        }

        private void losujWyniki()
        {
            wylosowaneWyniki.Clear();
            for(int i=0;i<wylosowaniStudenci.Count;i++)
            {
                while (wylosowaniStudenci[i].wyniki.Count < wylosowaniStudenci[i].semestr * 5)
                {
                    var przedmiot = wylosowanePrzedmioty[rand.Next(0, wylosowanePrzedmioty.Count)];
                    var przedmiotstudenta = wylosowaniStudenci[i].wyniki.Find((x) => x.idprzedmiotu == przedmiot.id);
                    if (przedmiotstudenta != null)
                    {
                        przedmiotstudenta.wynik = rand.Next(4, 12)*0.5f;
                    }
                    else
                    {
                        wylosowaniStudenci[i].wyniki.Add(new Wyniki(przedmiot.id, przedmiot.nazwa,
                            wylosowaniStudenci[i].nrindeksu, rand.Next(4, 12) * 0.5f));
                    }
                }
                wylosowaneWyniki.AddRange(wylosowaniStudenci[i].wyniki);
            }
        }

        private void losujProtokoly1()
        {
            foreach (var prowadzacySkladowych in wylosowaniProwadzacySkladowych)
            {
                int tmp = rand.Next(1, 5);
                for (int i = 0; i < tmp; i++)
                {
                    wylosowaneWierszeArkuszu1.Add(new Arkusz1(id, prowadzacySkladowych.idskladowej,
                        prowadzacySkladowych.idprowadzacego, rand.Next(1,23)));
                    id++;
                }
            }
        }

        private void losujProtokoly2()
        {
            foreach (var arkusz1 in wylosowaneWierszeArkuszu1)
            {
                SkladowaPrzedmiotu tmp = wylosowaneSkladowePrzedmiotu.Find((x) => x.idskladowej == arkusz1.idskladowej);
                int ilosczajec = (int) (tmp.iloscgodzin/1.5f);
                int indekspoczatkowy = rand.Next(0, wylosowaniStudenci.Count);
                int indekskoncowy = ((indekspoczatkowy + 10) > wylosowaniStudenci.Count)
                    ? (wylosowaniStudenci.Count - indekspoczatkowy)/2
                    : 10;
                var studencidanegoterminu = wylosowaniStudenci.GetRange(indekspoczatkowy, indekskoncowy);
                for (int i = 0; i < ilosczajec; i++)
                {
                    float mozliwepunkty = (float)rand.NextDouble() * 10.0f;
                    int rok = rand.Next(12, 14);
                    int dzien = rand.Next(1, 28);
                    string data = "20" + rok + "-" +
                                  (rok == 12 ? rand.Next(10, 13).ToString() : ("0" + rand.Next(1, 7))) + "-" +
                                  (dzien < 10 ? "0" + dzien : dzien.ToString()) + " 00:00:00.000";
                    foreach(var student in studencidanegoterminu)
                    {
                        string arrayterminy = rand.Next(0, 10) != 0 ? "TAK" : "NIE";
                        float uzyskanewyniki = rand.Next(30, 100)*0.01f*mozliwepunkty;
                        wylosowaneWierszeArkuszu2.Add(new Arkusz2(student.nrindeksu, arkusz1.idterminu, arrayterminy,
                            uzyskanewyniki, mozliwepunkty, data));
                    }
                }     
            }
        }

        private void aktualizujProwadzacych()
        {
            for (int i = 0; i < rand.Next(0, wylosowaniProwadzacy.Count/10); i++)
            {
                var prowadzacy = wylosowaniProwadzacy[rand.Next(0, wylosowaniProwadzacy.Count)];
                switch (prowadzacy.tytul)
                {
                    case "mgr":
                        prowadzacy.tytul = "dr";
                        break;
                    case "dr":
                        prowadzacy.tytul = "prof";
                        break;
                }
                wylosowaniProwadzacy.Remove(prowadzacy);
                wylosowaniProwadzacy.Add(prowadzacy);
            }
            for (int i = 0; i < rand.Next(0, wylosowaniProwadzacy.Count / 10); i++)
            {
                var prowadzacy = wylosowaniProwadzacy[rand.Next(0, wylosowaniProwadzacy.Count)];
                prowadzacy.nazwisko = nazwiska[rand.Next(0, nazwiska.Count)];
                wylosowaniProwadzacy.Remove(prowadzacy);
                wylosowaniProwadzacy.Add(prowadzacy);
            }
            for (int i = 0; i < rand.Next(0, wylosowaniProwadzacy.Count / 10); i++)
            {
                var prowadzacy = wylosowaniProwadzacy[rand.Next(0, wylosowaniProwadzacy.Count)];
                prowadzacy.wiek += 1;
                prowadzacy.staz += 1;
                wylosowaniProwadzacy.Remove(prowadzacy);
                wylosowaniProwadzacy.Add(prowadzacy);
            }
        }

        private void aktualizujProwadzacychSkladowych()
        {
            for (int i = 0; i < rand.Next(0, wylosowaniProwadzacySkladowych.Count / 10); i++)
            {
                var prowadzacy = wylosowaniProwadzacySkladowych[rand.Next(0, wylosowaniProwadzacySkladowych.Count)];
                prowadzacy.idskladowej = wylosowaneSkladowePrzedmiotu[rand.Next(0, wylosowaneSkladowePrzedmiotu.Count)].idskladowej;

                wylosowaniProwadzacySkladowych.Remove(prowadzacy);
                wylosowaniProwadzacySkladowych.Add(prowadzacy);
            }
        }

        private void aktualizujStudentow()
        {
            for(int i=0;i<wylosowaniStudenci.Count;i++)
            {
                wylosowaniStudenci[i].wyniki.ForEach((x) =>
                {
                    if (x.wynik < 3.0)
                    {
                        wylosowaniStudenci[i].dlugects += wylosowanePrzedmioty.Find((y) => y.nazwa == x.nazwaprzedmiotu).ects;
                    }
                });
            }
            foreach (var studenci in wylosowaniStudenci)
            {
                if (studenci.semestr < 10)
                {
                    studenci.semestr ++;
                    studenci.rok = studenci.semestr/2 + 1;
                }
            }
        }

        private void onClick(object sender, RoutedEventArgs e)
        {
            if (button.Content == "Generuj")
                button.Content = "Generuj drugi raz";
            else
                button.Content = "Generuj";
            button.IsEnabled = false;
            iloscProwadzacych = Convert.ToInt32(textbox.Text);
            wczytywanieWorker.RunWorkerAsync();
            aktualizujProwadzacych();
            aktualizujProwadzacychSkladowych();
            aktualizujStudentow();
        }

        private void losujWszystko(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker w = sender as BackgroundWorker;
            losujProwadzacych();
            w.ReportProgress(10);
            Thread.Sleep(1);
            losujPrzedmioty();
            w.ReportProgress(20);
            Thread.Sleep(1);
            losujSkladowePrzedmiotu();
            w.ReportProgress(30);
            Thread.Sleep(1);
            losujProwadzacychSkladowych();
            w.ReportProgress(40);
            Thread.Sleep(1);
            losujStudentow();
            w.ReportProgress(50);
            Thread.Sleep(1);
            losujWyniki();
            w.ReportProgress(60);
            Thread.Sleep(1);
            losujProtokoly1();
            w.ReportProgress(70);
            Thread.Sleep(1);
            losujProtokoly2();
            w.ReportProgress(100);
            Thread.Sleep(1);

        }

        private void zapisz(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker w = sender as BackgroundWorker;

            w.ReportProgress(10);
            Thread.Sleep(1);
            string wylosowaniProwadzacytext = wylosowaniProwadzacy.Aggregate("", (current, x) => current + x.toString2());
            string wylosowanePrzedmiotytext = wylosowanePrzedmioty.Aggregate("", (current, x) => current + x.toString2());
            string wylosowaneRodzajeSkladowychtext = wylosowaneRodzajeSkladowych.Aggregate("", (current, x) => current + x.toString2());
            string wylosowaneSkladowePrzedmiotutext = wylosowaneSkladowePrzedmiotu.Aggregate("", (current, x) => current + x.toString2());
            
            StringBuilder wynikiBuilder = new StringBuilder();
            for (int i = 0; i < wylosowaneWyniki.Count; i++)
            {
                wynikiBuilder.Append(wylosowaneWyniki[i].toString2());
            }
            string wylosowaneWynikitext = wynikiBuilder.ToString();
            string wylosowaniProwadzacySkladowychutext = wylosowaniProwadzacySkladowych.Aggregate("", (current, x) => current + x.toString2());
            string wylosowaniStudencitext = wylosowaniStudenci.Aggregate("", (current, x) => current + x.toString2());
            string wylosowaneWierszeArkuszu1text = wylosowaneWierszeArkuszu1.Aggregate("", (current, x) => current + x.toString2());
            StringBuilder arkusz2Builder = new StringBuilder();
            for (int i = 0; i < wylosowaneWierszeArkuszu2.Count; i++)
            {
                arkusz2Builder.Append(wylosowaneWierszeArkuszu2[i].toString2());
            }
            string wylosowaneWierszeArkuszu2text = arkusz2Builder.ToString();

            w.ReportProgress(50);
            Thread.Sleep(1);
            System.IO.File.WriteAllText("prowadzacy.bulk", wylosowaniProwadzacytext);
            System.IO.File.WriteAllText("przedmioty.bulk", wylosowanePrzedmiotytext);
            System.IO.File.WriteAllText("rodzaje_skladowych.bulk", wylosowaneRodzajeSkladowychtext);
            System.IO.File.WriteAllText("skladowe_przedmiotow.bulk", wylosowaneSkladowePrzedmiotutext);
            System.IO.File.WriteAllText("wyniki.bulk", wylosowaneWynikitext);
            System.IO.File.WriteAllText("prowadzacy_skladowych_czesci.bulk", wylosowaniProwadzacySkladowychutext);
            System.IO.File.WriteAllText("studenci.bulk", wylosowaniStudencitext);
            System.IO.File.WriteAllText("arkusz1.csv", wylosowaneWierszeArkuszu1text);
            System.IO.File.WriteAllText("arkusz2.csv", wylosowaneWierszeArkuszu2text);
            w.ReportProgress(100);
            Thread.Sleep(1);
        }

        private void zapiszprzycisk(object sender, RoutedEventArgs e)
        {
            worker.RunWorkerAsync();
        }
    }
}
