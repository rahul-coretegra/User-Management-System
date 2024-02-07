using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.ManagementModels.VMs;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.PostgreSqlModels;
using User_Management_System.PostgreSqlModels.SupremeModels;
using User_Management_System.SD;

namespace User_Management_System.Controllers.ManagementControllers
{
    [ApiController]
    [Route(SDRoutes.RoleAndAccessManagement)]
    [Authorize(Policy = SDPolicies.SupremeAccess)]
    public class RoleAndAccessManagementController : Controller
    {
        private readonly IManagementWork _management;
        private readonly IDbContextConfigurations _dbContextConfigurations;

        public RoleAndAccessManagementController(IManagementWork managementWork, IDbContextConfigurations dbContextConfigurations)
        {
            _management = managementWork;
            _dbContextConfigurations = dbContextConfigurations;
        }

        [HttpGet(SDRoutes.Route)]
        public async Task<IActionResult> GetRoute(string RouteId)
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
                    var role = await context.PsqlUOW.Routes.FirstOrDefaultAsync(filter: d => d.RouteId == RouteId);
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

        [HttpGet(SDRoutes.Routes)]
        public async Task<IActionResult> GetAllRoutes()
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
                    var list = await context.PsqlUOW.Routes.GetAllAsync();
                    return Ok(list);
                }
                else return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }


        [HttpPost(SDRoutes.CreateRoute)]
        public async Task<IActionResult> CreateRoute([FromBody] RouteVM RouteVM)
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
                        var indb = await context.PsqlUOW.Routes.FirstOrDefaultAsync(d => d.RoutePath == RouteVM.RoutePath);
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
                            await context.PsqlUOW.Routes.AddAsync(route);
                            return Ok(new { message = "Created" });
                        }
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

        [HttpPut(SDRoutes.UpdateRoute)]
        public async Task<IActionResult> UpdateRoute([FromBody] RouteVM RouteVM)
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
                        var indb = await context.PsqlUOW.Routes.GetAsync(RouteVM.RouteId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        var indbExists = await context.PsqlUOW.Routes.FirstOrDefaultAsync(d => d.RouteId != RouteVM.RouteId && d.RoutePath == RouteVM.RoutePath);

                        if (indbExists != null)
                            return BadRequest(new { message = "Exists" });

                        await context.PsqlUOW.Routes.UpdateAsync(RouteVM.RouteId, async entity =>
                        {
                            entity.RoutePath = RouteVM.RoutePath;
                            entity.RouteName = RouteVM.RouteName;
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


        
        [HttpDelete(SDRoutes.DeleteRoute)]
        public async Task<IActionResult> DeleteRoute(string RouteId)
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
                    var indb = await context.PsqlUOW.Routes.GetAsync(RouteId);
                    if (indb == null)
                        return NotFound(new { message = "Not Found" });

                    var propshave = await context.PsqlUOW.RoleAndAccess.FirstOrDefaultAsync(ur => ur.RouteId== indb.RouteId && ur.IsAccess == TrueFalse.True);
                    if (propshave != null)
                        return BadRequest(new { message = "ObjectDepends" });

                    await context.PsqlUOW.Routes.RemoveAsync(RouteId);
                    return Ok(new { message = "Deleted" });
                }
                else return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });

            }
        }

        [HttpGet(SDRoutes.RoleAndAccesses)]
        public async Task<IActionResult> GetRoleAndAccesses(string UniqueId)
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
                    var role = await context.PsqlUOW.RoleAndAccess.FirstOrDefaultAsync(filter: d => d.UniqueId == UniqueId, includeProperties: "UserRole,Route");
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


        [HttpGet(SDRoutes.RolesAndAccesses)]
        public async Task<IActionResult> GetAllRolesAndAccesses()
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
                    var list = await context.PsqlUOW.RoleAndAccess.GetAllAsync(includeProperties: "UserRole,Route");
                    return Ok(list);
                }
                else return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }


        [HttpPost(SDRoutes.UpsertRolesAndAccesses)]
        public async Task<IActionResult> UpsertRolesAndAccesses([FromBody] RoleAndAccessVM[] roleAndAccesses)
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
                    foreach (var roleAndroute in roleAndAccesses)
                    {
                        var roleAndrouteInDb = await context.PsqlUOW.RoleAndAccess.FirstOrDefaultAsync(x => x.RoleId == roleAndroute.RoleId && x.RouteId == roleAndroute.RouteId);

                        if (roleAndrouteInDb == null)
                        {
                            RoleAndAccess addRoleAndroute = new RoleAndAccess()
                            {
                                UniqueId = _management.UniqueId(),
                                RoleId = roleAndroute.RoleId,
                                RouteId = roleAndroute.RouteId,
                                IsAccess = TrueFalse.True
                            };
                            await context.PsqlUOW.RoleAndAccess.AddAsync(addRoleAndroute);
                        }
                        else if (roleAndrouteInDb.IsAccess != roleAndroute.IsAccess)
                        {
                            await context.PsqlUOW.RoleAndAccess.UpdateAsync(roleAndrouteInDb.UniqueId, async entity =>
                            {
                                entity.IsAccess = roleAndroute.IsAccess;
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
