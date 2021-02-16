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
       // Choroby chorobaEdit;
       // Lekarze lekarzEdit;
        //PrzychodniaProjectDBEntities context = new PrzychodniaProjectDBEntities();
       // CollectionViewSource chorobyViewSource1;
       // private Choroby _selectedChoroby;
       /* public Choroby selectedChoroby
        {
            get { return _selectedChoroby; }
            set { _selectedChoroby = value;
               // OnPropertyChanged("selectedChoroby");
            }
            
        }*/

        public MainWindow()
        {
            InitializeComponent();
            populateChorobyGrid();
            populateLekarzeGrid();
            populatePacjenciGrid();
            
            DataContext = this;
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
            NewChoroba nc = new NewChoroba(grdChoroby.SelectedItem as Choroby);
            nc.Activate();
            nc.Show();
            nc.chorobyEntityChanged += ChorobyEntityChanged_Handler;
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
                        //db.Lekarze.Remove(lekarz);
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
            NewLekarz nl = new NewLekarz(grdLekarze.SelectedItem as Lekarze);
            nl.Activate();
            nl.Show();
            nl.lekarzeEntityChanged += LekarzeEntityChanged_Handler;
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
            NewPacjent np = new NewPacjent(grdPacjenci.SelectedItem as Pacjenci);
            np.Activate();
            np.Show();
            np.pacjenciEntityChanged += PacjenciEntityChanged_Handler;
        }
        private List<Wizyty> readWizyty()
        {
            PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities();
            return db.Wizyty.ToList();
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
            nc.Show();
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
            nl.Show();
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
            np.Show();
            np.pacjenciEntityChanged += PacjenciEntityChanged_Handler;

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource wizytyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("wizytyViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // wizytyViewSource.Source = [generic data source]
        }

        


        /* private void Window_Loaded(object sender, RoutedEventArgs e)
         {
             chorobyViewSource1 = ((System.Windows.Data.CollectionViewSource)(this.FindResource("chorobyViewSource1")));
             //context. += new EventHandler(context_SaveChanges);
             // Load data by setting the CollectionViewSource.Source property:
             // chorobyViewSource1.Source = [generic data source]

             context.Choroby.Load();
             chorobyViewSource1.Source = context.Choroby.Local;

         }*/




        /*private void Window_Loaded(object sender, RoutedEventArgs e)
         {

             Projekt_programowanie_obiektowe.PrzychodniaProjectDBDataSet przychodniaProjectDBDataSet = ((Projekt_programowanie_obiektowe.PrzychodniaProjectDBDataSet)(this.FindResource("przychodniaProjectDBDataSet")));
             // Load data into the table Choroby. You can modify this code as needed.chorobyViewSource
             Projekt_programowanie_obiektowe.PrzychodniaProjectDBDataSetTableAdapters.ChorobyTableAdapter przychodniaProjectDBDataSetChorobyTableAdapter = new Projekt_programowanie_obiektowe.PrzychodniaProjectDBDataSetTableAdapters.ChorobyTableAdapter();
             przychodniaProjectDBDataSetChorobyTableAdapter.Fill(przychodniaProjectDBDataSet.Choroby);
             System.Windows.Data.CollectionViewSource chorobyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("chorobyViewSource")));
             chorobyViewSource.View.MoveCurrentToFirst();
         }
        */
    }
}
