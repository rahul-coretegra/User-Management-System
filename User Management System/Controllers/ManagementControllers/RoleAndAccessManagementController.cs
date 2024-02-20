using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using System.Data;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.ManagementModels.VMs;
using User_Management_System.ManagementRepository.IManagementRepository;
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

        [HttpGet(SDRoutes.Routes)]
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

        [HttpPost(SDRoutes.CreateRoute)]
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

                    else if(projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
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

        [HttpPut(SDRoutes.UpdateRoute)]
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
                        var indb = await context.mongoUnitOfWork.Routes.FirstOrDefaultAsync(x=>x.RouteId == RouteVM.RouteId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        var indbExists = await context.mongoUnitOfWork.Routes.FirstOrDefaultAsync(d => d.RouteId != indb.RouteId && d.RoutePath == RouteVM.RoutePath);

                        if (indbExists != null)
                            return BadRequest(new { message = "Exists" });

                        await context.mongoUnitOfWork.Routes.UpdateAsync(x=>x.RouteId == indb.RouteId, async entity =>
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
        
        [HttpDelete(SDRoutes.DeleteRoute)]
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

                    var propshave = await context.psqlUnitOfWork.RoleAndAccess.FirstOrDefaultAsync(ur => ur.RouteId== indb.RouteId && ur.IsAccess == TrueFalse.True);
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
                    var indb = await context.mongoUnitOfWork.Routes.FirstOrDefaultAsync(m=>m.RouteId ==RouteId);
                    if (indb == null)
                        return NotFound(new { message = "Not Found" });

                    var propshave = await context.mongoUnitOfWork.RoleAndAccess.FirstOrDefaultAsync(ur => ur.RouteId == indb.RouteId && ur.IsAccess == TrueFalse.True);
                    if (propshave != null)
                        return BadRequest(new { message = "ObjectDepends" });

                    await context.mongoUnitOfWork.Routes.RemoveAsync(m=>m.RouteId == RouteId);
                    return Ok(new { message = "Deleted" });
                }
                else return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });

            }
        }

        [HttpGet(SDRoutes.RoleAndAccess)]
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

        [HttpGet(SDRoutes.RolesAndAccesses)]
        public async Task<IActionResult> GetAllRolesAndAccesses()
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
                    foreach ( var item in list )
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

        [HttpGet(SDRoutes.AccessByRoleId)]
        public async Task<IActionResult> AccessByRoleId(string RoleId)
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



        [HttpPost(SDRoutes.UpsertRolesAndAccesses)]
        public async Task<IActionResult> UpsertRolesAndAccesses([FromBody] RoleAndAccess[] roleAndAccesses)
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
                            await context.mongoUnitOfWork.RoleAndAccess.UpdateAsync(x=>x.UniqueId == roleAndrouteInDb.UniqueId, async entity =>
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
