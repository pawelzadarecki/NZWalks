﻿using FluentValidation;

namespace NZWalks.Api.Validators
{
    public class AddWalkDifficultyRequestValidator: AbstractValidator<Models.DTO.AddWalkDifficultyRequest>
    {
        public AddWalkDifficultyRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
