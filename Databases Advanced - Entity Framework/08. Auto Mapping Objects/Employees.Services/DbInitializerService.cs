namespace Employees.Services
{
    using Contracts;
    using Data;

    using Microsoft.EntityFrameworkCore;

    public class DbInitializerService : IDbInitializerService
    {
        private readonly EmployeesDbContext context;

        public DbInitializerService(EmployeesDbContext context)
        {
            this.context = context;
        }

        public void InitializeDatabase()
        {
            this.context.Database.Migrate();
        }
    }
}
