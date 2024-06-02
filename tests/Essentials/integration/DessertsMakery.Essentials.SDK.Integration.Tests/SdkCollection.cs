namespace DessertsMakery.Essentials.SDK.Integration.Tests;

public abstract class SdkCollection<TSdkFixture> : ICollectionFixture<TSdkFixture>
    where TSdkFixture : SdkFixture;
