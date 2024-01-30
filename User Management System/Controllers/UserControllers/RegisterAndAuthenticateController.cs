using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using User_Management_System.Models.EnumModels;
using User_Management_System.Models.SupremeModels;
using User_Management_System.Models.UserModels;
using User_Management_System.Models.UserModels.UserVM;
using User_Management_System.Repositories.IRepository;
using User_Management_System.Repositories.RegisterAndAuthenticate;
using User_Management_System.SD;
using User_Management_System.TwilioModule;

namespace User_Management_System.Controllers.UserControllers
{
    [ApiController]
    [Route(SDRoutes.RegisterAndAuthenticate)]
    public class RegisterAndAuthenticateController : Controller
    {

        private readonly IUnitOfWork _iunitofwork;
        private readonly IRegisterAndAuthenticateRepository _auth;
        private readonly ITwilioRepository _twilio;

        public RegisterAndAuthenticateController(IUnitOfWork unitofwork,
            IRegisterAndAuthenticateRepository userauthenticationrepository, ITwilioRepository twilio)
        {
            _iunitofwork = unitofwork;
            _auth = userauthenticationrepository;
            _twilio = twilio;
        }

        [HttpPost(SDRoutes.RegisterUser)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterVM user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isuniqueuser = await _auth.IsUniqueUser(user.phoneNumber, user.email);
                    if (!isuniqueuser)
                        return NotFound(new { message = "Exists" });
                    var userUniqueCode = _iunitofwork.GenrateAlphaNumricUniqueCode();
                    User registerUser = new User
                    {
                        userUniqueCode = userUniqueCode,
                        username = user.username,
                        phoneNumber = user.phoneNumber,
                        isVerifiedPhoneNumber = TrueFalse.False,
                        email = user.email,
                        isVerifiedEmail = TrueFalse.False,
                        address = user.address,
                        password = user.password,
                        createdAt = DateTime.UtcNow,
                        userRoles = new List<UserAndRoles>
                            {
                                new UserAndRoles
                                {
                                    userAndRoleUniqueId = _iunitofwork.GenrateAlphaNumricUniqueCode(),
                                    userUniqueCode = userUniqueCode,
                                    roleUniqueCode = SDValues.IndividualRoleCode,
                                    accessToRole = TrueFalse.True
                                }
                            }
                    };
                    await _auth.RegisterUser(registerUser);

                    return Ok(new { message = "Created" });
                }
                else
                    return BadRequest(new { message = "BadRequest" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpPost(SDRoutes.SendOtpUsingTwilio)]
        public async Task<IActionResult> SendOtpVerivicationCode([FromBody] UserVerification verificationVM)
        {
            try
            {
                var userindb = await _iunitofwork.Users.FirstOrDefaultAsync(u => u.phoneNumber == verificationVM.identity);
                if (userindb == null)
                    return NotFound(new { message = "NotFound" });

                if (userindb.isVerifiedPhoneNumber == TrueFalse.True)
                    return Ok(new { message = "Already Verified" });
                else
                {
                    var otp = _twilio.SendVerificationCode(userindb.phoneNumber);
                    var indb = await _iunitofwork.UserVerifications.FirstOrDefaultAsync(x => x.identity == userindb.phoneNumber);
                    if (indb == null)
                    {
                        UserVerification verification = new UserVerification()
                        {
                            identity = userindb.phoneNumber,
                            otp = otp,
                            otpTimeStamp = DateTime.UtcNow,
                        };
                        await _iunitofwork.UserVerifications.AddAsync(verification);
                    }
                    else
                    {
                        await _iunitofwork.UserVerifications.UpdateAsync(indb.identity, async entity =>
                        {
                            entity.otp = otp;
                            entity.otpTimeStamp = DateTime.UtcNow;
                            await Task.CompletedTask;
                        });
                    }
                    return Ok(new { message = "Message Sent" });
                }

            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpPut(SDRoutes.VerifyEmailsAndMessages)]
        public async Task<IActionResult> VerifyEmailsAndMessages([FromBody] UserVerification verificationVM)
        {
            try
            {
                var userindb = await _iunitofwork.Users.FirstOrDefaultAsync(u => u.email == verificationVM.identity || u.phoneNumber == verificationVM.identity);

                var verificationindb = await _iunitofwork.UserVerifications.FirstOrDefaultAsync(v =>v.identity == userindb.phoneNumber);

                if (_iunitofwork.UserVerifications.IsOtpExpired(verificationindb))
                    return BadRequest(new { message = "otp expired" });
                else
                {
                    var isverified = await _iunitofwork.UserVerifications.IsVerified(verificationVM.identity, verificationVM.otp);
                    if (isverified)
                    {
                        await _iunitofwork.Users.UpdateAsync(userindb.userUniqueCode, async entity =>
                        {
                            entity.isVerifiedPhoneNumber = TrueFalse.True;
                            if (entity.isVerifiedEmail == TrueFalse.True)
                                entity.isActiveUser = TrueFalse.True;
                            await Task.CompletedTask;

                        });
                        await _iunitofwork.UserVerifications.RemoveAsync(verificationindb.identity);
                        return Ok(new { status = isverified });

                    }
                    else
                        return BadRequest(new { status = isverified });

                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }



        [HttpPost(SDRoutes.Authenticate)]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateVM VM)
        {
            try
            {
                var userindb = await _iunitofwork.Users.FirstOrDefaultAsync(u => u.phoneNumber == VM.Identity

                                || u.userUniqueCode == VM.Identity || u.email == VM.Identity, includeProperties: "userRoles.userRole");

                if (userindb == null)
                    return NotFound(new { message = "Not Found" });

                else if (userindb.password != VM.Password)
                    return BadRequest(new { message = "Wrong Password" });

                else if (userindb.isActiveUser != TrueFalse.True)
                    return BadRequest(new { message = "User Not Active." });
                else
                {

                    if (VM.roleUniqueCode == null)
                    {
                        var userroles = userindb.userRoles.FindAll(x => x.accessToRole == TrueFalse.True);
                        if (userroles.Count == 0)
                            return BadRequest(new { message = "Sorry !! You don't have access to Any Role." });
                        else if (userroles.Count == 1)
                        {
                            var tokenindb = await _auth.Authenticate(VM.Identity, userroles.First().roleUniqueCode);
                            return Ok(new { token = tokenindb });
                        }
                        else
                        {
                            return Ok(new { UserRoles = userroles });
                        }
                    }
                    else
                    {
                        var userrole = userindb.userRoles.FirstOrDefault(x => x.roleUniqueCode == VM.roleUniqueCode && x.accessToRole == TrueFalse.True);
                        if (userrole == null)
                            return BadRequest(new { message = "Sorry !! You don't have access to this Role." });

                        var tokenindb = await _auth.Authenticate(VM.Identity, userrole.roleUniqueCode);

                        return Ok(new { token = tokenindb });
                    }
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpGet(SDRoutes.GetUserByPhoneNumber)]
        [Authorize(Policy = SDPolicies.IsAccess)]
        public async Task<IActionResult> GetUserByPhoneNumber(string phoneNumber)
        {
            try
            {
                var userPhoneNumberInClaim = User.FindFirstValue(ClaimTypes.MobilePhone);

                var indb = await _iunitofwork.Users.FirstOrDefaultAsync(d => d.phoneNumber == phoneNumber, includeProperties: "userRoles.userRole");
                if (indb == null)
                    return NotFound(new { message = "NotFound" });

                else if (indb.phoneNumber != userPhoneNumberInClaim)
                    return BadRequest(new { message = "NoAccess" });
                else
                {
                    indb.password = null;
                    return Ok(indb);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpPut(SDRoutes.UpdateUser)]
        [Authorize(Policy = SDPolicies.IsAccess)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserVM user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var inRolePhone = User.FindFirstValue(ClaimTypes.MobilePhone);

                    var indb = await _iunitofwork.Users.FirstOrDefaultAsync(d => d.userUniqueCode == user.userUniqueCode);

                    var inDbExists = await _iunitofwork.Users.FirstOrDefaultAsync(d => (d.phoneNumber == user.phoneNumber || d.email == user.email) && d.userUniqueCode != indb.userUniqueCode);

                    if (indb == null)
                        return NotFound(new { message = "NotFound" });

                    if (inRolePhone != indb.phoneNumber)
                        return NotFound(new { message = "No access" });

                    if (inDbExists != null)
                        return BadRequest(new { message = "Data Not Available" });

                    await _iunitofwork.Users.UpdateAsync(indb.userUniqueCode, async entity =>
                    {
                        entity.username = user.username;

                        if (indb.email != user.email)
                        {
                            entity.isVerifiedEmail = TrueFalse.False;
                            entity.isActiveUser = TrueFalse.False;

                        }
                        else
                            entity.email = user.email;

                        if (indb.phoneNumber != user.phoneNumber)
                        {
                            entity.isVerifiedPhoneNumber = TrueFalse.False;
                            entity.isActiveUser = TrueFalse.False;
                        }
                        else
                            entity.phoneNumber = user.phoneNumber;
                        entity.updatedAt = DateTime.UtcNow;
                        await Task.CompletedTask;
                    });

                    return Ok(new { message = "Updated" });
                }
                else
                    return BadRequest(new { message = "BadRequest" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpPut(SDRoutes.ResetPassword)]
        public async Task<IActionResult> ResetPassword(string Identity, [FromBody] PasswordVM VM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userindb = await _iunitofwork.Users.FirstOrDefaultAsync(u => u.phoneNumber == Identity

                                 || u.userUniqueCode == Identity || u.email == Identity);

                    if (userindb == null)
                        return NotFound(new { message = "NotFound" });


                    await _iunitofwork.Users.UpdateAsync(userindb.userUniqueCode, async entity =>
                    {
                        entity.password = VM.Password;

                        entity.updatedAt = DateTime.UtcNow;
                        await Task.CompletedTask;
                    });

                    return Ok(new { message = "Updated" });

                }
                else
                    return BadRequest(new { message = "BadRequest" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }
    }
}
