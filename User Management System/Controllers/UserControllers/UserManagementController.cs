using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.ManagementModels.VMs;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.PostgreSqlModels;
using User_Management_System.SD;

namespace User_Management_System.Controllers.UserControllers
{

    [ApiController]
    [Route(SDRoutes.UserManagement)]
    [Authorize(Policy = SDPolicies.SupremeAccess)]
    public class UserManagementController : Controller
    {
        private readonly IManagementWork _management;
        private readonly IDbContextConfigurations _dbContextConfigurations;

        public UserManagementController(IManagementWork management, IDbContextConfigurations dbContextConfigurations)
        {
            _dbContextConfigurations = dbContextConfigurations;
            _management = management;
        }

        [HttpGet(SDRoutes.User)]
        public async Task<IActionResult> GetUser(string Identity)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("projectUniqueId", out var projectUniqueId);
                var projectInDb = await _management.Projects.FirstOrDefaultAsync(p => p.ProjectUniqueId == projectUniqueId.ToString());

                var context = _dbContextConfigurations.configureDbContext(projectInDb);
                if (projectInDb.TypeOfDatabase == TypeOfDatabase.PostgreSql)
                {
                    var indb = await context.PsqlUOW.Users.FirstOrDefaultAsync(d => d.PhoneNumber == Identity || d.Email == Identity || d.UserId == Identity, includeProperties: "UserRoles.UserRole");

                    if (indb == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(indb);
                }
                else
                    return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpGet(SDRoutes.Users)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("projectUniqueId", out var projectUniqueId);
                var projectInDb = await _management.Projects.FirstOrDefaultAsync(p => p.ProjectUniqueId == projectUniqueId.ToString());
                if (projectInDb == null)
                    return NotFound();
                var context = _dbContextConfigurations.configureDbContext(projectInDb);

                if (projectInDb.TypeOfDatabase == TypeOfDatabase.PostgreSql)
                {
                    var list = await context.PsqlUOW.Users.GetAllAsync(includeProperties: "UserRoles.UserRole");
                    return Ok(list);
                }
                else
                    return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }


        [HttpPost(SDRoutes.RegisterUser)]
        public async Task<IActionResult> RegisterUser([FromBody] UserVM User)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("projectUniqueId", out var projectUniqueId);
                var projectInDb = await _management.Projects.FirstOrDefaultAsync(p => p.ProjectUniqueId == projectUniqueId.ToString());
                if (projectInDb == null)
                    return NotFound();
                var context = _dbContextConfigurations.configureDbContext(projectInDb);
                if (projectInDb.TypeOfDatabase == TypeOfDatabase.PostgreSql)
                {
                    if (ModelState.IsValid)
                    {
                        var isuniqueuser = await context.Psqlauthentication.IsUniqueUser(User.PhoneNumber, User.Email);
                        if (!isuniqueuser)
                            return NotFound(new { message = "Exists" });
                        var UserId = _management.UniqueId();
                        User registerUser = new User
                        {
                            UserId = UserId,
                            UserName = User.UserName,
                            IsVerifiedPhoneNumber = TrueFalse.True,
                            PhoneNumber = User.PhoneNumber,
                            IsVerifiedEmail = TrueFalse.True,
                            Email = User.Email,
                            IsActiveUser = TrueFalse.True,
                            Address = User.Address,
                            Password = User.Password,
                            CreatedAt = DateTime.UtcNow,
                            UserRoles = new List<UserAndRoles>
                            {
                                new UserAndRoles
                                {
                                    UniqueId = _management.UniqueId(),
                                    UserId = UserId,
                                    RoleId = SDValues.IndividualRoleCode,
                                    AccessToRole = TrueFalse.True
                                }
                            }
                        };
                        await context.Psqlauthentication.RegisterUser(registerUser);
                        return Ok(new { message = "Created" });
                    }
                    else
                        return BadRequest(new { message = "BadRequest" });
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpPut(SDRoutes.UpdateUser)]
        public async Task<IActionResult> UpdateUser([FromBody] UserVM User)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("projectUniqueId", out var projectUniqueId);
                var projectInDb = await _management.Projects.FirstOrDefaultAsync(p => p.ProjectUniqueId == projectUniqueId.ToString());

