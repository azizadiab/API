using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentApi.Model;
using System.Net.Cache;
using System.Runtime.CompilerServices;
namespace StudentApi.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/Students")]
    [ApiController]

    public class StudentsController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {

            if (!StudentDataSimulation.StudentsList.Any())
            {
                return NotFound("No Students Found ");
            }
            return Ok(StudentDataSimulation.StudentsList);
        }


        [HttpGet("Passed", Name = "GetPassedStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Student>> GetPassedStudents()
        {
            var passedStudents = StudentDataSimulation.StudentsList.Where(Student => Student.Grade >= 50).ToList();
            // passedStudents.Clear();
            if (passedStudents.Count == 0)
            {
                return NotFound("Not Student Found");
            }

            return Ok(passedStudents);
        }

        [HttpGet("AverageGrade", Name = "GetAverageGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<double> GetAverageGrade()
        {
           // StudentDataSimulation.StudentsList.Clear();

            if (StudentDataSimulation.StudentsList.Count == 0)
            {
                return NotFound("Not Student Found");
            }

            var AverageGrade = StudentDataSimulation.StudentsList.Average(Student => Student.Grade);
            return Ok(AverageGrade);

        }

        [HttpGet("ById/{id}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<Student> GetStudentById(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }
            var Student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            if (Student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }
            return Ok(Student);
        }


        [HttpPost(Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<Student> AddStudent(Student newStudent)
        {

            if (newStudent == null || string.IsNullOrEmpty(newStudent.Name) || newStudent.Age <= 0 || newStudent.Grade < 0)
            {
                return BadRequest("Invalid Student Data");
            }

            newStudent.Id = StudentDataSimulation.StudentsList.Count > 0 ? StudentDataSimulation.StudentsList.Max(s => s.Id) + 1 : 1;

            StudentDataSimulation.StudentsList.Add(newStudent);
            return CreatedAtRoute("GetStudentById", new { id = newStudent.Id }, newStudent);
        }


        [HttpDelete("{id}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult DeleteStudent(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            var Student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            if (Student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            StudentDataSimulation.StudentsList.Remove(Student);
            return Ok($"Student with ID {id} has been deleted.");
            //return NoContent(); // statuscode 204
        }



        [HttpPut("{id}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<Student> UpdateStudent(int id, Student UpdateStudent)
        {
            if (UpdateStudent == null || id < 1 || string.IsNullOrEmpty(UpdateStudent.Name) || UpdateStudent.Age <= 0 || UpdateStudent.Grade < 0)
            {
                return BadRequest("Invalid Student Data");
            }

            var Student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            if (Student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            Student.Name = UpdateStudent.Name;
            Student.Age = UpdateStudent.Age;
            Student.Grade = UpdateStudent.Grade;

            return Ok(Student);
        }

    }
}
