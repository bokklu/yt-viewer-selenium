using FluentValidation;
using System;
using YTViewer.Contracts;
using YTViewer.Contracts.Options;

namespace YTViewer.Application.Validators
{
    internal class ViewOptionsValidator : AbstractValidator<ViewOptions>
    {
        public ViewOptionsValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(opts => opts.VideoUrl)
                .NotEmpty()
                .WithMessage("Video URL property is invalid.");

            RuleFor(opts => opts.ViewCount)
                .NotNull()
                .WithMessage("View Count property is null.")
                .GreaterThan(0)
                .WithMessage("View Count property must be greater than 0.");

            RuleFor(opts => opts.Addon)
                .NotEmpty()
                .WithMessage("Addon property is invalid.")
                .IsEnumName(typeof(Addon), false)
                .WithMessage("Addon property value is not one of the listed addons.");
        }
    }
}
