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
    public interface ICourseEnrollService
    {
        Task<IEnumerable<CourseStudent>> FindAllAsync();
        Task<Course> FindSingleAsync(long id);
        Task<ApiSuccessResponse> CreateAsync(CourseEnrollCreateRequest request);
        Task<ApiSuccessResponse> UpdateAsync(long id, CourseEnrollUpdateRequest request);
        Task<ApiSuccessResponse> DeleteAsync(long id);
    }

    public class CourseEnrollService : ICourseEnrollService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseEnrollService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CourseStudent>> FindAllAsync()
        {
            var courseEnroll = await _unitOfWork.CourseEnrollRepository.FindAllAsync();
            if (courseEnroll == null)
                throw new MyAppException("The course enroll list is not found!");
            return courseEnroll;
        }

        public Task<Course> FindSingleAsync(long id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ApiSuccessResponse> CreateAsync(CourseEnrollCreateRequest request)
        {
            var courseEnroll = new CourseStudent()
            {
                CourseId = request.CourseId,
                StudentId = request.StudentId
            };

            var alreadyCourseEnrollOrNot = await _unitOfWork.CourseEnrollRepository.FindSingleAsync(x =>
                x.CourseId == request.CourseId && x.StudentId == request.StudentId);

            if (alreadyCourseEnrollOrNot != null)
            {
                throw new MyAppException("The given student id already enrolled by this course.");
            }

            await _unitOfWork.CourseEnrollRepository.CreateAsync(courseEnroll);
            if (await _unitOfWork.AppSaveChangesAsync())
            {
                return new ApiSuccessResponse()
                {
                    StatusCode = 200,
                    Message = "The course enrolls has been successfully done."
                };
            }
            
            throw new MyAppException("Something went wrong!");
        }

        public Task<ApiSuccessResponse> UpdateAsync(long id, CourseEnrollUpdateRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiSuccessResponse> DeleteAsync(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}