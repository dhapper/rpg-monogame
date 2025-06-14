using Microsoft.Xna.Framework;

public class ShopSystem
{
    public string Line;
    // public Entity[] Options;
    public ItemConfig[] Options;

    // private EntityManager _entityManager;
    

    public ShopSystem()
    {
        // _entityManager = entityManager;

        InitShop(
            "Need for seed?",
            [
                Constants.Items.PumpkinSeeds,
                Constants.Items.PotatoSeeds,
                Constants.Items.WateringCan,
            ]
        );
    }

    public void Update()
    {
        CheckIfHoveringSlot();
    }

    private void CheckIfHoveringSlot()
    {
        
    }

    public void InitShop(string line, ItemConfig[] options)
    {
        Line = line;
        Options = options;

        CreateLayout();
    }

    public Rectangle[] ItemBoxes;
    public Rectangle MenuBox;
    public int Spacer = (int)(2 * Constants.ScaleFactor);

    private void CreateLayout()
    {
        Vector2 textSize = AssetStore.GameFont.MeasureString("abc");
        int yOffset = (int)textSize.Y;
        int width = (int)(100 * Constants.ScaleFactor);
        int height = (int)textSize.Y + Spacer;

        MenuBox = new Rectangle(Constants.TileSize, Constants.TileSize, width + Spacer * 2, yOffset + Options.Length * (height + Spacer));

        ItemBoxes = new Rectangle[Options.Length];
        for (int i = 0; i < Options.Length; i++)
        {
            ItemBoxes[i] = new Rectangle(MenuBox.X + Spacer, MenuBox.Y + yOffset + i * (height + Spacer), width, height);
        }
    }
}