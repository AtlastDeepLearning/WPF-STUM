using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WPF_STUM.Pages
{
    /// <summary>
    /// Interaction logic for addPerson.xaml
    /// </summary>
    public partial class addPerson : Window
    {
        private string connectionString = "Data Source=atlast\\sqlexpress03;Initial Catalog=SchoolDatabase;Integrated Security=True;";
        public addPerson()
        {
            InitializeComponent();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove(); // Allows dragging of the window
            }
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get values from textboxes
                string firstName = fnameLb.Text;
                string middleName = mnameLb.Text;
                string lastName = lnameLb.Text;

                // Insert a new person into the database
                InsertNewPerson(firstName, middleName, lastName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding new person: " + ex.Message);
            }
        }

        private void InsertNewPerson(string firstName, string middleName, string lastName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Generate a new integer for Person_ID
                    int personId = GetNextPersonId();

                    // Insert a new person into the Person table with a specific Person_ID
                    string insertPersonQuery = "INSERT INTO Person (Person_ID, Last_Name, Given_Name, Middle_Name) VALUES (@PersonID, @LastName, @FirstName, @MiddleName)";

                    using (SqlCommand cmdInsertPerson = new SqlCommand(insertPersonQuery, connection))
                    {
                        cmdInsertPerson.Parameters.AddWithValue("@PersonID", personId);
                        cmdInsertPerson.Parameters.AddWithValue("@LastName", lastName);
                        cmdInsertPerson.Parameters.AddWithValue("@FirstName", firstName);
                        cmdInsertPerson.Parameters.AddWithValue("@MiddleName", middleName);

                        // Execute the insert query for the Person table
                        cmdInsertPerson.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting new person: " + ex.Message);
            }
        }

        private int GetNextPersonId()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Get the next available integer for Person_ID
                    string getNextIdQuery = "SELECT MAX(Person_ID) + 1 FROM Person";

                    using (SqlCommand cmdGetNextId = new SqlCommand(getNextIdQuery, connection))
                    {
                        // Execute the query to get the next available integer
                        object result = cmdGetNextId.ExecuteScalar();

                        // If the result is DBNull, return a default starting value
                        if (result == DBNull.Value)
                        {
                            return 1;
                        }

                        // Otherwise, parse the result as an integer
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting next Person_ID: " + ex.Message);
                return -1; // Return a default value in case of an error
            }
        }


    }
}
