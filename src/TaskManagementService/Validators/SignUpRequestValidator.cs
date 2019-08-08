using FluentValidation;
using Lionize.TaskManagement.ApiModels;

namespace TIKSN.Lionize.TaskManagementService.Validators
{
    public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpRequestValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
            RuleFor(x => x.PasswordConfirmation).NotNull().NotEmpty().Equal(x => x.Password).WithMessage("Password confirmation is incorrect.");
        }
    }
}