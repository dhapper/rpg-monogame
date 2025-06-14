public class ShopSystem
{
    public string Line;
    public Entity[] Options;

    public ShopSystem(EntityManager entityManager, AssetStore assets)
    {
        InitShop(
            "Need for seed?",
            [
                Constants.Items.PumpkinSeeds,
                Constants.Items.PotatoSeeds,
            ]
        );
    }

    public void InitShop(string line, ItemConfig[] options)
    {
        Line = line;
        // Options = ItemFactory.CreateItem(options, );
    }
}