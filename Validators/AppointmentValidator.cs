using FluentValidation;
using Hospital.DTOs;

namespace Hospital.Validators
{
    public class CreateAppointmentRequestValidator : AbstractValidator<CreateAppointmentRequest>
    {
        public CreateAppointmentRequestValidator()
        {
            RuleFor(x => x.DoctorId).GreaterThan(0);
            RuleFor(x => x.AppointmentDate).NotEmpty();
            RuleFor(x => x.TimeSlot).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Symptoms).MaximumLength(500);
        }
    }

    public class UpdateAppointmentStatusRequestValidator : AbstractValidator<UpdateAppointmentStatusRequest>
    {
        public UpdateAppointmentStatusRequestValidator()
        {
            RuleFor(x => x.AppointmentId).GreaterThan(0);
            RuleFor(x => x.Status).NotEmpty().MaximumLength(20);
            RuleFor(x => x.DoctorNotes).MaximumLength(500);
        }
    }
}
