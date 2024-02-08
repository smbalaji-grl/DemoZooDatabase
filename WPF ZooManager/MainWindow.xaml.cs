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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WPF_ZooManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection conn;
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["WPF_ZooManager.Properties.Settings.DemoDBConnectionString"].ConnectionString;
            conn = new SqlConnection(connectionString);
            showZoos();
            showAnimal();

        }

        private void showAnimal()
        {
            try
            {
                string showAllDisplay = "select * from Animal";
                SqlDataAdapter adapter = new SqlDataAdapter(showAllDisplay,conn);
                using(adapter)
                {
                    DataTable animalTable = new DataTable();
                    adapter.Fill(animalTable);
                    ListassociatedDisplayAnimal.DisplayMemberPath = "Animal";
                    ListassociatedDisplayAnimal.SelectedValue = "Id";
                    ListassociatedDisplayAnimal.ItemsSource = animalTable.DefaultView;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }







        private void showZoos()
        {
            try
            {
                string showAllDataInZooQuery = "select * from Zoo";
                SqlDataAdapter adapter = new SqlDataAdapter(showAllDataInZooQuery, conn);

                using (adapter)
                {
                    DataTable zooTable = new DataTable();
                    adapter.Fill(zooTable);
                    listZoos.DisplayMemberPath = "Location";
                    listZoos.SelectedValuePath = "Id";
                    listZoos.ItemsSource = zooTable.DefaultView;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void showAssociateAnimal()
        {
            try
            {

                string showAllDataInAnimal = "select * from Animal a inner join ZooAnimal za on a.id = za.AnimalID where za.ZooID =@Zoo_ID";
                SqlCommand command = new SqlCommand(showAllDataInAnimal, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                using (adapter)
                {
                    command.Parameters.AddWithValue("@Zoo_ID", listZoos.SelectedValue);
                    DataTable AnimalTable = new DataTable();
                    adapter.Fill(AnimalTable);
                    ListassociatedAnimal.DisplayMemberPath = "Animal";
                    ListassociatedAnimal.SelectedValue = "Id";
                    ListassociatedAnimal.ItemsSource = AnimalTable.DefaultView;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void listZoos_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                showAssociateAnimal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }


}
