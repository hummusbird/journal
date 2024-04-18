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

Log.Initialize();
Config.Load();

if (Config.JournalEnabled)
{
    Journal.Load();

    app.MapGet("/api/journal/date/{date}", (string date) =>
    {
        string journal = Journal.ReadDate(date);
        if (journal == "") { return Results.NotFound(); }
        return Results.Text(journal);
    });

    app.MapGet("/api/journal/today", () =>
    {
        string journal = Journal.ReadDate(DateTime.Now.ToString("yyyy-MM-dd"));
        if (journal == "") { return Results.NotFound(); }
        return Results.Text(journal);
    });

    app.MapPost("/api/journal/new", async Task<IResult> (HttpRequest request) =>
    {
        if (!request.HasFormContentType) { return Results.BadRequest(); }

        var form = await request.ReadFormAsync();
        var data = form.ToList().Find(key => key.Key == "data");

        if (string.IsNullOrEmpty(data.Value)) // no data included
        {
            return Results.BadRequest();
        }

        Journal.AddEntry(data.Value!);
        Log.Info("New journal entry: " + data.Value);

        return Results.Ok();
    });
}

app.Run();