using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StudentApiClient
{


    public class Program
    {

        static readonly HttpClient httpClient = new HttpClient();
        static async Task Main(string[] Args)

        {
            httpClient.BaseAddress = new Uri("http://localhost:5224/api/Students/");
            await GetAlltudents();
            await GetPassedStudents();
            await GetAverageGrade();
            await GetStudentById(1);
            await GetStudentById(10);
            await GetStudentById(0);
            var newStudent = new Student { Name = "Aziza", Age = 55, Grade = 77 };
            await AddStudent(newStudent);
            var newStudent1 = new Student { Name = "", Age = 55, Grade = 77 };
           await AddStudent(newStudent1);

            await DeleteStudent(2);
            await DeleteStudent(10);
            await DeleteStudent(0);

            var updataStudent= new Student { Name = "xxx", Age = 55, Grade = 77 };

            await UpdateStudent(5, updataStudent);

            await GetAlltudents();

        }

        static async Task GetAlltudents()
        {
            try
            {
                Console.WriteLine("\n________________________________");
                Console.WriteLine("\nFetching All Students...\n");

                var Response = await httpClient.GetAsync("All");
                if (Response.IsSuccessStatusCode)
                {

                    var students = await Response.Content.ReadFromJsonAsync<List<Student>>();

                    if (students != null && students.Count > 0)
                    {
                        foreach (var student in students)
                        {
                            Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
                        }
                    }

                    else if (Response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {

                        Console.WriteLine("No averageGrade found.");
                    }


                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }

        static async Task GetPassedStudents()
        {
            try
            {
                Console.WriteLine("\n________________________________");
                Console.WriteLine("\nFetching Passed Students...\n");
                var response = await httpClient.GetAsync("Passed");
                if (response.IsSuccessStatusCode)
                {
                    var passedStudents = await response.Content.ReadFromJsonAsync<List<Student>>();
                    if (passedStudents != null && passedStudents.Count > 0)
                    {
                        foreach (var student in passedStudents)
                        {
                            Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        Console.WriteLine("No Passed Studens Fount");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }


        }


        static async Task GetAverageGrade()
        {
            try
            {
                Console.WriteLine("\n________________________________");
                Console.WriteLine("\nFetching Average Grade...\n");
                var response = await httpClient.GetAsync("AverageGrade");
                if (response.IsSuccessStatusCode)
                {
                    var averageGrade = await response.Content.ReadFromJsonAsync<double>();

                    Console.WriteLine($"Average Grade: {averageGrade}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("No Average Grade");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }


        }

        static async Task GetStudentById(int id)
        {
            try
            {
                Console.WriteLine("\n________________________________");
                Console.WriteLine("\nFetching Student By Id...\n");

                var Response = await httpClient.GetAsync($"ById/{id}");
                if (Response.IsSuccessStatusCode)
                {

                    var student = await Response.Content.ReadFromJsonAsync<Student>();

                    if (student != null)
                    {

                        Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");

                    }
                }
                else if (Response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {

                    Console.WriteLine($"Not Found: Student with ID {id} not found.");
                }

                else if (Response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {

                    Console.WriteLine($"Bad Request: Not accepted ID {id}");
                }
                

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        static async Task AddStudent(Student newStudent)

        {
            try { 
             Console.WriteLine("\n________________________________");
            Console.WriteLine("\nAdding a new Student \n");

            var Response = await httpClient.PostAsJsonAsync("", newStudent);
            if (Response.IsSuccessStatusCode)
            {

                var addedstudent = await Response.Content.ReadFromJsonAsync<Student>();

               
                    Console.WriteLine($" Added Student  ID: {addedstudent.Id}, Name: {addedstudent.Name}, Age: {addedstudent.Age}, Grade: {addedstudent.Grade}");
            
            }
            
            else if (Response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {

                Console.WriteLine($"Bad Request: Invalid Student Data");
            }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }

        static async Task DeleteStudent(int id)
        {

            try
            {
                Console.WriteLine("\n________________________________");
                Console.WriteLine("\nDeleting student with ID {id}...");

                var Response = await httpClient.DeleteAsync($"{id}");
                if (Response.IsSuccessStatusCode)
                {
                   

                    Console.WriteLine($"Student with ID {id} has been deleted.");

                }

                else if (Response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {

                    Console.WriteLine($"Bad Request: Not accepted ID {id}");
                }

                else if (Response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {

                    Console.WriteLine($"Not Found: Student with ID {id} not found.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }

        static async Task UpdateStudent(int id, Student updataStudent)
        {

            try
            {
                Console.WriteLine("\n________________________________");
                Console.WriteLine("\nUpdata student with ID {id}...");

                var Response = await httpClient.PutAsJsonAsync($"{id}", updataStudent);
                if (Response.IsSuccessStatusCode)
                {

                    var student = await Response.Content.ReadFromJsonAsync<Student>();
                    if (student !=null)
                    {
                        Console.WriteLine($"Update Student ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
                    }


                }

                else if (Response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {

                    Console.WriteLine($"Failed to update student: Invalid data.");
                }

                else if (Response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {

                    Console.WriteLine($"Not Found: Student with ID {id} not found.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }
        public class Student
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public int Grade { get; set; }
        }

    }