namespace User_Management_System.OutlookSmtpConfigurations
{
    public interface IOutlookSmtpRepository
    {
        public Task<string> SendEmail(string Email);
    }
}
