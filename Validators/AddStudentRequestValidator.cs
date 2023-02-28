using FluentValidation;
using HallManagementTest2.Requests.Add;

namespace HallManagementTest2.Validators
{
    public class AddStudentRequestValidator : AbstractValidator<AddStudentRequest>
    {
        public AddStudentRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.DateOfBirth).NotEmpty();
            RuleFor(x => x.Mobile).NotEmpty();
            RuleFor(x => x.Gender).NotEmpty();
            RuleFor(x => x.State).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.StudyLevel).NotEmpty();
            RuleFor(x => x.School).NotEmpty();
            RuleFor(x => x.Department).NotEmpty();
            RuleFor(x => x.Course).NotEmpty();
        }
    }
}
