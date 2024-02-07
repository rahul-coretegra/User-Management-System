using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Management_System.ManagementModels;
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

        [HttpGet(SDRoutes.Project)]
        public async Task<IActionResult> GetProject(string ProjectUniqueId)
        {
            try
            {
                var projectInDB = await _management.Projects.FirstOrDefaultAsync(filter: d => d.ProjectUniqueId == ProjectUniqueId);
                if (projectInDB == null)
                    return NotFound(new { message = "NotFound" });
                return Ok(projectInDB);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }


        [HttpGet(SDRoutes.Projects)]
        public async Task<IActionResult> GetProjects()
        {
            try
            {
                var list = await _management.Projects.GetAllAsync();
                return Ok(list);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }



        [HttpPost(SDRoutes.CreateProject)]
        public async Task<IActionResult> CreateProject([FromBody] Project Project)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var indb = await _management.Projects.FirstOrDefaultAsync(d => d.ProjectName == Project.ProjectName || d.ConnectionString == Project.ConnectionString);
                    if (indb != null)
                        return BadRequest(new { message = "Exists" });
                    else
                    {
                        Project.ProjectUniqueId = _management.UniqueId();
                        var result = _dbContextConfigurations.establishDbConnection(Project);

                        if (result == true)
                        {
                            await _management.Projects.AddAsync(Project);
                            return Ok(new { message = "Created" });
                        }
                        else
                        {
                            return BadRequest(new { message = "sorry" });
                        }
                    }
                }
                catch (Exception)
                {
                    return StatusCode(500, new { message = "Database Error" });
                }
            }
            else
                return BadRequest(new { message = "BadRequest" });
        }

        [HttpPut(SDRoutes.UpadateProject)]
        public async Task<IActionResult> UpdateProject([FromBody] Project Project)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var indb = await _management.Projects.GetAsync(Project.ProjectUniqueId);
                    if (indb == null)
                        return NotFound(new { message = "Not Found" });

                    var indbExists = await _management.Projects.FirstOrDefaultAsync(d => d.ProjectUniqueId != Project.ProjectUniqueId && (d.ProjectName == Project.ProjectName || d.ConnectionString == Project.ConnectionString));

                    if (indbExists != null)
                        return BadRequest(new { message = "Exists" });

                    await _management.Projects.UpdateAsync(Project.ProjectUniqueId, async entity =>
                    {
                        entity.ProjectName = Project.ProjectName;
                        entity.ProjectDescription = Project.ProjectDescription;
                        entity.OwnerName = Project.OwnerName;
                        await Task.CompletedTask;
                    });
                    return Ok(new { message = "Updated" });
                }
                catch (Exception)
                {
                    return StatusCode(500, new { message = "Database Error" });
                }
            }
            else
                return BadRequest(new { message = "BadRequest" });
        }
    }
}
