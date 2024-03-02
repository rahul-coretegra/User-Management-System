using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.SD;

namespace User_Management_System.Controllers.ManagementControllers
{
    [Route(SDRoutes.ConfigureServiceManagement)]
    [ApiController]
    [Authorize(Policy = SDPolicies.SupremeAccess)]
    public class ConfigureServiceManagementController : Controller
    {
        private readonly IManagementWork _management;
        public ConfigureServiceManagementController(IManagementWork management)
        {
            _management = management;
        }

        [HttpGet(SDRoutes.Get)]
        public async Task<IActionResult> GetItem(string UniqueId)
        {
            try
            {
                var item = await _management.ConfigureServices.FirstOrDefaultAsync(filter: d => d.UniqueId == UniqueId, includeProperties: "Service");
                if (item == null)
                    return NotFound(new { message = "NotFound" });
                return Ok(item);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpGet(SDRoutes.GetAll)]
        public async Task<IActionResult> GetAll(string ServiceUniqueId)
        {
            try
            {

                if (ServiceUniqueId == null)
                    return Ok(await _management.ConfigureServices.GetAllAsync(includeProperties: "Service"));
                else
                    return Ok(await _management.ConfigureServices.GetAllAsync(r => r.ServiceUniqueId == ServiceUniqueId && r.IsConfigured == TrueFalse.True));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpPost(SDRoutes.Create)]
        public async Task<IActionResult> ConfigureItem([FromBody] ConfigureService serviceAndItem)
        {
            try
            {
                var InDb = await _management.ConfigureServices.FirstOrDefaultAsync(x => x.ItemName == serviceAndItem.ItemName && x.ServiceUniqueId == serviceAndItem.ServiceUniqueId);

                if (InDb != null)
                    return BadRequest(new { message = "Exists" });
                else
                {
                    ConfigureService configureService = new ConfigureService()
                    {
                        UniqueId = _management.UniqueId(),
                        ItemName = serviceAndItem.ItemName,
                        ServiceUniqueId = serviceAndItem.ServiceUniqueId,
                        IsConfigured = TrueFalse.True
                    };
                    await _management.ConfigureServices.AddAsync(configureService);

                    return Ok(new { message = "Ok" });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpPost(SDRoutes.Update)]
        public async Task<IActionResult> UpdateItem([FromBody] ConfigureService serviceAndItem)
        {
            try
            {
                var InDb = await _management.ConfigureServices.FirstOrDefaultAsync(x => x.UniqueId == serviceAndItem.UniqueId);

                if (InDb == null)
                    return NotFound(new { message = "Not Found" });

                var InDbExists = await _management.ConfigureServices.FirstOrDefaultAsync(x => x.ItemName == serviceAndItem.ItemName && x.ServiceUniqueId == serviceAndItem.ServiceUniqueId && x.UniqueId != InDb.UniqueId);
                
                if (InDbExists != null)
                    return BadRequest(new { message = "Exists" });
                else
                {
                    await _management.ConfigureServices.UpdateAsync(InDb.UniqueId, async entity =>
                    {
                        entity.ItemName = serviceAndItem.ItemName;
                        entity.IsConfigured = serviceAndItem.IsConfigured;
                        await Task.CompletedTask;
                    });
                    return Ok(new { message = "Ok" });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
