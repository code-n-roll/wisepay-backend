namespace WisePay.Web.Users.Models
{
    public class CurrentUserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string CardLastFourDigits { get; set; }
    }
}
