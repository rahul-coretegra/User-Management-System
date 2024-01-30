using Microsoft.AspNetCore.Identity.UI.Services;
using User_Management_System.DbModule;

namespace User_Management_System.OutlookSmtpConfigurations
{
    public class OutlookSmtpRepository : IOutlookSmtpRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        public OutlookSmtpRepository(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<string> SendEmail(string Email)
        {
            string randomValue = new Random().Next(100000, 999999).ToString();
            string subject = "Verify Your Email Address for Identification";
            string body = $@"
                        <html>                  
                            <body>
                                <div style=""font-family: Helvetica,Arial,sans-serif;min-width:1000px;overflow:auto;line-height:2"">
                                    <div style=""margin:50px auto;width:70%;padding:20px 0"">
                                    <div style=""border-bottom:1px solid #eee"">
                                        <h3 style=""font-size:1.4em;color: #00466a;text-decoration:none;font-weight:600"">UKBOCW</h3>
                                    </div>
                                    <p style=""font-size:1.1em"">Hi,</p>
                                    <p>Use the following OTP to complete your Sign Up procedures. OTP is valid for 3 minutes</p>
                                    <h2 style=""background: #00466a;margin: 0 auto;width: max-content;padding: 0 10px;color: #fff;border-radius: 4px;""> {randomValue}</h2>
                                    <p style=""font-size:0.9em;"">Regards,<br />UKBOCW</p>
                                    <hr style=""border:none;border-top:1px solid #eee"" />
                                    <div style=""float:right;padding:8px 0;color:#aaa;font-size:0.8em;line-height:1;font-weight:300"">
                                        <p>UKBOWC Inc</p>
                                    </div>
                                    </div>
                                </div>
                            </body>
                        </html>";
            await _emailSender.SendEmailAsync(Email, subject, body);
            return randomValue;
        }
    }
}
