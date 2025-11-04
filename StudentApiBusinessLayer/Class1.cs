using StudentApiDataAccessLayer;
using static StudentApiDataAccessLayer.StudentDTO;

namespace StudentAPIBusinessLayer
{
    public class Student
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }

        public StudentDTO SDTO { get { return new StudentDTO(this.Id, this.Name, this.Age, this.Grade); } }
        public Student(StudentDTO SDTO, enMode Mode = enMode.AddNew)
        {
            this.Id = SDTO.Id;
            this.Name = SDTO.Name;
            this.Age = SDTO.Age;
            this.Grade = SDTO.Grade;
            this.Mode = Mode;
        }




        public static List<StudentDTO> GetAllStudents()
        {
            return StudentData.GetAllStudents();
        }

        public static List<StudentDTO> GetPassedStudents()
        {
            return StudentData.GetPassedStudents();
        }



        public static double GetAverageGrade()
        {
            return StudentData.GetAverageGrade();
        }

        public static Student Find(int Id)
        {

            StudentDTO SDTO = StudentData.GetStudentById(Id);
            if(SDTO != null)
            {
                return new Student(SDTO, enMode.Update);
            }
            return null;

        }

      private  bool _AddStudent()
        {
            
            this.Id = StudentData.AddStudent(SDTO);
            return (this.Id !=-1);

        }

        private bool _UpdateStudent()
        {
            return StudentData.UpdateStudent(SDTO);
        }


        public bool Save()
        {

            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddStudent())
                    {
                        Mode = enMode.Update;
                        return true;
                    }else
                    {
                        return false; 
                    }

                case enMode.Update:
                    return _UpdateStudent();
            }
            return false;
        }
        

        public static bool DeleteStudend(int ID)
        {
            return StudentData.DeleteStudent(ID);
        }

    }
}
