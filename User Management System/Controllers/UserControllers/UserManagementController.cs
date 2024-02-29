using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.ManagementModels.VMs;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.SD;

namespace User_Management_System.Controllers.UserControllers
{

    [ApiController]
    [Route(SDRoutes.UserManagement)]
    [Authorize(Policy = SDPolicies.SupremeAccess)]
    public class UserManagementController : Controller
    {
        private readonly IManagementWork _management;
        private readonly IDbContextConfigurations _dbContextConfigurations;

        public UserManagementController(IManagementWork management, IDbContextConfigurations dbContextConfigurations)
        {
            _dbContextConfigurations = dbContextConfigurations;
            _management = management;
        }

        [HttpGet(SDRoutes.Get)]
        public async Task<IActionResult> GetUser(string Identity)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("projectUniqueId", out var projectUniqueId);
                var projectInDb = await _management.Projects.FirstOrDefaultAsync(p => p.ProjectUniqueId == projectUniqueId.ToString());

                var context = _dbContextConfigurations.configureDbContexts(projectInDb);
                if (projectInDb.TypeOfDatabase == TypeOfDatabase.PostgreSql)
                {
                    var entity = await context.psqlUnitOfWork.Users.FirstOrDefaultAsync(x => x.UserId == Identity || x.Email == Identity || x.PhoneNumber == Identity);

                    var userAndRoles = (await context.psqlUnitOfWork.UserAndRoles.GetAllAsync(x => x.UserId == entity.UserId, includeProperties: "UserRole")).ToList();

                    PsqlUserVM user = new PsqlUserVM()
                    {
                        Id = entity.Id,
                        UserId = entity.UserId,
                        UserName = entity.UserName,
                        Email = entity.Email,
                        Password = entity.Password,
                        PhoneNumber = entity.PhoneNumber,
                        Address = entity.Address,
                        IsActiveUser = entity.IsActiveUser,
                        IsVerifiedEmail = entity.IsVerifiedEmail,
                        IsVerifiedPhoneNumber = entity.IsVerifiedPhoneNumber,
                        CreatedAt = entity.CreatedAt,
                        UpdatedAt = entity.UpdatedAt,
                        UserAndRoles = userAndRoles
                    };
                    return Ok(user);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var entity = await context.mssqlUnitOfWork.Users.FirstOrDefaultAsync(x => x.UserId == Identity || x.Email == Identity || x.PhoneNumber == Identity);

                    var userAndRoles = (await context.mssqlUnitOfWork.UserAndRoles.GetAllAsync(x => x.UserId == entity.UserId, includeProperties: "UserRole")).ToList();

                    MssqlUserVM user = new MssqlUserVM()
                    {
                        UserId = entity.UserId,
                        UserName = entity.UserName,
                        Email = entity.Email,
                        Password = entity.Password,
                        PhoneNumber = entity.PhoneNumber,
                        Address = entity.Address,
                        IsActiveUser = entity.IsActiveUser,
                        IsVerifiedEmail = entity.IsVerifiedEmail,
                        IsVerifiedPhoneNumber = entity.IsVerifiedPhoneNumber,
                        CreatedAt = entity.CreatedAt,
                        UpdatedAt = entity.UpdatedAt,
                        UserAndRoles = userAndRoles
                    };
                    return Ok(user);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var entity = await context.mongoUnitOfWork.Users.FirstOrDefaultAsync(x => x.UserId == Identity || x.Email == Identity || x.PhoneNumber == Identity);

                    var userAndRoles = await context.mongoUnitOfWork.UserAndRoles.GetAllAsync(x => x.UserId == entity.UserId);

                    foreach (var userandrole in userAndRoles)
                    {
                        var userRole = await context.mongoUnitOfWork.UserRoles.FirstOrDefaultAsync(x => x.RoleId == userandrole.RoleId);
                        userandrole.UserRole = userRole;
                    }
                    MongoDBUserVM user = new MongoDBUserVM()
                    {
                        UserId = entity.UserId,
                        UserName = entity.UserName,
                        Email = entity.Email,
                        Password = entity.Password,
                        PhoneNumber = entity.PhoneNumber,
                        Address = entity.Address,
                        IsActiveUser = entity.IsActiveUser,
                        IsVerifiedEmail = entity.IsVerifiedEmail,
                        IsVerifiedPhoneNumber = entity.IsVerifiedPhoneNumber,
                        CreatedAt = entity.CreatedAt,
                        UpdatedAt = entity.UpdatedAt,
                        UserAndRoles = userAndRoles.ToList(),
                    };
                    return Ok(user);
                }
                else return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }

