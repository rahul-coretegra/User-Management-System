using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.SD;
using User_Management_System.ManagementModels.VMs;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.Controllers.ProjectManagementControllers
{
    [ApiController]
    [Route(SDRoutes.ServiceConfigurationManagement)]
    [Authorize(Policy = SDPolicies.SupremeAccess)]
    public class ServiceConfigurationManagementController : Controller
    {
        private readonly IManagementWork _management;
        private readonly IDbContextConfigurations _dbContextConfigurations;
        public ServiceConfigurationManagementController(IManagementWork management, IDbContextConfigurations dbContextConfigurations)
        {
            _management = management;
            _dbContextConfigurations = dbContextConfigurations;
        }

        [HttpGet(SDRoutes.Get)]
        public async Task<IActionResult> GetConfiguredService(string UniqueId)
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
                    var congfiguredService = await context.psqlUnitOfWork.ConfigureServices.FirstOrDefaultAsync(d => d.UniqueId == UniqueId,includeProperties:"Items");
                    if (congfiguredService == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(congfiguredService);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var congfiguredService = await context.mssqlUnitOfWork.ConfigureServices.FirstOrDefaultAsync(d => d.UniqueId == UniqueId, includeProperties: "Items");
                    if (congfiguredService == null)
                        return NotFound(new { message = "NotFound" });
                    return Ok(congfiguredService);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var congfiguredService = await context.mongoUnitOfWork.ConfigureServices.FirstOrDefaultAsync(d => d.UniqueId == UniqueId);
                    //var congfiguredItems = await context.mongoUnitOfWork.Items.FirstOrDefaultAsync(d => d. == UniqueId);

                    //List<MongoDbModels.Item> items = new List<MongoDbModels.Item>();
                    //foreach (var configurationItem in ConfigurationItems)
                    //{
                    //    MicrosoftSqlServerModels.Item item = new MicrosoftSqlServerModels.Item()
                    //    {
                    //        ItemId = configurationItem.ItemId,
                    //        ItemName = configurationItem.ItemName,
                    //        ItemValue = configurationItem.ItemValue
                    //    };
                    //    items.Add(item);
                    //}
                    if (congfiguredService == null)
                        return NotFound(new { message = "NotFound" });
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
        public async Task<IActionResult> GetAllConfiguredServices()
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
                    var list = await context.psqlUnitOfWork.ConfigureServices.GetAllAsync( includeProperties: "Items");           
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var list = await context.mssqlUnitOfWork.ConfigureServices.GetAllAsync(includeProperties: "Items");
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var list = await context.mongoUnitOfWork.ConfigureServices.GetAllAsync();
                    return Ok(list);
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
        public async Task<IActionResult> ConfigureService(string ServiceUniqueId, [FromBody] ConfigureItem[] ConfigurationItems)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpContext.Request.Headers.TryGetValue("projectUniqueId", out var projectUniqueId);
                    var projectInDb = await _management.Projects.FirstOrDefaultAsync(p => p.ProjectUniqueId == projectUniqueId.ToString());
                    if (projectInDb == null)
                        return NotFound();
                    var serviceInMDb = await _management.Services.FirstOrDefaultAsync(x => x.ServiceUniqueId == ServiceUniqueId);
                    if (serviceInMDb == null)
                        return NotFound();

                    var context = _dbContextConfigurations.configureDbContexts(projectInDb);

                    if (projectInDb.TypeOfDatabase == TypeOfDatabase.PostgreSql)
                    {
                        var indb = await context.psqlUnitOfWork.ConfigureServices.FirstOrDefaultAsync(d => d.ServiceUniqueId == ServiceUniqueId);
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });

                        List<PostgreSqlModels.Item> items = new List<PostgreSqlModels.Item>();
                        foreach(var configurationItem in ConfigurationItems)
                        {
                            PostgreSqlModels.Item item = new PostgreSqlModels.Item()
                            {
                                ItemId = configurationItem.ItemId,
                                ItemName = configurationItem.ItemName,
                                ItemValue = configurationItem.ItemValue
                            };
                            items.Add(item);
                        }
                        PostgreSqlModels.ConfigureService configureService = new PostgreSqlModels.ConfigureService()
                        {
                            UniqueId = _management.UniqueId(),
                            ServiceUniqueId = serviceInMDb.ServiceUniqueId,
                            ServiceName = serviceInMDb.ServiceName,
                            ServiceType = serviceInMDb.ServiceType,
                            Items = items
                        };
                        await context.psqlUnitOfWork.ConfigureServices.AddAsync(configureService);
                        return Ok(new { message = "Created" });
                    }

                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                    {
                        var indb = await context.mssqlUnitOfWork.ConfigureServices.FirstOrDefaultAsync(d => d.ServiceUniqueId == ServiceUniqueId);
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });

                        List<MicrosoftSqlServerModels.Item> items = new List<MicrosoftSqlServerModels.Item>();
                        foreach (var configurationItem in ConfigurationItems)
                        {
                            MicrosoftSqlServerModels.Item item = new MicrosoftSqlServerModels.Item()
                            {
                                ItemId = configurationItem.ItemId,
                                ItemName = configurationItem.ItemName,
                                ItemValue = configurationItem.ItemValue
                            };
                            items.Add(item);
                        }
                        MicrosoftSqlServerModels.ConfigureService configureService = new MicrosoftSqlServerModels.ConfigureService()
                        {
                            UniqueId = _management.UniqueId(),
                            ServiceUniqueId = serviceInMDb.ServiceUniqueId,
                            ServiceName = serviceInMDb.ServiceName,
                            ServiceType = serviceInMDb.ServiceType,
                            Items = items
                        };
                        await context.mssqlUnitOfWork.ConfigureServices.AddAsync(configureService);
                        return Ok(new { message = "Created" });
                    }

                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                    {
                        var indb = await context.mongoUnitOfWork.ConfigureServices.FirstOrDefaultAsync(d => d.ServiceUniqueId == ServiceUniqueId);
                        if (indb != null)
                            return BadRequest(new { message = "Exists" });

                        List<MongoDbModels.Item> items = new List<MongoDbModels.Item>();
                        foreach (var configurationItem in ConfigurationItems)
                        {
                            MongoDbModels.Item item = new MongoDbModels.Item()
                            {
                                ItemId = configurationItem.ItemId,
                                ItemName = configurationItem.ItemName,
                                ItemValue = configurationItem.ItemValue
                            };
                            items.Add(item);
                        }
                        MongoDbModels.ConfigureService configureService = new MongoDbModels.ConfigureService()
                        {
                            UniqueId = _management.UniqueId(),
                            ServiceUniqueId = serviceInMDb.ServiceUniqueId,
                            ServiceName = serviceInMDb.ServiceName,
                            ServiceType = serviceInMDb.ServiceType,
                            Items = items
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
        public async Task<IActionResult> UpadateConfiguredService(string UniqueId, [FromBody] ConfigureItem[] ConfigurationItems)
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
                        var indb = await context.psqlUnitOfWork.ConfigureServices.FirstOrDefaultAsync(x => x.UniqueId == UniqueId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        List<PostgreSqlModels.Item> items = new List<PostgreSqlModels.Item>();
                        foreach (var configurationItem in ConfigurationItems)
                        {
                            PostgreSqlModels.Item item = new PostgreSqlModels.Item()
                            {
                                ItemId = configurationItem.ItemId,
                                ItemName = configurationItem.ItemName,
                                ItemValue = configurationItem.ItemValue
                            };
                            items.Add(item);
                        }
                        await context.psqlUnitOfWork.ConfigureServices.UpdateAsync(indb.UniqueId, async entity =>
                        {

                            entity.Items = items;
                            await Task.CompletedTask;
                        });
                        return Ok(new { message = "Updated" });
                    }

                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                    {
                        var indb = await context.mssqlUnitOfWork.ConfigureServices.GetAsync(UniqueId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        List<MicrosoftSqlServerModels.Item> items = new List<MicrosoftSqlServerModels.Item>();
                        foreach (var configurationItem in ConfigurationItems)
                        {
                            MicrosoftSqlServerModels.Item item = new MicrosoftSqlServerModels.Item()
                            {
                                ItemId = configurationItem.ItemId,
                                ItemName = configurationItem.ItemName,
                                ItemValue = configurationItem.ItemValue
                            };
                            items.Add(item);
                        }
                        await context.mssqlUnitOfWork.ConfigureServices.UpdateAsync(indb.UniqueId, async entity =>
                        {
                            entity.Items = items;
                            await Task.CompletedTask;
                        });
                        return Ok(new { message = "Updated" });
                    }

                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                    {
                        var indb = await context.mongoUnitOfWork.ConfigureServices.FirstOrDefaultAsync(x => x.UniqueId == UniqueId);
                        if (indb == null)
                            return NotFound(new { message = "Not Found" });

                        List<MongoDbModels.Item> items = new List<MongoDbModels.Item>();
                        foreach (var configurationItem in ConfigurationItems)
                        {
                            MongoDbModels.Item item = new MongoDbModels.Item()
                            {
                                ItemId = configurationItem.ItemId,
                                ItemName = configurationItem.ItemName,
                                ItemValue = configurationItem.ItemValue
                            };
                            items.Add(item);
                        }
                        await context.mongoUnitOfWork.ConfigureServices.UpdateAsync(x => x.UniqueId == indb.UniqueId, async entity =>
                        {
                            entity.Items = items;
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
