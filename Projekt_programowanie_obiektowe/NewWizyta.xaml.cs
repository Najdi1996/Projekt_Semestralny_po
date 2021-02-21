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
    /// Interaction logic for NewWizyta.xaml
    /// </summary>
    public partial class NewWizyta : Window
    {
        public delegate void WizytyEntityChanged();
        public event WizytyEntityChanged wizytyEntityChanged;
        public NewWizyta()
        {
            InitializeComponent();
        }
        public NewWizyta(Wizyty wizyta)
        {
            InitializeComponent();
            data_wizytyDatePicker.SelectedDate = wizyta.data_wizyty;
            nr_lekarzaTextBox.Text = wizyta.nr_lekarza.ToString();
            nr_wizytyTextBox.Text = wizyta.nr_wizyty.ToString();
            pesel_pacjentaTextBox.Text = wizyta.pesel_pacjenta;
            pesel_pacjentaTextBox.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource wizytyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("wizytyViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // wizytyViewSource.Source = [generic data source]
        }

        private void btnZapiszWizyty_Click(object sender, RoutedEventArgs e)
        {
            Wizyty wizyta = new Wizyty
            {
                data_wizyty = data_wizytyDatePicker.DisplayDate,
                nr_lekarza = int.Parse(nr_lekarzaTextBox.Text),
                nr_wizyty = int.Parse(nr_wizytyTextBox.Text),
                pesel_pacjenta = pesel_pacjentaTextBox.Text
               

            };
            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {
                string msg;
                if (pesel_pacjentaTextBox.IsEnabled)
                {
                    db.Wizyty.Add(wizyta);
                    msg = "Informacja o wizycie dodana do bazy";
                }
                else
                {
                    db.Entry(wizyta).State = EntityState.Modified;
                    msg = "Informacja o wizycie została zmieniona w bazie";
                }
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    MessageBox.Show("Wystąpił problem z zapisem do bazy , opis błędu : " + ex.InnerException.InnerException.Message);
                    return;
                }
                MessageBox.Show(msg);


                if (wizytyEntityChanged != null)
                {
                    wizytyEntityChanged();
                }
                this.Close();

            }
        }
    }
}

