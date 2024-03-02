using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.ManagementModels.VMs;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.SD;

namespace User_Management_System.Controllers.ProjectManagementControllers
{
    [ApiController]
    [Route(SDRoutes.MenuAccess)]
    [Authorize(Policy = SDPolicies.SupremeAccess)]
    public class MenuAccessController : Controller
    {
        private readonly IManagementWork _management;
        private readonly IDbContextConfigurations _dbContextConfigurations;

        public MenuAccessController(IManagementWork managementWork, IDbContextConfigurations dbContextConfigurations)
        {
            _management = managementWork;
            _dbContextConfigurations = dbContextConfigurations;
        }

        [HttpGet(SDRoutes.Get)]
        public async Task<IActionResult> GetRoleAndMenu(string UniqueId)
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
                    var access = await context.psqlUnitOfWork.MenuAccess.FirstOrDefaultAsync(filter: d => d.UniqueId == UniqueId, includeProperties: "UserRole,Menu");
                    if (access == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(access);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var access = await context.mssqlUnitOfWork.MenuAccess.FirstOrDefaultAsync(filter: d => d.UniqueId == UniqueId, includeProperties: "UserRole,Menu");
                    if (access == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(access);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var access = await context.mongoUnitOfWork.MenuAccess.FirstOrDefaultAsync(filter: d => d.UniqueId == UniqueId);

                    if (access == null)
                        return NotFound(new { message = "NotFound" });

                    MongoMenuAccess item = new MongoMenuAccess()
                    {
                        Id = access.Id,
                        UniqueId = access.UniqueId,
                        RoleId = access.RoleId,
                        UserRole = await context.mongoUnitOfWork.UserRoles.FirstOrDefaultAsync(filter: d => d.RoleId == access.RoleId),
                        MenuId = access.MenuId,
                        Menu = await context.mongoUnitOfWork.Menus.FirstOrDefaultAsync(filter: d => d.MenuId == access.MenuId),
                        IsAccess = access.IsAccess
                    };
                    return Ok(item);
                }
                else return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpGet(SDRoutes.GetAll)]
        public async Task<IActionResult> GetAllRolesAndMenus(string RoleId)
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
                    if(RoleId == null)
                        return Ok(await context.psqlUnitOfWork.MenuAccess.GetAllAsync(includeProperties: "UserRole,Menu"));
                    else
                        return Ok(await context.psqlUnitOfWork.MenuAccess.GetAllAsync(ma=>ma.RoleId == RoleId && ma.IsAccess == TrueFalse.True, includeProperties: "UserRole,Menu"));

                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    if (RoleId == null)
                        return Ok(await context.mssqlUnitOfWork.MenuAccess.GetAllAsync(includeProperties: "UserRole,Menu"));
                    else
                        return Ok(await context.mssqlUnitOfWork.MenuAccess.GetAllAsync(ma => ma.RoleId == RoleId && ma.IsAccess ==TrueFalse.True , includeProperties: "UserRole,Menu"));

                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {

                    if (RoleId == null)
                    {
                        var list = await context.mongoUnitOfWork.MenuAccess.GetAllAsync();
                        List<MongoMenuAccess> items = new List<MongoMenuAccess>();
                        foreach (var access in list)
                        {
                            MongoMenuAccess roleAndMenu = new MongoMenuAccess()
                            {
                                Id = access.Id,
                                UniqueId = access.UniqueId,
                                RoleId = access.RoleId,
                                UserRole = await context.mongoUnitOfWork.UserRoles.FirstOrDefaultAsync(filter: d => d.RoleId == access.RoleId),
                                MenuId = access.MenuId,
                                Menu = await context.mongoUnitOfWork.Menus.FirstOrDefaultAsync(filter: d => d.MenuId == access.MenuId),
                                IsAccess = access.IsAccess
                            };
                            items.Add(roleAndMenu);
                        }
                        return Ok(items);
                    }
                    else
                    {
                        var list = await context.mongoUnitOfWork.MenuAccess.GetAllAsync(ma => ma.RoleId == RoleId && ma.IsAccess == TrueFalse.True);

                        List<MongoMenuAccess> items = new List<MongoMenuAccess>();
                        foreach (var item in list)
                        {
                            MongoMenuAccess roleAndMenu = new MongoMenuAccess()
                            {
                                Id = item.Id,
                                UniqueId = item.UniqueId,
                                RoleId = item.RoleId,
                                MenuId = item.MenuId,
                                Menu = await context.mongoUnitOfWork.Menus.FirstOrDefaultAsync(filter: d => d.MenuId == item.MenuId),
                                IsAccess = item.IsAccess
                            };
                            items.Add(roleAndMenu);
                        }
                        return Ok(items);
                    }

                }
                else return BadRequest();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        
        [HttpPost(SDRoutes.UpSertMenuAccess)]
        public async Task<IActionResult> UpSertRoleAccessOfMenus([FromBody] MenuAccessVM[] rolesAndMenus)
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
                    foreach (var roleAndmenu in rolesAndMenus)
                    {
                        var roleAndmenuInDb = await context.psqlUnitOfWork.MenuAccess.FirstOrDefaultAsync(x => x.RoleId == roleAndmenu.RoleId && x.MenuId == roleAndmenu.MenuId);

                        if (roleAndmenuInDb == null)
                        {
                            PostgreSqlModels.MenuAccess addRoleAndMenu = new PostgreSqlModels.MenuAccess()
                            {
                                UniqueId = _management.UniqueId(),
                                RoleId = roleAndmenu.RoleId,
                                MenuId = roleAndmenu.MenuId,
                                IsAccess = TrueFalse.True
                            };
                            await context.psqlUnitOfWork.MenuAccess.AddAsync(addRoleAndMenu);
                        }
                        else if (roleAndmenuInDb.IsAccess != roleAndmenuInDb.IsAccess)
                        {
                            await context.psqlUnitOfWork.MenuAccess.UpdateAsync(roleAndmenuInDb.UniqueId, async entity =>
                            {
                                entity.IsAccess = roleAndmenu.IsAccess;
                                await Task.CompletedTask;
                            });
                        }
                    }
                    return Ok(new { message = "Ok" });
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    foreach (var roleAndmenu in rolesAndMenus)
                    {
                        var roleAndmenuInDb = await context.mssqlUnitOfWork.MenuAccess.FirstOrDefaultAsync(x => x.RoleId == roleAndmenu.RoleId && x.MenuId == roleAndmenu.MenuId);

                        if (roleAndmenuInDb == null)
                        {
                            MicrosoftSqlServerModels.MenuAccess addRoleAndmenu = new MicrosoftSqlServerModels.MenuAccess()
                            {
                                UniqueId = _management.UniqueId(),
                                RoleId = roleAndmenu.RoleId,
                                MenuId = roleAndmenu.MenuId,
                                IsAccess = TrueFalse.True
                            };
                            await context.mssqlUnitOfWork.MenuAccess.AddAsync(addRoleAndmenu);
                        }
                        else if (roleAndmenuInDb.IsAccess != roleAndmenuInDb.IsAccess)
                        {
                            await context.mssqlUnitOfWork.MenuAccess.UpdateAsync(roleAndmenuInDb.UniqueId, async entity =>
                            {
                                entity.IsAccess = roleAndmenu.IsAccess;
                                await Task.CompletedTask;
                            });
                        }
                    }
                    return Ok(new { message = "Ok" });
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    foreach (var roleAndmenu in rolesAndMenus)
                    {
                        var roleAndmenuInDb = await context.mongoUnitOfWork.MenuAccess.FirstOrDefaultAsync(x => x.RoleId == roleAndmenu.RoleId && x.MenuId == roleAndmenu.MenuId);

                        if (roleAndmenuInDb == null)
                        {
                            MongoDbModels.MenuAccess addRoleAndmenu = new MongoDbModels.MenuAccess()
                            {
                                UniqueId = _management.UniqueId(),
                                RoleId = roleAndmenu.RoleId,
                                MenuId = roleAndmenu.MenuId,
                                IsAccess = TrueFalse.True
                            };
                            await context.mongoUnitOfWork.MenuAccess.AddAsync(addRoleAndmenu);
                        }
                        else if (roleAndmenuInDb.IsAccess != roleAndmenu.IsAccess)
                        {
                            await context.mongoUnitOfWork.MenuAccess.UpdateAsync(x => x.UniqueId == roleAndmenuInDb.UniqueId, async entity =>
                            {
                                entity.IsAccess = roleAndmenu.IsAccess;
                                await Task.CompletedTask;
                            });
                        }
                    }
                    return Ok(new { message = "Ok" });
                }
                else return BadRequest();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
