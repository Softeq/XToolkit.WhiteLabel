namespace RemoteServices.Auth.Models
{
    public enum ErrorCodes
    {
        // NotFound
        UserNotFound = 1001,
        UserRoleNotFound = 1003,

        // InputValidation
        UserIsInDeletedState = 2001,
        RequestModelValidationFailed = 2003,
        InvalidEmailConfirmationCodeException = 2005,
        InvalidResendEmailCodeException = 2006,
        InvalidResetPasswordCodeException = 2007,
        EmailAlreadyTaken = 2009,
        IdentityError = 2010,
        ActivationCodeExpired = 2012,
        PendingAllowedRequirementFailed = 2015,
        PasswordHasExpired = 2019,
        PasswordHashIsNotUnique = 2020,
        InactiveUserState = 4001,
        EmailIsNotConfirmed = 4002,
        UserIsInPendingState = 4003,

        // Conflict
        UserHadAlreadyAcceptedTermsAndPolicies = 5000,
        UserWithDefinedEmailAlreadyExist = 5001,
        EmailAlreadyConfirmed = 5002,
        UnsupportedRole = 5008,
        UserIsAlreadyInDeletedState = 5016,
        UserIsNotInActiveState = 5038,
        UnsupportedAuthorizationStatus = 5045,
        UserIsNotInPendingOrInactiveState = 5050,
        UnsupportedTokenPurpose = 5069,

        // Unknown errors
        UnknownIdentityError = 6000,
        UnknownError = 6001
    }
}
