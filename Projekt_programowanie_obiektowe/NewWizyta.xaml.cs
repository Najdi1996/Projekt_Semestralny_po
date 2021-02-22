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
        List<Lekarze> lekarze;
        List<Choroby> choroby;
        List<Pacjenci> pacjenci;
        Wizyty wizyta;

        public NewWizyta(List<Lekarze> lekarze, List<Choroby> choroby , List<Pacjenci> pacjenci)
        {
            InitializeComponent();
            PrepareWindowData(lekarze, choroby,  pacjenci);

        }
        public delegate void WizytyEntityChanged();
        public event WizytyEntityChanged wizytyEntityChanged;
        public NewWizyta()
        {
            InitializeComponent();
        }
        public NewWizyta(List<Lekarze> lekarze, List<Choroby> choroby, List<Pacjenci> pacjenci , Wizyty wizyta , List<Choroby> chorobyselected)
        {
            InitializeComponent();
            PrepareWindowData(lekarze, choroby, pacjenci);
            this.wizyta = wizyta;
            nr_lekarzaComboBox.SelectedItem = lekarze.Where(ll => ll.nr_lekarza == wizyta.nr_lekarza).First();
            pesel_pacjentaComboBox.SelectedItem = pacjenci.Where(pp => pp.pesel_pacjenta == wizyta.pesel_pacjenta).First();
            //List<Choroby> chorobyselected = choroby.Where(chch => wizyta.Choroby.Any(wc => wc.nr_choroby == chch.nr_choroby)).ToList();
            chorobyselected.ForEach(chrf => grdChorobyAddWizyty.SelectedItems.Add(chrf));
            //grdChorobyAddWizyty.SelectedItems.Add(chorobyselected);
            //Lekarze qq = lekarze.Where(ll => ll.nr_lekarza == wizyta.nr_lekarza).First();
            /*
            data_wizytyDatePicker.SelectedDate = wizyta.data_wizyty;
            nr_lekarzaTextBox.Text = wizyta.nr_lekarza.ToString();
            nr_wizytyTextBox.Text = wizyta.nr_wizyty.ToString();
            pesel_pacjentaTextBox.Text = wizyta.pesel_pacjenta;
            pesel_pacjentaTextBox.IsEnabled = false;
            */
        }
        private void PrepareWindowData(List<Lekarze> lekarze, List<Choroby> choroby, List<Pacjenci> pacjenci)
        {
            this.lekarze = lekarze;
            this.choroby = choroby;
            this.pacjenci = pacjenci;
            nr_lekarzaComboBox.ItemsSource = lekarze;
            pesel_pacjentaComboBox.ItemsSource = pacjenci;
            grdChorobyAddWizyty.ItemsSource = choroby;
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
                nr_lekarza = (nr_lekarzaComboBox.SelectedItem as Lekarze).nr_lekarza,
                pesel_pacjenta = (pesel_pacjentaComboBox.SelectedItem as Pacjenci).pesel_pacjenta
                 
        };
            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {

                string msg;
                foreach (Choroby chr in grdChorobyAddWizyty.SelectedItems)
                {
                    db.Choroby.Attach(chr);
                    chr.Wizyty.Add(wizyta);
                }

                db.Wizyty.Add(wizyta);

                if (pesel_pacjentaComboBox != null && nr_lekarzaComboBox != null && data_wizytyDatePicker != null)
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

