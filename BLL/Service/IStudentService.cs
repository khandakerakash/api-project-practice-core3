using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Response;
using DLL.DbContext;
using DLL.Model;
using DLL.Repository;

namespace BLL.Service
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> FindAllAsync();
        Task<Student> FindOneAsync(long id);
        Task<ApiSuccessResponse> CreateAsync(StudentCreateRequest request);
        Task UpdateAsync(Student student);
        Task DeleteAsync(long id);
    }

    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<Student>> FindAllAsync()
        {
            return await _studentRepository.FindAllAsync();
        }

        public async Task<Student> FindOneAsync(long id)
        {
            return await _studentRepository.FindOneAsync(id);
        }

        public async Task<ApiSuccessResponse> CreateAsync(StudentCreateRequest request)
        {
            var student = new Student()
            {
                Name = request.Name,
                Email = request.Email
            };

            await _studentRepository.CreateAsync(student);
            if (await _studentRepository.SaveChangesAsync() > 0)
            {
                return new ApiSuccessResponse()
                {
                    StatusCode = 200,
                    Message = "Student has been Successfully Created."
                };
            }

            return null;
        }

        public async Task UpdateAsync(Student student)
        {
            await _studentRepository.UpdateAsync(student);
        }

        public async Task DeleteAsync(long id)
        {
            await _studentRepository.DeleteAsync(id);
        }
    }
}