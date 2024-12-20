using Microsoft.EntityFrameworkCore;
using Valhalla_v3.Shared;
using Valhalla_v3.Shared.CarHistory;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Database;

public interface IValhallaContext
{
    DbSet<Operator> Operator { get; set; }
    DbSet<Comment> Comment { get; set; }
    DbSet<Job> Job { get; set; }
    DbSet<Project> Project { get; set; }
    DbSet<Car> Car { get; set; }
    DbSet<CarHistoryFuel> CarHistoryFuels { get; set; }
    DbSet<CarHistoryRepair> CarHistoryRepairs { get; set; }
    DbSet<GasStation> GasStations { get; set; }
    DbSet<Mechanic> Mechanics { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class ValhallaContext : DbContext, IValhallaContext
{
    public virtual DbSet<Operator> Operator { get; set; }
    public virtual DbSet<Comment> Comment { get; set; }
    public virtual DbSet<Job> Job { get; set; }
    public virtual DbSet<Project> Project { get; set; }
    public virtual DbSet<Car> Car { get; set; }
    public virtual DbSet<CarHistoryFuel> CarHistoryFuels { get; set; }
    public virtual DbSet<CarHistoryRepair> CarHistoryRepairs { get; set; }
    public virtual DbSet<GasStation> GasStations { get; set; }
    public virtual DbSet<Mechanic> Mechanics { get; set; }

    private readonly IConfiguration _configuration;

    public ValhallaContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = _configuration.GetConnectionString("Connection");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureOperatorTable(modelBuilder);
        ConfigureCommentTable(modelBuilder);
        ConfigureJobTable(modelBuilder);
        ConfigureProjectTable(modelBuilder);
        ConfigureCarTable(modelBuilder);
        ConfigureCarHistoryFuelTable(modelBuilder);
        ConfigureCarHistoryRepairTable(modelBuilder);
        ConfigureGasStationTable(modelBuilder);
        ConfigureMechanicTable(modelBuilder);
    }

    private void ConfigureOperatorTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Operator>().ToTable("Operators");
    }

    private void ConfigureCommentTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>().ToTable("TaskComments");

        modelBuilder.Entity<Comment>()
            .HasOne(b => b.OperatorCreate)
            .WithMany()
            .HasForeignKey(b => b.OperatorCreateId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Comment>()
            .HasOne(b => b.OperatorModify)
            .WithMany()
            .HasForeignKey(b => b.OperatorModifyId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    private void ConfigureJobTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Job>().ToTable("Tasks");

        modelBuilder.Entity<Job>()
            .HasOne(b => b.OperatorCreate)
            .WithMany()
            .HasForeignKey(b => b.OperatorCreateId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Job>()
            .HasOne(b => b.OperatorModify)
            .WithMany()
            .HasForeignKey(b => b.OperatorModifyId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Job>()
            .HasOne(b => b.Project)
            .WithMany(a => a.Tasks)
            .HasForeignKey(b => b.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Job>()
            .HasMany(b => b.Comments)
            .WithOne(a => a.Job);
    }

    private void ConfigureProjectTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>().ToTable("Projects");

        modelBuilder.Entity<Project>()
            .HasOne(b => b.OperatorCreate)
            .WithMany()
            .HasForeignKey(b => b.OperatorCreateId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Project>()
            .HasOne(b => b.OperatorModify)
            .WithMany()
            .HasForeignKey(b => b.OperatorModifyId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Project>()
            .HasMany(b => b.Tasks)
            .WithOne(a => a.Project);
    }

    private void ConfigureCarTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>().ToTable("Car");

        modelBuilder.Entity<Car>()
            .HasOne(b => b.OperatorCreate)
            .WithMany()
            .HasForeignKey(b => b.OperatorCreateId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Car>()
            .HasOne(b => b.OperatorModify)
            .WithMany()
            .HasForeignKey(b => b.OperatorModifyId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Car>()
            .HasMany(b => b.CarHistoryRepair)
            .WithOne(a => a.Car);

        modelBuilder.Entity<Car>()
            .HasMany(b => b.Fuels)
            .WithOne(a => a.Car);
        
        modelBuilder.Entity<Car>()
            .Property(c => c.InsuranceCost)
            .HasColumnType("decimal(18,2)");
        
        modelBuilder.Entity<Car>()
            .Property(c => c.SurveyCost)
            .HasColumnType("decimal(18,2)");
    }

    private void ConfigureCarHistoryFuelTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarHistoryFuel>().ToTable("CarHistoryFuel");

        modelBuilder.Entity<CarHistoryFuel>()
            .HasOne(b => b.OperatorCreate)
            .WithMany()
            .HasForeignKey(b => b.OperatorCreateId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CarHistoryFuel>()
            .HasOne(b => b.OperatorModify)
            .WithMany()
            .HasForeignKey(b => b.OperatorModifyId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CarHistoryFuel>()
            .HasOne(b => b.GasStation)
            .WithMany(a => a.Fuels);

        modelBuilder.Entity<CarHistoryFuel>()
            .Property(c => c.CostPerLitr)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<CarHistoryFuel>()
            .Property(c => c.Cost)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<CarHistoryFuel>()
            .HasOne(b => b.Car)
            .WithMany(a => a.Fuels)
            .HasForeignKey(b => b.CarId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }

    private void ConfigureCarHistoryRepairTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarHistoryRepair>().ToTable("CarHistoryRepair");

        modelBuilder.Entity<CarHistoryRepair>()
            .Property(c => c.Cost)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<CarHistoryRepair>()
            .HasOne(b => b.OperatorCreate)
            .WithMany()
            .HasForeignKey(b => b.OperatorCreateId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CarHistoryRepair>()
            .HasOne(b => b.OperatorModify)
            .WithMany()
            .HasForeignKey(b => b.OperatorModifyId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CarHistoryRepair>()
            .HasOne(b => b.Mechanic)
            .WithMany(a => a.Repair)
            .HasForeignKey(b => b.MechanicId);

        modelBuilder.Entity<CarHistoryRepair>()
            .HasOne(b => b.Car)
            .WithMany(a => a.CarHistoryRepair)
            .HasForeignKey(b => b.CarId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }

    private void ConfigureGasStationTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GasStation>().ToTable("GasStation");

        modelBuilder.Entity<GasStation>()
            .HasOne(b => b.OperatorCreate)
            .WithMany()
            .HasForeignKey(b => b.OperatorCreateId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<GasStation>()
            .HasOne(b => b.OperatorModify)
            .WithMany()
            .HasForeignKey(b => b.OperatorModifyId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<GasStation>()
            .HasMany(b => b.Fuels)
            .WithOne(a => a.GasStation);
    }

    private void ConfigureMechanicTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mechanic>().ToTable("Mechanic");

        modelBuilder.Entity<Mechanic>()
            .HasOne(b => b.OperatorCreate)
            .WithMany()
            .HasForeignKey(b => b.OperatorCreateId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Mechanic>()
            .HasOne(b => b.OperatorModify)
            .WithMany()
            .HasForeignKey(b => b.OperatorModifyId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Mechanic>()
            .HasMany(b => b.Repair)
            .WithOne(a => a.Mechanic);
    }
}

