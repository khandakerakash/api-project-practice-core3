using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Response;
using DLL.Model;
using DLL.Repository;
using Utility.Exceptions;

namespace BLL.Service
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> FindAllAsync();
        Task<Student> FindSingleAsync(long id);
        Task<ApiSuccessResponse> CreateAsync(StudentCreateRequest request);
        Task<ApiSuccessResponse> UpdateAsync(long id, StudentUpdateRequest request);
        Task<ApiSuccessResponse> DeleteAsync(long id);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsRollNoExistsAsync(string rollNo);
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

        public async Task<Student> FindSingleAsync(long id)
        {
            var student = await _studentRepository.FindSingleAsync(x => x.StudentId == id);
            if (student == null)
                throw new MyAppException("The student with a given id is not found!");
            return student;
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
            if (await _studentRepository.SaveChangesAsync())
            {
                return new ApiSuccessResponse()
                {
                    StatusCode = 200,
                    Message = "The Student has been Successfully Created."
                };
            }
            
            throw new MyAppException("Something went wrong!");
        }

        public async Task<ApiSuccessResponse> UpdateAsync(long id, StudentUpdateRequest request)
        {
            var student = await _studentRepository.FindSingleAsync(x => x.StudentId == id);
            if (student == null)
                throw new MyAppException("The student with a given id is not found!");

            var studentEmailAlreadyExists =
                await _studentRepository.FindSingleAsync(x => x.Email == request.Email && x.Email != student.Email);
            if (studentEmailAlreadyExists != null)
                throw new MyAppException("The student with a given email already exists in our system!");

            var studentRollNoAlreadyExists =
                await _studentRepository.FindSingleAsync(x => x.RollNo == request.RollNo && x.Email != student.RollNo);
            if (studentRollNoAlreadyExists != null)
                throw new MyAppException("The student with a given roll no. already exists in our system!");
            
            student.Name = request.Name;
            student.Email = request.Email;
            student.RollNo = request.RollNo;
            
            _studentRepository.Update(student);
            if (await _studentRepository.SaveChangesAsync())
                return new ApiSuccessResponse()
                {
                    StatusCode = 200,
                    Message = "The Student has been Successfully Updated."
                };
            
            throw new MyAppException("Something went wrong!");
        }

        public async Task<ApiSuccessResponse> DeleteAsync(long id)
        {
            var student = await _studentRepository.FindSingleAsync(x => x.StudentId == id);
            if (student == null)
                throw new MyAppException("The student with a given id is not found!");

            _studentRepository.Delete(student);
            if (await _studentRepository.SaveChangesAsync())
                return new ApiSuccessResponse()
                {
                    StatusCode = 200,
                    Message = "The Student has been Successfully Updated."
                };
            
            throw new MyAppException("Something went wrong!");
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            var student = await _studentRepository.FindSingleAsync(x => x.Email == email);
            return student == null ? true : false;
        }

        public async Task<bool> IsRollNoExistsAsync(string rollNo)
        {
            var student = await _studentRepository.FindSingleAsync(x => x.RollNo == rollNo);
            return student == null ? true : false;
        }
    }
}