using FreeSql.Internal;
using FreeSql;
using DotNetCore.CAP.FakeFreeSql.Util;
using DotNetCore.CAP.FakeFreeSql;
using DotNetCore.CAP;
using Savorboard.CAP.InMemoryMessageQueue;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

FreeSqlBuilder freeSqlBuilder = new FreeSqlBuilder()
                            .UseConnectionString(Helper.GetDataType(builder.Configuration.GetConnectionString("DataType")),
                            builder.Configuration.GetConnectionString("Connection"))
                            .UseNameConvert(NameConvertType.PascalCaseToUnderscoreWithUpper);
IFreeSql isql = freeSqlBuilder.Build();
builder.Services.AddSingleton(isql);

builder.Services.AddCap(options =>
{
    options.UseFakeFreeSql();
    options.UseInMemoryMessageQueue();
    options.UseDashboard();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();