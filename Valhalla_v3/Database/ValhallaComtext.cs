using Microsoft.EntityFrameworkCore;
using Valhalla_v3.Shared;
using Valhalla_v3.Shared.CarHistory;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Database;

public class ValhallaComtext : DbContext
{
    public DbSet<Operator> Operator { get; set; }
    //TO DO
    public DbSet<Comment> Comment { get; set; }
    public DbSet<Shared.ToDo.Task> Task { get; set; }
    public DbSet<Project> Project { get; set; }
    //Car History
    public DbSet<Car> Car { get; set; }
    public DbSet<CarHistoryFuel> CarHistoryFuels { get; set; }
    public DbSet<CarHistoryRepair> CarHistoryRepairs { get; set; }
    public DbSet<GasStation> GasStations { get; set; }
    public DbSet<Mechanic> Mechanics { get; set; }


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = $"Data Source=DESKTOP-I45S3D6;" +
                  $"Initial Catalog=Valhallav3;" +
                  "Integrated Security=SSPI;Encrypt=True;TrustServerCertificate=True;";

        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Configure the Operator table
        modelBuilder.Entity<Operator>().ToTable("Operators");

        #region Configure the Comment table
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
		#endregion
        # region Configure the Task table
		modelBuilder.Entity<Shared.ToDo.Task>().ToTable("Tasks");

        modelBuilder.Entity<Shared.ToDo.Task>()
            .HasOne(b => b.OperatorCreate)
            .WithMany()
            .HasForeignKey(b => b.OperatorCreateId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Shared.ToDo.Task>()
            .HasOne(b => b.OperatorModify)
            .WithMany()
            .HasForeignKey(b => b.OperatorModifyId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Shared.ToDo.Task>()
            .HasOne(b => b.Project)
            .WithMany(a => a.Tasks)
            .HasForeignKey(b => b.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
		#endregion
        #region Configure the Project table
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
		#endregion
        #region Configure the Car table
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
		#endregion
		#region Configure the CarHistoryFuel table
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
			.HasOne(b => b.Car)
			.WithMany(a => a.Fuels)
			.HasForeignKey(b => b.CarId)
			.IsRequired()
			.OnDelete(DeleteBehavior.NoAction);
		#endregion
		#region Configure the CarHistoryFuel table
		modelBuilder.Entity<CarHistoryRepair>().ToTable("CarHistoryRepair");

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
		#endregion
		#region Configure the GasStation table
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
		#endregion
		#region Configure the Mechanic table
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
		#endregion
	}
}
