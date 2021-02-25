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
    /// Interaction logic for NewPacjent.xaml
    /// </summary>
    public partial class NewPacjent : Window
    {
        public NewPacjent()
        {
            InitializeComponent();
        }
        public NewPacjent(Pacjenci pacjent)
        {
            InitializeComponent();
            imie_pacjentaTextBox.Text = pacjent.imie_pacjenta;
            nazwisko_pacjentaTextBox.Text = pacjent.nazwisko_pacjenta;
            ulicaTextBox.Text = pacjent.ulica;
            kod_pocztowyTextBox.Text = pacjent.kod_pocztowy;
            miejscowoscTextBox.Text = pacjent.miejscowosc;
            pesel_pacjentaTextBox.Text = pacjent.pesel_pacjenta;
            pesel_pacjentaTextBox.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource pacjenciViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pacjenciViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // pacjenciViewSource.Source = [generic data source]
        }

        private void btnZapiszPacjenci_Click(object sender, RoutedEventArgs e)
        {
            Pacjenci pacjent = new Pacjenci
            {
                imie_pacjenta = imie_pacjentaTextBox.Text,
                nazwisko_pacjenta = nazwisko_pacjentaTextBox.Text,
                ulica = ulicaTextBox.Text,
                kod_pocztowy = kod_pocztowyTextBox.Text,
                miejscowosc = miejscowoscTextBox.Text,
                pesel_pacjenta = pesel_pacjentaTextBox.Text

            };
            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {
                string msg;
                if (pesel_pacjentaTextBox.IsEnabled)
                {
                    db.Pacjenci.Add(pacjent);
                    msg = "Informacja o pacjencie dodana do bazy";
                }
                else
                {
                    db.Entry(pacjent).State = EntityState.Modified;
                    msg = "Informacja o pacjencie została zmieniona w bazie";
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
