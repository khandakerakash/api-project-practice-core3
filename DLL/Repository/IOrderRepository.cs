using DLL.DbContext;
using DLL.Model;
using DLL.UnitOfWorks;

namespace DLL.Repository
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
    }

    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}