                var context = _dbContextConfigurations.configureDbContext(projectInDb);

                if (ModelState.IsValid)
                {
                    if (projectInDb.TypeOfDatabase == TypeOfDatabase.PostgreSql)
                    {

                        var indb = await context.PsqlUOW.Users.GetAsync(User.UserId);

                        var inDbExists = await context.PsqlUOW.Users.FirstOrDefaultAsync(d => (d.PhoneNumber == User.PhoneNumber || d.Email == User.Email) && d.UserId != indb.UserId);

                        if (indb == null)
                            return NotFound(new { message = "NotFound" });

                        if (inDbExists != null)
                            return BadRequest(new { message = "Data Not Available" });

                        await context.PsqlUOW.Users.UpdateAsync(indb.UserId, async entity =>
                        {
                            entity.UserName = User.UserName;
                            entity.Email = User.Email;
                            entity.PhoneNumber = User.PhoneNumber;
                            entity.Address = User.Address;
                            entity.UpdatedAt = DateTime.UtcNow;
                            if (indb.IsActiveUser != User.IsActiveUser)
                                entity.IsActiveUser = User.IsActiveUser;
                            await Task.CompletedTask;
                        });
                        var updateduser = await context.PsqlUOW.Users.FirstOrDefaultAsync(d => d.UserId == indb.UserId, includeProperties: "UserRoles.UserRole");

                        if (updateduser.IsActiveUser == TrueFalse.False)
                        {
                            foreach (var UserRole in updateduser.UserRoles)
                            {
                                await context.PsqlUOW.UserAndRoles.UpdateAsync(UserRole.UniqueId, async entity =>
                                {
                                    entity.AccessToRole = TrueFalse.False;
                                    await Task.CompletedTask;
                                });
                            }
                        }
                        else if (updateduser.IsActiveUser == TrueFalse.True)
                        {
                            foreach (var UserRole in updateduser.UserRoles)
                            {
                                if(UserRole.RoleId == SDValues.IndividualRoleCode) {
                                    await context.PsqlUOW.UserAndRoles.UpdateAsync(UserRole.UniqueId, async entity =>
                                    {
                                        entity.AccessToRole = TrueFalse.True;
                                        await Task.CompletedTask;
                                    });
                                }             
                            }
                        }
                        return Ok(new { message = "Updated" });
                    }
                    else
                        return NotFound();
                }
                else
                    return BadRequest(new { message = "BadRequest" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpPost(SDRoutes.UpsertUserAndRoles)]
        public async Task<IActionResult> UpsertUserAndRoles([FromBody] UserAndRolesVM[] UserRoles)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("projectUniqueId", out var projectUniqueId);
                var projectInDb = await _management.Projects.FirstOrDefaultAsync(p => p.ProjectUniqueId == projectUniqueId.ToString());
                if (projectInDb == null)
                    return NotFound();
                var context = _dbContextConfigurations.configureDbContext(projectInDb);

                if (projectInDb.TypeOfDatabase == TypeOfDatabase.PostgreSql)
                {
                    foreach (var userRole in UserRoles)
                    {
                        var userRoleInDb = await context.PsqlUOW.UserAndRoles.FirstOrDefaultAsync(x => x.RoleId == userRole.RoleId && x.UserId == userRole.UserId);

                        if (userRoleInDb == null)
                        {
                            var UniqueId = _management.UniqueId();
                            UserAndRoles addUserRole = new UserAndRoles()
                            {
                                UniqueId = UniqueId,
                                UserId = userRole.UserId,
                                RoleId = userRole.RoleId,
                                AccessToRole = TrueFalse.True
                            };
                            await context.PsqlUOW.UserAndRoles.AddAsync(addUserRole);
                        }
                        else if (userRoleInDb.AccessToRole != userRole.AccessToRole)
                        {
                            await context.PsqlUOW.UserAndRoles.UpdateAsync(userRoleInDb.UniqueId, async entity =>
                            {
                                entity.AccessToRole = userRole.AccessToRole;
                                await Task.CompletedTask;
                            });
                        }
                    }
                    return Ok(new { message = "Ok" });
                }
                else return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }
    }

}

