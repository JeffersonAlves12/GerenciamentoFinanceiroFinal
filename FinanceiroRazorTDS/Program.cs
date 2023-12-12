using Microsoft.EntityFrameworkCore;
using FinanceiroRazorTDS.Data;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


// Adicione a seguinte linha para configurar o serviço do DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"), 
        new MySqlServerVersion(new Version(8, 0, 35)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure() // Adicione esta linha
    ));

// Adicione serviços ao contêiner.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddHttpClient();



var app = builder.Build();

async Task ApplyMigrationsAsync(AppDbContext context)
{
    try
    {
        // Verifica se as migrations estão pendentes e aplica-as
        if (await context.Database.GetPendingMigrationsAsync() is var migrations && migrations.Any())
        {
            await context.Database.MigrateAsync();
        }
    }
    catch (Exception ex)
    {
        // Tratamento de exceções (aqui você pode adicionar algum tipo de logging)
        Console.WriteLine($"Erro ao aplicar migrations: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // Aplica as migrations em produção
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await ApplyMigrationsAsync(dbContext);
}
else
{
    // Aplica as migrations em desenvolvimento
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await ApplyMigrationsAsync(dbContext);
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();


app.MapGet("/testdb", async (AppDbContext dbContext) =>
{
    try
    {
        // Tenta fazer uma consulta no banco de dados
        var canConnect = await dbContext.Database.CanConnectAsync();
        return canConnect ? "Conexão com o banco de dados foi um sucesso!" : "Não foi possível conectar ao banco de dados.";
    }
    catch (Exception ex)
    {
        // Retorna a mensagem de erro se a conexão falhar
        return $"Erro ao conectar ao banco de dados: {ex.Message}";
    }
});

app.Run();
