using Microsoft.AspNetCore.HttpOverrides;

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
        });
});

WebApplication app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();  // redirect 80 to 443
app.UseDefaultFiles();      // use index.html & index.css
app.UseStaticFiles();       // enable static file serving
app.UseCors(MyAllowSpecificOrigins);

app.MapGet("/api/upload", async Task<IResult> (HttpRequest request) =>
{
    return Results.Ok();
});

Log.Initialize();
Journal.Load();
app.Run();