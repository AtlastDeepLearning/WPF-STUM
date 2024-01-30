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

    public partial class Student : UserControl
    {
        private ObservableCollection<StudentModel> students = new ObservableCollection<StudentModel>();
        private string connectionString = "Data Source=atlast\\sqlexpress03;Initial Catalog=SchoolDatabase;Integrated Security=True;";

        public Student()
        {
            InitializeComponent();
            LoadStudentData();
        }

        private class StudentModel
        {
            public string Student_Number { get; set; }
            public int Year { get; set; }
            public string Last_Name { get; set; }
            public string Given_Name { get; set; }
            public string Middle_Name { get; set; }
            public string Program_Description { get; set; }
        }

        private void LoadStudentData()
        {
            try
            {
                students.Clear(); // Clear the ObservableCollection before reloading

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT S.Student_Number, S.Year, P.Last_Name, P.Given_Name, P.Middle_Name, Pr.Program_Description " +
                                                           "FROM Student S " +
                                                           "INNER JOIN Person P ON S.Person_ID = P.Person_ID " +
                                                           "INNER JOIN Program Pr ON S.Program_ID = Pr.Program_ID", connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StudentModel student = new StudentModel
                                {
                                    Student_Number = reader["Student_Number"].ToString(),
                                    Year = Convert.ToInt32(reader["Year"]),
                                    Last_Name = reader["Last_Name"].ToString(),
                                    Given_Name = reader["Given_Name"].ToString(),
                                    Middle_Name = reader["Middle_Name"].ToString(),
                                    Program_Description = reader["Program_Description"].ToString()
                                };

                                students.Add(student);
                            }
                        }
                    }
                }

                // Set the ItemsSource of the data grid to the collection of students
                studentDataGrid.ItemsSource = students;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private class PersonModel
        {
            public string StudentNumber { get; set; }
            public string Year { get; set; }
            public string Program { get; set; }
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