        [HttpGet(SDRoutes.GetAll)]
        public async Task<IActionResult> GetAllUsers(string RoleId)
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
                    var users = await context.psqlUnitOfWork.Users.GetAllAsync();
                    List<PsqlUserVM> list = new List<PsqlUserVM>();
                    foreach (var user in users)
                    {
                        PsqlUserVM psqlUser = new PsqlUserVM()
                        {
                            Id = user.Id,
                            UserId = user.UserId,
                            UserName = user.UserName,
                            Email = user.Email,
                            Password = user.Password,
                            PhoneNumber = user.PhoneNumber,
                            Address = user.Address,
                            IsActiveUser = user.IsActiveUser,
                            IsVerifiedEmail = user.IsVerifiedEmail,
                            IsVerifiedPhoneNumber = user.IsVerifiedPhoneNumber,
                            CreatedAt = user.CreatedAt,
                            UpdatedAt = user.UpdatedAt,
                        };
                        if (RoleId == null)
                            psqlUser.UserAndRoles = (await context.psqlUnitOfWork.UserAndRoles.GetAllAsync(x => x.UserId == user.UserId, includeProperties: "UserRole")).ToList();
                        else
                            psqlUser.UserAndRoles =(await context.psqlUnitOfWork.UserAndRoles.GetAllAsync(x => x.UserId == user.UserId && x.RoleId == RoleId, includeProperties: "UserRole")).ToList();
                        list.Add(psqlUser);
                    }
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    var users = await context.mssqlUnitOfWork.Users.GetAllAsync();
                    List<MssqlUserVM> list = new List<MssqlUserVM>();
                    foreach (var user in users)
                    {
                        MssqlUserVM mssqlUser = new MssqlUserVM()
                        {
                            Id = user.Id,
                            UserId = user.UserId,
                            UserName = user.UserName,
                            Email = user.Email,
                            Password = user.Password,
                            PhoneNumber = user.PhoneNumber,
                            Address = user.Address,
                            IsActiveUser = user.IsActiveUser,
                            IsVerifiedEmail = user.IsVerifiedEmail,
                            IsVerifiedPhoneNumber = user.IsVerifiedPhoneNumber,
                            CreatedAt = user.CreatedAt,
                            UpdatedAt = user.UpdatedAt,
                        };
                        if (RoleId == null)
                            mssqlUser.UserAndRoles = (await context.mssqlUnitOfWork.UserAndRoles.GetAllAsync(x => x.UserId == user.UserId, includeProperties: "UserRole")).ToList();
                        else
                            mssqlUser.UserAndRoles = (await context.mssqlUnitOfWork.UserAndRoles.GetAllAsync(x => x.UserId == user.UserId && x.RoleId == RoleId, includeProperties: "UserRole")).ToList();
                        list.Add(mssqlUser);
                    }
                    return Ok(list);
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    var users = await context.mongoUnitOfWork.Users.GetAllAsync();
                    List<MongoDBUserVM> list = new List<MongoDBUserVM>();
                    foreach (var user in users)
                    {
                        MongoDBUserVM mongoDbUser = new MongoDBUserVM()
                        {
                            Id = user.Id,
                            UserId = user.UserId,
                            UserName = user.UserName,
                            Email = user.Email,
                            Password = user.Password,
                            PhoneNumber = user.PhoneNumber,
                            Address = user.Address,
                            IsActiveUser = user.IsActiveUser,
                            IsVerifiedEmail = user.IsVerifiedEmail,
                            IsVerifiedPhoneNumber = user.IsVerifiedPhoneNumber,
                            CreatedAt = user.CreatedAt,
                            UpdatedAt = user.UpdatedAt,
                        };
                        if (RoleId == null)
                        {
                            var userAndRoles = await context.mongoUnitOfWork.UserAndRoles.GetAllAsync(x => x.UserId == user.UserId);
                            foreach (var userandrole in userAndRoles)
                            {
                                var userRole = await context.mongoUnitOfWork.UserRoles.FirstOrDefaultAsync(x => x.RoleId == userandrole.RoleId);
                                userandrole.UserRole = userRole;
                            }
                            mongoDbUser.UserAndRoles = userAndRoles.ToList();
                        }
                        else
                        {
                            var userAndRoles = await context.mongoUnitOfWork.UserAndRoles.GetAllAsync(x => x.UserId  == user.UserId && x.RoleId == RoleId);
                            foreach (var userandrole in userAndRoles)
                            {
                                var userRole = await context.mongoUnitOfWork.UserRoles.FirstOrDefaultAsync(x => x.RoleId == userandrole.RoleId);
                                userandrole.UserRole = userRole;
                            }
                            mongoDbUser.UserAndRoles = userAndRoles.ToList();
                        }
                        list.Add(mongoDbUser);
                    }
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

