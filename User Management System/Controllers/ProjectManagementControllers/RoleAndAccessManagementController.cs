using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.ManagementModels.VMs;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.SD;

namespace User_Management_System.Controllers.ProjectManagementControllers
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

        [HttpGet(SDRoutes.Get)]
        public async Task<IActionResult> GetRoleAndAccess(string UniqueId)
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
                    var role = await context.psqlUnitOfWork.RoleAndAccess.FirstOrDefaultAsync(filter: d => d.UniqueId == UniqueId, includeProperties: "UserRole,Route");
                    if (role == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(role);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var role = await context.mssqlUnitOfWork.RoleAndAccess.FirstOrDefaultAsync(filter: d => d.UniqueId == UniqueId, includeProperties: "UserRole,Route");
                    if (role == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(role);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var role = await context.mongoUnitOfWork.RoleAndAccess.FirstOrDefaultAsync(filter: d => d.UniqueId == UniqueId);

                    if (role == null)
                        return NotFound(new { message = "NotFound" });
                    var userrole = await context.mongoUnitOfWork.UserRoles.FirstOrDefaultAsync(filter: d => d.RoleId == role.RoleId);
                    var route = await context.mongoUnitOfWork.Routes.FirstOrDefaultAsync(filter: d => d.RouteId == role.RouteId);

                    MongoRoleAndAccess roleAndAccess = new MongoRoleAndAccess()
                    {
                        Id = role.Id,
                        UniqueId = role.UniqueId,
                        RoleId = role.RoleId,
                        UserRole = userrole,
                        RouteId = role.RouteId,
                        Route = route,
                        IsAccess = role.IsAccess
                    };
                    return Ok(roleAndAccess);
                }
                else return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpGet(SDRoutes.GetAll)]
        public async Task<IActionResult> GetAllRolesAndAccess()
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
                    var list = await context.psqlUnitOfWork.RoleAndAccess.GetAllAsync(includeProperties: "UserRole,Route");
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var list = await context.mssqlUnitOfWork.RoleAndAccess.GetAllAsync(includeProperties: "UserRole,Route");
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var list = await context.mongoUnitOfWork.RoleAndAccess.GetAllAsync();
                    List<MongoRoleAndAccess> items = new List<MongoRoleAndAccess>();
                    foreach (var item in list)
                    {
                        var userrole = await context.mongoUnitOfWork.UserRoles.FirstOrDefaultAsync(filter: d => d.RoleId == item.RoleId);
                        var route = await context.mongoUnitOfWork.Routes.FirstOrDefaultAsync(filter: d => d.RouteId == item.RouteId);

                        MongoRoleAndAccess roleAndAccess = new MongoRoleAndAccess()
                        {
                            Id = item.Id,
                            UniqueId = item.UniqueId,
                            RoleId = item.RoleId,
                            UserRole = userrole,
                            RouteId = item.RouteId,
                            Route = route,
                            IsAccess = item.IsAccess
                        };
                        items.Add(roleAndAccess);
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
        public async Task<IActionResult> GetAccessByRoleId(string RoleId)
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
                    var roleAndRoutes = (await context.psqlUnitOfWork.RoleAndAccess.GetAllAsync(r => r.RoleId == RoleId && r.IsAccess == TrueFalse.True, includeProperties: "Route,UserRole")).ToList();
                    return Ok(roleAndRoutes);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var roleAndRoutes = (await context.mssqlUnitOfWork.RoleAndAccess.GetAllAsync(r => r.RoleId == RoleId && r.IsAccess == TrueFalse.True, includeProperties: "Route,UserRole")).ToList();
                    return Ok(roleAndRoutes);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var roleAndRoutes = (await context.mongoUnitOfWork.RoleAndAccess.GetAllAsync(r => r.RoleId == RoleId && r.IsAccess == TrueFalse.True)).ToList();

                    List<MongoRoleAndAccess> items = new List<MongoRoleAndAccess>();
                    foreach (var item in roleAndRoutes)
                    {
                        var route = await context.mongoUnitOfWork.Routes.FirstOrDefaultAsync(filter: d => d.RouteId == item.RouteId);

                        MongoRoleAndAccess roleAndRoute = new MongoRoleAndAccess()
                        {
                            Id = item.Id,
                            UniqueId = item.UniqueId,
                            RoleId = item.RoleId,
                            RouteId = item.RouteId,
                            Route = route,
                            IsAccess = item.IsAccess
                        };
                        items.Add(roleAndRoute);
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
        public async Task<IActionResult> UpSertRoleAccessOfRoutes([FromBody] RoleAndAccess[] roleAndAccesses)
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
                    foreach (var roleAndroute in roleAndAccesses)
                    {
                        var roleAndrouteInDb = await context.psqlUnitOfWork.RoleAndAccess.FirstOrDefaultAsync(x => x.RoleId == roleAndroute.RoleId && x.RouteId == roleAndroute.RouteId);

                        if (roleAndrouteInDb == null)
                        {
                            PostgreSqlModels.RoleAndAccess addRoleAndroute = new PostgreSqlModels.RoleAndAccess()
                            {
                                UniqueId = _management.UniqueId(),
                                RoleId = roleAndroute.RoleId,
                                RouteId = roleAndroute.RouteId,
                                IsAccess = TrueFalse.True
                            };
                            await context.psqlUnitOfWork.RoleAndAccess.AddAsync(addRoleAndroute);
                        }
                        else if (roleAndrouteInDb.IsAccess != roleAndroute.IsAccess)
                        {
                            await context.psqlUnitOfWork.RoleAndAccess.UpdateAsync(roleAndrouteInDb.UniqueId, async entity =>
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
                    foreach (var roleAndroute in roleAndAccesses)
                    {
                        var roleAndrouteInDb = await context.mssqlUnitOfWork.RoleAndAccess.FirstOrDefaultAsync(x => x.RoleId == roleAndroute.RoleId && x.RouteId == roleAndroute.RouteId);

                        if (roleAndrouteInDb == null)
                        {
                            MicrosoftSqlServerModels.RoleAndAccess addRoleAndroute = new MicrosoftSqlServerModels.RoleAndAccess()
                            {
                                UniqueId = _management.UniqueId(),
                                RoleId = roleAndroute.RoleId,
                                RouteId = roleAndroute.RouteId,
                                IsAccess = TrueFalse.True
                            };
                            await context.mssqlUnitOfWork.RoleAndAccess.AddAsync(addRoleAndroute);
                        }
                        else if (roleAndrouteInDb.IsAccess != roleAndroute.IsAccess)
                        {
                            await context.mssqlUnitOfWork.RoleAndAccess.UpdateAsync(roleAndrouteInDb.UniqueId, async entity =>
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
                    foreach (var roleAndroute in roleAndAccesses)
                    {
                        var roleAndrouteInDb = await context.mongoUnitOfWork.RoleAndAccess.FirstOrDefaultAsync(x => x.RoleId == roleAndroute.RoleId && x.RouteId == roleAndroute.RouteId);

                        if (roleAndrouteInDb == null)
                        {
                            MongoDbModels.RoleAndAccess addRoleAndroute = new MongoDbModels.RoleAndAccess()
                            {
                                UniqueId = _management.UniqueId(),
                                RoleId = roleAndroute.RoleId,
                                RouteId = roleAndroute.RouteId,
                                IsAccess = TrueFalse.True
                            };
                            await context.mongoUnitOfWork.RoleAndAccess.AddAsync(addRoleAndroute);
                        }
                        else if (roleAndrouteInDb.IsAccess != roleAndroute.IsAccess)
                        {
                            await context.mongoUnitOfWork.RoleAndAccess.UpdateAsync(x => x.UniqueId == roleAndrouteInDb.UniqueId, async entity =>
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
