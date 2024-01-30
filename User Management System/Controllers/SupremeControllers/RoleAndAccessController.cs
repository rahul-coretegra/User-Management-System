using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using User_Management_System.Models.EnumModels;
using User_Management_System.Models.SupremeModels;
using User_Management_System.Repositories.IRepository;
using User_Management_System.SD;

namespace User_Management_System.Controllers.SupremeControllers
{
    [ApiController]
    [Route(SDRoutes.RoleAndAccess)]
    [Authorize(Policy = SDPolicies.SupremeLevel)]

    public class RoleAndAccessController : Controller
    {
        private readonly IUnitOfWork _iunitofwork;
        public RoleAndAccessController(IUnitOfWork iunitofwork)
        {
            _iunitofwork = iunitofwork;
        }

        [HttpGet(SDRoutes.GetAll)]
        [Authorize(Policy = SDPolicies.IsAccess)]
        public async Task<IActionResult> GetAllRolesAndAccess()
        {
            try
            {
                var list = await _iunitofwork.RoleAndAccess.GetAllAsync(includeProperties: "userRole");
                return Ok(list);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpGet(SDRoutes.Get)]
        [Authorize(Policy = SDPolicies.IsAccess)]
        public async Task<IActionResult> GetRolesAndAccess(string roleAndAccessId)
        {
            try
            {
                var role = await _iunitofwork.RoleAndAccess.FirstOrDefaultAsync(filter: d => d.roleAndAccessId == roleAndAccessId, includeProperties: "userRole");
                if (role == null)
                    return NotFound(new { message = "NotFound" });
                return Ok(role);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpPost(SDRoutes.CreateAndUpdateRoleAndAccess)]
        [Authorize(Policy = SDPolicies.IsAccess)]
        public async Task<IActionResult> CreateAndUpdateRoleAndAccess([FromBody] RoleAndAccess[] roleAndAccesses)
        {
            try
            {
                foreach (var roleAndAccess in roleAndAccesses)
                {
                    var roleAndAccessInDb = await _iunitofwork.RoleAndAccess.FirstOrDefaultAsync(x => x.roleUniqueCode == roleAndAccess.roleUniqueCode && x.routePath == roleAndAccess.routePath);

                    if (roleAndAccessInDb == null)
                    {
                        RoleAndAccess addRoleAndAccess = new RoleAndAccess()
                        {
                            roleAndAccessId = _iunitofwork.GenrateAlphaNumricUniqueCode(),
                            routePath = roleAndAccess.routePath,
                            routeName = roleAndAccess.routeName,
                            roleUniqueCode = roleAndAccess.roleUniqueCode,
                            isAccess = TrueFalse.True
                        };
                        await _iunitofwork.RoleAndAccess.AddAsync(addRoleAndAccess);
                    }                   
                    else if (roleAndAccessInDb.isAccess != roleAndAccess.isAccess )
                    {
                        await _iunitofwork.RoleAndAccess.UpdateAsync(roleAndAccessInDb.roleAndAccessId, async entity =>
                        {
                            entity.isAccess = roleAndAccess.isAccess;
                            await Task.CompletedTask;
                        });
                    }
                }
                return Ok(new { message = "Ok" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }
    }

}
