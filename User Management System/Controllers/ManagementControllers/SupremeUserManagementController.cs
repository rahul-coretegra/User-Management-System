using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Management_System.ManagementModels;
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

        [HttpGet(SDRoutes.SupremeUser)]
        [Authorize(Policy = SDPolicies.SupremeAccess)]
        public async Task<IActionResult> GetSupremeUser(string UniqueId)
        {
            try
            {
                var user = await _management.SupremeUsers.FirstOrDefaultAsync(x => x.UniqueId == UniqueId);
                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpGet(SDRoutes.SupremeUsers)]
        [Authorize(Policy = SDPolicies.SupremeAccess)]
        public async Task<IActionResult> GetSupremeUsers()
        {
            try
            {
                var users = await _management.SupremeUsers.GetAllAsync();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpPost(SDRoutes.RegisterSupremeUser)]
        [Authorize(Policy = SDPolicies.SupremeAccess)]
        public async Task<IActionResult> RegisterSupremeUser([FromBody] SupremeUser SupremeUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var indb = await _management.SupremeUsers.FirstOrDefaultAsync(d => d.PhoneNumber == SupremeUser.PhoneNumber || d.Email == SupremeUser.Email);
                    if (indb != null)
                        return BadRequest(new { message = "Exists" });
                    else
                    {
                        SupremeUser user = new SupremeUser()
                        {
                            UniqueId = _management.UniqueId(),
                            UserName = SupremeUser.UserName,
                            Email = SupremeUser.Email,
                            PhoneNumber = SupremeUser.PhoneNumber,
                            Password = SupremeUser.Password,
                            CreatedAt = DateTime.UtcNow,
                            SupremeAccess = true
                        };
                        await _management.SupremeUsers.AddAsync(user);
                        return Ok(new { message = "Created" });
                    }
                }
                catch (Exception)
                {
                    return StatusCode(500, new { message = "Database Error" });
                }
            }
            else
                return BadRequest(new { message = "BadRequest" });
        }


        [HttpPost(SDRoutes.Authenticate)]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateSupremeUser SupremeUser)
        {
            try
            {
                var userindb = await _management.SupremeUsers.FirstOrDefaultAsync(u => u.PhoneNumber == SupremeUser.Identity

                            || u.UniqueId == SupremeUser.Identity || u.Email == SupremeUser.Identity);

                if (userindb == null)
                    return NotFound(new { message = "Not Found" });

                else if (userindb.Password !=   SupremeUser.Password)
                    return BadRequest(new { message = "Wrong Password" });

                else if (userindb.SupremeAccess != true)
                    return BadRequest(new { message = "User Not Active." });
                else
                {
                    var tokenindb = await _management.SupremeUsers.Authenticate(SupremeUser.Identity);
                    return Ok(new { token = tokenindb });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }


        [HttpPut(SDRoutes.UpdateSupremeUser)]
        [Authorize(Policy = SDPolicies.SupremeAccess)]
        public async Task<IActionResult> UpdateSupremeUser([FromBody] SupremeUser supremeUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var indb = await _management.SupremeUsers.FirstOrDefaultAsync(d => d.UniqueId == supremeUser.UniqueId);
                    if (indb == null)
                        return NotFound(new { message = "Not found" });

                    var indbExists = await _management.SupremeUsers.FirstOrDefaultAsync(d => d.UniqueId != indb.UniqueId && (d.UserName == supremeUser.UserName || d.Email == supremeUser.Email));

                    if (indbExists != null)
                        return BadRequest(new { message = "Exists" });

                    await _management.SupremeUsers.UpdateAsync(indb.UniqueId, async entity =>
                    {

                        entity.UserName = supremeUser.UserName;
                        entity.Email = supremeUser.Email;
                        entity.PhoneNumber = supremeUser.PhoneNumber;
                        entity.Password = supremeUser.Password;
                        entity.SupremeAccess = supremeUser.SupremeAccess;
                        await Task.CompletedTask;
                    });
                    return Ok(new { message = "Updated" });

                }
                catch (Exception)
                {
                    return StatusCode(500, new { message = "Database Error" });
                }
            }
            else
                return BadRequest(new { message = "BadRequest" });
        }

    }
}

