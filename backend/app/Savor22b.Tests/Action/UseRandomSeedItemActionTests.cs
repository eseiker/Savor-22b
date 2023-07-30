namespace Savor22b.Tests.Action;

using System;
using Libplanet.Crypto;
using Libplanet.State;
using Xunit;
using Savor22b.Action;
using Savor22b.States;
using Libplanet;

public class UseRandomSeedItemActionTests : ActionTests
{
    public UseRandomSeedItemActionTests()
    {
    }

    [Fact]
    public void UseRandomSeedItemActionExecute_AddsSeedToSeedStateList()
    {
        var seedStateID = Guid.NewGuid();
        IAccountStateDelta state = new DummyState();
        var random = new DummyRandom(1);
        InventoryState beforeInventoryState = new InventoryState();

        var itemState = new ItemState(Guid.NewGuid(), 1);
        beforeInventoryState = beforeInventoryState.AddItem(itemState);
        state = state.SetState(SignerAddress(), beforeInventoryState.Serialize());

        var action = new UseRandomSeedItemAction(seedStateID, itemState.StateID);

        state = action.Execute(new DummyActionContext
        {
            PreviousStates = state,
            Signer = SignerAddress(),
            Random = random,
            Rehearsal = false,
            BlockIndex = 1,
        });

        var inventoryStateEncoded = state.GetState(SignerAddress());
        InventoryState inventoryState =
            inventoryStateEncoded is Bencodex.Types.Dictionary bdict
                ? new InventoryState(bdict)
                : throw new Exception();

        Assert.Equal(inventoryState.SeedStateList.Count, 1);
        Assert.Equal(inventoryState.ItemStateList.Count, 0);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(10000)]
    public void UseRandomSeedItemActionExecute_AddsSeedStateToExistsSeedsList(int existsSeedsListLength)
    {
        IAccountStateDelta state = new DummyState();
        InventoryState beforeInventoryState = new InventoryState();
        var itemState = new ItemState(Guid.NewGuid(), 1);

        for (int i = 0; i < existsSeedsListLength; i++)
        {
            var newSeed = new SeedState(Guid.NewGuid(), 1);
            beforeInventoryState = beforeInventoryState.AddSeed(newSeed);
        }

        beforeInventoryState = beforeInventoryState.AddItem(itemState);
        state = state.SetState(SignerAddress(), beforeInventoryState.Serialize());

        var random = new DummyRandom(1);

        var action = new UseRandomSeedItemAction(Guid.NewGuid(), itemState.StateID);

        state = action.Execute(new DummyActionContext
        {
            PreviousStates = state,
            Signer = SignerAddress(),
            Random = random,
            Rehearsal = false,
            BlockIndex = 1,
        });

        var afterInventoryStateEncoded = state.GetState(SignerAddress());
        InventoryState afterInventoryState =
            afterInventoryStateEncoded is Bencodex.Types.Dictionary bdict
                ? new InventoryState(bdict)
                : throw new Exception();

        Assert.Equal(existsSeedsListLength + 1, afterInventoryState.SeedStateList.Count);
        Assert.Equal(afterInventoryState.ItemStateList.Count, 0);
    }
}