using FluentValidation;
using Project.HIV_Treatment_and_Medical_Services_System_BE.Application.Enums;
using Project.HIV_Treatment_and_Medical_Services_System_BE.Application.Usecases.Login;

namespace Project.HIV_Treatment_and_Medical_Services_System_BE.Application.Validators
{
    /// <summary>
    /// Login command handler.
    /// </summary>
    /// <seealso cref="FluentValidation.AbstractValidator&lt;Project.HIV_Treatment_and_Medical_Services_System_BE.Application.Services.Login.LoginCommand&gt;" />
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommandValidator"/> class.
        /// </summary>
        public LoginCommandValidator()
        {
            RuleFor(o => o.UserName)
                .NotEmpty()
                .WithErrorCode(nameof(ErrorCode.UsernameIsEmpty))
                .WithMessage(Resources.UsernameIsEmpty);

            RuleFor(o => o.Password)
                .NotNull()
                .WithErrorCode(nameof(ErrorCode.UsernameIsNull))
                .WithMessage(Resources.UsernameIsRequired);

            RuleFor(o => o.Password)
                .NotEmpty()
                .WithErrorCode(nameof(ErrorCode.PasswordIsEmpty))
                .WithMessage(Resources.PasswordIsEmpty);

            RuleFor(o => o.Password)
                .NotNull()
                .WithErrorCode(nameof(ErrorCode.PasswordIsNull))
                .WithMessage(Resources.PasswordIsRequired);
        }
    }
}
