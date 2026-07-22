namespace Healtive.Infrastructure.Seed;

public interface IDatabaseSeeder
{
    Task SeedAsync();
}