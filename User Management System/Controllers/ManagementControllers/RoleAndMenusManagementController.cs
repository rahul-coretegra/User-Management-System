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

        [HttpGet(SDRoutes.Menu)]
        public async Task<IActionResult> GetMenu(string MenuId)
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
                    var menu = await context.psqlUnitOfWork.Menus.FirstOrDefaultAsync(filter: d => d.MenuId == MenuId);
                    if (menu == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(menu);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var menu = await context.mssqlUnitOfWork.Menus.FirstOrDefaultAsync(filter: d => d.MenuId == MenuId);
                    if (menu == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(menu);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var role = await context.mongoUnitOfWork.Menus.FirstOrDefaultAsync(filter: d => d.MenuId == MenuId);
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

        [HttpGet(SDRoutes.Menus)]
        public async Task<IActionResult> GetAllMenus()
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
                    var list = await context.psqlUnitOfWork.Menus.GetAllAsync();
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var list = await context.mssqlUnitOfWork.Menus.GetAllAsync();
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var list = await context.mongoUnitOfWork.Menus.GetAllAsync();
                    return Ok(list);
                }
                else return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpGet(SDRoutes.MenusByParentId)]
        public async Task<IActionResult> MenusByParent(string ParentId)
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
                    var list = await context.psqlUnitOfWork.Menus.GetAllAsync(x => x.ParentId == ParentId);
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var list = await context.mssqlUnitOfWork.Menus.GetAllAsync(x => x.ParentId == ParentId);
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var list = await context.mongoUnitOfWork.Menus.GetAllAsync(x => x.ParentId == ParentId);
                    return Ok(list);
                }
                else return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpGet(SDRoutes.MenusWithSubMenus)]
        public async Task<IActionResult> MenusWithSubMenus()
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
                    var allMenus = (await context.psqlUnitOfWork.Menus.GetAllAsync()).ToList();
                    var result = allMenus
                        .Where(menu => menu.ParentId == null)
                        .Select(mainMenu => context.psqlUnitOfWork.Menus.GetMenuWithSubmenus(mainMenu, allMenus))
                        .ToList();
                    return Ok(result);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var allMenus = (await context.mssqlUnitOfWork.Menus.GetAllAsync()).ToList();
                    var result = allMenus
                        .Where(menu => menu.ParentId == null)
                        .Select(mainMenu => context.mssqlUnitOfWork.Menus.GetMenuWithSubmenus(mainMenu, allMenus))
                        .ToList();
                    return Ok(result);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var allMenus = (await context.mongoUnitOfWork.Menus.GetAllAsync()).ToList();
                    var result = allMenus
                        .Where(menu => menu.ParentId == null)
                        .Select(mainMenu => context.mongoUnitOfWork.Menus.GetMenuWithSubmenus(mainMenu, allMenus))
                        .ToList();
                    return Ok(result);
                }
                else return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpPost(SDRoutes.CreateMenu)]
        public async Task<IActionResult> CreateMenu([FromBody] Menu VM)
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
                        var indb = await context.psqlUnitOfWork.Menus.FirstOrDefaultAsync();
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });
                        else
                        {
                            PostgreSqlModels.Menu menu = new PostgreSqlModels.Menu()
                            {
                                MenuId = _management.UniqueId(),
                                MenuName = VM.MenuName,
                                MenuPath = VM.MenuPath,
                                MenuIcon = VM.MenuIcon,
                                ParentId = VM.ParentId,
                                Status = TrueFalse.True,
                            };
                            await context.psqlUnitOfWork.Menus.AddAsync(menu);
                            return Ok(new { message = "Created" });
                        }
                    }

                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                    {
                        var indb = await context.mssqlUnitOfWork.Menus.FirstOrDefaultAsync(d => d.MenuPath == VM.MenuPath && d.ParentId == VM.ParentId);
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });
                        else
                        {
                            MicrosoftSqlServerModels.Menu menu = new MicrosoftSqlServerModels.Menu()
                            {
                                MenuId = _management.UniqueId(),
                                MenuName = VM.MenuName,
                                MenuPath = VM.MenuPath,
                                MenuIcon = VM.MenuIcon,
                                ParentId = VM.ParentId,
                                Status = TrueFalse.True,
                            };
                            await context.mssqlUnitOfWork.Menus.AddAsync(menu);
                            return Ok(new { message = "Created" });
                        }
                    }
                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                    {
                        var indb = await context.mongoUnitOfWork.Menus.FirstOrDefaultAsync(d => d.MenuPath == VM.MenuPath && d.ParentId == VM.ParentId);
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });
                        else
                        {
                            MongoDbModels.Menu menu = new MongoDbModels.Menu()
                            {
                                MenuId = _management.UniqueId(),
                                MenuName = VM.MenuName,
                                MenuPath = VM.MenuPath,
                                MenuIcon = VM.MenuIcon,
                                ParentId = VM.ParentId,
                                Status = TrueFalse.True,
                            };
                            await context.mongoUnitOfWork.Menus.AddAsync(menu);
                            return Ok(new { message = "Created" });
                        }
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

        [HttpPut(SDRoutes.UpdateMenu)]
        public async Task<IActionResult> UpdateMenu([FromBody] Menu VM)
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
                        var indb = await context.psqlUnitOfWork.Menus.GetAsync(VM.MenuId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        var indbExists = await context.psqlUnitOfWork.Menus.FirstOrDefaultAsync(d => (d.MenuId != indb.MenuId) && d.MenuPath == VM.MenuPath && d.ParentId == VM.ParentId);

                        if (indbExists != null)
                            return BadRequest(new { message = "Exists" });

                        await context.psqlUnitOfWork.Menus.UpdateAsync(indb.MenuId, async entity =>
                        {
                            entity.MenuName = VM.MenuName;
                            entity.MenuPath = VM.MenuPath;
                            entity.MenuIcon = VM.MenuIcon;
                            entity.ParentId = VM.ParentId;
                            entity.Status = TrueFalse.True;
                            await Task.CompletedTask;
                        });

                        return Ok(new { message = "Updated" });
                    }
                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                    {
                        var indb = await context.mssqlUnitOfWork.Menus.GetAsync(VM.MenuId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        var indbExists = await context.mssqlUnitOfWork.Menus.FirstOrDefaultAsync(d => (d.MenuId != indb.MenuId) && d.MenuPath == VM.MenuPath && d.ParentId == VM.ParentId);

                        if (indbExists != null)
                            return BadRequest(new { message = "Exists" });

                        await context.mssqlUnitOfWork.Menus.UpdateAsync(indb.MenuId, async entity =>
                        {
                            entity.MenuName = VM.MenuName;
                            entity.MenuPath = VM.MenuPath;
                            entity.MenuIcon = VM.MenuIcon;
                            entity.ParentId = VM.ParentId;
                            entity.Status = TrueFalse.True;
                            await Task.CompletedTask;
                        });
                        return Ok(new { message = "Updated" });
                    }
                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                    {
                        var indb = await context.mongoUnitOfWork.Menus.FirstOrDefaultAsync(x => x.MenuId == VM.MenuId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        var indbExists = await context.mongoUnitOfWork.Menus.FirstOrDefaultAsync(d => (d.MenuId != indb.MenuId) && d.MenuPath == VM.MenuPath && d.ParentId == VM.ParentId);

                        if (indbExists != null)
                            return BadRequest(new { message = "Exists" });

                        await context.mongoUnitOfWork.Menus.UpdateAsync(x => x.MenuId == indb.MenuId, async entity =>
                        {
                            entity.MenuName = VM.MenuName;
                            entity.MenuPath = VM.MenuPath;
                            entity.MenuIcon = VM.MenuIcon;
                            entity.ParentId = VM.ParentId;
                            entity.Status = TrueFalse.True;
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

        [HttpDelete(SDRoutes.DeleteMenu)]
        public async Task<IActionResult> DeleteMenu(string MenuId)
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
                    var indb = await context.psqlUnitOfWork.Menus.GetAsync(MenuId);
                    if (indb == null)
                        return NotFound(new { message = "Not Found" });

                    var propshave = await context.psqlUnitOfWork.RoleAndMenus.FirstOrDefaultAsync(ur => ur.MenuId == indb.MenuId && ur.IsAccess == TrueFalse.True);
                    if (propshave != null)
                        return BadRequest(new { message = "ObjectDepends" });

                    await context.psqlUnitOfWork.Menus.RemoveAsync(MenuId);
                    return Ok(new { message = "Deleted" });
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var indb = await context.mssqlUnitOfWork.Menus.GetAsync(MenuId);
                    if (indb == null)
                        return NotFound(new { message = "Not Found" });

                    var propshave = await context.mssqlUnitOfWork.RoleAndMenus.FirstOrDefaultAsync(ur => ur.MenuId == indb.MenuId && ur.IsAccess == TrueFalse.True);
                    if (propshave != null)
                        return BadRequest(new { message = "ObjectDepends" });

                    await context.mssqlUnitOfWork.Menus.RemoveAsync(MenuId);
                    return Ok(new { message = "Deleted" });
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var indb = await context.mongoUnitOfWork.Menus.FirstOrDefaultAsync(m => m.MenuId == MenuId);
                    if (indb == null)
                        return NotFound(new { message = "Not Found" });

                    var propshave = await context.mongoUnitOfWork.RoleAndMenus.FirstOrDefaultAsync(ur => ur.MenuId == indb.MenuId && ur.IsAccess == TrueFalse.True);
                    if (propshave != null)
                        return BadRequest(new { message = "ObjectDepends" });

                    await context.mongoUnitOfWork.Menus.RemoveAsync(m => m.MenuId == indb.MenuId);
                    return Ok(new { message = "Deleted" });
                }
                else return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });

            }
        }

        

       
        [HttpGet(SDRoutes.RoleAndMenu)]
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

        [HttpGet(SDRoutes.RolesAndMenus)]
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

        [HttpGet(SDRoutes.MenusByRoleId)]
        public async Task<IActionResult> MenusByRoleId(string RoleId)
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



        [HttpPost(SDRoutes.UpsertRolesAndMenus)]
        public async Task<IActionResult> UpsertRolesAndMenus([FromBody] RoleAndMenus[] roleAndMenus)
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
