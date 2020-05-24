using System.Collections.Generic;
using System.Threading.Tasks;
using DLL.MongoReport.Model;
using MongoDB.Driver;

namespace DLL.MongoReport.Repository
{
    public interface IDepartmentStudentMongoRepository
    {
        Task<DepartmentStudentMongoModel> CreateAsync(DepartmentStudentMongoModel departmentStudentMongoModel);
        Task<List<DepartmentStudentMongoModel>> GetAllAsync();
    }

    public class DepartmentStudentMongoRepository : IDepartmentStudentMongoRepository
    {
        private readonly MongoDbContext _context;

        public DepartmentStudentMongoRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<DepartmentStudentMongoModel> CreateAsync(DepartmentStudentMongoModel departmentStudentMongoModel)
        {
            await _context.DepartmentStudentList.InsertOneAsync(departmentStudentMongoModel);
            return departmentStudentMongoModel;
        }

        public async Task<List<DepartmentStudentMongoModel>> GetAllAsync()
        {
            var filterBuilder = Builders<DepartmentStudentMongoModel>.Filter;
            var filter = filterBuilder.Empty;
            var sort = Builders<DepartmentStudentMongoModel>.Sort.Descending("_id");
            return await _context.DepartmentStudentList.Find(filter).Sort(sort).ToListAsync();
        }
    }
}