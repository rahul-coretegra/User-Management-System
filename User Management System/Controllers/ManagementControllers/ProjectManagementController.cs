using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpCompress.Common;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.SD;

namespace User_Management_System.Controllers.ManagementControllers
{
    [ApiController]
    [Route(SDRoutes.ProjectManagement)]
    [Authorize(Policy = SDPolicies.SupremeAccess)]
    public class ProjectManagementController : Controller
    {
        private readonly IManagementWork _management;
        private readonly IDbContextConfigurations _dbContextConfigurations;
        public ProjectManagementController(IManagementWork management, IDbContextConfigurations dbContextConfigurations)
        {
            _management = management;
            _dbContextConfigurations = dbContextConfigurations;
        }

        [HttpGet(SDRoutes.Get)]
        public async Task<IActionResult> GetProject(string ProjectUniqueId)
        {
            try
            {
                var projectInDB = await _management.Projects.FirstOrDefaultAsync(filter: d => d.ProjectUniqueId == ProjectUniqueId);
                if (projectInDB == null)
                    return NotFound(new { message = "NotFound" });
                return Ok(projectInDB);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpGet(SDRoutes.GetAll)]
        public async Task<IActionResult> GetAllProjects()
        {
            try
            {
                var list = await _management.Projects.GetAllAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpPost(SDRoutes.Create)]
        public async Task<IActionResult> CreateProject([FromBody] Project Project)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var inDb = await _management.Projects.FirstOrDefaultAsync(x=>x.ProjectName == Project.ProjectName || x.ConnectionString == Project.ConnectionString);

                    if (inDb != null)
                        return BadRequest(new { message = "Exists" });

                    Project.ProjectUniqueId = _management.UniqueId();
                    Project.MigrateDatabase = TrueFalse.False;
                    Project.Status = TrueFalse.False;                 
                    await _management.Projects.AddAsync(Project);
                    return Ok(new { message = "Created" });                
                }
                else
                    return BadRequest(new { message = "BadRequest" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpPut(SDRoutes.MigrateDatabase)]
        public async Task<IActionResult> MigrateDatabase(string ProjectUniqueId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var projectInDb = await _management.Projects.FirstOrDefaultAsync(p => p.ProjectUniqueId == ProjectUniqueId);

                    if (projectInDb == null)
                        return NotFound(new { message = "Not Found" });

                    var result = _dbContextConfigurations.establishDbConnection(projectInDb);

                    if (result == true)
                    {
                        await _management.Projects.UpdateAsync(projectInDb.ProjectUniqueId, async entity =>
                        {
                            entity.MigrateDatabase = TrueFalse.True;
                            entity.Status = TrueFalse.True;
                            await Task.CompletedTask;
                        });
                        return Ok(new { message = "Updated" });

                    }
                    else
                    {
                        return BadRequest(new { message = "sorry" });
                    }
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
        public async Task<IActionResult> UpdateProjects([FromBody] Project Project)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var indb = await _management.Projects.GetAsync(Project.ProjectUniqueId);
                    if (indb == null)
                        return NotFound(new { message = "Not Found" });

                    var indbExists = await _management.Projects.FirstOrDefaultAsync(d => d.ProjectUniqueId != indb.ProjectUniqueId && d.ConnectionString == Project.ConnectionString);

                    if (indbExists != null)
                        return BadRequest(new { message = "Exists" });

                    await _management.Projects.UpdateAsync(indb.ProjectUniqueId, async entity =>
                    {
                        entity.ProjectName = Project.ProjectName;
                        entity.ProjectDescription = Project.ProjectDescription;
                        entity.OwnerName = Project.OwnerName;
                        entity.Status = Project.Status;
                        await Task.CompletedTask;
                    });
                    return Ok(new { message = "Updated" });
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
