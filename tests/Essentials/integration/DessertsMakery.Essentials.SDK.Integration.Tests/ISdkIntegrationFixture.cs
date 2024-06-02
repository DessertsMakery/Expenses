namespace DessertsMakery.Essentials.SDK.Integration.Tests;

public interface ISdkIntegrationFixture<TSdk> : IClassFixture<TSdk>
    where TSdk : SdkFixture;
