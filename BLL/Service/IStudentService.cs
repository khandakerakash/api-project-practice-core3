using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Response;
using DLL.Model;
using DLL.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
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
        Task<List<StudentReportResponse>> StudentDepartmentInfoListAsync();
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsRollNoExistsAsync(string rollNo);
        Task<bool> IsDepartmentIdExistsAsync(long departmentId);
    }

    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;


        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Student>> FindAllAsync()
        {
            return await _unitOfWork.StudentRepository.FindAllAsync();
        }

        public async Task<Student> FindSingleAsync(long id)
        {
            var student = await _unitOfWork.StudentRepository.FindSingleAsync(x => x.StudentId == id);
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
                RollNo = request.RollNo,
                DepartmentId = request.DepartmentId
            };
            
            await _unitOfWork.StudentRepository.CreateAsync(student);
            if (await _unitOfWork.AppSaveChangesAsync())
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
            var student = await _unitOfWork.StudentRepository.FindSingleAsync(x => x.StudentId == id);
            if (student == null)
                throw new MyAppException("The student with a given id is not found!");

            var studentEmailAlreadyExists =
                await _unitOfWork.StudentRepository.FindSingleAsync(x => x.Email == request.Email && x.Email != student.Email);
            if (studentEmailAlreadyExists != null)
                throw new MyAppException("The student with a given email already exists in our system!");

            var studentRollNoAlreadyExists =
                await _unitOfWork.StudentRepository.FindSingleAsync(x => x.RollNo == request.RollNo && x.RollNo != student.RollNo);
            if (studentRollNoAlreadyExists != null)
                throw new MyAppException("The student with a given roll no. already exists in our system!");
            
            student.Name = request.Name;
            student.Email = request.Email;
            student.RollNo = request.RollNo;
            student.DepartmentId = request.DepartmentId;

            _unitOfWork.StudentRepository.Update(student);
            if (await _unitOfWork.AppSaveChangesAsync())
                return new ApiSuccessResponse()
                {
                    StatusCode = 200,
                    Message = "The Student has been successfully updated."
                };
            
            throw new MyAppException("Something went wrong!");
        }

        public async Task<ApiSuccessResponse> DeleteAsync(long id)
        {
            var student = await _unitOfWork.StudentRepository.FindSingleAsync(x => x.StudentId == id);
            if (student == null)
                throw new MyAppException("The student with a given id is not found!");

            _unitOfWork.StudentRepository.Delete(student);
            if (await _unitOfWork.AppSaveChangesAsync())
                return new ApiSuccessResponse()
                {
                    StatusCode = 200,
                    Message = "The Student has been successfully deleted."
                };
            
            throw new MyAppException("Something went wrong!");
        }

        public async Task<List<StudentReportResponse>> StudentDepartmentInfoListAsync()
        {
            var students = await _unitOfWork.StudentRepository.QueryAll().Include(x => x.Department).ToListAsync();
            
            var result = new List<StudentReportResponse>();
            
            foreach (var student in students)
            {
                result.Add(new StudentReportResponse()
                {
                    Name = student.Name,
                    Email = student.Email,
                    RollNo = student.RollNo,
                    DepartmentCode = student.Department.Code,
                    DepartmentName = student.Department.Name
                });
            }

            if(result == null)
                throw new MyAppException("The student with department info. is not found!");
            return result;
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            var student = await _unitOfWork.StudentRepository.FindSingleAsync(x => x.Email == email);
            return student == null ? true : false;
        }

        public async Task<bool> IsRollNoExistsAsync(string rollNo)
        {
            var student = await _unitOfWork.StudentRepository.FindSingleAsync(x => x.RollNo == rollNo);
            return student == null ? true : false;
        }

        public async Task<bool> IsDepartmentIdExistsAsync(long departmentId)
        {
            var department = await _unitOfWork.DepartmentRepository.FindSingleAsync(x => x.DepartmentId == departmentId);
            return department != null ? true : false;
        }
    }
}