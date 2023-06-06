using tryitter.Entities;
using tryitter.Models;

namespace tryitter.Repository
{
    public interface IStudentRepository
  {
    public string AddStudent(Student studentInput);
    public string Login(StudentLogin studentLogin);
    public string UpdateStudent(int id, Student studentInput);
    public string DeleteStudent(Student student);
    public Student GetStudent(string name);
    public List<StudentResponse> GetAllStudents();

  }
}