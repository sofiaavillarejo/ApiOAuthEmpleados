using ApiOAuthEmpleados.Data;
using ApiOAuthEmpleados.Helpers;
using ApiOAuthEmpleados.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

HelperCryptography.Initialize(builder.Configuration);
//INYECTAMOS HttpContextAccessor-> porq quiero acceder al usuario en clases q no sean del usuario
builder.Services.AddHttpContextAccessor();


//CREAMOS UNA INSTANCIA DE NUESTRO HELPER -> creado para quitar de aqui la configuracion y tenerla todo en un mismo sitio
//SE DEBE CREAR SOLO UNA VEZ PARA QUE NUESTRA APP PUEDA VALIDAR TODO LO QUE HA CREADO
HelperActionServicesOAuth helper = new HelperActionServicesOAuth(builder.Configuration);
builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);
//HABILITAMOS LA SEGURIDAD USANDO LA CLASE HELPER
builder.Services.AddAuthentication(helper.GetAuthenticateSchema()).AddJwtBearer(helper.GetJwtBearerOptions());

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("SqlAzure");
builder.Services.AddTransient<RepositoryHospital>();
builder.Services.AddDbContext<HospitalContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.MapOpenApi();

app.UseHttpsRedirection();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Api Seguridad Empleados");
    options.RoutePrefix = "";
});
//EN ESTAS DOS, EL ORDEN IMPORTA -> SI LO PONEMOS AL REVÉS NO VA A FUNCIONAR
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
