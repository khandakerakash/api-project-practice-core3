﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Response;
using DLL.Model;
using DLL.UnitOfWorks;
using Utility.Exceptions;

namespace BLL.Service
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> FindAllAsync();
        Task<Course> FindSingleAsync(long id);
        Task<ApiSuccessResponse> CreateAsync(CourseCreateRequest request);
        Task<ApiSuccessResponse> UpdateAsync(long id, CourseCreateRequest request);
        Task<ApiSuccessResponse> DeleteAsync(long id);
        Task<bool> IsCourseCodeExistsAsync(string code);
        Task<bool> IsCourseNameExistsAsync(string name);
        Task<bool> IsStudentIdExistsAsync(long studentId);
    }

    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Course>> FindAllAsync()
        {
            var course = await _unitOfWork.CourseRepository.FindAllAsync();
            if (course == null)
                throw new MyAppException("The course list is not found!");
            return course;
        }

        public async Task<Course> FindSingleAsync(long id)
        {
            var course = await _unitOfWork.CourseRepository.FindSingleAsync(x => x.CourseId == id);
            if(course == null)
                throw new MyAppException("The course with given id is not found!");
            return course;
        }

        public async Task<ApiSuccessResponse> CreateAsync(CourseCreateRequest request)
        {
            var course = new Course()
            {
                Code = request.Code,
                Name = request.Name
            };

            await _unitOfWork.CourseRepository.CreateAsync(course);
            if (await _unitOfWork.AppSaveChangesAsync())
            {
                return new ApiSuccessResponse()
                {
                    StatusCode = 200,
                    Message = "The course has been successfully created."
                };
            }
            
            throw new MyAppException("Something went wrong!");
        }

        public async Task<ApiSuccessResponse> UpdateAsync(long id, CourseCreateRequest request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ApiSuccessResponse> DeleteAsync(long id)
        {
            var course = await _unitOfWork.CourseRepository.FindSingleAsync(x => x.CourseId == id);
            if (course == null)
                throw new MyAppException("The course with a given id is not found!");

            _unitOfWork.CourseRepository.Delete(course);
            if (await _unitOfWork.AppSaveChangesAsync())
                return new ApiSuccessResponse()
                {
                    StatusCode = 200,
                    Message = "The course has been successfully deleted."
                };
            
            throw new MyAppException("Something went wrong!");
        }

        public async Task<bool> IsCourseCodeExistsAsync(string code)
        {
            var course = await _unitOfWork.CourseRepository.FindSingleAsync(x => x.Code == code);
            return course == null ? true : false;
        }

        public async Task<bool> IsCourseNameExistsAsync(string name)
        {
            var course = await _unitOfWork.CourseRepository.FindSingleAsync(x => x.Name == name);
            return course == null ? true : false;
        }

        public async Task<bool> IsStudentIdExistsAsync(long studentId)
        {
            var student = await _unitOfWork.StudentRepository.FindSingleAsync(x => x.StudentId == studentId);
            return student != null ? true : false;
        }
    }
}