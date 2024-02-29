using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.ManagementModels.VMs;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.SD;

namespace User_Management_System.Controllers.ProjectManagementControllers
{
    [ApiController]
    [Route(SDRoutes.RoleAndMenusManagement)]
    [Authorize(Policy = SDPolicies.SupremeAccess)]
    public class RoleAndMenusManagementController : Controller
    {
        private readonly IManagementWork _management;
        private readonly IDbContextConfigurations _dbContextConfigurations;

        public RoleAndMenusManagementController(IManagementWork managementWork, IDbContextConfigurations dbContextConfigurations)
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
                    var roleAndMenu = await context.psqlUnitOfWork.RoleAndMenus.FirstOrDefaultAsync(filter: d => d.UniqueId == UniqueId, includeProperties: "UserRole,Menu");
                    if (roleAndMenu == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(roleAndMenu);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var roleAndMenu = await context.mssqlUnitOfWork.RoleAndMenus.FirstOrDefaultAsync(filter: d => d.UniqueId == UniqueId, includeProperties: "UserRole,Menu");
                    if (roleAndMenu == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(roleAndMenu);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var roleAndMenu = await context.mongoUnitOfWork.RoleAndMenus.FirstOrDefaultAsync(filter: d => d.UniqueId == UniqueId);

                    if (roleAndMenu == null)
                        return NotFound(new { message = "NotFound" });
                    var userrole = await context.mongoUnitOfWork.UserRoles.FirstOrDefaultAsync(filter: d => d.RoleId == roleAndMenu.RoleId);
                    var route = await context.mongoUnitOfWork.Menus.FirstOrDefaultAsync(filter: d => d.MenuId == roleAndMenu.MenuId);

                    MongoRoleAndMenu item = new MongoRoleAndMenu()
                    {
                        Id = roleAndMenu.Id,
                        UniqueId = roleAndMenu.UniqueId,
                        RoleId = roleAndMenu.RoleId,
                        UserRole = userrole,
                        MenuId = roleAndMenu.MenuId,
                        Menu = route,
                        IsAccess = roleAndMenu.IsAccess
                    };
                    return Ok(item);
                }
                else return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpGet(SDRoutes.GetAll)]
        public async Task<IActionResult> GetAllRolesAndMenus()
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
                    var list = await context.psqlUnitOfWork.RoleAndMenus.GetAllAsync(includeProperties: "UserRole,Menu");
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var list = await context.mssqlUnitOfWork.RoleAndMenus.GetAllAsync(includeProperties: "UserRole,Menu");
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var list = await context.mongoUnitOfWork.RoleAndMenus.GetAllAsync();
                    List<MongoRoleAndMenu> items = new List<MongoRoleAndMenu>();
                    foreach (var item in list)
                    {
                        var userrole = await context.mongoUnitOfWork.UserRoles.FirstOrDefaultAsync(filter: d => d.RoleId == item.RoleId);
                        var menu = await context.mongoUnitOfWork.Menus.FirstOrDefaultAsync(filter: d => d.MenuId == item.MenuId);

                        MongoRoleAndMenu roleAndMenu = new MongoRoleAndMenu()
                        {
                            Id = item.Id,
                            UniqueId = item.UniqueId,
                            RoleId = item.RoleId,
                            UserRole = userrole,
                            MenuId = item.MenuId,
                            Menu = menu,
                            IsAccess = item.IsAccess
                        };
                        items.Add(roleAndMenu);
                    }
                    return Ok(items);
                }
                else return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpGet(SDRoutes.GetByRoleId)]
        public async Task<IActionResult> GetMenusByRoleId(string RoleId)
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
                    var roleAndMenus = (await context.psqlUnitOfWork.RoleAndMenus.GetAllAsync(r => r.RoleId == RoleId && r.IsAccess == TrueFalse.True, includeProperties: "Menu,UserRole")).ToList();
                    return Ok(roleAndMenus);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var roleAndMenus = (await context.mssqlUnitOfWork.RoleAndMenus.GetAllAsync(r => r.RoleId == RoleId && r.IsAccess == TrueFalse.True, includeProperties: "Menu,UserRole")).ToList();
                    return Ok(roleAndMenus);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var roleAndMenus = (await context.mongoUnitOfWork.RoleAndMenus.GetAllAsync(r => r.RoleId == RoleId && r.IsAccess == TrueFalse.True)).ToList();

                    List<MongoRoleAndMenu> items = new List<MongoRoleAndMenu>();
                    foreach (var item in roleAndMenus)
                    {
                        var menu = await context.mongoUnitOfWork.Menus.FirstOrDefaultAsync(filter: d => d.MenuId == item.MenuId);

                        MongoRoleAndMenu roleAndMenu = new MongoRoleAndMenu()
                        {
                            Id = item.Id,
                            UniqueId = item.UniqueId,
                            RoleId = item.RoleId,
                            MenuId = item.MenuId,
                            Menu = menu,
                            IsAccess = item.IsAccess
                        };
                        items.Add(roleAndMenu);
                    }
                    return Ok(items);
                }
                else return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpPost(SDRoutes.UpSertRoleAccess)]
        public async Task<IActionResult> UpSertRoleAccessOfMenus([FromBody] RoleAndMenus[] roleAndMenus)
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
                    foreach (var roleAndmenu in roleAndMenus)
                    {
                        var roleAndmenuInDb = await context.psqlUnitOfWork.RoleAndMenus.FirstOrDefaultAsync(x => x.RoleId == roleAndmenu.RoleId && x.MenuId == roleAndmenu.MenuId);

                        if (roleAndmenuInDb == null)
                        {
                            PostgreSqlModels.RoleAndMenus addRoleAndMenu = new PostgreSqlModels.RoleAndMenus()
                            {
                                UniqueId = _management.UniqueId(),
                                RoleId = roleAndmenu.RoleId,
                                MenuId = roleAndmenu.MenuId,
                                IsAccess = TrueFalse.True
                            };
                            await context.psqlUnitOfWork.RoleAndMenus.AddAsync(addRoleAndMenu);
                        }
                        else if (roleAndmenuInDb.IsAccess != roleAndmenuInDb.IsAccess)
                        {
                            await context.psqlUnitOfWork.RoleAndMenus.UpdateAsync(roleAndmenuInDb.UniqueId, async entity =>
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
                    foreach (var roleAndmenu in roleAndMenus)
                    {
                        var roleAndmenuInDb = await context.mssqlUnitOfWork.RoleAndMenus.FirstOrDefaultAsync(x => x.RoleId == roleAndmenu.RoleId && x.MenuId == roleAndmenu.MenuId);

                        if (roleAndmenuInDb == null)
                        {
                            MicrosoftSqlServerModels.RoleAndMenus addRoleAndmenu = new MicrosoftSqlServerModels.RoleAndMenus()
                            {
                                UniqueId = _management.UniqueId(),
                                RoleId = roleAndmenu.RoleId,
                                MenuId = roleAndmenu.MenuId,
                                IsAccess = TrueFalse.True
                            };
                            await context.mssqlUnitOfWork.RoleAndMenus.AddAsync(addRoleAndmenu);
                        }
                        else if (roleAndmenuInDb.IsAccess != roleAndmenuInDb.IsAccess)
                        {
                            await context.mssqlUnitOfWork.RoleAndMenus.UpdateAsync(roleAndmenuInDb.UniqueId, async entity =>
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
                    foreach (var roleAndmenu in roleAndMenus)
                    {
                        var roleAndmenuInDb = await context.mongoUnitOfWork.RoleAndMenus.FirstOrDefaultAsync(x => x.RoleId == roleAndmenu.RoleId && x.MenuId == roleAndmenu.MenuId);

                        if (roleAndmenuInDb == null)
                        {
                            MongoDbModels.RoleAndMenus addRoleAndmenu = new MongoDbModels.RoleAndMenus()
                            {
                                UniqueId = _management.UniqueId(),
                                RoleId = roleAndmenu.RoleId,
                                MenuId = roleAndmenu.MenuId,
                                IsAccess = TrueFalse.True
                            };
                            await context.mongoUnitOfWork.RoleAndMenus.AddAsync(addRoleAndmenu);
                        }
                        else if (roleAndmenuInDb.IsAccess != roleAndmenu.IsAccess)
                        {
                            await context.mongoUnitOfWork.RoleAndMenus.UpdateAsync(x => x.UniqueId == roleAndmenuInDb.UniqueId, async entity =>
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
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

    }
}
