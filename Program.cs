using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test.Repro;
using System.Threading.Tasks;
using System.IO;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json",
                        true,
                        false);

builder.Services.AddGrpc();
builder.Services.AddSingleton<TestService>();
builder.WebHost.ConfigureKestrel(options =>
{
  if (File.Exists("/tmp/srv.sock"))
  {
    File.Delete("/tmp/srv.sock");
  }

  options.ListenUnixSocket("/tmp/srv.sock",
    listenOptions => listenOptions.Protocols = HttpProtocols.Http2);
});
var app = builder.Build();

app.UseGrpcWeb(new GrpcWebOptions
{
  DefaultEnabled = true,
});
app.MapGrpcService<TestService>();
app.UseRouting();
app.Run();

/// <inheritdoc/>
public class TestService : TestSrv.TestSrvBase
{
  /// <inheritdoc/>
  public override Task<TestResponse> Test(TestRequest request, ServerCallContext context)
  {
    return Task.FromResult(new TestResponse{ ResponseString = $"Received {request.TestString}" });
  }
}