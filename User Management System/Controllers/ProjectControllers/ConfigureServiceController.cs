using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.SD;
using User_Management_System.ManagementModels.VMs;
using User_Management_System.ManagementModels.EnumModels;


namespace User_Management_System.Controllers.ProjectManagementControllers
{
    [ApiController]
    [Route(SDRoutes.ConfigureService)]
    [Authorize(Policy = SDPolicies.SupremeAccess)]
    public class ConfigureServiceController : Controller
    {
        private readonly IManagementWork _management;
        private readonly IDbContextConfigurations _dbContextConfigurations;
        public ConfigureServiceController(IManagementWork management, IDbContextConfigurations dbContextConfigurations)
        {
            _management = management;
            _dbContextConfigurations = dbContextConfigurations;
        }

        [HttpGet(SDRoutes.Get)]
        public async Task<IActionResult> GetConfiguredItem(string UniqueId)
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
                    var congfiguredService = await context.psqlUnitOfWork.ConfigureServices.FirstOrDefaultAsync(filter: d => d.UniqueId == UniqueId, includeProperties: "Service");
                    if (congfiguredService == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(congfiguredService);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var congfiguredService = await context.mssqlUnitOfWork.ConfigureServices.FirstOrDefaultAsync(filter: d => d.UniqueId == UniqueId, includeProperties: "Service");
                    if (congfiguredService == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(congfiguredService);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var congfiguredService = await context.mongoUnitOfWork.ConfigureServices.FirstOrDefaultAsync(filter: d => d.UniqueId == UniqueId);

                    if (congfiguredService == null)
                        return NotFound(new { message = "NotFound" });

                    congfiguredService.Service = await context.mongoUnitOfWork.Services.FirstOrDefaultAsync(x => x.ServiceId == congfiguredService.ServiceId);
                    return Ok(congfiguredService);
                }
                else return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpGet(SDRoutes.GetAll)]
        public async Task<IActionResult> GetAll(string ServiceId)
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
                    if (ServiceId == null)
                        return Ok(await context.psqlUnitOfWork.ConfigureServices.GetAllAsync(includeProperties: "Service"));
                    else
                        return Ok(await context.psqlUnitOfWork.ConfigureServices.GetAllAsync(r => r.ServiceId == ServiceId && r.IsConfigured == TrueFalse.True));

                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    if (ServiceId == null)
                        return Ok(await context.mssqlUnitOfWork.ConfigureServices.GetAllAsync(includeProperties: "Service"));
                    else
                        return Ok(await context.mssqlUnitOfWork.ConfigureServices.GetAllAsync(r => r.ServiceId == ServiceId && r.IsConfigured == TrueFalse.True));

                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    if (ServiceId == null)
                    {
                        var list = await context.mongoUnitOfWork.ConfigureServices.GetAllAsync();
                        foreach (var item in list)
                        {
                            item.Service = await context.mongoUnitOfWork.Services.FirstOrDefaultAsync(x => x.ServiceId == item.ServiceId);
                        }
                        return Ok(list);
                    }
                    else
                    {
                        return Ok(await context.mongoUnitOfWork.ConfigureServices.GetAllAsync(r => r.ServiceId == ServiceId && r.IsConfigured == TrueFalse.True));
                    }

                }
                else
                    return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpPost(SDRoutes.Create)]
        public async Task<IActionResult> ConfigureService([FromBody] ConfigureServiceVM Item)
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
                        var indb = await context.psqlUnitOfWork.ConfigureServices.FirstOrDefaultAsync(d => d.ServiceId == Item.ServiceId && d.ItemUniqueId == Item.ItemUniqueId);
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });

                        PostgreSqlModels.ConfigureService configureService = new PostgreSqlModels.ConfigureService()
                        {
                            UniqueId = _management.UniqueId(),
                            ItemUniqueId = Item.ItemUniqueId,
                            ItemName = Item.ItemName,
                            ItemValue = Item.ItemValue,
                            ServiceId = Item.ServiceId,
                            IsConfigured = Item.IsConfigured
                        };
                        await context.psqlUnitOfWork.ConfigureServices.AddAsync(configureService);
                        return Ok(new { message = "Created" });
                    }

                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                    {
                        var indb = await context.mssqlUnitOfWork.ConfigureServices.FirstOrDefaultAsync(d => d.ServiceId == Item.ServiceId && d.ItemUniqueId == Item.ItemUniqueId);
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });

                        MicrosoftSqlServerModels.ConfigureService configureService = new MicrosoftSqlServerModels.ConfigureService()
                        {
                            UniqueId = _management.UniqueId(),
                            ItemUniqueId = Item.ItemUniqueId,
                            ItemName = Item.ItemName,
                            ItemValue = Item.ItemValue,
                            ServiceId = Item.ServiceId,
                            IsConfigured = Item.IsConfigured
                        };
                        await context.mssqlUnitOfWork.ConfigureServices.AddAsync(configureService);
                        return Ok(new { message = "Created" });
                    }

                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                    {
                        var indb = await context.mongoUnitOfWork.ConfigureServices.FirstOrDefaultAsync(d => d.ServiceId == Item.ServiceId && d.ItemUniqueId == Item.ItemUniqueId);
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });

                        MongoDbModels.ConfigureService configureService = new MongoDbModels.ConfigureService()
                        {
                            UniqueId = _management.UniqueId(),
                            ItemUniqueId = Item.ItemUniqueId,
                            ItemName = Item.ItemName,
                            ItemValue = Item.ItemValue,
                            ServiceId = Item.ServiceId,
                            IsConfigured = Item.IsConfigured
                        };
                        await context.mongoUnitOfWork.ConfigureServices.AddAsync(configureService);
                        return Ok(new { message = "Created" });
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
        public async Task<IActionResult> UpadateConfiguredService([FromBody] ConfigureServiceVM Item)
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
                        var indb = await context.psqlUnitOfWork.ConfigureServices.FirstOrDefaultAsync(x => x.UniqueId != Item.UniqueId && x.ServiceId == Item.ServiceId && x.ItemUniqueId == Item.ItemUniqueId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        await context.psqlUnitOfWork.ConfigureServices.UpdateAsync(indb.UniqueId, async entity =>
                        {
                            entity.ItemValue = Item.ItemValue;
                            entity.IsConfigured = Item.IsConfigured;
                            await Task.CompletedTask;
                        });
                        return Ok(new { message = "Updated" });
                    }

                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                    {
                        var indb = await context.mssqlUnitOfWork.ConfigureServices.FirstOrDefaultAsync(x => x.UniqueId != Item.UniqueId && x.ServiceId == Item.ServiceId && x.ItemUniqueId == Item.ItemUniqueId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        await context.mssqlUnitOfWork.ConfigureServices.UpdateAsync(indb.UniqueId, async entity =>
                        {
                            entity.ItemValue = Item.ItemValue;
                            entity.IsConfigured = Item.IsConfigured;
                            await Task.CompletedTask;
                        });
                        return Ok(new { message = "Updated" });
                    }

                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                    {
                        var indb = await context.mongoUnitOfWork.ConfigureServices.FirstOrDefaultAsync(x => x.UniqueId != Item.UniqueId && x.ServiceId == Item.ServiceId && x.ItemUniqueId == Item.ItemUniqueId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        await context.mongoUnitOfWork.ConfigureServices.UpdateAsync(x => x.UniqueId == indb.UniqueId, async entity =>
                        {
                            entity.ItemValue = Item.ItemValue;
                            entity.IsConfigured = Item.IsConfigured;
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

    }
}
