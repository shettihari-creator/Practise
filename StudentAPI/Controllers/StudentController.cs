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