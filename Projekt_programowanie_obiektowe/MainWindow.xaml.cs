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
        }
        private void populateChorobyGrid()
        {
            //grdChoroby.AutoGenerateColumns = false;
            PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities();
            grdChoroby.ItemsSource = db.Choroby.ToList();
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
    }
}
