namespace User_Management_System.TwilioModule
{
    public interface ITwilioRepository
    {
        string SendVerificationCode(string phoneNumber);
    }
}
