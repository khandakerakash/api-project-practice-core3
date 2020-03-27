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
        Task UpdateAsync(Department department);
        Task DeleteAsync(long id);
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
            if (await _departmentRepository.SaveChangesAsync() > 0)
            {
                return new ApiSuccessResponse()
                {
                    StatusCode = 200,
                    Message = "Department has been created successfully."
                };
            }

            return null;
        }

        public async Task UpdateAsync(Department department)
        {
            throw new System.NotImplementedException();
        }

        public async Task DeleteAsync(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}