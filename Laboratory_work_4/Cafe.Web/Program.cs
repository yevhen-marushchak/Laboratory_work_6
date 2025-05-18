using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cafe.BLL.Facades;
using Cafe.BLL.Services;
using Cafe.DAL;
using Cafe.DAL.Entities;
using Cafe.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterType<CafeDbContext>().AsSelf().InstancePerLifetimeScope();
    containerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<GenericRepository<Room>>().As<IGenericRepository<Room>>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<GenericRepository<Reservation>>().As<IGenericRepository<Reservation>>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<GenericRepository<Activity>>().As<IGenericRepository<Activity>>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();

    containerBuilder.RegisterType<RoomService>().As<IRoomService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<ReservationService>().As<IReservationService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();

    containerBuilder.RegisterType<CafeFacade>().AsSelf().InstancePerLifetimeScope();
});

builder.Services.AddDbContext<CafeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews()
    .AddMvcOptions(options =>
    {
        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
            _ => "�� ���� � ����'�������.");
    });

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CafeDbContext>();

    if (!context.Rooms.Any())
    {
        context.Rooms.AddRange(
            new Room { Name = "���� � �������", IsAvailable = true },
            new Room { Name = "���� ��� ����", IsAvailable = true }
        );
        context.SaveChanges();
    }

    if (!context.Activities.Any())
    {
        context.Activities.AddRange(
            new Activity { Name = "�������� ������", Description = "�������� ������ �� �������� �����" },
            new Activity { Name = "�������� ��䳿 �� �����", Description = "�������� ����� �� �������� �����" },
            new Activity { Name = "������� ����", Description = "����� � ������� ����" },
            new Activity { Name = "������ ���������", Description = "����� �� ���������" }
        );
        context.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();