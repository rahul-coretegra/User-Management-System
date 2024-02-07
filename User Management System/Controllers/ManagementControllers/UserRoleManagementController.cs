using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.ManagementModels.VMs;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.PostgreSqlModels;
using User_Management_System.SD;

namespace User_Management_System.Controllers.ManagementControllers
{
    [ApiController]
    [Route(SDRoutes.UserRoleManagement)]
    [Authorize(Policy = SDPolicies.SupremeAccess)]
    public class UserRoleManagementController : Controller
    {
        private readonly IManagementWork _management;
        private readonly IDbContextConfigurations _dbContextConfigurations;

        public UserRoleManagementController(IManagementWork managementWork, IDbContextConfigurations dbContextConfigurations)
        {
            _management = managementWork;
            _dbContextConfigurations = dbContextConfigurations;
        }

        [HttpGet(SDRoutes.UserRole)]
        public async Task<IActionResult> GetUserRole(string RoleId)
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
                    var role = await context.PsqlUOW.UserRoles.FirstOrDefaultAsync(d => d.RoleId == RoleId);
                    if (role == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(role);
                }
                else return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }


        [HttpGet(SDRoutes.UserRoles)]
        public async Task<IActionResult> GetUserRoles()
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
                    var list = await context.PsqlUOW.UserRoles.GetAllAsync();
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


        [HttpPost(SDRoutes.CreateUserRole)]
        public async Task<IActionResult> CreateUserRole([FromBody] UserRoleVM UserRole)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpContext.Request.Headers.TryGetValue("projectUniqueId", out var projectUniqueId);
                    var projectInDb = await _management.Projects.FirstOrDefaultAsync(p => p.ProjectUniqueId == projectUniqueId.ToString());
                    if (projectInDb == null)
                        return NotFound();
                    var context = _dbContextConfigurations.configureDbContext(projectInDb);

                    if (projectInDb.TypeOfDatabase == TypeOfDatabase.PostgreSql)
                    {
                        var indb = await context.PsqlUOW.UserRoles.FirstOrDefaultAsync(d => d.RoleName == UserRole.RoleName);
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });
                        if (UserRole.RoleName == SDValues.IndividualRole)
                        {
                            UserRole role = new UserRole()
                            {
                                RoleName = SDValues.IndividualRole,
                                RoleId = SDValues.IndividualRoleCode,
                                RoleLevel = RoleLevels.Primary,
                            };
                            await context.PsqlUOW.UserRoles.AddAsync(role);
                        }
                        else
                        {
                            UserRole role = new UserRole()
                            {
                                RoleName = UserRole.RoleName,
                                RoleId = _management.UniqueId(),
                                RoleLevel = UserRole.RoleLevel
                            };
                            await context.PsqlUOW.UserRoles.AddAsync(role);

                        }
                        return Ok(new { message = "Created" });
                    }
                    else
                        return BadRequest();
                }
                else
                    return BadRequest(new { message = "BadRequest" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpPut(SDRoutes.UpadateUserRole)]
        public async Task<IActionResult> UpdateUserRole([FromBody] UserRole UserRole)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpContext.Request.Headers.TryGetValue("projectUniqueId", out var projectUniqueId);
                    var projectInDb = await _management.Projects.FirstOrDefaultAsync(p => p.ProjectUniqueId == projectUniqueId.ToString());
                    if (projectInDb == null)
                        return NotFound();
                    var context = _dbContextConfigurations.configureDbContext(projectInDb);

                    if (projectInDb.TypeOfDatabase == TypeOfDatabase.PostgreSql)
                    {
                        var indb = await context.PsqlUOW.UserRoles.GetAsync(UserRole.RoleId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        var indbExists = await context.PsqlUOW.UserRoles.FirstOrDefaultAsync(d => d.RoleId != UserRole.RoleId && d.RoleName == UserRole.RoleName);

                        if (indbExists != null)
                            return BadRequest(new { message = "Exists" });

                        await context.PsqlUOW.UserRoles.UpdateAsync(UserRole.RoleId, async entity =>
                        {
                            entity.RoleName = UserRole.RoleName;
                            entity.RoleLevel = UserRole.RoleLevel;
                            await Task.CompletedTask;
                        });

                        return Ok(new { message = "Updated" });
                    }
                    else
                        return BadRequest();
                }
                else
                    return BadRequest(new { message = "BadRequest" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });

            }
        }

        [HttpDelete(SDRoutes.DeleteUserRole)]
        public async Task<IActionResult> DeleteUserRole(string RoleId)
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
                    var indb = await context.PsqlUOW.UserRoles.GetAsync(RoleId);
                    if (indb == null)
                        return NotFound(new { message = "Not Found" });

                    var propshave = await context.PsqlUOW.UserAndRoles.FirstOrDefaultAsync(ur => ur.RoleId == indb.RoleId);
                    if (propshave != null)
                        return BadRequest(new { message = "ObjectDepends" });

                    await context.PsqlUOW.UserRoles.RemoveAsync(RoleId);
                    return Ok(new { message = "Deleted" });
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
