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
                var context = _dbContextConfigurations.configureDbContexts(projectInDb);

                if (projectInDb.TypeOfDatabase == TypeOfDatabase.PostgreSql)
                {
                    var role = await context.psqlUnitOfWork.UserRoles.FirstOrDefaultAsync(d => d.RoleId == RoleId);
                    if (role == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(role);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var role = await context.mssqlUnitOfWork.UserRoles.FirstOrDefaultAsync(d => d.RoleId == RoleId);
                    if (role == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(role);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var role = await context.mongoUnitOfWork.UserRoles.FirstOrDefaultAsync(d => d.RoleId == RoleId);
                    if (role == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(role);
                }
                else return Ok();
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
                var context = _dbContextConfigurations.configureDbContexts(projectInDb);

                if (projectInDb.TypeOfDatabase == TypeOfDatabase.PostgreSql)
                {
                    var list = await context.psqlUnitOfWork.UserRoles.GetAllAsync();
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var list = await context.mssqlUnitOfWork.UserRoles.GetAllAsync();
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var list = await context.mongoUnitOfWork.UserRoles.GetAllAsync();
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


        [HttpPost(SDRoutes.CreateUserRole)]
        public async Task<IActionResult> CreateUserRole([FromBody] UserRole UserRole)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpContext.Request.Headers.TryGetValue("projectUniqueId", out var projectUniqueId);
                    var projectInDb = await _management.Projects.FirstOrDefaultAsync(p => p.ProjectUniqueId == projectUniqueId.ToString());
                    if (projectInDb == null)
                        return NotFound();
                    var context = _dbContextConfigurations.configureDbContexts(projectInDb);

                    if (projectInDb.TypeOfDatabase == TypeOfDatabase.PostgreSql)
                    {
                        var indb = await context.psqlUnitOfWork.UserRoles.FirstOrDefaultAsync(d => d.RoleName == UserRole.RoleName);
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });
                        if (UserRole.RoleName == SDValues.IndividualRole)
                        {
                            PostgreSqlModels.UserRole role = new PostgreSqlModels.UserRole()
                            {
                                RoleName = SDValues.IndividualRole,
                                RoleId = SDValues.IndividualRoleId,
                                RoleLevel = RoleLevels.Primary,
                            };
                            await context.psqlUnitOfWork.UserRoles.AddAsync(role);
                        }
                        else 
                        {
                            PostgreSqlModels.UserRole role = new PostgreSqlModels.UserRole()
                            {
                                RoleName = UserRole.RoleName,
                                RoleId = _management.UniqueId(),
                                RoleLevel = UserRole.RoleLevel
                            };
                            await context.psqlUnitOfWork.UserRoles.AddAsync(role);

                        }
                        return Ok(new { message = "Created" });
                    }
                    
                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                    {
                        var indb = await context.mssqlUnitOfWork.UserRoles.FirstOrDefaultAsync(d => d.RoleName == UserRole.RoleName);
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });
                        if (UserRole.RoleName == SDValues.IndividualRole)
                        {
                            MicrosoftSqlServerModels.UserRole role = new MicrosoftSqlServerModels.UserRole()
                            {
                                RoleName = SDValues.IndividualRole,
                                RoleId = SDValues.IndividualRoleId,
                                RoleLevel = RoleLevels.Primary,
                            };
                            await context.mssqlUnitOfWork.UserRoles.AddAsync(role);
                        }
                        else
                        {
                            MicrosoftSqlServerModels.UserRole role = new MicrosoftSqlServerModels.UserRole()
                            {
                                RoleName = UserRole.RoleName,
                                RoleId = _management.UniqueId(),
                                RoleLevel = UserRole.RoleLevel
                            };
                            await context.mssqlUnitOfWork.UserRoles.AddAsync(role);

                        }
                        return Ok(new { message = "Created" });
                    }

                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                    {
                        var indb = await context.mongoUnitOfWork.UserRoles.FirstOrDefaultAsync(d => d.RoleName == UserRole.RoleName);
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });
                        if (UserRole.RoleName == SDValues.IndividualRole)
                        {
                            MongoDbModels.UserRole role = new MongoDbModels.UserRole()
                            {
                                RoleName = SDValues.IndividualRole,
                                RoleId = SDValues.IndividualRoleId,
                                RoleLevel = RoleLevels.Primary,
                            };
                            await context.mongoUnitOfWork.UserRoles.AddAsync(role);
                        }
                        else
                        {
                            MongoDbModels.UserRole role = new MongoDbModels.UserRole()
                            {
                                RoleName = UserRole.RoleName,
                                RoleId = _management.UniqueId(),
                                RoleLevel = UserRole.RoleLevel
                            };
                            await context.mongoUnitOfWork.UserRoles.AddAsync(role);

                        }
                        return Ok(new { message = "Created" });
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
                    var context = _dbContextConfigurations.configureDbContexts(projectInDb);

                    if (projectInDb.TypeOfDatabase == TypeOfDatabase.PostgreSql)
                    {
                        var indb = await context.psqlUnitOfWork.UserRoles.GetAsync(UserRole.RoleId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        var indbExists = await context.psqlUnitOfWork.UserRoles.FirstOrDefaultAsync(d => d.RoleId != indb.RoleId && d.RoleName == UserRole.RoleName);

                        if (indbExists != null)
                            return BadRequest(new { message = "Exists" });

                        await context.psqlUnitOfWork.UserRoles.UpdateAsync(indb.RoleId, async entity =>
                        {
                            entity.RoleName = UserRole.RoleName;
                            entity.RoleLevel = UserRole.RoleLevel;
                            await Task.CompletedTask;
                        });

                        return Ok(new { message = "Updated" });
                    }
                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                    {
                        var indb = await context.mssqlUnitOfWork.UserRoles.GetAsync(UserRole.RoleId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        var indbExists = await context.mssqlUnitOfWork.UserRoles.FirstOrDefaultAsync(d => d.RoleId != indb.RoleId && d.RoleName == UserRole.RoleName);

                        if (indbExists != null)
                            return BadRequest(new { message = "Exists" });

                        await context.mssqlUnitOfWork.UserRoles.UpdateAsync(indb.RoleId, async entity =>
                        {
                            entity.RoleName = UserRole.RoleName;
                            entity.RoleLevel = UserRole.RoleLevel;
                            await Task.CompletedTask;
                        });

                        return Ok(new { message = "Updated" });
                    }
                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                    {
                        var indb = await context.mongoUnitOfWork.UserRoles.FirstOrDefaultAsync(x=>x.RoleId ==UserRole.RoleId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        var indbExists = await context.mongoUnitOfWork.UserRoles.FirstOrDefaultAsync(d => d.RoleId != indb.RoleId && d.RoleName == UserRole.RoleName);

                        if (indbExists != null)
                            return BadRequest(new { message = "Exists" });

                        await context.mongoUnitOfWork.UserRoles.UpdateAsync(x => x.RoleId == indb.RoleId, async entity =>
                        {
                            entity.RoleName = UserRole.RoleName;
                            entity.RoleLevel = UserRole.RoleLevel;
                            await Task.CompletedTask;
                        });

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

        [HttpDelete(SDRoutes.DeleteUserRole)]
        public async Task<IActionResult> DeleteUserRole(string RoleId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("projectUniqueId", out var projectUniqueId);
                var projectInDb = await _management.Projects.FirstOrDefaultAsync(p => p.ProjectUniqueId == projectUniqueId.ToString());
                if (projectInDb == null)
                    return NotFound();
                var context = _dbContextConfigurations.configureDbContexts(projectInDb);

                if (projectInDb.TypeOfDatabase == TypeOfDatabase.PostgreSql)
                {
                    var indb = await context.psqlUnitOfWork.UserRoles.GetAsync(RoleId);
                    if (indb == null)
                        return NotFound(new { message = "Not Found" });

                    var propshave = await context.psqlUnitOfWork.UserAndRoles.FirstOrDefaultAsync(ur => ur.RoleId == indb.RoleId);
                    if (propshave != null)
                        return BadRequest(new { message = "ObjectDepends" });

                    await context.psqlUnitOfWork.UserRoles.RemoveAsync(RoleId);
                    return Ok(new { message = "Deleted" });
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var indb = await context.mssqlUnitOfWork.UserRoles.GetAsync(RoleId);
                    if (indb == null)
                        return NotFound(new { message = "Not Found" });

                    var propshave = await context.mssqlUnitOfWork.UserAndRoles.FirstOrDefaultAsync(ur => ur.RoleId == indb.RoleId);
                    if (propshave != null)
                        return BadRequest(new { message = "ObjectDepends" });

                    await context.mssqlUnitOfWork.UserRoles.RemoveAsync(RoleId);
                    return Ok(new { message = "Deleted" });
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var indb = await context.mongoUnitOfWork.UserRoles.FirstOrDefaultAsync(x=>x.RoleId==RoleId);
                    if (indb == null)
                        return NotFound(new { message = "Not Found" });

                    var propshave = await context.mongoUnitOfWork.UserAndRoles.FirstOrDefaultAsync(ur => ur.RoleId == indb.RoleId);
                    if (propshave != null)
                        return BadRequest(new { message = "ObjectDepends" });

                    await context.mongoUnitOfWork.UserRoles.FirstOrDefaultAsync(x => x.RoleId == RoleId);
                    return Ok(new { message = "Deleted" });
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
