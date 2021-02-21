using System;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekt_programowanie_obiektowe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
            populateChorobyGrid();
            populateLekarzeGrid();
            populatePacjenciGrid();
            populateWizyty();
           
        }
        private List<Choroby> readChoroby()
        {
            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {
                return db.Choroby.ToList();
            };
        }
        private void populateChorobyGrid()
        {
            grdChoroby.ItemsSource = this.readChoroby();
        }

        private void btnEditChoroba_Click(object sender, RoutedEventArgs e)
        {
            NewChoroba nce = new NewChoroba(grdChoroby.SelectedItem as Choroby);
            nce.Activate();
            nce.ShowDialog();
            nce.chorobyEntityChanged += ChorobyEntityChanged_Handler;
        }

        private void btnDeleteChoroba_Click(object sender, RoutedEventArgs e)
        {
            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {

                Choroby choroba = (Choroby)grdChoroby.CurrentItem;

                if (choroba != null)
                {
                    try
                    {
                        db.Entry(choroba).State = EntityState.Deleted;
                        db.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                    {
                        MessageBox.Show("Wystąpił problem z usunięciem z bazy , opis błędu : " + ex.InnerException.InnerException.Message);
                        return;
                    }
                }

                populateChorobyGrid();
                MessageBox.Show("Informacja o lekarzu została usunięta z bazy");

                }
            }

        private List<Lekarze> readLekarze()
        {

            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {
                return db.Lekarze.ToList();
            };
            

        }
        private void populateLekarzeGrid()
        {
            grdLekarze.ItemsSource = this.readLekarze();

        }

        private void btnDeleteLekarze_Click(object sender, RoutedEventArgs e)
        {
            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {

                Lekarze lekarz = (Lekarze)grdLekarze.CurrentItem;

                if (lekarz != null)
                {
                    try
                    {
                        db.Entry(lekarz).State = EntityState.Deleted;
                        db.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                    {
                        MessageBox.Show("Wystąpił problem z usunięciem z bazy , opis błędu : " + ex.InnerException.InnerException.Message);
                        return;
                    }
                }

                populateLekarzeGrid();
                MessageBox.Show("Informacja o lekarzu została usunięta z bazy");
                
            }
        }
        private void btnEditLekarze_Click(object sender, RoutedEventArgs e)
        {
            NewLekarz nle = new NewLekarz(grdLekarze.SelectedItem as Lekarze);
            nle.Activate();
            nle.ShowDialog();
            nle.lekarzeEntityChanged += LekarzeEntityChanged_Handler;
        }

        private List<Pacjenci> readPacjenci()
        {
            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {
                return db.Pacjenci.ToList();
            };
        }

        private void populatePacjenciGrid()
        {
            grdPacjenci.ItemsSource = this.readPacjenci();
        }
        private void btnDeletePacjenci_Click(object sender, RoutedEventArgs e)
        {
            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {

                Pacjenci pacjent = (Pacjenci)grdPacjenci.CurrentItem;

                if (pacjent != null)
                {
                    try
                    {
                        db.Entry(pacjent).State = EntityState.Deleted;
                        db.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                    {
                        MessageBox.Show("Wystąpił problem z usunięciem z bazy , opis błędu : " + ex.InnerException.InnerException.Message);
                        return;
                    }
                }

                populatePacjenciGrid();
                MessageBox.Show("Informacja o pacjencie została usunięta z bazy");

            }
       }
        private void btnEditPacjenci_Click(object sender, RoutedEventArgs e)
        {
            NewPacjent npe = new NewPacjent(grdPacjenci.SelectedItem as Pacjenci);
            npe.Activate();
            npe.ShowDialog();
            npe.pacjenciEntityChanged += PacjenciEntityChanged_Handler;
        }
        private List<Wizyty> readWizyty()
        {
            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {
                return db.Wizyty.Include(ll => ll.Lekarze).Include(pp => pp.Pacjenci).ToList();
            };
        }
        private void btnDeleteWizyty_Click(object sender, RoutedEventArgs e)
        {
            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {

                Wizyty wizyta = (Wizyty)grdWizyty.CurrentItem;

                if (wizyta != null)
                {
                    try
                    {
                        db.Entry(wizyta).State = EntityState.Deleted;
                        db.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                    {
                        MessageBox.Show("Wystąpił problem z usunięciem z bazy , opis błędu : " + ex.InnerException.InnerException.Message);
                        return;
                    }
                }

                populateWizyty();
                MessageBox.Show("Informacja o wizycie została usunięta z bazy");

            }
        }
        private void btnEditWizyty_Click(object sender, RoutedEventArgs e)
        {
            NewWizyta nwe = new NewWizyta(this.readLekarze(), this.readChoroby(), this.readPacjenci(), grdWizyty.SelectedItem as Wizyty );
            nwe.Activate();
            nwe.ShowDialog();
            nwe.wizytyEntityChanged += WizytyEntityChanged_Handler;
        }


        private void populateWizyty()
        {
            grdWizyty.ItemsSource = this.readWizyty();
            
        }
        private void grdWizyty_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            populateWizyty();
        }

        private void populateUmawianie()
        {
            grdUmawianieChoroby.ItemsSource = this.readChoroby();
            grdUmawianieLekarzy.ItemsSource = this.readLekarze();
            grdUmawianiePacjenci.ItemsSource = this.readPacjenci();
        }
        
        private void TabItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            populateUmawianie();
        }

        private void btnZapiszWizyte_Click(object sender, RoutedEventArgs e)
        {
            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {
                Lekarze lekarz = (Lekarze)grdUmawianieLekarzy.SelectedItem;
                Pacjenci pacjent = (Pacjenci)grdUmawianiePacjenci.SelectedItem;
                List<Choroby> choroba = (List<Choroby>)grdUmawianieChoroby.SelectedItems.OfType<Choroby>().ToList();
                Wizyty wizyty = new Wizyty();
                db.Lekarze.Attach(lekarz);
                lekarz.Wizyty.Add(wizyty);
                db.Pacjenci.Attach(pacjent);
                pacjent.Wizyty.Add(wizyty);
                wizyty.data_wizyty = WizytyDate.SelectedDate.Value;
                foreach (Choroby chr in choroba)
                {
                    db.Choroby.Attach(chr);
                    chr.Wizyty.Add(wizyty);
                }

                db.Wizyty.Add(wizyty);
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    MessageBox.Show("Wystąpił problem z zapisem do bazy , opis błędu : " + ex.InnerException.InnerException.Message);
                    return;
                }


                MessageBox.Show("Informacja o wizycie dodana do bazy");


            }

        }

        private void ChorobyEntityChanged_Handler()
        {
            populateChorobyGrid();
        }

        private void btnNowaChoroba_Click(object sender, RoutedEventArgs e)
        {
            NewChoroba nc = new NewChoroba();
            nc.Activate();
            nc.ShowDialog();
            nc.chorobyEntityChanged += ChorobyEntityChanged_Handler;
        }


        private void LekarzeEntityChanged_Handler()
        {
            populateLekarzeGrid(); 
        }


        private void btnNowyLekarz_Click(object sender, RoutedEventArgs e)
        {
            NewLekarz nl = new NewLekarz();
            nl.Activate();
            nl.ShowDialog();
            nl.lekarzeEntityChanged += LekarzeEntityChanged_Handler;
            
        }

        private void PacjenciEntityChanged_Handler()
        {
            populatePacjenciGrid();
        }

        private void btnNowyPacjent_Click(object sender, RoutedEventArgs e)
        {
            
            NewPacjent np = new NewPacjent();
            np.Activate();
            np.ShowDialog();
            np.pacjenciEntityChanged += PacjenciEntityChanged_Handler;

        }
        private void WizytyEntityChanged_Handler()
        {
            populateWizyty();
        }

        private void btnNowaWizyta_Click(object sender, RoutedEventArgs e)
        {
            NewWizyta nw = new NewWizyta(this.readLekarze(), this.readChoroby(), this.readPacjenci());
            nw.Activate();
            nw.ShowDialog();
            nw.wizytyEntityChanged += WizytyEntityChanged_Handler;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource wizytyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("wizytyViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // wizytyViewSource.Source = [generic data source]
        }
    }
}
