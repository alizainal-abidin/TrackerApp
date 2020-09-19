namespace IdentityServer.Models.ViewModels
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string FullName { get; internal set; }

        public string PhoneNumber { get; internal set; }
    }
}