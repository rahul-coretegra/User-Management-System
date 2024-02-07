using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.ManagementModels.VMs;
using User_Management_System.ManagementRepository.IManagementRepository;
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
                    var indb = await context.psqlUnitOfWork.Users.FirstOrDefaultAsync(d => d.PhoneNumber == Identity || d.Email == Identity || d.UserId == Identity, includeProperties: "UserRoles.UserRole");

                    if (indb == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(indb);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var indb = await context.mssqlUnitOfWork.Users.FirstOrDefaultAsync(d => d.PhoneNumber == Identity || d.Email == Identity || d.UserId == Identity, includeProperties: "UserRoles.UserRole");

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
                    var list = await context.psqlUnitOfWork.Users.GetAllAsync(includeProperties: "UserRoles.UserRole");
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var list = await context.mssqlUnitOfWork.Users.GetAllAsync(includeProperties: "UserRoles.UserRole");
                    return Ok(list);
                }
                else
                    return Ok();
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
                        var isuniqueuser = await context.psqlUnitOfWork.Users.IsUniqueUser(User.PhoneNumber, User.Email);
                        if (!isuniqueuser)
                            return NotFound(new { message = "Exists" });
                        var UserId = _management.UniqueId();
                        PostgreSqlModels.User registerUser = new PostgreSqlModels.User
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
                            UserRoles = new List<PostgreSqlModels.UserAndRoles>
                            {
                                new PostgreSqlModels.UserAndRoles
                                {
                                    UniqueId = _management.UniqueId(),
                                    UserId = UserId,
                                    RoleId = SDValues.IndividualRoleId,
                                    AccessToRole = TrueFalse.True
                                }
                            }
                        };
                        await context.psqlUnitOfWork.Users.RegisterUser(registerUser);
                        return Ok(new { message = "Created" });
                    }
                    else
                        return BadRequest(new { message = "BadRequest" });
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    if (ModelState.IsValid)
                    {
                        var isuniqueuser = await context.mssqlUnitOfWork.Users.IsUniqueUser(User.PhoneNumber, User.Email);
                        if (!isuniqueuser)
                            return NotFound(new { message = "Exists" });
                        var UserId = _management.UniqueId();
                        MicrosoftSqlServerModels.User registerUser = new MicrosoftSqlServerModels.User
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
                            UserRoles = new List<MicrosoftSqlServerModels.UserAndRoles>
                            {
                                new MicrosoftSqlServerModels.UserAndRoles
                                {
                                    UniqueId = _management.UniqueId(),
                                    UserId = UserId,
                                    RoleId = SDValues.IndividualRoleId,
                                    AccessToRole = TrueFalse.True
                                }
                            }
                        };
                        await context.mssqlUnitOfWork.Users.RegisterUser(registerUser);
                        return Ok(new { message = "Created" });
                    }
                    else
                        return BadRequest(new { message = "BadRequest" });
                }
                else
                    return Ok();
                
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

                        var indb = await context.psqlUnitOfWork.Users.GetAsync(User.UserId);

                        var inDbExists = await context.psqlUnitOfWork.Users.FirstOrDefaultAsync(d => (d.PhoneNumber == User.PhoneNumber || d.Email == User.Email) && d.UserId != indb.UserId);

                        if (indb == null)
                            return NotFound(new { message = "NotFound" });

                        if (inDbExists != null)
                            return BadRequest(new { message = "Data Not Available" });

                        await context.psqlUnitOfWork.Users.UpdateAsync(indb.UserId, async entity =>
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
                        var updateduser = await context.psqlUnitOfWork.Users.FirstOrDefaultAsync(d => d.UserId == indb.UserId, includeProperties: "UserRoles.UserRole");

                        if (updateduser.IsActiveUser == TrueFalse.False)
                        {
                            foreach (var UserRole in updateduser.UserRoles)
                            {
                                await context.psqlUnitOfWork.UserAndRoles.UpdateAsync(UserRole.UniqueId, async entity =>
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
                                if(UserRole.RoleId == SDValues.IndividualRoleId) {
                                    await context.psqlUnitOfWork.UserAndRoles.UpdateAsync(UserRole.UniqueId, async entity =>
                                    {
                                        entity.AccessToRole = TrueFalse.True;
                                        await Task.CompletedTask;
                                    });
                                }             
                            }
                        }
                        return Ok(new { message = "Updated" });
                    }
                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                    {

                        var indb = await context.mssqlUnitOfWork.Users.GetAsync(User.UserId);

                        var inDbExists = await context.mssqlUnitOfWork.Users.FirstOrDefaultAsync(d => (d.PhoneNumber == User.PhoneNumber || d.Email == User.Email) && d.UserId != indb.UserId);

                        if (indb == null)
                            return NotFound(new { message = "NotFound" });

                        if (inDbExists != null)
                            return BadRequest(new { message = "Data Not Available" });

                        await context.mssqlUnitOfWork.Users.UpdateAsync(indb.UserId, async entity =>
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
                        var updateduser = await context.mssqlUnitOfWork.Users.FirstOrDefaultAsync(d => d.UserId == indb.UserId, includeProperties: "UserRoles.UserRole");

                        if (updateduser.IsActiveUser == TrueFalse.False)
                        {
                            foreach (var UserRole in updateduser.UserRoles)
                            {
                                await context.mssqlUnitOfWork.UserAndRoles.UpdateAsync(UserRole.UniqueId, async entity =>
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
                                if (UserRole.RoleId == SDValues.IndividualRoleId)
                                {
                                    await context.mssqlUnitOfWork.UserAndRoles.UpdateAsync(UserRole.UniqueId, async entity =>
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
                        return Ok();
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
                        var userRoleInDb = await context.psqlUnitOfWork.UserAndRoles.FirstOrDefaultAsync(x => x.RoleId == userRole.RoleId && x.UserId == userRole.UserId);

                        if (userRoleInDb == null)
                        {
                            var UniqueId = _management.UniqueId();
                            PostgreSqlModels.UserAndRoles addUserRole = new PostgreSqlModels.UserAndRoles()
                            {
                                UniqueId = UniqueId,
                                UserId = userRole.UserId,
                                RoleId = userRole.RoleId,
                                AccessToRole = TrueFalse.True
                            };
                            await context.psqlUnitOfWork.UserAndRoles.AddAsync(addUserRole);
                        }
                        else if (userRoleInDb.AccessToRole != userRole.AccessToRole)
                        {
                            await context.psqlUnitOfWork.UserAndRoles.UpdateAsync(userRoleInDb.UniqueId, async entity =>
                            {
                                entity.AccessToRole = userRole.AccessToRole;
                                await Task.CompletedTask;
                            });
                        }
                    }
                    return Ok(new { message = "Ok" });
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    foreach (var userRole in UserRoles)
                    {
                        var userRoleInDb = await context.psqlUnitOfWork.UserAndRoles.FirstOrDefaultAsync(x => x.RoleId == userRole.RoleId && x.UserId == userRole.UserId);

                        if (userRoleInDb == null)
                        {
                            var UniqueId = _management.UniqueId();
                            MicrosoftSqlServerModels.UserAndRoles addUserRole = new MicrosoftSqlServerModels.UserAndRoles()
                            {
                                UniqueId = UniqueId,
                                UserId = userRole.UserId,
                                RoleId = userRole.RoleId,
                                AccessToRole = TrueFalse.True
                            };
                            await context.mssqlUnitOfWork.UserAndRoles.AddAsync(addUserRole);
                        }
                        else if (userRoleInDb.AccessToRole != userRole.AccessToRole)
                        {
                            await context.mssqlUnitOfWork.UserAndRoles.UpdateAsync(userRoleInDb.UniqueId, async entity =>
                            {
                                entity.AccessToRole = userRole.AccessToRole;
                                await Task.CompletedTask;
                            });
                        }
                    }
                    return Ok(new { message = "Ok" });
                }
                else 
                    return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }
    }

}

