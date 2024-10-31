using System.Collections.Generic; // Import the System.Collections.Generic namespace for using generic collections
using System.Linq; // Import the System.Linq namespace for LINQ operations
using System.Windows; // Import the System.Windows namespace for WPF (Windows Presentation Foundation) features
using System.Configuration; // Import the System.Configuration namespace for accessing application configuration settings
using System.Collections.ObjectModel; // Import the System.Collections.ObjectModel namespace for using observable collections

namespace LinqToSQL.ViewModels // Define a namespace for the ViewModels related to the LinqToSQL application
{
    public class MainWindowViewModel : ViewModelBase // Define the MainWindowViewModel class inheriting from ViewModelBase
    {
        private LinqToSqlDataClassesDataContext dataContext; // Declare a private field for the data context to interact with the database

        public ObservableCollection<University> Universities { get; set; } // Declare a public property for a collection of Universities
        public ObservableCollection<Student> Students { get; set; } // Declare a public property for a collection of Students
        public ObservableCollection<Lecture> Lectures { get; set; } // Declare a public property for a collection of Lectures
        public ObservableCollection<StudentLecture> StudentLectures { get; set; } // Declare a public property for a collection of StudentLecture associations

        public ObservableCollection<Lecture> ToniesLectures { get; set; } // Declare a public property for Tonie's Lectures

        public ObservableCollection<Student> StudentsFromYale { get; set; } // Declare a public property for Students from Yale

        public MainWindowViewModel() // Constructor for the MainWindowViewModel class
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LinqToSQL.Properties.Settings.LnqToSQLConnectionString"].ConnectionString; // Retrieve the connection string from the application's configuration file
            dataContext = new LinqToSqlDataClassesDataContext(connectionString); // Initialize the data context with the connection string

            Universities = new ObservableCollection<University>(dataContext.University.ToList()); // Load universities from the database into the Universities collection
            Students = new ObservableCollection<Student>(dataContext.Student.ToList()); // Load students from the database into the Students collection
            Lectures = new ObservableCollection<Lecture>(dataContext.Lecture.ToList()); // Load lectures from the database into the Lectures collection
            StudentLectures = new ObservableCollection<StudentLecture>(dataContext.StudentLecture.ToList()); // Load student lectures from the database into the StudentLectures collection

            ToniesLectures = new ObservableCollection<Lecture>(GetLecturesFromTonie()); // Load Tonie's lectures into the ToniesLectures collection

            StudentsFromYale = new ObservableCollection<Student>(GetAllStudentsFromYale()); // Load students from Yale into the StudentsFromYale collection
        }

        public void InsertUniversity() // Method to insert universities into the database
        {
            dataContext.ExecuteCommand("delete from university"); // Delete existing universities from the database

            University yale = new University { Name = "Yale" }; // Create a new University instance for Yale
            University beijingTech = new University { Name = "Beijing Tech" }; // Create a new University instance for Beijing Tech
            dataContext.University.InsertOnSubmit(yale); // Insert the Yale university instance into the data context
            dataContext.University.InsertOnSubmit(beijingTech); // Insert the Beijing Tech university instance into the data context

            dataContext.SubmitChanges(); // Submit changes to the database
            Universities.Add(yale); // Add the Yale university instance to the Universities collection
            Universities.Add(beijingTech); // Add the Beijing Tech university instance to the Universities collection
        }

        public void InsertStudent() // Method to insert students into the database
        {
            University yale = dataContext.University.First(un => un.Name == "Yale"); // Retrieve the Yale university instance from the database
            University beijingTech = dataContext.University.First(un => un.Name == "Beijing Tech"); // Retrieve the Beijing Tech university instance from the database

            List<Student> students = new List<Student>
            {
                new Student { Name = "Carly", Gender = "female", UniversityId = yale.Id }, // Create a new Student instance for Carly
                new Student { Name = "Tonie", Gender = "male", University = yale }, // Create a new Student instance for Tonie
                new Student { Name = "Leyle", Gender = "female", University = beijingTech }, // Create a new Student instance for Leyle
                new Student { Name = "Jame", Gender = "trans-gender", University = beijingTech } // Create a new Student instance for Jame
            };

            dataContext.Student.InsertAllOnSubmit(students); // Insert the list of students into the data context
            dataContext.SubmitChanges(); // Submit changes to the database

            foreach (var student in students) // Loop through each student in the list
            {
                Students.Add(student); // Add the student instance to the Students collection
            }

            // Refresh Tonie's lectures after inserting students
        }

