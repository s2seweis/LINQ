// Importing necessary namespaces
using System.Collections.Generic;       // Provides classes for defining generic collections, like List<T>
using System.Security.AccessControl;    // Provides classes for access and audit control
using System.Security.Cryptography;     // Provides cryptographic services, including secure data encoding and decoding

// Declaration of the Student class
public class Student
{
    //Declares a public class and makes it accessible from other parts of the program.
    // Unique identifier for each student (primary key in a database context)
    public int Id { get; set; }

    // Stores the name of the student
    public string Name { get; set; }

    // Stores the gender of the student
    public string Gender { get; set; }

    // Foreign key linking the student to a specific university
    public int UniversityId { get; set; }

    // Navigation property to access the University associated with the student
    public University University { get; set; }

    // Collection property for associated StudentLecture objects (many-to-many relationship with Lecture)
    public List<StudentLecture> StudentLecture { get; set; }
}

// Declaration of the University class
public class University
{
    public int Id { get; set; }

    public string Name { get; set; }
}

public class Lecture
{
    public int Id { get; set; }

    public string Name { get; set; }
}

public class StudentLecture
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public Student Student { get; set; }

    public int LectureId { get; set; }

    public Lecture Lecture { get; set; }
}
