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
    /// Interaction logic for NewLekarz.xaml
    /// </summary>
    public partial class NewLekarz : Window
    {
        public delegate void LekarzeEntityChanged();
        public event LekarzeEntityChanged lekarzeEntityChanged;
        public NewLekarz()
        {
            InitializeComponent();
        }
        public NewLekarz(Lekarze lekarz)
        {

            InitializeComponent();
            imie_lekarzaTextBox.Text = lekarz.imie_lekarza;
            nazwisko_lekarzaTextBox.Text = lekarz.nazwisko_lekarza;
            nr_lekarzaTextBox.Text = lekarz.nr_lekarza.ToString();
            nr_lekarzaTextBox.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource lekarzeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("lekarzeViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // lekarzeViewSource.Source = [generic data source]
        }

        private void btnZapiszLekarze_Click(object sender, RoutedEventArgs e)
        {
            Lekarze lekarz = new Lekarze
            {
                imie_lekarza = imie_lekarzaTextBox.Text,
                nazwisko_lekarza = nazwisko_lekarzaTextBox.Text,
                nr_lekarza = int.Parse(nr_lekarzaTextBox.Text) 
            };
            using (PrzychodniaProjectDBEntities db = new PrzychodniaProjectDBEntities())
            {
                string msg;
                if (nr_lekarzaTextBox.IsEnabled)
                {
                    db.Lekarze.Add(lekarz);
                    msg = "Informacja o lekarzu dodana do bazy";
                }
                else
                {
                    db.Entry(lekarz).State = EntityState.Modified;
                    msg = "Informacja o lekarzu została zmieniona w bazie";
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
                
                
                if (lekarzeEntityChanged != null)
                {
                    lekarzeEntityChanged();
                }
                this.Close();

            }
        }
    }
}
