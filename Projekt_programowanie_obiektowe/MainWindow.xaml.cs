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
                db.SaveChanges();
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
            String ChorobaID = grdChoroby.CurrentItem.ToString();
        }
    }
}
