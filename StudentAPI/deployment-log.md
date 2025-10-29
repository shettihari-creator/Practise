# Student API Development and AWS Lambda Deployment Log

## Initial API Creation
1. Created a .NET Core API project with Student management functionality
2. Created the following structure:
   - StudentAPI/
     - Controllers/StudentController.cs
     - Models/Student.cs
     - Data/StudentStore.cs

## API Components

### Student Model
```csharp
namespace StudentAPI.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Grade { get; set; }
    }
}
```

### StudentStore (Static Data)
```csharp
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

        // CRUD operations methods
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
```

### Student Controller
```csharp
using Microsoft.AspNetCore.Mvc;
using StudentAPI.Models;
using StudentAPI.Data;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        // GET: api/student
        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            return Ok(StudentStore.GetAllStudents());
        }

        // GET: api/student/5
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            var student = StudentStore.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        // POST: api/student
        [HttpPost]
        public ActionResult<Student> CreateStudent(Student student)
        {
            StudentStore.AddStudent(student);
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        // PUT: api/student/5
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            var existingStudent = StudentStore.GetStudent(id);
            if (existingStudent == null)
            {
                return NotFound();
            }

            StudentStore.UpdateStudent(student);
            return NoContent();
        }

        // DELETE: api/student/5
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = StudentStore.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }

            StudentStore.DeleteStudent(id);
            return NoContent();
        }
    }
}
```

## AWS Lambda Deployment Steps

1. Added AWS Lambda support packages to the project
2. Created LambdaEntryPoint.cs for AWS Lambda integration
3. Created aws-lambda-tools-defaults.json for Lambda configuration
4. Created IAM role "StudentAPI-lambda-role1" with necessary permissions
5. Deployed to AWS Lambda using dotnet lambda deploy-function
6. Created API Gateway to expose the Lambda function

### Lambda Configuration
```json
{
    "Information": [
        "This file provides default values for the deployment wizard inside Visual Studio and the AWS Lambda commands added to the .NET SDK.",
        "To learn more about the Lambda commands with the .NET SDK execute the following command at the command line in the project root directory.",
        "dotnet lambda help",
        "All the command line options for the Lambda command can be specified in this file."
    ],
    "profile": "",
    "region": "us-east-1",
    "configuration": "Release",
    "function-runtime": "dotnet8",
    "function-memory-size": 256,
    "function-timeout": 30,
    "function-handler": "StudentAPI::StudentAPI.LambdaEntryPoint::FunctionHandler",
    "function-name": "StudentAPI",
    "function-description": "Student API Lambda function",
    "package-type": "Zip",
    "function-architecture": "arm64",
    "function-role": "arn:aws:iam::670513421403:role/StudentAPI-lambda-role1",
    "function-subnets": "",
    "function-security-groups": "",
    "tracing-mode": "PassThrough",
    "environment-variables": ""
}
```

### API Gateway Configuration
- Created HTTP API Gateway
- API Endpoint: https://15qki99508.execute-api.us-east-1.amazonaws.com
- API ID: 15qki99508

## API Endpoints
1. Get all students: GET /api/student
2. Get specific student: GET /api/student/{id}
3. Create student: POST /api/student
4. Update student: PUT /api/student/{id}
5. Delete student: DELETE /api/student/{id}

## Development Environment
- .NET SDK: 8.0 and 9.0 installed
- AWS CLI configured with appropriate credentials
- Region: us-east-1

## Notes and Issues Encountered
1. Initially had issues with .NET runtime versions
2. Resolved IAM role permissions issues
3. Successfully configured API Gateway integration
4. Added necessary Lambda permissions for API Gateway invocation

## Deployment Status
Need to verify Lambda function creation in AWS Console as it wasn't visible in the expected location.