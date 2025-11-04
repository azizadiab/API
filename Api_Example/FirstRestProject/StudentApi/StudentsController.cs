using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentApi.DataSimulation;
using StudentApi.Model;
using System;
using System.Collections.Generic;

namespace StudentApi.Controllers
{
    // [Route("api/[controller]")]
    [Route("api/Student")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet ("All", Name = "GetAllStudents")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Student>> GetAllStudents() //Define a method to get all students.
        {

           if(StudentDataSimulation.StudentsList.Count==0)
            {
                return NotFound("Not Data");
            }
            
            return Ok(StudentDataSimulation.StudentsList);//Returns the list of students.

        }



        [HttpGet("Passed", Name = "GetPassedStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Student> GetPassedStudents()
        {

            var passedStudents = StudentDataSimulation.StudentsList.Where(Student => Student.Grade >= 50).ToList();

          //  passedStudents.Clear();

            if (passedStudents.Count==0)
            {
                return NotFound("Not Found Data");
            }
                
            return Ok(passedStudents);// Return the list of students who passed.

        }



        [HttpGet("Avg", Name = "GetAvgStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<double> GetAverageGrade()
        {
          // StudentDataSimulation.StudentsList.Clear();

            if (StudentDataSimulation.StudentsList.Count == 0)
            {
                return NotFound("Not Data");
            }

          

            var averageGrade = StudentDataSimulation.StudentsList.Average(Student => Student.Grade);
            return Ok(averageGrade);

        }


        [HttpGet("{id}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Student> GetStudentById(int id)
        {

            if (id < 1)
            {
                return BadRequest($"Not accepted ID:  {id}");
            }

            var Student = StudentDataSimulation.StudentsList.FirstOrDefault(S => S.Id == id);

            if (Student == null)
            {
                return NotFound("Not Fount Student By" + id);
            }

            return Ok(Student);
           
        }

        //for add new we use Http Post
        [HttpPost(Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Student>AddStudent(Student newStudent)
        {
            //we validate the data here
            if (newStudent == null || string.IsNullOrEmpty(newStudent.Name) || newStudent.Age < 0 || newStudent.Grade < 0)
            {
                return BadRequest("Invalid student data.");
            }

            newStudent.Id = StudentDataSimulation.StudentsList.Count > 0 ? StudentDataSimulation.StudentsList.Max(s => s.Id) + 1 : 1;
            StudentDataSimulation.StudentsList.Add(newStudent);

            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
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
            
            
            var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            StudentDataSimulation.StudentsList.Remove(student);
            return Ok($"Student with ID {id} has been deleted.");


        }

        

        [HttpPut("{id}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<Student>UpdateStudent(int id, Student updateStudent)
        {
            if (id < 0 || updateStudent == null || string.IsNullOrEmpty(updateStudent.Name) || updateStudent.Age < 0 || updateStudent.Grade < 0) 
            {
                return BadRequest("Invalid student data.");
              
            }

            var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            if(student==null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            student.Name = updateStudent.Name;
            student.Age = updateStudent.Age;
            student.Grade = updateStudent.Grade;
            return Ok(student);
        }

    }


}
