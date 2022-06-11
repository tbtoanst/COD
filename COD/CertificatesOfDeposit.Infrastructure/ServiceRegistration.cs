
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionDict = new Dictionary<DatabaseConnectionName, string>
            {
                { DatabaseConnectionName.Connection1, configuration.GetConnectionString("DefaultConnection") },
                { DatabaseConnectionName.Connection2, configuration.GetConnectionString("DefaultConnection") }
            };
            // Inject this dict
            services.AddSingleton<IDictionary<DatabaseConnectionName, string>>(connectionDict);

            // Inject the factory
            services.AddTransient<IDbConnectionFactory, DapperDbConnectionFactory>();
            services.AddTransient<IUnitOfWorkContext, UnitOfWorkContext>();
            services.AddTransient<IConnectionContext, UnitOfWorkContext>();

            services.AddTransient<IBranchRepository, BranchRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<IMenuRepository, MenuRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserRoleRepository, UserRoleRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IPermitRepository, PermitRepository>();
            services.AddTransient<ISellRepository, SellRepository>();
            services.AddTransient<IBuyRepository, BuyRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<IAccountTranFccRepository, AccountTranFccRepository>();
            services.AddTransient<IAccountClassFccRepository, AccountClassFccRepository>();
            services.AddTransient<IAccountClassFccRepository, AccountClassFccRepository>();
            services.AddTransient<IBuyContractRepository, BuyContractRepository>();
            services.AddTransient<ISellContractRepository, SellContractRepository>();
            services.AddTransient<IBankPassbookRepository, BankPassbookRepository>(); 
            services.AddTransient<IJointHolderRepository, JointHolderRepository>();
            services.AddTransient<IStepStatusRepository, StepStatusRepository>();
            services.AddTransient<ITransactionLogRepository, TransactionLogRepository>();
            services.AddTransient<ITransactionLockRepository, TransactionLockRepository>();
        }
    }
}
