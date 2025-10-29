using StudentAPI.Models;

namespace StudentAPI.Data
{
    public static class StudentStore
    {
        private static List<Student> _students = new List<Student>
        {
            new Student { Id = 1, Name = "John Doe", Age = 20, Grade = "A" },
            new Student { Id = 2, Name = "Jane Smith", Age = 19, Grade = "B" },
            new Student { Id = 3, Name = "Bob Johnson", Age = 21, Grade = "A-" }
        };

        public static List<Student> GetAllStudents()
        {
            return _students;
        }

        public static Student? GetStudent(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id);
        }

        public static void AddStudent(Student student)
        {
            if (_students.Count > 0)
            {
                student.Id = _students.Max(s => s.Id) + 1;
            }
            else
            {
                student.Id = 1;
            }
            _students.Add(student);
        }

        public static void UpdateStudent(Student student)
        {
            var existingStudent = _students.FirstOrDefault(s => s.Id == student.Id);
            if (existingStudent != null)
            {
                existingStudent.Name = student.Name;
                existingStudent.Age = student.Age;
                existingStudent.Grade = student.Grade;
            }
        }

        public static bool DeleteStudent(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                return _students.Remove(student);
            }
            return false;
        }
    }
}