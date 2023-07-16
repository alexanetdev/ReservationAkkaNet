using Akka.Actor;
using Akka.Cluster.Hosting;
using Akka.Hosting;
using Akka.Remote.Hosting;
using ReservationChallenge.Data.EF;
using ReservationChallenge.Service;
using Microsoft.EntityFrameworkCore;
using ReservationChallenge.Data.Repositories;
using Akka.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAkka("ReservationChallenge", configurationBuilder =>
{
    configurationBuilder
    .WithRemoting("localhost", 8110)
    .WithClustering(new ClusterOptions()
    {
        Roles = new[] { "reservationChallenge" },
        SeedNodes = new[] { "akka.tcp://ReservationChallenge@localhost:8110" }
    })
    .WithActors((system, registry) =>
    {
        var providerScheduleSlot = system.ActorOf(DependencyResolver.For(system).Props<ReservationChallengeActor>(), "reservationChallenge");
        registry.TryRegister<ReservationChallengeActor>(providerScheduleSlot);
    });
});

builder.Services.AddControllers();

builder.Services.AddDbContext<IReservationChallengeDbContext, ReservationChallengeContext>(options =>
{
    options.UseSqlite("{secretGoesHere}");
});

builder.Services.AddSingleton<IProviderScheduleRepository, ProviderScheduleRepository>();
builder.Services.AddSingleton<IProviderRepository, ProviderRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    app.MapControllers();
});

app.Run();
