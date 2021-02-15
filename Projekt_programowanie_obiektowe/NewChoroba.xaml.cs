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
using System.Windows.Shapes;

namespace Projekt_programowanie_obiektowe
{
    /// <summary>
    /// Interaction logic for NewChoroba.xaml
    /// </summary>
    public partial class NewChoroba : Window
    {
        CollectionViewSource chorobyViewSource;
        PrzychodniaProjectDBEntities context = new PrzychodniaProjectDBEntities();
        public delegate void ChorobyEntityChanged();
        public event ChorobyEntityChanged chorobyEntityChanged;
        public NewChoroba()
        {
            InitializeComponent();
        }
        private void NewChorobaHandler(object sender, RoutedEventArgs e)
        {
            var ch = chorobyViewSource.View;
            Choroby choroba = new Choroby
            {
                nr_choroby = nr_chorobyTextBox.Text,
                opis_choroby = opis_chorobyTextBox.Text
            };
            context.Choroby.Add(choroba);
            context.SaveChanges();
            MessageBox.Show("Informacja o chorobie została dodana do bazy");
            if (chorobyEntityChanged != null)
            {
                chorobyEntityChanged();
            }
            this.Close();
           

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            chorobyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("chorobyViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            chorobyViewSource.Source = context.Choroby.Local;
            //chorobyViewSource = DataContext;
        }
    }
}
