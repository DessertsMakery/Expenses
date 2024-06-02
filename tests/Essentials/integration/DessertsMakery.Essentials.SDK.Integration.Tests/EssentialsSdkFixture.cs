using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace DessertsMakery.Essentials.SDK.Integration.Tests;

public sealed class EssentialsSdkFixture : SdkFixture
{
    protected override Assembly TestAssembly => typeof(EssentialsSdkFixture).Assembly;

    protected override void ConfigureServices(IServiceCollection services) => services.AddEssentialsSdk(Configuration);
}
