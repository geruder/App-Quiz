var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<vocabQuizAPI.Repositories.IWordRepository, vocabQuizAPI.Repositories.WordRepository>();
builder.Services.AddScoped<vocabQuizAPI.Repositories.IScorecardRepository, vocabQuizAPI.Repositories.ScorecardRepository>();
builder.Services.AddScoped<vocabQuizAPI.Repositories.IAuthRepository, vocabQuizAPI .Repositories.AuthRepository>();
builder.Services.AddScoped<vocabQuizAPI.Repositories.IHistoryRepository, vocabQuizAPI.Repositories.HistoryRepository>();
builder.Services.AddScoped<vocabQuizAPI.Repositories.ICategoryRepository, vocabQuizAPI.Repositories.CategoryRepository>();


builder.Services.AddCors(options =>  //frontend istekleri kabul etmek icin
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
