﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.SD;

namespace User_Management_System.Controllers.ManagementControllers
{
    [Route(SDRoutes.ServiceManagement)]
    [ApiController]
    [Authorize(Policy = SDPolicies.SupremeAccess)]
    public class ServiceManagementController : Controller
    {
        private readonly IManagementWork _management;
        public ServiceManagementController(IManagementWork management)
        {
            _management = management;
        }

        [HttpGet(SDRoutes.Get)]
        public async Task<IActionResult> GetService(string ServiceUniqueId)
        {
            try
            {
                var serviceInDB = await _management.Services.FirstOrDefaultAsync(filter: d => d.ServiceUniqueId == ServiceUniqueId);
                if (serviceInDB == null)
                    return NotFound(new { message = "NotFound" });
                return Ok(serviceInDB);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpGet(SDRoutes.GetAll)]
        public async Task<IActionResult> GetAllServices()
        {
            try
            {
                var list = await _management.Services.GetAllAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpPost(SDRoutes.Create)]
        public async Task<IActionResult> CreateService([FromBody] Service Service)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var indb = await _management.Services.FirstOrDefaultAsync(d => d.ServiceName == Service.ServiceName && d.ServiceType == Service.ServiceType);
                    if (indb != null)
                        return BadRequest(new { message = "Exists" });
                    else
                    {
                        Service.ServiceUniqueId = _management.UniqueId();
                        Service.Status = TrueFalse.True;
                        await _management.Services.AddAsync(Service);

                        return Ok(new { message = "Created" });
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
        public async Task<IActionResult> UpdateService([FromBody] Service Service)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var indb = await _management.Services.GetAsync(Service.ServiceUniqueId);
                    if (indb == null)
                        return NotFound(new { message = "Not Found" });

                    var indbExists = await _management.Services.FirstOrDefaultAsync(d => d.ServiceUniqueId != indb.ServiceUniqueId && d.ServiceType == Service.ServiceType && d.ServiceName == Service.ServiceName);

                    if (indbExists != null)
                        return BadRequest(new { message = "Exists" });

                    await _management.Services.UpdateAsync(indb.ServiceUniqueId, async entity =>
                    {
                        entity.ServiceName = Service.ServiceName;
                        entity.ServiceType = Service.ServiceType;
                        entity.Status = Service.Status;
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

        //[HttpGet(SDRoutes.Delete)]
        //public async Task<IActionResult> DeleteService(string ServiceUniqueId)
        //{
        //    try
        //    {
        //        var serviceInDB = await _management.Services.FirstOrDefaultAsync(filter: d => d.ServiceUniqueId == ServiceUniqueId);
        //        if (serviceInDB == null)
        //            return NotFound(new { message = "NotFound" });
        //        else
        //            await _management.Services.RemoveAsync(ServiceUniqueId);
        //        return Ok(new { message = "Deleted" });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message, ex);
        //    }
        //}

    }
}
