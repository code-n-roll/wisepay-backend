namespace WisePay.Web.Core.ClientInteraction
{
    public enum ErrorCode
    {
        ServerError,
        MultipleErrors,

        AuthError,
        InvalidCredentials,
        ValidationError,

        NotFound,
        InvalidRequestFormat
    }
}
