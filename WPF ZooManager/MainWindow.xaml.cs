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
                SqlDataAdapter adapter = new SqlDataAdapter(showAllDisplay, conn);
                using (adapter)
                {
                    DataTable animalTable = new DataTable();
                    adapter.Fill(animalTable);
                    ListassociatedDisplayAnimal.DisplayMemberPath = "Animal";
                    ListassociatedDisplayAnimal.SelectedValuePath = "Id";
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
        private void showAllAnimal()
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
                    ListassociatedAnimal.SelectedValuePath = "Id";
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
                showAllAnimal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DeleteZoo_button(object sender, RoutedEventArgs e)
        {
            try
            {
                string deleteSpecificZoo = "delete  from Zoo where Id= @zoo_Id";
                SqlCommand command = new SqlCommand(deleteSpecificZoo, conn);
                conn.Open();
                command.Parameters.AddWithValue("@zoo_ID", listZoos.SelectedValue);
                command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
                showZoos();
            }
        }

        private void Addzoo_click(object sender, RoutedEventArgs e)
        {
            try
            {
                string AddZooOnClick = "Insert into Zoo values (@Location)";
                SqlCommand command = new SqlCommand(AddZooOnClick, conn);
                conn.Open();
                command.Parameters.AddWithValue("@Location", textBoxZooName.Text);
                command.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
                showZoos();
            }
        }

        private void ADD_animal_click(object sender, RoutedEventArgs e)
        {
            try
            {
                string addAnimal = "insert into ZooAnimal values (@ZooID,@AnimalID)";
                SqlCommand command = new SqlCommand(addAnimal, conn);
                conn.Open();
                command.Parameters.AddWithValue("@AnimalID", ListassociatedDisplayAnimal.SelectedValue);
                command.Parameters.AddWithValue("@ZooID", listZoos.SelectedValue);
                command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
                showAllAnimal();
            }
        }

        private void delete_animal_inAnimalLIst(object sender, RoutedEventArgs e)
        {
            try
            {
                string deleteAnimal = "delete from  Animal where Id=@Animal_Id";
                SqlCommand sql_cmmd = new SqlCommand(deleteAnimal, conn);
                conn.Open();
                sql_cmmd.Parameters.AddWithValue("@Animal_Id", ListassociatedDisplayAnimal.SelectedValue);
                sql_cmmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
                showAllAnimal();
                showAnimal();
            }
        }

        private void AddAnimalInAnimalList(object sender, RoutedEventArgs e)
        {
            try
            {
                string addAnimal = "Insert into Animal values (@animal_name)";
                SqlCommand sql_commsnd = new SqlCommand(addAnimal, conn);
                conn.Open();
                sql_commsnd.Parameters.AddWithValue("@animal_name", textBoxZooName.Text);
                sql_commsnd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
                showAllAnimal();
                showAnimal();
            }
        }
    }


}
