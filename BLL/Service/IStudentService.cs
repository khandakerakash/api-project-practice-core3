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
        Task<ApiSuccessResponse> UpdateAsync(long id, StudentCreateRequest request);
        Task<ApiSuccessResponse> DeleteAsync(long id);
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
                Email = request.Email,
                RollNo = request.RollNo
            };

            await _studentRepository.CreateAsync(student);
            return new ApiSuccessResponse()
            {
                StatusCode = 200,
                Message = "The Student has been Successfully Created."
            };
        }

        public async Task<ApiSuccessResponse> UpdateAsync(long id, StudentCreateRequest request)
        {
            var student = await _studentRepository.FindOneAsync(id);
            if (student == null)
            {
                return new ApiSuccessResponse()
                {
                    StatusCode = 304,
                    Message = "Something went wrong!"
                };
            }
            
            student.Name = request.Name;
            student.Email = request.Email;
            student.RollNo = request.RollNo;
            
            await _studentRepository.UpdateAsync(student);
            return new ApiSuccessResponse()
            {
                StatusCode = 200,
                Message = "The Student has been Successfully Updated."
            };
        }

        public async Task<ApiSuccessResponse> DeleteAsync(long id)
        {
            var student = await _studentRepository.FindOneAsync(id);
            if (student == null)
            {
                return new ApiSuccessResponse()
                {
                    StatusCode = 404,
                    Message = "The student is not found with this given id!"
                };
            }

            await _studentRepository.DeleteAsync(student);
            return new ApiSuccessResponse()
            {
                StatusCode = 200,
                Message = "The student has been Successfully Deleted."
            };
        }
    }
}