namespace Boom;

public class Customer {
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
}

public class CustomerViewModel {
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
}

public class DemoDbContext : DbContext {
    public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options) { }
    public virtual DbSet<Customer> Customers => Set<Customer>();
}

public class ThisTestWillFailAfterAResharperCodeCleanup {
    [Fact]
    public void Test1() {
        var config = new MapperConfiguration(cfg => cfg.CreateMap<Customer, CustomerViewModel>());
        var mapper = config.CreateMapper();
        var sqliteConnection = new SqliteConnection("Filename=:memory:");
        sqliteConnection.Open();
        var options = new DbContextOptionsBuilder<DemoDbContext>().UseSqlite(sqliteConnection).Options;
        var dbContext = new DemoDbContext(options);
        dbContext.Database.EnsureCreated();
        dbContext.Database.ExecuteSqlRaw("INSERT INTO Customers(Id, Name) VALUES(1, 'Aardvark')");
        var customer = dbContext.Customers.First();
        var model = mapper.Map<CustomerViewModel>(customer);
        model.Name.ShouldBe("Aardvark");
    }
}