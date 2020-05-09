using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Service;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Request
{
    public class StudentUpdateRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string RollNo { get; set; }
        public long DepartmentId { get; set; }
    }

    public class StudentUpdateRequestValidator : AbstractValidator<StudentUpdateRequest>
    {
        private readonly IServiceProvider _serviceProvider;

        public StudentUpdateRequestValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(5).MaximumLength(25);
            RuleFor(x => x.Email).NotNull().NotEmpty().MinimumLength(10).MaximumLength(25).EmailAddress();
            RuleFor(x => x.RollNo).NotNull().NotEmpty().MinimumLength(5).MaximumLength(25);
            RuleFor(x => x.DepartmentId).NotNull().NotEmpty()
                .MustAsync(DepartmentIdExists).WithMessage("The given department Id doesn't exist in our system.");
        }

        private async Task<bool> DepartmentIdExists(long departmentId, CancellationToken token)
        {
            if (departmentId == 0)
            {
                return true;
            }
            var departmentService = _serviceProvider.GetRequiredService<IDepartmentService>();
            return await departmentService.IsDepartmentIdExistsAsync(departmentId);
        }
    }
}