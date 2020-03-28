using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Response;
using DLL.Model;
using DLL.Repository;

namespace BLL.Service
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> FindAllAsync();
        Task<Department> FindOneAsync(long id);
        Task<ApiSuccessResponse> CreateAsync(DepartmentCreateRequest request);
        Task<ApiSuccessResponse> UpdateAsync(long id, DepartmentUpdateRequest request);
        Task<ApiSuccessResponse> DeleteAsync(long id);
        Task<bool> IsNameExistsAsync(string name);
        Task<bool> IsCodeExistsAsync(string code);
    }

    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<Department>> FindAllAsync()
        {
            return await _departmentRepository.FindAllAsync();
        }

        public async Task<Department> FindOneAsync(long id)
        {
            return await _departmentRepository.FindOneAsync(id);
        }

        public async Task<ApiSuccessResponse> CreateAsync(DepartmentCreateRequest request)
        {
            var department = new Department()
            {
                Name = request.Name,
                Code = request.Code
            };

            await _departmentRepository.CreateAsync(department);
            return new ApiSuccessResponse()
            {
                StatusCode = 200,
                Message = "Department has been created successfully."
            };
        }

        public async Task<ApiSuccessResponse> UpdateAsync(long id, DepartmentUpdateRequest request)
        {
            var department = await _departmentRepository.FindOneAsync(id);

            if (department == null)
            {
                return new ApiSuccessResponse()
                {
                    StatusCode = 304,
                    Message = "Something went wrong!" 
                };
            }

            department.Name = request.Name;
            department.Code = request.Code;
            
            await _departmentRepository.UpdateAsync(department);
            return new ApiSuccessResponse()
            {
                StatusCode = 200,
                Message = "The Department has been Successfully Updated."
            };
        }

        public async Task<ApiSuccessResponse> DeleteAsync(long id)
        {
            var department = await _departmentRepository.FindOneAsync(id);

            if (department == null)
            {
                return new ApiSuccessResponse()
                {
                    StatusCode = 404,
                    Message = "The student is not found with this given id!"
                };
            }
            
            await _departmentRepository.DeleteAsync(department);
            return new ApiSuccessResponse()
            {
                StatusCode = 200,
                Message = "The department has been Successfully Deleted."
            };
        }

        public async Task<bool> IsNameExistsAsync(string name)
        {
            var department = await _departmentRepository.IsNameExistsAsync(name);
            return department == null;
        }

        public async Task<bool> IsCodeExistsAsync(string code)
        {
            var department = await _departmentRepository.IsCodeExistsAsync(code);
            return department == null;
        }
    }
}