using Microsoft.AspNetCore.Mvc;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.ManagementModels.VMs;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.SD;

namespace User_Management_System.Controllers.ProjectControllers
{
    [Route(SDRoutes.Service)]
    [ApiController]
    public class ServiceController : Controller
    {
        private readonly IManagementWork _management;
        private readonly IDbContextConfigurations _dbContextConfigurations;

        public ServiceController(IManagementWork managementWork, IDbContextConfigurations dbContextConfigurations)
        {
            _management = managementWork;
            _dbContextConfigurations = dbContextConfigurations;
        }

        [HttpGet(SDRoutes.Get)]
        public async Task<IActionResult> GetService(string ServiceId)
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
                    var service = await context.psqlUnitOfWork.Services.FirstOrDefaultAsync(filter: d => d.ServiceId == ServiceId);
                    if (service == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(service);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var service = await context.mssqlUnitOfWork.Services.FirstOrDefaultAsync(filter: d => d.ServiceId == ServiceId);
                    if (service == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(service);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var service = await context.mongoUnitOfWork.Services.FirstOrDefaultAsync(filter: d => d.ServiceId == ServiceId);
                    if (service == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(service);
                }
                else
                    return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpGet(SDRoutes.GetAll)]
        public async Task<IActionResult> GetAll()
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
                    var list = await context.psqlUnitOfWork.Services.GetAllAsync();
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var list = await context.mssqlUnitOfWork.Services.GetAllAsync();
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var list = await context.mongoUnitOfWork.Services.GetAllAsync();
                    return Ok(list);
                }
                else return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpPost(SDRoutes.Create)]
        public async Task<IActionResult> CreateService([FromBody] ServiceVM ServiceVM)
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
                        var indb = await context.psqlUnitOfWork.Services.FirstOrDefaultAsync(d => d.ServiceUniqueId == ServiceVM.ServiceUniqueId ||d.ServiceName == ServiceVM.ServiceName );
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });
                        else
                        {
                            var service = new PostgreSqlModels.Service()
                            {
                                ServiceId = _management.UniqueId(),
                                ServiceUniqueId = ServiceVM.ServiceUniqueId,
                                ServiceName = ServiceVM.ServiceName,
                                ServiceType = ServiceVM.ServiceType,
                                Status = ServiceVM.Status
                            };
                            await context.psqlUnitOfWork.Services.AddAsync(service);
                            return Ok(new { message = "Created" });
                        }
                    }

                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                    {
                        var indb = await context.mssqlUnitOfWork.Services.FirstOrDefaultAsync(d => d.ServiceUniqueId == ServiceVM.ServiceUniqueId || d.ServiceName == ServiceVM.ServiceName);
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });
                        else
                        {
                            var service = new MicrosoftSqlServerModels.Service()
                            {
                                ServiceId = _management.UniqueId(),
                                ServiceUniqueId = ServiceVM.ServiceUniqueId,
                                ServiceName = ServiceVM.ServiceName,
                                ServiceType = ServiceVM.ServiceType,
                                Status = ServiceVM.Status
                            };
                            await context.mssqlUnitOfWork.Services.AddAsync(service);
                            return Ok(new { message = "Created" });
                        }
                    }
                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                    {
                        var indb = await context.mongoUnitOfWork.Services.FirstOrDefaultAsync(d => d.ServiceUniqueId == ServiceVM.ServiceUniqueId || d.ServiceName == ServiceVM.ServiceName);
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });
                        else
                        {
                            var service = new MongoDbModels.Service()
                            {
                                ServiceId = _management.UniqueId(),
                                ServiceUniqueId = ServiceVM.ServiceUniqueId,
                                ServiceName = ServiceVM.ServiceName,
                                ServiceType = ServiceVM.ServiceType,
                                Status = ServiceVM.Status
                            };
                            await context.mongoUnitOfWork.Services.AddAsync(service);
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
        public async Task<IActionResult> UpdateService([FromBody] ServiceVM ServiceVM)
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
                        var indb = await context.psqlUnitOfWork.Services.FirstOrDefaultAsync(d => d.ServiceId == ServiceVM.ServiceId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });
                        var indbExists = await context.psqlUnitOfWork.Services.FirstOrDefaultAsync(d => d.ServiceId != indb.ServiceId && (d.ServiceUniqueId == ServiceVM.ServiceUniqueId || d.ServiceName == ServiceVM.ServiceName));
                        if (indbExists != null)
                            return BadRequest(new { message = "Exists" });
                        else
                        {
                            await context.psqlUnitOfWork.Services.UpdateAsync(indb.ServiceId, async entity =>
                            {
                                entity.Status = ServiceVM.Status;
                                await Task.CompletedTask;
                            });

                            return Ok(new { message = "Updated" });
                        }
                    }

                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                    {
                        var indb = await context.mssqlUnitOfWork.Services.FirstOrDefaultAsync(d => d.ServiceId == ServiceVM.ServiceId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });
                        var indbExists = await context.mssqlUnitOfWork.Services.FirstOrDefaultAsync(d => d.ServiceId != indb.ServiceId && (d.ServiceUniqueId == ServiceVM.ServiceUniqueId || d.ServiceName == ServiceVM.ServiceName));
                        if (indbExists != null)
                            return BadRequest(new { message = "Exists" });
                        else
                        {
                            await context.mssqlUnitOfWork.Services.UpdateAsync(indb.ServiceId, async entity =>
                            {
                                entity.Status = ServiceVM.Status;
                                await Task.CompletedTask;
                            });

                            return Ok(new { message = "Updated" });
                        }
                    }
                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                    {
                        var indb = await context.mongoUnitOfWork.Services.FirstOrDefaultAsync(d => d.ServiceId == ServiceVM.ServiceId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });
                        var indbExists = await context.mongoUnitOfWork.Services.FirstOrDefaultAsync(d => d.ServiceId != indb.ServiceId && (d.ServiceUniqueId == ServiceVM.ServiceUniqueId || d.ServiceName == ServiceVM.ServiceName));
                        if (indbExists != null)
                            return BadRequest(new { message = "Exists" });
                        else
                        {
                            await context.mongoUnitOfWork.Services.UpdateAsync(x=>x.ServiceId == indb.ServiceId, async entity =>
                            {
                                entity.Status = ServiceVM.Status;
                                await Task.CompletedTask;
                            });

                            return Ok(new { message = "Updated" });
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
    }
}
