using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System.Data;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace StudentApiDataAccessLayer
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }




        public StudentDTO(int id, string name, int age, int grade)
        {
            this.Id = id;
            this.Name = name;
            this.Age = age;
            this.Grade = grade;
        }

    }



    public class StudentData
    {

        static string _connectionString = "Server=localhost;Database=StudentsDB;User Id=sa;Password=sa123456;Encrypt=False;" +
            "       TrustServerCertificate=True;Connection Timeout=30;";

        public static ParameterDirection Direction { get; private set; }

        public static List<StudentDTO> GetAllStudents()
        {
            var StudentList = new List<StudentDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))

            using (SqlCommand command = new SqlCommand("SP_GetAllStudents", conn))
            {
                command.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {



                    while (reader.Read())
                    {

                        StudentList.Add(new StudentDTO(
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("Name")),
                            reader.GetInt32(reader.GetOrdinal("Age")),
                            reader.GetInt32(reader.GetOrdinal("Grade"))


                        ));

                    }


                }

            }

            return StudentList;

        }



        public static List<StudentDTO> GetPassedStudents()
        {

            var StudentList = new List<StudentDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))

            using (SqlCommand command = new SqlCommand("SP_GetPassedStudents", conn))
            {
                command.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {


                    while (reader.Read())
                    {

                        StudentList.Add(new StudentDTO(
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("Name")),
                            reader.GetInt32(reader.GetOrdinal("Age")),
                            reader.GetInt32(reader.GetOrdinal("Grade"))


                        ));

                    }


                }

            }

            return StudentList;


        }

        public static double GetAverageGrade()
        {
            double averageGrade = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAverageGrade", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        averageGrade = Convert.ToDouble(result);
                    }
                    else
                    {
                        averageGrade = 0;
                    }

                }
                return averageGrade;

            }

        }

        public static StudentDTO GetStudentById(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetStudentById", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentId", id);
                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new StudentDTO
                        (

                             reader.GetInt32(reader.GetOrdinal("Id")),
                             reader.GetString(reader.GetOrdinal("Name")),
                             reader.GetInt32(reader.GetOrdinal("Age")),
                             reader.GetInt32(reader.GetOrdinal("Grade"))

                         );

                        }
                        else
                        {
                            return null;
                        }


                    }


                }

            }


        }


        public static int AddStudent(StudentDTO studentDTO)
        {
            
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_AddStudent", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", studentDTO.Name);
                    command.Parameters.AddWithValue("@Age", studentDTO.Age);
                    command.Parameters.AddWithValue("@Grade", studentDTO.Grade);

                    SqlParameter outputIdParam = new SqlParameter("@NewStudentId", SqlDbType.Int)
                    {
                       Direction = ParameterDirection.Output
                    };
                   
                    command.Parameters.Add(outputIdParam);


                    conn.Open();

                    command.ExecuteNonQuery();

                    return (int)outputIdParam.Value;


                }

            }

        }

        public static bool UpdateStudent(StudentDTO studentDTO)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateStudent", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentId", studentDTO.Id);
                    command.Parameters.AddWithValue("@Name", studentDTO.Name);
                    command.Parameters.AddWithValue("@Age", studentDTO.Age);
                    command.Parameters.AddWithValue("@Grade", studentDTO.Grade);

                    conn.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
    

        }

        public static bool DeleteStudent(int studentId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using(SqlCommand command = new SqlCommand("SP_DeleteStudent", conn))
                    {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    conn.Open();
                    int RowsAffected = (int)command.ExecuteScalar();
                    return (RowsAffected == 1);
                   
                }
            }

        }
    }
}

            

    
        

    
