﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Response;
using DLL.Model;
using DLL.UnitOfWorks;
using Utility.Exceptions;

namespace BLL.Service
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> FindAllAsync();
        Task<Department> FindSingleAsync(long id);
        Task<ApiSuccessResponse> CreateAsync(DepartmentCreateRequest request);
        Task<ApiSuccessResponse> UpdateAsync(long id, DepartmentUpdateRequest request);
        Task<ApiSuccessResponse> DeleteAsync(long id);
        Task<bool> IsNameExistsAsync(string name);
        Task<bool> IsCodeExistsAsync(string code);
    }

    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Department>> FindAllAsync()
        {
            return await _unitOfWork.DepartmentRepository.FindAllAsync();
        }

        public async Task<Department> FindSingleAsync(long id)
        {
            var department = await _unitOfWork.DepartmentRepository.FindSingleAsync(x=> x.DepartmentId == id);
            if (department == null)
                throw new MyAppException("The department not found!");
            return department;
        }

        public async Task<ApiSuccessResponse> CreateAsync(DepartmentCreateRequest request)
        {
            var department = new Department()
            {
                Name = request.Name,
                Code = request.Code
            };

            await _unitOfWork.DepartmentRepository.CreateAsync(department);

            if (await _unitOfWork.AppSaveChangesAsync())
                return new ApiSuccessResponse()
                {
                    StatusCode = 200,
                    Message = "Department has been created successfully."
                };
            
            throw new MyAppException("Something went wrong!");
        }

        public async Task<ApiSuccessResponse> UpdateAsync(long id, DepartmentUpdateRequest request)
        {
            var department = await _unitOfWork.DepartmentRepository.FindSingleAsync(x => x.DepartmentId == id);
            if (department == null)
                throw new MyAppException("The department not found!");
            
            var departmentNameAlreadyExists =
                await _unitOfWork.DepartmentRepository.FindSingleAsync(x => x.Name == request.Name && x.Name != department.Name);
            if (departmentNameAlreadyExists != null)
                throw new MyAppException("The department with a given name is already in our system!");

            var departmentCodeAlreadyExists =
                await _unitOfWork.DepartmentRepository.FindSingleAsync(x => x.Code == request.Code && x.Code != department.Code);
            if (departmentCodeAlreadyExists != null)
                throw new MyAppException("The department with a given code is already in our system!");
            
            department.Name = request.Name;
            department.Code = request.Code;
            
            _unitOfWork.DepartmentRepository.Update(department);
            if (await _unitOfWork.AppSaveChangesAsync())
                return new ApiSuccessResponse()
                {
                    StatusCode = 200,
                    Message = "The department has been successfully updated."
                };
            
            throw new MyAppException("Something went wrong!");
        }

        public async Task<ApiSuccessResponse> DeleteAsync(long id)
        {
            var department = await _unitOfWork.DepartmentRepository.FindSingleAsync(x => x.DepartmentId == id);

            if (department == null)
                throw new MyAppException("The department not found!");
            
            _unitOfWork.DepartmentRepository.Delete(department);
            if (await _unitOfWork.AppSaveChangesAsync())
                return new ApiSuccessResponse()
                {
                    StatusCode = 200,
                    Message = "The department has been successfully deleted."
                };
            
            throw new MyAppException("Something went wrong!");
        }

        public async Task<bool> IsNameExistsAsync(string name)
        {
            var department = await _unitOfWork.DepartmentRepository.FindSingleAsync(x => x.Name == name);
            return department == null ? true : false;
        }

        public async Task<bool> IsCodeExistsAsync(string code)
        {
            var department = await _unitOfWork.DepartmentRepository.FindSingleAsync(x => x.Code == code);
            return department == null ? true : false;
        }
    }
}