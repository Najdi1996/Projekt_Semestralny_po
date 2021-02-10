using System;
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
        public MainWindow()
        {
            InitializeComponent();
            populateChorobyGrid();
            populateLekarzeGrid();
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
                    MessageBox.Show("Wystąpił problem z zapisem do bazy , opis błędu : " + ex.InnerException.InnerException.Message);
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
                        MessageBox.Show("Wystąpił problem z zapisem do bazy , opis błędu : " + ex.InnerException.InnerException.Message);
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
            if(txtOpisChoroby.Text != this.chorobaEdit.opis_choroby)
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
            PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities();
            return db.Lekarze.ToList();
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
        }

        private List<Pacjenci> readPacjenci()
        {
            PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities();
            return db.Pacjenci.ToList();
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
    }
}
