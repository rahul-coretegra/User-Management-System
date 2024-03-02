using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.ManagementModels.VMs;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.SD;

namespace User_Management_System.Controllers.ProjectManagementControllers
{
    [ApiController]
    [Route(SDRoutes.RouteAccess)]
    [Authorize(Policy = SDPolicies.SupremeAccess)]
    public class RouteAccessController : Controller
    {
        private readonly IManagementWork _management;
        private readonly IDbContextConfigurations _dbContextConfigurations;

        public RouteAccessController(IManagementWork managementWork, IDbContextConfigurations dbContextConfigurations)
        {
            _management = managementWork;
            _dbContextConfigurations = dbContextConfigurations;
        }

        [HttpGet(SDRoutes.Get)]
        public async Task<IActionResult> Get(string UniqueId)
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
                    var access = await context.psqlUnitOfWork.RouteAccess.FirstOrDefaultAsync(filter: d => d.UniqueId == UniqueId, includeProperties: "UserRole,Route");
                    if (access == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(access);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var access = await context.mssqlUnitOfWork.RouteAccess.FirstOrDefaultAsync(filter: d => d.UniqueId == UniqueId, includeProperties: "UserRole,Route");
                    if (access == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(access);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var access = await context.mongoUnitOfWork.RouteAccess.FirstOrDefaultAsync(filter: d => d.UniqueId == UniqueId);

                    if (access == null)
                        return NotFound(new { message = "NotFound" });

                    MongoRouteAccess roleAndAccess = new MongoRouteAccess()
                    {
                        Id = access.Id,
                        UniqueId = access.UniqueId,
                        RoleId = access.RoleId,
                        UserRole = await context.mongoUnitOfWork.UserRoles.FirstOrDefaultAsync(filter: d => d.RoleId == access.RoleId),
                        RouteId = access.RouteId,
                        Route = await context.mongoUnitOfWork.Routes.FirstOrDefaultAsync(filter: d => d.RouteId == access.RouteId),
                        IsAccess = access.IsAccess
                    };
                    return Ok(roleAndAccess);
                }
                else return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpGet(SDRoutes.GetAll)]
        public async Task<IActionResult> GetAll(string RoleId)
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
                    if (RoleId == null)
                        return Ok(await context.psqlUnitOfWork.RouteAccess.GetAllAsync(includeProperties: "UserRole,Route"));
                    else
                        return Ok(await context.psqlUnitOfWork.RouteAccess.GetAllAsync(ma => ma.RoleId == RoleId && ma.IsAccess == TrueFalse.True, includeProperties: "UserRole,Route"));

                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    if (RoleId == null)
                        return Ok(await context.mssqlUnitOfWork.RouteAccess.GetAllAsync(includeProperties: "UserRole,Route"));
                    else
                        return Ok(await context.mssqlUnitOfWork.RouteAccess.GetAllAsync(ma => ma.RoleId == RoleId && ma.IsAccess == TrueFalse.True, includeProperties: "UserRole,Route"));

                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {

                    if (RoleId == null)
                    {
                        var list = await context.mongoUnitOfWork.RouteAccess.GetAllAsync();
                        List<MongoRouteAccess> items = new List<MongoRouteAccess>();
                        foreach (var item in list)
                        {
                            MongoRouteAccess roleAndAccess = new MongoRouteAccess()
                            {
                                Id = item.Id,
                                UniqueId = item.UniqueId,
                                RoleId = item.RoleId,
                                UserRole = await context.mongoUnitOfWork.UserRoles.FirstOrDefaultAsync(filter: d => d.RoleId == item.RoleId),
                                RouteId = item.RouteId,
                                Route = await context.mongoUnitOfWork.Routes.FirstOrDefaultAsync(filter: d => d.RouteId == item.RouteId),
                                IsAccess = item.IsAccess
                            };
                            items.Add(roleAndAccess);
                        }

                        return Ok(items);
                    }
                    else
                    {
                        var list = (await context.mongoUnitOfWork.RouteAccess.GetAllAsync(r => r.RoleId == RoleId && r.IsAccess == TrueFalse.True)).ToList();

                        List<MongoRouteAccess> items = new List<MongoRouteAccess>();
                        foreach (var item in list)
                        {

                            MongoRouteAccess roleAndRoute = new MongoRouteAccess()
                            {
                                Id = item.Id,
                                UniqueId = item.UniqueId,
                                RoleId = item.RoleId,
                                RouteId = item.RouteId,
                                Route = await context.mongoUnitOfWork.Routes.FirstOrDefaultAsync(filter: d => d.RouteId == item.RouteId),
                                IsAccess = item.IsAccess
                            };
                            items.Add(roleAndRoute);
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

        [HttpPost(SDRoutes.UpSertRouteAccess)]
        public async Task<IActionResult> UpSertRoleAccessOfRoutes([FromBody] RouteAccess[] rolesAndRoutes)
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
                    foreach (var roleAndroute in rolesAndRoutes)
                    {
                        var roleAndrouteInDb = await context.psqlUnitOfWork.RouteAccess.FirstOrDefaultAsync(x => x.RoleId == roleAndroute.RoleId && x.RouteId == roleAndroute.RouteId);

                        if (roleAndrouteInDb == null)
                        {
                            PostgreSqlModels.RouteAccess addRoleAndroute = new PostgreSqlModels.RouteAccess()
                            {
                                UniqueId = _management.UniqueId(),
                                RoleId = roleAndroute.RoleId,
                                RouteId = roleAndroute.RouteId,
                                IsAccess = TrueFalse.True
                            };
                            await context.psqlUnitOfWork.RouteAccess.AddAsync(addRoleAndroute);
                        }
                        else if (roleAndrouteInDb.IsAccess != roleAndroute.IsAccess)
                        {
                            await context.psqlUnitOfWork.RouteAccess.UpdateAsync(roleAndrouteInDb.UniqueId, async entity =>
                            {
                                entity.IsAccess = roleAndroute.IsAccess;
                                await Task.CompletedTask;
                            });
                        }
                    }
                    return Ok(new { message = "Ok" });
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    foreach (var roleAndroute in rolesAndRoutes)
                    {
                        var roleAndrouteInDb = await context.mssqlUnitOfWork.RouteAccess.FirstOrDefaultAsync(x => x.RoleId == roleAndroute.RoleId && x.RouteId == roleAndroute.RouteId);

                        if (roleAndrouteInDb == null)
                        {
                            MicrosoftSqlServerModels.RouteAccess addRoleAndroute = new MicrosoftSqlServerModels.RouteAccess()
                            {
                                UniqueId = _management.UniqueId(),
                                RoleId = roleAndroute.RoleId,
                                RouteId = roleAndroute.RouteId,
                                IsAccess = TrueFalse.True
                            };
                            await context.mssqlUnitOfWork.RouteAccess.AddAsync(addRoleAndroute);
                        }
                        else if (roleAndrouteInDb.IsAccess != roleAndroute.IsAccess)
                        {
                            await context.mssqlUnitOfWork.RouteAccess.UpdateAsync(roleAndrouteInDb.UniqueId, async entity =>
                            {
                                entity.IsAccess = roleAndroute.IsAccess;
                                await Task.CompletedTask;
                            });
                        }
                    }
                    return Ok(new { message = "Ok" });
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    foreach (var roleAndroute in rolesAndRoutes)
                    {
                        var roleAndrouteInDb = await context.mongoUnitOfWork.RouteAccess.FirstOrDefaultAsync(x => x.RoleId == roleAndroute.RoleId && x.RouteId == roleAndroute.RouteId);

                        if (roleAndrouteInDb == null)
                        {
                            MongoDbModels.RouteAccess addRoleAndroute = new MongoDbModels.RouteAccess()
                            {
                                UniqueId = _management.UniqueId(),
                                RoleId = roleAndroute.RoleId,
                                RouteId = roleAndroute.RouteId,
                                IsAccess = TrueFalse.True
                            };
                            await context.mongoUnitOfWork.RouteAccess.AddAsync(addRoleAndroute);
                        }
                        else if (roleAndrouteInDb.IsAccess != roleAndroute.IsAccess)
                        {
                            await context.mongoUnitOfWork.RouteAccess.UpdateAsync(x => x.UniqueId == roleAndrouteInDb.UniqueId, async entity =>
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }

}
