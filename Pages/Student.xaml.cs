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
            public string Program_Description { get; set; }
            public int Year { get; set; }

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
                                    Program_Description = reader["Program_Description"].ToString(),
                                    Year = Convert.ToInt32(reader["Year"])
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

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            addStudent add = new addStudent();
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
                // Check if a row is selected in the data grid
                if (studentDataGrid.SelectedItem != null)
                {
                    // Get the selected student from the ObservableCollection
                    StudentModel selectedStudent = (StudentModel)studentDataGrid.SelectedItem;

                    // Delete the student record from the database
                    DeleteStudentRecord(selectedStudent);

                    // Remove the student from the ObservableCollection
                    students.Remove(selectedStudent);

                    // Refresh the data grid
                    studentDataGrid.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void DeleteStudentRecord(StudentModel student)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Delete the student record from the Student table
                    string deleteStudentQuery = "DELETE FROM Student WHERE Student_Number = @StudentNumber";

                    using (SqlCommand cmdDeleteStudent = new SqlCommand(deleteStudentQuery, connection))
                    {
                        cmdDeleteStudent.Parameters.AddWithValue("@StudentNumber", student.Student_Number);

                        // Execute the delete query for the Student table
                        cmdDeleteStudent.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting student record: " + ex.Message);
            }
        }

    }
}