        public void InsertLectures() // Method to insert lectures into the database
        {
            Lecture math = new Lecture { Name = "Math" }; // Create a new Lecture instance for Math
            Lecture history = new Lecture { Name = "History" }; // Create a new Lecture instance for History

            dataContext.Lecture.InsertOnSubmit(math); // Insert the Math lecture into the data context
            dataContext.Lecture.InsertOnSubmit(history); // Insert the History lecture into the data context

            dataContext.SubmitChanges(); // Submit changes to the database
            Lectures.Add(math); // Add the Math lecture instance to the Lectures collection
            Lectures.Add(history); // Add the History lecture instance to the Lectures collection
        }

        public void InsertStudentLectureAssociations() // Method to insert associations between students and lectures
        {
            Student carly = dataContext.Student.First(st => st.Name == "Carly"); // Retrieve Carly's student instance from the database
            Student tonie = dataContext.Student.First(st => st.Name == "Tonie"); // Retrieve Tonie's student instance from the database

            Lecture math = dataContext.Lecture.First(lc => lc.Name == "Math"); // Retrieve the Math lecture instance from the database
            Lecture history = dataContext.Lecture.First(lc => lc.Name == "History"); // Retrieve the History lecture instance from the database

            StudentLecture slCarlyMath = new StudentLecture { Student = carly, Lecture = math }; // Associate Carly with Math lecture
            StudentLecture slTonieMath = new StudentLecture { Student = tonie, Lecture = math }; // Associate Tonie with Math lecture
            StudentLecture slTonieHistory = new StudentLecture { Student = tonie, Lecture = history }; // Associate Tonie with History lecture

            dataContext.StudentLecture.InsertOnSubmit(slCarlyMath); // Insert Carly's association into the data context
            dataContext.StudentLecture.InsertOnSubmit(slTonieMath); // Insert Tonie's Math association into the data context
            dataContext.StudentLecture.InsertOnSubmit(slTonieHistory); // Insert Tonie's History association into the data context

            dataContext.SubmitChanges(); // Submit changes to the database

            StudentLectures.Add(slCarlyMath); // Add Carly's association to the StudentLectures collection
            StudentLectures.Add(slTonieMath); // Add Tonie's Math association to the StudentLectures collection
            StudentLectures.Add(slTonieHistory); // Add Tonie's History association to the StudentLectures collection

            // Refresh Tonie's lectures after inserting associations
        }

        public List<University> GetUniversityOfTonie() // Method to get Tonie's university
        {
            Student tonie = dataContext.Student.First(st => st.Name == "Tonie"); // Retrieve Tonie's student instance from the database
            return new List<University> { tonie.University }; // Return a list containing Tonie's associated university
        }

        public List<Lecture> GetLecturesFromTonie() // Method to get lectures associated with Tonie
        {
            Student tonie = dataContext.Student.First(st => st.Name == "Tonie"); // Retrieve Tonie's student instance from the database
            return tonie.StudentLecture.Select(sl => sl.Lecture).ToList(); // Return a list of lectures associated with Tonie
        }

        public List<Student> GetAllStudentsFromYale() // Method to get all students from Yale
        {
            var studentsFromYale = from student in dataContext.Student // Query to select students from the database
                                   where student.University.Name == "Yale" // Filter to get students associated with Yale
                                   select student; // Select the student entities

            return studentsFromYale.ToList(); // Convert the query result to a list and return it
        }
    }
}
