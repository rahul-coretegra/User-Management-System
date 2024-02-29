using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.SD;

namespace User_Management_System.Controllers.ManagementControllers
{
    [Route(SDRoutes.ItemManagement)]
    [ApiController]
    [Authorize(Policy = SDPolicies.SupremeAccess)]
    public class ItemManagementController : Controller
    {
        private readonly IManagementWork _management;
        public ItemManagementController(IManagementWork management)
        {
            _management = management;
        }

        [HttpGet(SDRoutes.Get)]
        [Authorize(Policy = SDPolicies.SupremeAccess)]
        public async Task<IActionResult> GetItems(string ItemUniqueId)
        {
            try
            {
                var item = await _management.Items.FirstOrDefaultAsync(x => x.ItemUniqueId == ItemUniqueId, includeProperties: "Service");
                return Ok(item);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpGet(SDRoutes.GetAll)]
        [Authorize(Policy = SDPolicies.SupremeAccess)]
        public async Task<IActionResult> GetAllItems(string ServiceUniqueId)
        {
            try
            {
                var items = new List<Item>();
                if (ServiceUniqueId == null)
                    items = (await _management.Items.GetAllAsync(includeProperties: "Service")).ToList();
                else
                    items = (await _management.Items.GetAllAsync(filter: x => x.ServiceUniqueId == ServiceUniqueId)).ToList();
                return Ok(items);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpPost(SDRoutes.Create)]
        public async Task<IActionResult> CreateItem([FromBody] Item Item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var indb = await _management.Items.FirstOrDefaultAsync(d => d.ItemName == Item.ItemName && d.ServiceUniqueId == Item.ServiceUniqueId);
                    if (indb != null)
                        return BadRequest(new { message = "Exists" });
                    else
                    {
                        Item.ItemUniqueId = _management.UniqueId();
                        await _management.Items.AddAsync(Item);
                        return Ok(new { message = "Created" });
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

        [HttpPut(SDRoutes.Update)]
        public async Task<IActionResult> UpdateItem([FromBody] Item Item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var indb = await _management.Items.GetAsync(Item.ItemUniqueId);
                    if (indb == null)
                        return NotFound(new { message = "Not Found" });

                    var indbExists = await _management.Items.FirstOrDefaultAsync(d => d.ItemUniqueId != indb.ItemUniqueId && d.ServiceUniqueId == Item.ServiceUniqueId && d.ItemName == Item.ItemName);

                    if (indbExists != null)
                        return BadRequest(new { message = "Exists" });

                    await _management.Items.UpdateAsync(indb.ItemUniqueId, async entity =>
                    {
                        entity.ItemName = Item.ItemName;
                        entity.ServiceUniqueId = Item.ServiceUniqueId;
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

        [HttpGet(SDRoutes.Delete)]
        public async Task<IActionResult> DeleteItem(string ItemUniqueId)
        {
            try
            {
                var serviceInDB = await _management.Items.FirstOrDefaultAsync(filter: d => d.ItemUniqueId == ItemUniqueId);
                if (serviceInDB == null)
                    return NotFound(new { message = "NotFound" });
                else
                    await _management.Services.RemoveAsync(ItemUniqueId);
                return Ok(new { message = "Deleted" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

    }


}
