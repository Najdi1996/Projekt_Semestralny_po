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

        
        /// <summary>
        /// Konstruktor dla wprowadzenia nowych wartości do tabeli.
        /// </summary>
        public NewWizyta(List<Lekarze> lekarze, List<Choroby> choroby, List<Pacjenci> pacjenci)
        {
            InitializeComponent();
            PrepareWindowData(lekarze, choroby, pacjenci);

        }
        /// <summary>
        /// Konstruktor dla edycji wprowadzonych wartości.
        /// </summary>
        /// <param name="lekarze">Pokazuje wszystkich lekarzy z listy Lekarzy.</param>
        /// <param name="choroby">Pokazuje wszystkie choroby z listy Chorób.</param>
        /// <param name="pacjenci">Pokazuje wszystkich pacjentów z listy Pacjentów.</param>
        /// <param name="wizyta">Odpowiada za pokazanie umówionych wizyt.</param>
        
        public NewWizyta(List<Lekarze> lekarze, List<Choroby> choroby, List<Pacjenci> pacjenci , Wizyty wizyta , List<Choroby> chorobyselected)
        {
            InitializeComponent();
            PrepareWindowData(lekarze, choroby, pacjenci);
            this.wizyta = wizyta;
            nr_lekarzaComboBox.SelectedItem = lekarze.Where(ll => ll.nr_lekarza == wizyta.nr_lekarza).First();
            pesel_pacjentaComboBox.SelectedItem = pacjenci.Where(pp => pp.pesel_pacjenta == wizyta.pesel_pacjenta).First();
            data_wizytyDatePicker.SelectedDate = wizyta.data_wizyty;
            chorobyselected.ForEach(chrf => grdChorobyAddWizyty.SelectedItems.Add(chrf));
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
            Wizyty wizyta;
            
           
            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {
                
                if (this.wizyta != null)
                {
                    db.Wizyty.Attach(this.wizyta);
                    wizyta = this.wizyta;
                    wizyta.Choroby.Clear();
                    wizyta.data_wizyty = (DateTime)data_wizytyDatePicker.SelectedDate;
                    wizyta.nr_lekarza = (nr_lekarzaComboBox.SelectedItem as Lekarze).nr_lekarza;
                    wizyta.pesel_pacjenta = (pesel_pacjentaComboBox.SelectedItem as Pacjenci).pesel_pacjenta;
                    foreach (Choroby chr in grdChorobyAddWizyty.SelectedItems)
                    {
                        db.Choroby.Attach(chr);
                        chr.Wizyty.Add(wizyta);
                    }
                }
                else
                {
                    wizyta = new Wizyty
                    {

                        data_wizyty = (DateTime)data_wizytyDatePicker.SelectedDate,
                        nr_lekarza = (nr_lekarzaComboBox.SelectedItem as Lekarze).nr_lekarza,
                        pesel_pacjenta = (pesel_pacjentaComboBox.SelectedItem as Pacjenci).pesel_pacjenta
                        
                    };
                    foreach (Choroby chr in grdChorobyAddWizyty.SelectedItems)
                    {
                        db.Choroby.Attach(chr);
                        chr.Wizyty.Add(wizyta);
                    }
                }

                string msg;
                if (pesel_pacjentaComboBox != null && nr_lekarzaComboBox != null && data_wizytyDatePicker != null)
                {
                    if(this.wizyta != null)
                    {
                        db.Entry(wizyta).State = EntityState.Modified;
                        msg = "Informacja o wizycie została zmieniona w bazie";
                    }
                    else
                    {
                        db.Wizyty.Add(wizyta);
                        msg = "Informacja o wizycie dodana do bazy";
                    }
                    
                }
                else
                {
                    MessageBox.Show("Dane wizyty nie zostały w pełni wprowadzone");
                    this.DialogResult = false;
                    return;
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

