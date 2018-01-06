namespace WisePay.Web.Core.Emails
{
    public interface IEmailConfiguration
    {
        string SmtpPassword { get; set; }
        int SmtpPort { get; set; }
        string SmtpServer { get; set; }
        string SmtpUsername { get; set; }
    }
}
