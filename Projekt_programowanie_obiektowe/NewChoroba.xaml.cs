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
using System.Windows.Shapes;

namespace Projekt_programowanie_obiektowe
{
    /// <summary>
    /// Interaction logic for NewChoroba.xaml
    /// </summary>
    public partial class NewChoroba : Window
    {
        /// <summary>
        /// Główny konstruktor Nowych Chorób.
        /// </summary>
        public NewChoroba()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Konstruktor odpowiedzialny za edycje w tabeli.
        /// </summary>
        public NewChoroba(Choroby choroba)
        {
            InitializeComponent();
            opis_chorobyTextBox.Text = choroba.opis_choroby;
            nr_chorobyTextBox.Text = choroba.nr_choroby;
            nr_chorobyTextBox.IsEnabled = false;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource chorobyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("chorobyViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // chorobyViewSource.Source = [generic data source]
        }

        private void BtnZapiszChoroba_Click(object sender, RoutedEventArgs e)
        {
            Choroby choroba = new Choroby
            {
                nr_choroby = nr_chorobyTextBox.Text,
                opis_choroby = opis_chorobyTextBox.Text
            };
            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {
                string msg;
                if (nr_chorobyTextBox.IsEnabled)
                {
                    db.Choroby.Add(choroba);
                    msg = "Informacja o chorobie dodana do bazy";
                }
                else
                {
                    db.Entry(choroba).State = EntityState.Modified;
                    msg = "Informacja o chorobie została zmieniona w bazie";
                }
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    MessageBox.Show("Wystąpił problem z zapisem do bazy , opis błędu : " + ex.InnerException.InnerException.Message);
                    this.DialogResult = false;
                    return;
                }
                MessageBox.Show(msg);
                this.DialogResult = true;
                this.Close();

            }
        }
    }
}
