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
        Choroby chorobaEdit;
        Lekarze lekarzEdit;
        PrzychodniaProjectDBEntities context = new PrzychodniaProjectDBEntities();
        CollectionViewSource chorobyViewSource1;
        private Choroby _selectedChoroby;
        public Choroby selectedChoroby
        {
            get { return _selectedChoroby; }
            set { _selectedChoroby = value;
               // OnPropertyChanged("selectedChoroby");
            }
            
        }

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
            PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities();
            return db.Choroby.ToList();
        }
        private void populateChorobyGrid()
        {
            grdChoroby.ItemsSource = this.readChoroby();
        }

        private void grdChoroby_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAddChoroba_Click(object sender, RoutedEventArgs e)
        {
            Choroby choroba = new Choroby();

            choroba.nr_choroby = txtNrChoroby.Text;
            choroba.opis_choroby = txtOpisChoroby.Text;
            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {
                db.Choroby.Add(choroba);
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    MessageBox.Show("Wystąpił problem z zapisem do bazy , opis błędu : " + ex.InnerException.InnerException.Message);
                    return;
                }

                populateChorobyGrid();
                MessageBox.Show("Informacja o chorobie dodana do bazy");
                clearChorobyTextBoxes();
            }
        }
        private void clearChorobyTextBoxes()
        {
            txtNrChoroby.Text = "";
            txtOpisChoroby.Text = "";
        }

        private void grdChoroby_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Choroby choroba = (Choroby)grdChoroby.CurrentItem;
            this.chorobaEdit = choroba;
            txtNrChoroby.Text = choroba.nr_choroby;
            txtNrChoroby.IsEnabled = false;
            txtOpisChoroby.Text = choroba.opis_choroby;
            btnDeleteChoroba.IsEnabled = true;
            //btnEditChoroba.IsEnabled = true;
            btnAddChoroba.IsEnabled = false;

        }

        private void grdChoroby_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnClearChoroba_Click(object sender, RoutedEventArgs e)
        {
            clearChorobyTextBoxes();
            txtNrChoroby.IsEnabled = true;
            btnDeleteChoroba.IsEnabled = false;
            btnEditChoroba.IsEnabled = false;
            btnAddChoroba.IsEnabled = true;
        }

        private void btnEditChoroba_Click(object sender, RoutedEventArgs e)
        {


            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {

                Choroby choroba = db.Choroby.SingleOrDefault(c => c.nr_choroby == txtNrChoroby.Text);

                if (choroba != null)
                {
                    try
                    {
                        choroba.opis_choroby = txtOpisChoroby.Text;
                        db.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                    {
                        MessageBox.Show("Wystąpił problem z zaaktualizowaniem choroby w bazie , opis błędu : " + ex.InnerException.InnerException.Message);
                        return;
                    }
                }

                populateChorobyGrid();
                MessageBox.Show("Informacja o chorobie zaktualizowana w bazie");
                clearChorobyTextBoxes();
                txtNrChoroby.IsEnabled = true;
            }
        }

        private void btnDeleteChoroba_Click(object sender, RoutedEventArgs e)
        {
            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {

                Choroby choroba = db.Choroby.SingleOrDefault(c => c.nr_choroby == txtNrChoroby.Text);

                if (choroba != null)
                {
                    try
                    {
                        db.Choroby.Remove(choroba);
                        db.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                    {
                        MessageBox.Show("Wystąpił problem z usunięciem z bazy , opis błędu : " + ex.InnerException.InnerException.Message);
                        return;
                    }
                }

                populateChorobyGrid();
                MessageBox.Show("Informacja o chorobie została usunięta z bazy");
                clearChorobyTextBoxes();
                txtNrChoroby.IsEnabled = true;
            }
        }

        private void txtOpisChoroby_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtOpisChoroby.Text != this.chorobaEdit.opis_choroby)
            {
                btnEditChoroba.IsEnabled = true;
            }
            else
            {
                btnEditChoroba.IsEnabled = false;
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
        private void grdLekarze_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void grdLekarze_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Lekarze lekarz = (Lekarze)grdLekarze.CurrentItem;
            this.lekarzEdit = lekarz;
            //txtNrLekarza.Text = int Lekarze.nr_lekarza;
            txtNrLekarza.IsEnabled = false;
            txtImieLekarza.Text = lekarz.imie_lekarza;
            txtNazwiskoLekarza.Text = lekarz.nazwisko_lekarza;
            btnDeleteLekarze.IsEnabled = false;
            btnAddLekarze.IsEnabled = false;
        }
        private void clearLekarzeTextBoxes()
        {
            txtNrLekarza.Text = "";
            txtImieLekarza.Text = "";
            txtNazwiskoLekarza.Text = "";
        }

        private void btnClearLekarze_Click(object sender, RoutedEventArgs e)
        {
            clearLekarzeTextBoxes();
            txtNrLekarza.IsEnabled = true;
            btnDeleteLekarze.IsEnabled = false;
            btnEditLekarze.IsEnabled = false;
            btnAddLekarze.IsEnabled = true;
        }
        private void btnAddLekarze_Click(object sender, RoutedEventArgs e)
        {
            Lekarze lekarz = new Lekarze();

            //lekarz.nr_lekarza = txtNrLekarza.Text;
            lekarz.imie_lekarza = txtImieLekarza.Text;
            lekarz.nazwisko_lekarza = txtNazwiskoLekarza.Text;

            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {
                db.Lekarze.Add(lekarz);
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    MessageBox.Show("Wystąpił problem z zapisem do bazy , opis błędu : " + ex.InnerException.InnerException.Message);
                    return;
                }

                populateLekarzeGrid();
                MessageBox.Show("Informacja o lekarzu dodana do bazy");
                clearLekarzeTextBoxes();
            }
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

        private void txtImieLekarza_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtImieLekarza.Text != this.lekarzEdit.imie_lekarza)
            {
                btnEditLekarze.IsEnabled = true;
            }
            else
            {
                btnEditLekarze.IsEnabled = false;
            }
        }

        private void txtNazwiskoLekarza_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNazwiskoLekarza.Text != this.lekarzEdit.nazwisko_lekarza)
            {
                btnEditLekarze.IsEnabled = true;
            }
            else
            {
                btnEditLekarze.IsEnabled = false;
            }
        }

        private List<Pacjenci> readPacjenci()
        {
            PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities();
            return db.Pacjenci.ToList();
        }

        private void populatePacjenciGrid()
        {
            grdPacjenci.ItemsSource = this.readPacjenci();
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
            Lekarze lekarz = (Lekarze)grdUmawianieLekarzy.SelectedItem;
            Pacjenci pacjent = (Pacjenci)grdUmawianiePacjenci.SelectedItem;
            List<Choroby> choroba = (List<Choroby>)grdUmawianieChoroby.SelectedItems.OfType<Choroby>().ToList();
            Wizyty wizyty = new Wizyty();
            wizyty.Lekarze = lekarz;
            wizyty.Pacjenci = pacjent;
            wizyty.Choroby = choroba;
            /*using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {
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
            */
        }

        // do skasowania!!!
        private void resNowaChorobaHandler(object sender, ExecutedRoutedEventArgs e)
        {
            NewChoroba nc = new NewChoroba();
            nc.Activate();
            nc.DataContext = context;
            nc.Show();
            nc.chorobyEntityChanged += ChorobyEntityChanged_Handler; 

        }
        private void btnDelChorobyHandler(object sender, ExecutedRoutedEventArgs e)
        {
            context.Choroby.Remove(selectedChoroby);

        }
        


        private void ChorobyEntityChanged_Handler()
        {
            context.Choroby.Load();
            chorobyViewSource1.View.Refresh();
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


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            chorobyViewSource1 = ((System.Windows.Data.CollectionViewSource)(this.FindResource("chorobyViewSource1")));
            //context. += new EventHandler(context_SaveChanges);
            // Load data by setting the CollectionViewSource.Source property:
            // chorobyViewSource1.Source = [generic data source]

            context.Choroby.Load();
            chorobyViewSource1.Source = context.Choroby.Local;
            
        }

        

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
