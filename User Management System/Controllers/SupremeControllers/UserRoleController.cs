using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User_Management_System.Models.EnumModels;
using User_Management_System.Models.SupremeModels;
using User_Management_System.Repositories.IRepository;
using User_Management_System.SD;

namespace User_Management_System.Controllers.SupremeControllers
{
    [ApiController]
    [Route(SDRoutes.UserRoles)]
    public class UserRoleController : Controller
    {
        private readonly IUnitOfWork _iunitofwork;
        public UserRoleController(IUnitOfWork iunitofwork)
        {
            _iunitofwork = iunitofwork;
        }

        [HttpGet(SDRoutes.GetAll)]
        [Authorize(Policy = SDPolicies.IsAccess)]
        [Authorize(Policy = SDPolicies.SecondaryLevel)]
        public async Task<IActionResult> GetAllUserRoles()
        {
            try
            {
                var userRoleInClaim = await _iunitofwork.UserRoles.FirstOrDefaultAsync(d => d.roleUniqueCode == User.FindFirst(ClaimTypes.Role).Value);
                IEnumerable<UserRole> list;

                switch (userRoleInClaim.roleLevel)
                {
                    case RoleLevels.SupremeLevel:
                        list = await _iunitofwork.UserRoles.GetAllAsync();
                        break;

                    case RoleLevels.Authority:
                        list = await _iunitofwork.UserRoles.GetAllAsync(d => d.roleLevel < RoleLevels.Authority);
                        break;

                    case RoleLevels.Intermediate:
                        list = await _iunitofwork.UserRoles.GetAllAsync(d => d.roleLevel < RoleLevels.Intermediate);
                        break;

                    default:
                        list = await _iunitofwork.UserRoles.GetAllAsync(d => d.roleLevel < RoleLevels.Secondary);
                        break;
                }

                return Ok(list);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpGet(SDRoutes.Get)]
        [Authorize(Policy = SDPolicies.IsAccess)]
        [Authorize(Policy = SDPolicies.SupremeLevel)]
        public async Task<IActionResult> GetUserRole(string roleUniqueCode)
        {
            try
            {
                var role = await _iunitofwork.UserRoles.FirstOrDefaultAsync(d => d.roleUniqueCode == roleUniqueCode);

                if (role == null)
                    return NotFound(new { message = "NotFound" });
                return Ok(role);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpPost(SDRoutes.Create)]
        [Authorize(Policy = SDPolicies.IsAccess)]
        [Authorize(Policy = SDPolicies.SupremeLevel)]
        public async Task<IActionResult> CreateUserRole([FromBody] UserRole userRole)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var indb = await _iunitofwork.UserRoles.FirstOrDefaultAsync(d => d.roleName == userRole.roleName);
                    if (indb != null)
                        return BadRequest(new { message = "Exists" });
                    if (userRole.roleName == SDValues.IndividualRole)
                    {
                        userRole.roleUniqueCode = SDValues.IndividualRoleCode;
                        userRole.roleLevel = RoleLevels.Primary;
                    }
                    else
                    {
                        userRole.roleUniqueCode = _iunitofwork.GenrateAlphaNumricUniqueCode();
                    }
                    await _iunitofwork.UserRoles.AddAsync(userRole);
                    return Ok(new { message = "Created" });
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
        [Authorize(Policy = SDPolicies.IsAccess)]
        [Authorize(Policy = SDPolicies.SupremeLevel)]
        public async Task<IActionResult> UpdateUserRole([FromBody] UserRole userRole)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var indb = await _iunitofwork.UserRoles.GetAsync(userRole.roleUniqueCode);
                    if (indb == null)
                        return NotFound(new { message = "Not Found" });

                    var indbExists = await _iunitofwork.UserRoles.FirstOrDefaultAsync(d => d.roleUniqueCode != userRole.roleUniqueCode && d.roleName == userRole.roleName);

                    if (indbExists != null)
                        return BadRequest(new { message = "Exists" });

                    await _iunitofwork.UserRoles.UpdateAsync(userRole.roleUniqueCode, async entity =>
                    {
                        entity.roleName = userRole.roleName;
                        entity.roleLevel = userRole.roleLevel;
                        await Task.CompletedTask;
                    });

                    return Ok(new { message = "Updated" });
                }
                catch (Exception)
                {
                }
                return StatusCode(500, new { message = "Database Error" });

            }
            else
                return BadRequest(new { message = "BadRequest" });
        }

        [HttpDelete(SDRoutes.Delete)]
        [Authorize(Policy = SDPolicies.IsAccess)]
        [Authorize(Policy = SDPolicies.SupremeLevel)]
        public async Task<IActionResult> DeleteUserRole(string roleUniqueCode)
        {
            try
            {
                var indb = await _iunitofwork.UserRoles.GetAsync(roleUniqueCode);
                if (indb == null)
                    return NotFound(new { message = "Not Found" });

                var propshave = await _iunitofwork.UserAndRoles.FirstOrDefaultAsync(ur=>ur.roleUniqueCode ==indb.roleUniqueCode);
                if (propshave != null)
                    return BadRequest(new { message = "ObjectDepends" });

                await _iunitofwork.UserRoles.RemoveAsync(roleUniqueCode);
                return Ok(new { message = "Deleted" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });

            }
        }
    }
}
