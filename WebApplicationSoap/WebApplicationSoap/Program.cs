using SoapCore;
using WebApplicationSoap.Services;

var builder = WebApplication.CreateBuilder(args);
// Register the ProductService
builder.Services.AddSingleton<IProductService, ProductService>();
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

// Enable SOAP endpoint
app.UseSoapEndpoint<IProductService>("/ProductService.asmx", new SoapEncoderOptions());

app.Run();
