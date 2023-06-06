using Microsoft.EntityFrameworkCore;
using tryitter.Models;

namespace tryitter.Repository;

public class TryitterContext : DbContext
{

  public DbSet<Post>? Posts { get; set; }
  public DbSet<Student>? Students { get; set; }
  public TryitterContext(DbContextOptions<TryitterContext> options)
      : base(options) { }
  public TryitterContext() { }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)
    {
      var connectionString = Environment.GetEnvironmentVariable("DOTNET_CONNECTION_STRING");

      optionsBuilder.UseSqlServer(@"Server=tcp:tryitter-api-server.database.windows.net,1433;Initial Catalog=tryitter;Persist Security Info=False;User ID=aejepsen;Password=GateGrape1264;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
     
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Student>()
    .HasKey(x => x.StudentId);

    modelBuilder.Entity<Post>()
    .HasKey(x => x.PostId);

    modelBuilder.Entity<Post>()
    .HasOne(c => c.Student)
    .WithMany(x => x.Posts)
    .HasForeignKey(d => d.StudentId);
  }
}