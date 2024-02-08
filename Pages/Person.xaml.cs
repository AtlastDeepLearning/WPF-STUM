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
            addPerson add = new addPerson();
            add.ShowDialog();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Implement code for editing person details
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if a row is selected
                if (personDataGrid.SelectedItem != null)
                {
                    // Get the selected person from the ObservableCollection
                    PersonModel selectedPerson = (PersonModel)personDataGrid.SelectedItem;

                    // Remove the person from the ObservableCollection
                    persons.Remove(selectedPerson);

                    // Delete the person data from the database
                    DeletePersonData(selectedPerson);

                    // Refresh the data grid
                    personDataGrid.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void DeletePersonData(PersonModel person)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Delete associated records in the Student table
                    string deleteStudentQuery = "DELETE FROM Student WHERE Person_ID = (SELECT Person_ID FROM Person WHERE Last_Name = @LastName AND Given_Name = @GivenName AND Middle_Name = @MiddleName)";

                    using (SqlCommand cmdDeleteStudent = new SqlCommand(deleteStudentQuery, connection))
                    {
                        cmdDeleteStudent.Parameters.AddWithValue("@LastName", person.Last_Name);
                        cmdDeleteStudent.Parameters.AddWithValue("@GivenName", person.Given_Name);
                        cmdDeleteStudent.Parameters.AddWithValue("@MiddleName", person.Middle_Name);

                        // Execute the delete query for the Student table
                        cmdDeleteStudent.ExecuteNonQuery();
                    }

                    // Delete the person from the Person table
                    string deletePersonQuery = "DELETE FROM Person WHERE Last_Name = @LastName AND Given_Name = @GivenName AND Middle_Name = @MiddleName";

                    using (SqlCommand cmdDeletePerson = new SqlCommand(deletePersonQuery, connection))
                    {
                        cmdDeletePerson.Parameters.AddWithValue("@LastName", person.Last_Name);
                        cmdDeletePerson.Parameters.AddWithValue("@GivenName", person.Given_Name);
                        cmdDeletePerson.Parameters.AddWithValue("@MiddleName", person.Middle_Name);

                        // Execute the delete query for the Person table
                        cmdDeletePerson.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting person data: " + ex.Message);
            }
        }


    }
}
