using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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

namespace WPF_STUM.Pages
{
    public partial class Person : UserControl
    {
        private ObservableCollection<PersonModel> persons = new ObservableCollection<PersonModel>();
        private string connectionString = "Data Source=atlast\\sqlexpress03;Initial Catalog=SchoolDatabase;Integrated Security=True;";

        public Person()
        {
            InitializeComponent();
            LoadPersonData();
        }

        private class PersonModel
        {
            public string Last_Name { get; set; }
            public string Given_Name { get; set; }
            public string Middle_Name { get; set; }
        }


        private void LoadPersonData()
        {
            try
            {
                persons.Clear(); // Clear the ObservableCollection before reloading

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT Last_Name, Given_Name, Middle_Name FROM Person", connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PersonModel person = new PersonModel
                                {
                                    Last_Name = reader["Last_Name"].ToString(),
                                    Given_Name = reader["Given_Name"].ToString(),
                                    Middle_Name = reader["Middle_Name"].ToString()
                                };

                                persons.Add(person);
                            }
                        }
                    }
                }

                // Set the ItemsSource of the data grid to the collection of persons
                personDataGrid.ItemsSource = persons;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("AddPerson", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Replace with actual values from your UI elements
                        cmd.Parameters.AddWithValue("@LastName", "Doe");
                        cmd.Parameters.AddWithValue("@GivenName", "John");
                        cmd.Parameters.AddWithValue("@MiddleName", "A.");

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Person added successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Implement code for editing person details
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Implement code for saving changes
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Implement code for deleting a person
        }
    }
}