        [HttpPost(SDRoutes.Register)]
        public async Task<IActionResult> RegisterUser([FromBody] User User)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("projectUniqueId", out var projectUniqueId);
                var projectInDb = await _management.Projects.FirstOrDefaultAsync(p => p.ProjectUniqueId == projectUniqueId.ToString());
                if (projectInDb == null)
                    return NotFound();
                var context = _dbContextConfigurations.configureDbContexts(projectInDb);
                if (ModelState.IsValid)
                {
                    if (projectInDb.TypeOfDatabase == TypeOfDatabase.PostgreSql)
                    {

                        var isuniqueuser = await context.psqlUnitOfWork.Users.IsUniqueUser(User.PhoneNumber, User.Email);
                        if (!isuniqueuser)
                            return NotFound(new { message = "Exists" });
                        var UserId = _management.UniqueId();
                        PostgreSqlModels.User registerUser = new PostgreSqlModels.User
                        {
                            UserId = UserId,
                            UserName = User.UserName,
                            IsVerifiedPhoneNumber = TrueFalse.True,
                            PhoneNumber = User.PhoneNumber,
                            IsVerifiedEmail = TrueFalse.True,
                            Email = User.Email,
                            IsActiveUser = TrueFalse.True,
                            Address = User.Address,
                            Password = User.Password,
                            CreatedAt = DateTime.UtcNow,
                        };
                        PostgreSqlModels.UserAndRoles userrole = new PostgreSqlModels.UserAndRoles
                        {
                            UniqueId = _management.UniqueId(),
                            UserId = UserId,
                            RoleId = SDValues.IndividualRoleId,
                            AccessToRole = TrueFalse.True
                        };

                        await context.psqlUnitOfWork.Users.RegisterUser(registerUser);
                        await context.psqlUnitOfWork.UserAndRoles.AddAsync(userrole);

                        return Ok(new { message = "Created" });

                    }
                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                    {

                        var isuniqueuser = await context.mssqlUnitOfWork.Users.IsUniqueUser(User.PhoneNumber, User.Email);
                        if (!isuniqueuser)
                            return NotFound(new { message = "Exists" });
                        var UserId = _management.UniqueId();
                        MicrosoftSqlServerModels.User registerUser = new MicrosoftSqlServerModels.User
                        {
                            UserId = UserId,
                            UserName = User.UserName,
                            IsVerifiedPhoneNumber = TrueFalse.True,
                            PhoneNumber = User.PhoneNumber,
                            IsVerifiedEmail = TrueFalse.True,
                            Email = User.Email,
                            IsActiveUser = TrueFalse.True,
                            Address = User.Address,
                            Password = User.Password,
                            CreatedAt = DateTime.UtcNow
                        };
                        MicrosoftSqlServerModels.UserAndRoles userRole = new MicrosoftSqlServerModels.UserAndRoles
                        {
                            UniqueId = _management.UniqueId(),
                            UserId = UserId,
                            RoleId = SDValues.IndividualRoleId,
                            AccessToRole = TrueFalse.True
                        };
                        await context.mssqlUnitOfWork.Users.RegisterUser(registerUser);
                        await context.mssqlUnitOfWork.UserAndRoles.AddAsync(userRole);

                        return Ok(new { message = "Created" });

                    }
                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                    {
                        var isuniqueuser = await context.mongoUnitOfWork.Users.IsUniqueUser(User.PhoneNumber, User.Email);
                        if (!isuniqueuser)
                            return NotFound(new { message = "Exists" });
                        var UserId = _management.UniqueId();
                        MongoDbModels.User registerUser = new MongoDbModels.User
                        {
                            UserId = UserId,
                            UserName = User.UserName,
                            IsVerifiedPhoneNumber = TrueFalse.True,
                            PhoneNumber = User.PhoneNumber,
                            IsVerifiedEmail = TrueFalse.True,
                            Email = User.Email,
                            IsActiveUser = TrueFalse.True,
                            Address = User.Address,
                            Password = User.Password,
                            CreatedAt = DateTime.UtcNow
                        };
                        MongoDbModels.UserAndRoles UserRoles = new MongoDbModels.UserAndRoles
                        {
                            UniqueId = _management.UniqueId(),
                            UserId = UserId,
                            RoleId = SDValues.IndividualRoleId,
                            AccessToRole = TrueFalse.True
                        };

                        await context.mongoUnitOfWork.Users.RegisterUser(registerUser);         
                        await context.mongoUnitOfWork.UserAndRoles.AddAsync(UserRoles);
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
        public async Task<IActionResult> UpdateUser([FromBody] User User)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("projectUniqueId", out var projectUniqueId);
                var projectInDb = await _management.Projects.FirstOrDefaultAsync(p => p.ProjectUniqueId == projectUniqueId.ToString());

                var context = _dbContextConfigurations.configureDbContexts(projectInDb);

                if (ModelState.IsValid)
                {
                    if (projectInDb.TypeOfDatabase == TypeOfDatabase.PostgreSql)
                    {

                        var indb = await context.psqlUnitOfWork.Users.GetAsync(User.UserId);

                        var inDbExists = await context.psqlUnitOfWork.Users.FirstOrDefaultAsync(d => (d.PhoneNumber == User.PhoneNumber || d.Email == User.Email) && d.UserId != indb.UserId);

                        if (indb == null)
                            return NotFound(new { message = "NotFound" });

                        if (inDbExists != null)
                            return BadRequest(new { message = "Data Not Available" });

                        await context.psqlUnitOfWork.Users.UpdateAsync(indb.UserId, async entity =>
                        {
                            entity.UserName = User.UserName;
                            entity.Email = User.Email;
                            entity.PhoneNumber = User.PhoneNumber;
                            entity.Address = User.Address;
                            entity.UpdatedAt = DateTime.UtcNow;
                            if (indb.IsActiveUser != User.IsActiveUser)
                                entity.IsActiveUser = User.IsActiveUser;
                            await Task.CompletedTask;
                        });
                        var updateduser = await context.psqlUnitOfWork.Users.FirstOrDefaultAsync(d => d.UserId == indb.UserId);
                        var userRoles = await context.psqlUnitOfWork.UserAndRoles.GetAllAsync(d => d.UserId == updateduser.UserId);

                        if (updateduser.IsActiveUser == TrueFalse.False)
                        {
                            foreach (var UserRole in userRoles)
                            {
                                await context.psqlUnitOfWork.UserAndRoles.UpdateAsync(UserRole.UniqueId, async entity =>
                                {
                                    entity.AccessToRole = TrueFalse.False;
                                    await Task.CompletedTask;
                                });
                            }
                        }
                        else if (updateduser.IsActiveUser == TrueFalse.True)
                        {
                            foreach (var UserRole in userRoles)
                            {
                                if (UserRole.RoleId == SDValues.IndividualRoleId)
                                {
                                    await context.psqlUnitOfWork.UserAndRoles.UpdateAsync(UserRole.UniqueId, async entity =>
                                    {
                                        entity.AccessToRole = TrueFalse.True;
                                        await Task.CompletedTask;
                                    });
                                }
                            }
                        }
                        return Ok(new { message = "Updated" });
                    }
                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                    {

                        var indb = await context.mssqlUnitOfWork.Users.GetAsync(User.UserId);

                        var inDbExists = await context.mssqlUnitOfWork.Users.FirstOrDefaultAsync(d => (d.PhoneNumber == User.PhoneNumber || d.Email == User.Email) && d.UserId != indb.UserId);

                        if (indb == null)
                            return NotFound(new { message = "NotFound" });

                        if (inDbExists != null)
                            return BadRequest(new { message = "Data Not Available" });

                        await context.mssqlUnitOfWork.Users.UpdateAsync(indb.UserId, async entity =>
                        {
                            entity.UserName = User.UserName;
                            entity.Email = User.Email;
                            entity.PhoneNumber = User.PhoneNumber;
                            entity.Address = User.Address;
                            entity.UpdatedAt = DateTime.UtcNow;
                            if (indb.IsActiveUser != User.IsActiveUser)
                                entity.IsActiveUser = User.IsActiveUser;
                            await Task.CompletedTask;
                        });
                        var updateduser = await context.mssqlUnitOfWork.Users.FirstOrDefaultAsync(d => d.UserId == indb.UserId, includeProperties: "UserRoles.UserRole");
                        var userRoles = await context.mssqlUnitOfWork.UserAndRoles.GetAllAsync(d => d.UserId == updateduser.UserId);

                        if (updateduser.IsActiveUser == TrueFalse.False)
                        {
                            foreach (var UserRole in userRoles)
                            {
                                await context.mssqlUnitOfWork.UserAndRoles.UpdateAsync(UserRole.UniqueId, async entity =>
                                {
                                    entity.AccessToRole = TrueFalse.False;
                                    await Task.CompletedTask;
                                });
                            }
                        }
                        else if (updateduser.IsActiveUser == TrueFalse.True)
                        {
                            foreach (var UserRole in userRoles)
                            {
                                if (UserRole.RoleId == SDValues.IndividualRoleId)
                                {
                                    await context.mssqlUnitOfWork.UserAndRoles.UpdateAsync(UserRole.UniqueId, async entity =>
                                    {
                                        entity.AccessToRole = TrueFalse.True;
                                        await Task.CompletedTask;
                                    });
                                }
                            }
                        }
                        return Ok(new { message = "Updated" });
                    }
                    else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                    {

                        var indb = await context.mongoUnitOfWork.Users.FirstOrDefaultAsync(x => x.UserId == User.UserId);

                        var inDbExists = await context.mongoUnitOfWork.Users.FirstOrDefaultAsync(d => (d.PhoneNumber == User.PhoneNumber || d.Email == User.Email) && d.UserId != indb.UserId);

                        if (indb == null)
                            return NotFound(new { message = "NotFound" });

                        if (inDbExists != null)
                            return BadRequest(new { message = "Data Not Available" });

                        await context.mongoUnitOfWork.Users.UpdateAsync(x => x.UserId == indb.UserId, async entity =>
                        {
                            entity.UserName = User.UserName;
                            entity.Email = User.Email;
                            entity.PhoneNumber = User.PhoneNumber;
                            entity.Address = User.Address;
                            entity.UpdatedAt = DateTime.UtcNow;
                            if (indb.IsActiveUser != User.IsActiveUser)
                                entity.IsActiveUser = User.IsActiveUser;
                            await Task.CompletedTask;
                        });
                        var updateduser = await context.mongoUnitOfWork.Users.FirstOrDefaultAsync(d => d.UserId == indb.UserId);
                        var userroles = await context.mongoUnitOfWork.UserAndRoles.GetAllAsync(d => d.User == updateduser);

                        if (updateduser.IsActiveUser == TrueFalse.False)
                        {
                            foreach (var UserRole in userroles)
                            {
                                await context.mongoUnitOfWork.UserAndRoles.UpdateAsync(x => x.UniqueId == UserRole.UniqueId, async entity =>
                                {
                                    entity.AccessToRole = TrueFalse.False;
                                    await Task.CompletedTask;
                                });
                            }
                        }
                        else if (updateduser.IsActiveUser == TrueFalse.True)
                        {
                            foreach (var UserRole in userroles)
                            {
                                if (UserRole.RoleId == SDValues.IndividualRoleId)
                                {
                                    await context.mongoUnitOfWork.UserAndRoles.UpdateAsync(x => x.UniqueId == UserRole.UniqueId, async entity =>
                                    {
                                        entity.AccessToRole = TrueFalse.True;
                                        await Task.CompletedTask;
                                    });
                                }
                            }
                        }
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

        [HttpPost(SDRoutes.UpSertUserAccess)]
        public async Task<IActionResult> UpSertUserAccessOfRoles([FromBody] UserAndRoles[] UserRoles)
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
                    foreach (var userRole in UserRoles)
                    {
                        var userRoleInDb = await context.psqlUnitOfWork.UserAndRoles.FirstOrDefaultAsync(x => x.RoleId == userRole.RoleId && x.UserId == userRole.UserId);

                        if (userRoleInDb == null)
                        {
                            var UniqueId = _management.UniqueId();
                            PostgreSqlModels.UserAndRoles addUserRole = new PostgreSqlModels.UserAndRoles()
                            {
                                UniqueId = UniqueId,
                                UserId = userRole.UserId,
                                RoleId = userRole.RoleId,
                                AccessToRole = TrueFalse.True
                            };
                            await context.psqlUnitOfWork.UserAndRoles.AddAsync(addUserRole);
                        }
                        else if (userRoleInDb.AccessToRole != userRole.AccessToRole)
                        {
                            await context.psqlUnitOfWork.UserAndRoles.UpdateAsync(userRoleInDb.UniqueId, async entity =>
                            {
                                entity.AccessToRole = userRole.AccessToRole;
                                await Task.CompletedTask;
                            });
                        }
                    }
                    return Ok(new { message = "Ok" });
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    foreach (var userRole in UserRoles)
                    {
                        var userRoleInDb = await context.psqlUnitOfWork.UserAndRoles.FirstOrDefaultAsync(x => x.RoleId == userRole.RoleId && x.UserId == userRole.UserId);

                        if (userRoleInDb == null)
                        {
                            var UniqueId = _management.UniqueId();
                            MicrosoftSqlServerModels.UserAndRoles addUserRole = new MicrosoftSqlServerModels.UserAndRoles()
                            {
                                UniqueId = UniqueId,
                                UserId = userRole.UserId,
                                RoleId = userRole.RoleId,
                                AccessToRole = TrueFalse.True
                            };
                            await context.mssqlUnitOfWork.UserAndRoles.AddAsync(addUserRole);
                        }
                        else if (userRoleInDb.AccessToRole != userRole.AccessToRole)
                        {
                            await context.mssqlUnitOfWork.UserAndRoles.UpdateAsync(userRoleInDb.UniqueId, async entity =>
                            {
                                entity.AccessToRole = userRole.AccessToRole;
                                await Task.CompletedTask;
                            });
                        }
                    }
                    return Ok(new { message = "Ok" });
                }
                else if (projectInDb.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    foreach (var userRole in UserRoles)
                    {
                        var userRoleInDb = await context.mongoUnitOfWork.UserAndRoles.FirstOrDefaultAsync(x => x.RoleId == userRole.RoleId && x.UserId == userRole.UserId);

                        if (userRoleInDb == null)
                        {
                            var UniqueId = _management.UniqueId();
                            MongoDbModels.UserAndRoles addUserRole = new MongoDbModels.UserAndRoles()
                            {
                                UniqueId = UniqueId,
                                UserId = userRole.UserId,
                                RoleId = userRole.RoleId,
                                AccessToRole = TrueFalse.True
                            };
                            await context.mongoUnitOfWork.UserAndRoles.AddAsync(addUserRole);
                        }
                        else if (userRoleInDb.AccessToRole != userRole.AccessToRole)
                        {
                            await context.mongoUnitOfWork.UserAndRoles.UpdateAsync(x => x.UniqueId == userRoleInDb.UniqueId, async entity =>
                            {
                                entity.AccessToRole = userRole.AccessToRole;
                                await Task.CompletedTask;
                            });
                        }
                    }
                    return Ok(new { message = "Ok" });
                }
                else
                    return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }
    }

}

