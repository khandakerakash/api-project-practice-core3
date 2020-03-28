using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Service;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Request
{
    public class DepartmentCreateRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
    
    public class DepartmentCreateRequestValidator : AbstractValidator<DepartmentCreateRequest> {
        private readonly IServiceProvider _serviceProvider;
        public DepartmentCreateRequestValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(2).MaximumLength(50)
                .MustAsync(NameIsExists).WithMessage("The Department with a given name already exists in our system.");
            RuleFor(x => x.Code).NotNull().NotEmpty().MinimumLength(2).MaximumLength(12)
                .MustAsync(CodeIsExists).WithMessage("The Department with a given code already exists in our system.");;
        }

        private async Task<bool> NameIsExists(string name, CancellationToken token)
        {
            if (string.IsNullOrEmpty(name))
            {
                return true;
            }

            var departmentService = _serviceProvider.GetRequiredService<IDepartmentService>();
            return await departmentService.IsNameExistsAsync(name);
        }

        private async Task<bool> CodeIsExists(string code, CancellationToken token)
        {
            if (string.IsNullOrEmpty(code))
            {
                return true;
            }

            var departmentService = _serviceProvider.GetRequiredService<IDepartmentService>();
            return await departmentService.IsCodeExistsAsync(code);
        }
    }
}