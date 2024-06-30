using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetValue<string>("ConnectionStrings:BlobStorage")));
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.Run();
