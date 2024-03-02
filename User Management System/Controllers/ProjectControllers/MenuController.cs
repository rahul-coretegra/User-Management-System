
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.ManagementModels.VMs;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.SD;

namespace User_Management_System.Controllers.ProjectManagementControllers
{
    [ApiController]
    [Route(SDRoutes.Menu)]
    [Authorize(Policy = SDPolicies.SupremeAccess)]
    public class MenuController : Controller
    {
        private readonly IManagementWork _management;
        private readonly IDbContextConfigurations _dbContextConfigurations;

        public MenuController(IManagementWork managementWork, IDbContextConfigurations dbContextConfigurations)
        {
            _management = managementWork;
            _dbContextConfigurations = dbContextConfigurations;
        }

        [HttpGet(SDRoutes.Get)]
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpGet(SDRoutes.GetAll)]
        public async Task<IActionResult> GetAllMenus(string ParentId)
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
                    if (ParentId == null)
                        return Ok(await context.psqlUnitOfWork.Menus.GetAllAsync());
                    else
                        return Ok(await context.psqlUnitOfWork.Menus.GetAllAsync(m=>m.ParentId == ParentId));

                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    if (ParentId == null)
                        return Ok(await context.psqlUnitOfWork.Menus.GetAllAsync());
                    else
                        return Ok(await context.psqlUnitOfWork.Menus.GetAllAsync(m => m.ParentId == ParentId));

                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    if (ParentId == null)
                        return Ok(await context.psqlUnitOfWork.Menus.GetAllAsync());
                    else
                        return Ok(await context.psqlUnitOfWork.Menus.GetAllAsync(m => m.ParentId == ParentId));
                }
                else return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpGet(SDRoutes.MenusWithSubMenus)]
        public async Task<IActionResult> GetMenusWithSubMenus()
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpPost(SDRoutes.Create)]
        public async Task<IActionResult> CreateMenu([FromBody] MenuVM VM)
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
                        var indb = await context.psqlUnitOfWork.Menus.FirstOrDefaultAsync(m=>m.MenuPath == VM.MenuPath && m.ParentId == VM.ParentId);
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpPut(SDRoutes.Update)]
        public async Task<IActionResult> UpdateMenu([FromBody] MenuVM VM)
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

                        var indbExists = await context.psqlUnitOfWork.Menus.FirstOrDefaultAsync(d => d.MenuId != indb.MenuId && d.MenuPath == VM.MenuPath && d.ParentId == VM.ParentId);

                        if (indbExists != null)
                            return BadRequest(new { message = "Exists" });

                        await context.psqlUnitOfWork.Menus.UpdateAsync(indb.MenuId, async entity =>
                        {
                            entity.MenuName = VM.MenuName;
                            entity.MenuPath = VM.MenuPath;
                            entity.MenuIcon = VM.MenuIcon;
                            entity.ParentId = VM.ParentId;
                            entity.Status = VM.Status;
                            await Task.CompletedTask;
                        });

                        return Ok(new { message = "Updated" });
                    }
                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                    {
                        var indb = await context.mssqlUnitOfWork.Menus.GetAsync(VM.MenuId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        var indbExists = await context.mssqlUnitOfWork.Menus.FirstOrDefaultAsync(d => d.MenuId != indb.MenuId && d.MenuPath == VM.MenuPath && d.ParentId == VM.ParentId);

                        if (indbExists != null)
                            return BadRequest(new { message = "Exists" });

                        await context.mssqlUnitOfWork.Menus.UpdateAsync(indb.MenuId, async entity =>
                        {
                            entity.MenuName = VM.MenuName;
                            entity.MenuPath = VM.MenuPath;
                            entity.MenuIcon = VM.MenuIcon;
                            entity.ParentId = VM.ParentId;
                            entity.Status = VM.Status;
                            await Task.CompletedTask;
                        });
                        return Ok(new { message = "Updated" });
                    }
                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                    {
                        var indb = await context.mongoUnitOfWork.Menus.FirstOrDefaultAsync(x => x.MenuId == VM.MenuId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        var indbExists = await context.mongoUnitOfWork.Menus.FirstOrDefaultAsync(d => d.MenuId != indb.MenuId && d.MenuPath == VM.MenuPath && d.ParentId == VM.ParentId);

                        if (indbExists != null)
                            return BadRequest(new { message = "Exists" });

                        await context.mongoUnitOfWork.Menus.UpdateAsync(x => x.MenuId == indb.MenuId, async entity =>
                        {
                            entity.MenuName = VM.MenuName;
                            entity.MenuPath = VM.MenuPath;
                            entity.MenuIcon = VM.MenuIcon;
                            entity.ParentId = VM.ParentId;
                            entity.Status = VM.Status;
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //[HttpDelete(SDRoutes.Delete)]
        //public async Task<IActionResult> DeleteMenu(string MenuId)
        //{
        //    try
        //    {
        //        HttpContext.Request.Headers.TryGetValue("projectUniqueId", out var projectUniqueId);
        //        var projectInDb = await _management.Projects.FirstOrDefaultAsync(p => p.ProjectUniqueId == projectUniqueId.ToString());
        //        if (projectInDb == null)
        //            return NotFound();
        //        var context = _dbContextConfigurations.configureDbContexts(projectInDb);

        //        if (projectInDb.TypeOfDatabase == TypeOfDatabase.PostgreSql)
        //        {
        //            var indb = await context.psqlUnitOfWork.Menus.GetAsync(MenuId);
        //            if (indb == null)
        //                return NotFound(new { message = "Not Found" });

        //            var propshave = await context.psqlUnitOfWork.MenuAccess.FirstOrDefaultAsync(ur => ur.MenuId == indb.MenuId);
        //            if (propshave != null)
        //                return BadRequest(new { message = "ObjectDepends" });

        //            await context.psqlUnitOfWork.Menus.RemoveAsync(MenuId);
        //            return Ok(new { message = "Deleted" });
        //        }
        //        else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
        //        {
        //            var indb = await context.mssqlUnitOfWork.Menus.GetAsync(MenuId);
        //            if (indb == null)
        //                return NotFound(new { message = "Not Found" });

        //            var propshave = await context.mssqlUnitOfWork.MenuAccess.FirstOrDefaultAsync(ur => ur.MenuId == indb.MenuId);
        //            if (propshave != null)
        //                return BadRequest(new { message = "ObjectDepends" });

        //            await context.mssqlUnitOfWork.Menus.RemoveAsync(MenuId);
        //            return Ok(new { message = "Deleted" });
        //        }
        //        else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
        //        {
        //            var indb = await context.mongoUnitOfWork.Menus.FirstOrDefaultAsync(m => m.MenuId == MenuId);
        //            if (indb == null)
        //                return NotFound(new { message = "Not Found" });

        //            var propshave = await context.mongoUnitOfWork.MenuAccess.FirstOrDefaultAsync(ur => ur.MenuId == indb.MenuId );
        //            if (propshave != null)
        //                return BadRequest(new { message = "ObjectDepends" });

        //            await context.mongoUnitOfWork.Menus.RemoveAsync(m => m.MenuId == indb.MenuId);
        //            return Ok(new { message = "Deleted" });
        //        }
        //        else return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message, ex);
        //    }
        //}
    }
}
