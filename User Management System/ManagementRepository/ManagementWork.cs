﻿using Microsoft.Extensions.Options;
using User_Management_System.ManagementConfigurations;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.SD;

namespace User_Management_System.ManagementRepository
{
    public class ManagementWork : IManagementWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IOptions<AppSettings> _appsettings;

        public ManagementWork(ApplicationDbContext context, IOptions<AppSettings> settings)
        {
            _context = context;
            _appsettings = settings;

            Projects = new ProjectRepository(_context);
            SupremeUsers = new SupremeUserRepository(_context, _appsettings);
            Services = new ServiceRepository(_context);
            ConfigureServices = new ConfigureServiceRepository(_context);

        }

        public IProjectReporistory Projects { private set; get; }

        public ISupremeUserRepository SupremeUsers { private set; get; }

        public IServiceRepository Services { private set; get; }

        public IConfigureServiceRepository ConfigureServices { private set; get; }


        public string UniqueId()
        {
            try
            {
                DateTime now = DateTime.Now;
                Random random = new Random();

                string additionalDigits = new string(Enumerable.Repeat(SDValues.ConstantStringKey, 6).Select(s => s[random.Next(s.Length)]).ToArray());

                string UniqueId = $"{additionalDigits}{now:yyyymmddHHssffff}";

                return UniqueId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

    }

}
