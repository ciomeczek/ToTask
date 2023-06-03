using ToTask;
using DotNetEnv;

Env.Load();

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    });

builder.Build().Run();