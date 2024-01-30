using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using User_Management_System.DbModule;
using User_Management_System.SD;

namespace User_Management_System.TwilioModule
{
    public class TwilioRepository : ITwilioRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly TwilioSettings _twilio;

        public TwilioRepository(IOptions<TwilioSettings> twilio, ApplicationDbContext context)
        {
            _twilio = twilio.Value;
            _context = context;
        }
        public string SendVerificationCode(string phoneNumber)
        {
            string authtoken = _twilio.AuthToken;
            string accountsid = _twilio.AccountSID;
            string phone = _twilio.PhoneNumber;

            string randomValue = new Random().Next(100000, 999999).ToString();

            TwilioClient.Init(accountsid, authtoken);
            MessageResource.Create(
                    body: SDValues.Message + randomValue + SDValues.OTPTimeStamp,
                    from: new Twilio.Types.PhoneNumber(phone),
                    to: "+91" + phoneNumber
                );
            return randomValue;
        }
    }
}
