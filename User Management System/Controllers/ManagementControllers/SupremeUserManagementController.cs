using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.ManagementModels.VMs;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.SD;

namespace User_Management_System.Controllers.ManagementControllers
{
    [ApiController]
    [Route(SDRoutes.SupremeUserManagement)]
    public class SupremeUserManagementController : Controller
    {
        private readonly IManagementWork _management;
        public SupremeUserManagementController(IManagementWork management)
        {
            _management = management;
        }

        [HttpGet(SDRoutes.Get)]
        [Authorize(Policy = SDPolicies.SupremeAccess)]
        public async Task<IActionResult> GetSupremeUser(string UserUniqueId)
        {
            try
            {
                var user = await _management.SupremeUsers.FirstOrDefaultAsync(x => x.UserUniqueId == UserUniqueId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpGet(SDRoutes.GetAll)]
        [Authorize(Policy = SDPolicies.SupremeAccess)]
        public async Task<IActionResult> GetAllSupremeUsers()
        {
            try
            {
                var users = await _management.SupremeUsers.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        [HttpPost(SDRoutes.Register)]
        [Authorize(Policy = SDPolicies.SupremeAccess)]
        public async Task<IActionResult> RegisterSupremeUser([FromBody] SupremeUser SupremeUser)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var indb = await _management.SupremeUsers.FirstOrDefaultAsync(d => d.PhoneNumber == SupremeUser.PhoneNumber || d.Email == SupremeUser.Email);
                    if (indb != null)
                        return BadRequest(new { message = "Exists" });
                    else
                    {
                        SupremeUser user = new SupremeUser()
                        {
                            UserUniqueId = _management.UniqueId(),
                            UserName = SupremeUser.UserName,
                            Email = SupremeUser.Email,
                            PhoneNumber = SupremeUser.PhoneNumber,
                            Password = SupremeUser.Password,
                            CreatedAt = DateTime.UtcNow,
                            SupremeAccess = SupremeUser.SupremeAccess,
                            Status = SupremeUser.Status,
                        };
                        await _management.SupremeUsers.AddAsync(user);
                        return Ok(new { message = "Created" });
                    }
                }
                else
                    return BadRequest(new { message = "BadRequest" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpPost(SDRoutes.Authenticate)]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateSupremeUser SupremeUser)
        {
            try
            {
                var userindb = await _management.SupremeUsers.FirstOrDefaultAsync(u => u.PhoneNumber == SupremeUser.Identity

                            || u.UserUniqueId == SupremeUser.Identity || u.Email == SupremeUser.Identity);

                if (userindb == null)
                    return NotFound(new { message = "Not Found" });

                else if (userindb.Password != SupremeUser.Password)
                    return BadRequest(new { message = "Wrong Password" });

                else if (userindb.Status != TrueFalse.True)
                    return BadRequest(new { message = "User Not Active." });
                else
                {
                    var tokenindb = await _management.SupremeUsers.Authenticate(SupremeUser.Identity);
                    return Ok(new { token = tokenindb });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpPut(SDRoutes.Update)]
        [Authorize(Policy = SDPolicies.SupremeAccess)]
        public async Task<IActionResult> UpdateSupremeUser([FromBody] SupremeUser SupremeUser)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var indb = await _management.SupremeUsers.FirstOrDefaultAsync(d => d.UserUniqueId == SupremeUser.UserUniqueId);
                    if (indb == null)
                        return NotFound(new { message = "Not found" });

                    var indbExists = await _management.SupremeUsers.FirstOrDefaultAsync(d => d.UserUniqueId != indb.UserUniqueId && (d.UserName == SupremeUser.UserName || d.Email == SupremeUser.Email));

                    if (indbExists != null)
                        return BadRequest(new { message = "Exists" });

                    await _management.SupremeUsers.UpdateAsync(indb.UserUniqueId, async entity =>
                    {

                        entity.UserName = SupremeUser.UserName;
                        entity.Email = SupremeUser.Email;
                        entity.PhoneNumber = SupremeUser.PhoneNumber;
                        entity.Password = SupremeUser.Password;
                        entity.SupremeAccess = SupremeUser.SupremeAccess;
                        entity.Status = SupremeUser.Status;
                        await Task.CompletedTask;
                    });
                    return Ok(new { message = "Updated" });

                }
                else
                    return BadRequest(new { message = "BadRequest" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}

