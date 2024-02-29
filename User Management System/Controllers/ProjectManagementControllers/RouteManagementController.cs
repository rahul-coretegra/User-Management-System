using Amazon.Auth.AccessControlPolicy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.SD;

namespace User_Management_System.Controllers.ProjectManagementControllers
{
    [ApiController]
    [Route(SDRoutes.RouteManagement)]
    [Authorize(Policy = SDPolicies.SupremeAccess)]
    public class RouteManagementController : Controller
    {
        private readonly IManagementWork _management;
        private readonly IDbContextConfigurations _dbContextConfigurations;

        public RouteManagementController(IManagementWork managementWork, IDbContextConfigurations dbContextConfigurations)
        {
            _management = managementWork;
            _dbContextConfigurations = dbContextConfigurations;
        }

        [HttpGet(SDRoutes.Get)]
        public async Task<IActionResult> GetRoute(string RouteId)
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
                    var role = await context.psqlUnitOfWork.Routes.FirstOrDefaultAsync(filter: d => d.RouteId == RouteId);
                    if (role == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(role);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var role = await context.mssqlUnitOfWork.Routes.FirstOrDefaultAsync(filter: d => d.RouteId == RouteId);
                    if (role == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(role);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var role = await context.mongoUnitOfWork.Routes.FirstOrDefaultAsync(filter: d => d.RouteId == RouteId);
                    if (role == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(role);
                }
                else
                    return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpGet(SDRoutes.GetAll)]
        public async Task<IActionResult> GetAllRoutes()
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
                    var list = await context.psqlUnitOfWork.Routes.GetAllAsync();
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var list = await context.mssqlUnitOfWork.Routes.GetAllAsync();
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var list = await context.mongoUnitOfWork.Routes.GetAllAsync();
                    return Ok(list);
                }
                else return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpPost(SDRoutes.Create)]
        public async Task<IActionResult> CreateRoute([FromBody] ManagementModels.VMs.Route RouteVM)
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
                        var indb = await context.psqlUnitOfWork.Routes.FirstOrDefaultAsync(d => d.RoutePath == RouteVM.RoutePath);
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });
                        else
                        {
                            PostgreSqlModels.Route route = new PostgreSqlModels.Route()
                            {
                                RouteId = _management.UniqueId(),
                                RouteName = RouteVM.RouteName,
                                RoutePath = RouteVM.RoutePath
                            };
                            await context.psqlUnitOfWork.Routes.AddAsync(route);
                            return Ok(new { message = "Created" });
                        }
                    }

                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                    {
                        var indb = await context.mssqlUnitOfWork.Routes.FirstOrDefaultAsync(d => d.RoutePath == RouteVM.RoutePath);
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });
                        else
                        {
                            MicrosoftSqlServerModels.Route route = new MicrosoftSqlServerModels.Route()
                            {
                                RouteId = _management.UniqueId(),
                                RouteName = RouteVM.RouteName,
                                RoutePath = RouteVM.RoutePath
                            };
                            await context.mssqlUnitOfWork.Routes.AddAsync(route);
                            return Ok(new { message = "Created" });
                        }
                    }
                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                    {
                        var indb = await context.mongoUnitOfWork.Routes.FirstOrDefaultAsync(d => d.RoutePath == RouteVM.RoutePath);
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });
                        else
                        {
                            MongoDbModels.Route route = new MongoDbModels.Route()
                            {
                                RouteId = _management.UniqueId(),
                                RouteName = RouteVM.RouteName,
                                RoutePath = RouteVM.RoutePath
                            };
                            await context.mongoUnitOfWork.Routes.AddAsync(route);
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

        [HttpPut(SDRoutes.Update)]
        public async Task<IActionResult> UpdateRoute([FromBody] ManagementModels.VMs.Route RouteVM)
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
                        var indb = await context.psqlUnitOfWork.Routes.GetAsync(RouteVM.RouteId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        var indbExists = await context.psqlUnitOfWork.Routes.FirstOrDefaultAsync(d => d.RouteId != indb.RouteId && d.RoutePath == RouteVM.RoutePath);

                        if (indbExists != null)
                            return BadRequest(new { message = "Exists" });

                        await context.psqlUnitOfWork.Routes.UpdateAsync(indb.RouteId, async entity =>
                        {
                            entity.RoutePath = RouteVM.RoutePath;
                            entity.RouteName = RouteVM.RouteName;

                            await Task.CompletedTask;
                        });

                        return Ok(new { message = "Updated" });
                    }
                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                    {
                        var indb = await context.mssqlUnitOfWork.Routes.GetAsync(RouteVM.RouteId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        var indbExists = await context.mssqlUnitOfWork.Routes.FirstOrDefaultAsync(d => d.RouteId != indb.RouteId && d.RoutePath == RouteVM.RoutePath);

                        if (indbExists != null)
                            return BadRequest(new { message = "Exists" });

                        await context.mssqlUnitOfWork.Routes.UpdateAsync(indb.RouteId, async entity =>
                        {
                            entity.RoutePath = RouteVM.RoutePath;
                            entity.RouteName = RouteVM.RouteName;
                            await Task.CompletedTask;
                        });
                        return Ok(new { message = "Updated" });
                    }
                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                    {
                        var indb = await context.mongoUnitOfWork.Routes.FirstOrDefaultAsync(x => x.RouteId == RouteVM.RouteId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        var indbExists = await context.mongoUnitOfWork.Routes.FirstOrDefaultAsync(d => d.RouteId != indb.RouteId && d.RoutePath == RouteVM.RoutePath);

                        if (indbExists != null)
                            return BadRequest(new { message = "Exists" });

                        await context.mongoUnitOfWork.Routes.UpdateAsync(x => x.RouteId == indb.RouteId, async entity =>
                        {
                            entity.RoutePath = RouteVM.RoutePath;
                            entity.RouteName = RouteVM.RouteName;
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

        [HttpDelete(SDRoutes.Delete)]
        public async Task<IActionResult> DeleteRoute(string RouteId)
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
                    var indb = await context.psqlUnitOfWork.Routes.GetAsync(RouteId);
                    if (indb == null)
                        return NotFound(new { message = "Not Found" });

                    var propshave = await context.psqlUnitOfWork.RoleAndAccess.FirstOrDefaultAsync(ur => ur.RouteId == indb.RouteId && ur.IsAccess == TrueFalse.True);
                    if (propshave != null)
                        return BadRequest(new { message = "ObjectDepends" });

                    await context.psqlUnitOfWork.Routes.RemoveAsync(RouteId);
                    return Ok(new { message = "Deleted" });
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var indb = await context.mssqlUnitOfWork.Routes.GetAsync(RouteId);
                    if (indb == null)
                        return NotFound(new { message = "Not Found" });

                    var propshave = await context.mssqlUnitOfWork.RoleAndAccess.FirstOrDefaultAsync(ur => ur.RouteId == indb.RouteId && ur.IsAccess == TrueFalse.True);
                    if (propshave != null)
                        return BadRequest(new { message = "ObjectDepends" });

                    await context.mssqlUnitOfWork.Routes.RemoveAsync(RouteId);
                    return Ok(new { message = "Deleted" });
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var indb = await context.mongoUnitOfWork.Routes.FirstOrDefaultAsync(m => m.RouteId == RouteId);
                    if (indb == null)
                        return NotFound(new { message = "Not Found" });

                    var propshave = await context.mongoUnitOfWork.RoleAndAccess.FirstOrDefaultAsync(ur => ur.RouteId == indb.RouteId && ur.IsAccess == TrueFalse.True);
                    if (propshave != null)
                        return BadRequest(new { message = "ObjectDepends" });

                    await context.mongoUnitOfWork.Routes.RemoveAsync(m => m.RouteId == RouteId);
                    return Ok(new { message = "Deleted" });
                }
                else return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });

            }
        }

    }
}
