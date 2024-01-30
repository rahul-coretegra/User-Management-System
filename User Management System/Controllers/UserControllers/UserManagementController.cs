using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User_Management_System.Models.EnumModels;
using User_Management_System.Models.SupremeModels;
using User_Management_System.Models.UserModels.UserViewModels;
using User_Management_System.Repositories.IRepository;
using User_Management_System.SD;

namespace User_Management_System.Controllers.UserControllers
{

    [ApiController]
    [Route(SDRoutes.UserManagement)]

    public class UserManagementController : Controller
    {
        private readonly IUnitOfWork _iunitofwork;

        public UserManagementController(IUnitOfWork unitofwork)
        {
            _iunitofwork = unitofwork;
        }

        [HttpGet(SDRoutes.GetUsers)]
        [Authorize(Policy = SDPolicies.IsAccess)]
        [Authorize(Policy = SDPolicies.SecondaryLevel)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var userRoleInClaim = await _iunitofwork.UserRoles.FirstOrDefaultAsync(d => d.roleUniqueCode == User.FindFirst(ClaimTypes.Role).Value);
                var users = new List<UserAndRoleVM>();
                var list = await _iunitofwork.UserAndRoles.GetAllAsync(x => x.userRole.roleLevel != RoleLevels.SupremeLevel, includeProperties: "user,userRole");
                foreach (var item in list)
                {
                    bool shouldAddItem = true;

                    switch (userRoleInClaim.roleLevel)
                    {
                        case RoleLevels.SupremeLevel:

                            shouldAddItem = !(item.userRole.roleLevel == RoleLevels.SupremeLevel);
                            break;

                        case RoleLevels.Authority:
                            shouldAddItem = !(item.userRole.roleLevel >= RoleLevels.Authority);
                            break;

                        case RoleLevels.Intermediate:
                            shouldAddItem = !(item.userRole.roleLevel >= RoleLevels.Intermediate);
                            break;

                        default:
                            shouldAddItem = !(item.userRole.roleLevel >= RoleLevels.Secondary);
                            break;
                    }

                    if (shouldAddItem)
                    {
                        UserAndRoleVM userVM = new UserAndRoleVM()
                        {
                            userUniqueCode = item.userUniqueCode,
                            username = item.user.username,
                            phoneNumber = item.user.phoneNumber,
                            email = item.user.email,
                            address = item.user.address,
                            userRole = item.userRole,
                            accessToRole = item.accessToRole
                        };
                        users.Add(userVM);
                    }
                }
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpPut(SDRoutes.RemoveAccessToRole)]
        [Authorize(Policy = SDPolicies.IsAccess)]
        [Authorize(Policy = SDPolicies.SecondaryLevel)]
        public async Task<IActionResult> RemoveAccessToRole(UserAndRoleVM userAndRoleVM)
        {
            try
            {
                var userRoleInClaim = await _iunitofwork.UserRoles.FirstOrDefaultAsync(d => d.roleUniqueCode == User.FindFirst(ClaimTypes.Role).Value);

                var indb = await _iunitofwork.UserAndRoles.FirstOrDefaultAsync(d => d.userUniqueCode == userAndRoleVM.userUniqueCode && d.roleUniqueCode == userAndRoleVM.userRole.roleUniqueCode);
                if (indb == null)
                    return NotFound(new { message = "NotFound" });

                if (indb.userRole.roleLevel == RoleLevels.SupremeLevel ||
                        (userRoleInClaim.roleLevel != RoleLevels.SupremeLevel
                        && indb.userRole.roleLevel >= userRoleInClaim.roleLevel))
                    return BadRequest(new { message = "NoAccess" });

                await _iunitofwork.UserAndRoles.UpdateAsync(indb.userAndRoleUniqueId, async entity =>
                {
                    if (indb.accessToRole == TrueFalse.True)
                        entity.accessToRole = TrueFalse.False;
                    else
                        entity.accessToRole = TrueFalse.True;
                    await Task.CompletedTask;
                });
                return Ok(new { message = "Success" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpGet(SDRoutes.GetAll)]
        [Authorize(Policy = SDPolicies.IsAccess)]
        [Authorize(Policy = SDPolicies.SupremeLevel)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var userList = await _iunitofwork.Users.GetAllAsync(includeProperties: "userRoles.userRole");
                foreach (var user in userList)
                    user.password = null;
                return Ok(userList);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpPost(SDRoutes.CreateAndUpdateUserRoles)]
        [Authorize(Policy = SDPolicies.IsAccess)]
        [Authorize(Policy = SDPolicies.SecondaryLevel)]
        public async Task<IActionResult> CreateAndUpdateUserRoles([FromBody] UserAndRoles[] userRoles)
        {
            try
            {
                var userRoleInClaim = await _iunitofwork.UserRoles.FirstOrDefaultAsync(d => d.roleUniqueCode == User.FindFirst(ClaimTypes.Role).Value);

                foreach (var userRole in userRoles)
                {
                    if (userRoleInClaim.roleLevel != RoleLevels.SupremeLevel &&
                        userRoleInClaim.roleLevel >= userRole.userRole.roleLevel)
                        return BadRequest(new { message = "NoAccess" });

                    var userRoleInDb = await _iunitofwork.UserAndRoles.FirstOrDefaultAsync(x => x.roleUniqueCode == userRole.roleUniqueCode && x.userUniqueCode == userRole.userUniqueCode);

                    if (userRoleInDb == null)
                    {
                        var userAndRoleUniqueId = _iunitofwork.GenrateAlphaNumricUniqueCode();
                        UserAndRoles addUserRole = new UserAndRoles()
                        {
                            userAndRoleUniqueId = userAndRoleUniqueId,
                            userUniqueCode = userRole.userUniqueCode,
                            roleUniqueCode = userRole.roleUniqueCode,
                            accessToRole = TrueFalse.True
                        };
                        await _iunitofwork.UserAndRoles.AddAsync(addUserRole);
                    }
                    else if (userRoleInDb.accessToRole != userRole.accessToRole)
                    {
                        await _iunitofwork.UserAndRoles.UpdateAsync(userRoleInDb.userAndRoleUniqueId,
                            async entity =>
                            {
                                entity.accessToRole = userRole.accessToRole;
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

        [HttpPut(SDRoutes.ActivateDeactivateUser)]
        [Authorize(Policy = SDPolicies.IsAccess)]
        [Authorize(Policy = SDPolicies.SupremeLevel)]
        public async Task<IActionResult> ActivateDeactivateUser(string userUniqueCode)
        {
            try
            {
                var indb = await _iunitofwork.Users.FirstOrDefaultAsync(d => d.userUniqueCode == userUniqueCode, includeProperties: "userRoles.userRole");
                if (indb == null)
                    return NotFound(new { message = "NotFound" });

                foreach (var userRole in indb.userRoles)
                    if (userRole.userRole.roleLevel == RoleLevels.SupremeLevel)
                        return BadRequest(new { message = "NoAccess" });

                await _iunitofwork.Users.UpdateAsync(indb.userUniqueCode, async entity =>
                {
                    if (indb.isActiveUser == TrueFalse.True)
                        entity.isActiveUser = TrueFalse.False;
                    else
                        entity.isActiveUser = TrueFalse.True;
                    await Task.CompletedTask;
                });
                return Ok(new { message = "Success" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }
    }

}

