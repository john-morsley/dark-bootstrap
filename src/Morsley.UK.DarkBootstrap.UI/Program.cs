using Morsley.UK.DarkBootstrap.UI.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<SmtpSettings>()
    .Bind(builder.Configuration.GetSection("SmtpSettings"))
    .ValidateOnStart();

builder.Services.AddSingleton<IValidateOptions<SmtpSettings>, SmtpSettingsValidator>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();