using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Configuration;
using System.Collections.ObjectModel;

namespace LinqToSQL.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private LinqToSqlDataClassesDataContext dataContext;

        public ObservableCollection<University> Universities { get; set; }
        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<Lecture> Lectures { get; set; }
        public ObservableCollection<StudentLecture> StudentLectures { get; set; }

        // New property to hold lectures for Tonie
        public ObservableCollection<Lecture> ToniesLectures { get; set; }

        public MainWindowViewModel()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LinqToSQL.Properties.Settings.LnqToSQLConnectionString"].ConnectionString;
            dataContext = new LinqToSqlDataClassesDataContext(connectionString);

            // Initialize collections
            //Universities = new ObservableCollection<University>(dataContext.University.ToList());
            //Students = new ObservableCollection<Student>(dataContext.Student.ToList());
            //Lectures = new ObservableCollection<Lecture>(dataContext.Lecture.ToList());
            //StudentLectures = new ObservableCollection<StudentLecture>(dataContext.StudentLecture.ToList());

            // Initialize Tonie's lectures
            ToniesLectures = new ObservableCollection<Lecture>(GetLecturesFromTonie());
        }

        public void InsertUniversity()
        {
            dataContext.ExecuteCommand("delete from university");

            University yale = new University { Name = "Yale" };
            University beijingTech = new University { Name = "Beijing Tech" };
            dataContext.University.InsertOnSubmit(yale);
            dataContext.University.InsertOnSubmit(beijingTech);

            dataContext.SubmitChanges();
            Universities.Add(yale);
            Universities.Add(beijingTech);
        }

        public void InsertStudent()
        {
            University yale = dataContext.University.First(un => un.Name == "Yale");
            University beijingTech = dataContext.University.First(un => un.Name == "Beijing Tech");

            List<Student> students = new List<Student>
            {
                new Student { Name = "Carly", Gender = "female", UniversityId = yale.Id },
                new Student { Name = "Tonie", Gender = "male", University = yale },
                new Student { Name = "Leyle", Gender = "female", University = beijingTech },
                new Student { Name = "Jame", Gender = "trans-gender", University = beijingTech }
            };

            dataContext.Student.InsertAllOnSubmit(students);
            dataContext.SubmitChanges();

            foreach (var student in students)
            {
                Students.Add(student);
            }

            // Refresh Tonie's lectures after inserting students
            RefreshToniesLectures();
        }

        public void InsertLectures()
        {
            Lecture math = new Lecture { Name = "Math" };
            Lecture history = new Lecture { Name = "History" };

            dataContext.Lecture.InsertOnSubmit(math);
            dataContext.Lecture.InsertOnSubmit(history);

            dataContext.SubmitChanges();
            Lectures.Add(math);
            Lectures.Add(history);
        }

        public void InsertStudentLectureAssociations()
        {
            Student carly = dataContext.Student.First(st => st.Name == "Carly");
            Student tonie = dataContext.Student.First(st => st.Name == "Tonie");

            Lecture math = dataContext.Lecture.First(lc => lc.Name == "Math");
            Lecture history = dataContext.Lecture.First(lc => lc.Name == "History");

            StudentLecture slCarlyMath = new StudentLecture { Student = carly, Lecture = math };
            StudentLecture slTonieMath = new StudentLecture { Student = tonie, Lecture = math };
            StudentLecture slTonieHistory = new StudentLecture { Student = tonie, Lecture = history };

            dataContext.StudentLecture.InsertOnSubmit(slCarlyMath);
            dataContext.StudentLecture.InsertOnSubmit(slTonieMath);
            dataContext.StudentLecture.InsertOnSubmit(slTonieHistory);

            dataContext.SubmitChanges();

            StudentLectures.Add(slCarlyMath);
            StudentLectures.Add(slTonieMath);
            StudentLectures.Add(slTonieHistory);

            // Refresh Tonie's lectures after inserting associations
            RefreshToniesLectures();
        }

        public List<University> GetUniversityOfTonie()
        {
            Student tonie = dataContext.Student.First(st => st.Name == "Tonie");
            return new List<University> { tonie.University };
        }

        public List<Lecture> GetLecturesFromTonie()
        {
            Student tonie = dataContext.Student.First(st => st.Name == "Tonie");
            return tonie.StudentLecture.Select(sl => sl.Lecture).ToList();
        }

        // New method to refresh Tonie's lectures
        private void RefreshToniesLectures()
        {
            ToniesLectures.Clear();
            var lectures = GetLecturesFromTonie();
            foreach (var lecture in lectures)
            {
                ToniesLectures.Add(lecture);
            }
        }
    }
}
