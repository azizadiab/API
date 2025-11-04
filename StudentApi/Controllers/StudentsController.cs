using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using StudentApi.Model;
using StudentAPIBusinessLayer;
using StudentApiDataAccessLayer;
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
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudents()
        {
            List<StudentDTO> StudentsList = StudentAPIBusinessLayer.Student.GetAllStudents();
            if (StudentsList.Count == 0)
            {
                return NotFound("No Students Found ");
            }
            return Ok(StudentsList);
        }


        [HttpGet("Passed", Name = "GetPassedStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Student>> GetPassedStudents()
        {
            List<StudentDTO> StudentsList = StudentAPIBusinessLayer.Student.GetPassedStudents();
            if (StudentsList.Count == 0)
            {
                return NotFound("Not student Found");
            }

            return Ok(StudentsList);
        }

        [HttpGet("AverageGrade", Name = "GetAverageGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<double> GetAverageGrade()
        {

            var AverageGrade = StudentAPIBusinessLayer.Student.GetAverageGrade();
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
            StudentAPIBusinessLayer.Student Student = StudentAPIBusinessLayer.Student.Find(id);
            if (Student == null)
            {
                return NotFound($"student with ID {id} not found.");
            }

            StudentDTO SDTO = Student.SDTO;
            return Ok(SDTO);
        }


        [HttpPost(Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<Student> AddStudent(StudentDTO newStudentDTO)
        {

            if (newStudentDTO == null || string.IsNullOrEmpty(newStudentDTO.Name) || newStudentDTO.Age <= 0 || newStudentDTO.Grade < 0)
            {
                return BadRequest("Invalid student Data");
            }

            //newStudentDTO.Id = StudentDataSimulation.StudentsList.Count > 0 ? StudentDataSimulation.StudentsList.Max(s => s.Id) + 1 : 1;

            StudentAPIBusinessLayer.Student student = new StudentAPIBusinessLayer.Student(new StudentDTO(
                            newStudentDTO.Id, newStudentDTO.Name, newStudentDTO.Age, newStudentDTO.Grade) );
            student.Save();
            newStudentDTO.Id = student.Id;

            return CreatedAtRoute("GetStudentById", new { id = newStudentDTO.Id }, newStudentDTO);
        }



        [HttpPut("{id}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<StudentDTO> UpdateStudent(int id, StudentDTO UpdateStudent)
        {
            if (UpdateStudent == null || id < 1 || string.IsNullOrEmpty(UpdateStudent.Name) || UpdateStudent.Age <= 0 || UpdateStudent.Grade < 0)
            {
                return BadRequest("Invalid student Data");
            }

            StudentAPIBusinessLayer.Student student = StudentAPIBusinessLayer.Student.Find(id);
                
            if (student == null)
            {
                return NotFound($"student with ID {id} not found.");
            }

            student.Name = UpdateStudent.Name;
            student.Age = UpdateStudent.Age;
            student.Grade = UpdateStudent.Grade;
            student.Save();

            return Ok(student.SDTO);
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

           
          if (StudentAPIBusinessLayer.Student.DeleteStudend(id))
           { 
                return Ok($"student with ID {id} has been deleted.");
            }
            else
            {
                return NotFound($"student with ID {id} not found.");
            }
           
        }


    }
}
