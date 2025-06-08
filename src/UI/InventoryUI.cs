using System.Drawing;
using System.Linq;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;

public class InventoryUI
{

    public Vector2[] HotbarSlotPositions, HotbarIconPositions;
    public Vector2[][] InventorySlotPositions, InventoryIconPositions;
    public int DefaultSlotSize = 22;

    private Camera2D _camera;
    private Viewport _viewport;
    private EntityManager _entityManager;
    private InventoryComponent _inventory;

    private int rows = 3;
    private int cols = 9;
    private int iconOffset = (int)(3 * Constants.ScaleFactor);


    public InventoryUI(Camera2D camera, Viewport viewport, EntityManager entityManager)
    {
        _camera = camera;
        _viewport = viewport;
        _entityManager = entityManager;
        // _inventory = _entityManager.EntitiesWithComponent<InventoryComponent>().FirstOrDefault().GetComponent<InventoryComponent>();

        HotbarSlotPositions = new Vector2[cols];
        HotbarIconPositions = new Vector2[cols];
        InventorySlotPositions = new Vector2[cols][];
        InventoryIconPositions = new Vector2[cols][];
        for (int i = 0; i < cols; i++)
        {
            InventorySlotPositions[i] = new Vector2[rows];
            InventoryIconPositions[i] = new Vector2[rows];
        }
    }

    public void InitializePlayerInventory()
    {
        _inventory = _entityManager.EntitiesWithComponent<InventoryComponent>().FirstOrDefault().GetComponent<InventoryComponent>();
    }

    public void Update()
    {
        CalculateLayout();
    }

    public Item IsHoveringItem(int x, int y)
    {
        //mouse xy

        //for loop through item vectors in inv & hotbar

        for (int i = 0; i < cols; i++)
        {

            Rectangle bounds = new Rectangle((int)HotbarSlotPositions[i].X, (int)HotbarSlotPositions[i].Y, DefaultSlotSize, DefaultSlotSize);
            if (bounds.Contains(x, y))
                return _inventory.HotbarItems[i];

            for (int j = 0; j < rows; j++)
            {
                bounds = new Rectangle((int)InventorySlotPositions[i][j].X, (int)InventorySlotPositions[i][j].Y, DefaultSlotSize, DefaultSlotSize);
                if (bounds.Contains(x, y))
                    return _inventory.InventoryItems[i][j];
            }
        }


        //if it clicked do something
        return null;

    }

    public void CalculateLayout()
    {
        int defaultSpacing = 20;
        int slotWidth = (int)(defaultSpacing * Constants.ScaleFactor);
        int x = (int)(_camera.Position.X + _viewport.Width / 2 - slotWidth * 9 / 2 - 1 * Constants.ScaleFactor);
        int yHotbar = (int)(_camera.Position.Y + 6 * (Constants.TileSize * Constants.ScaleFactor));
        int yInventory = (int)(_camera.Position.Y + 1 * (Constants.TileSize * Constants.ScaleFactor));

        for (int i = 0; i < cols; i++)
        {
            int xOffset = slotWidth * i;
            HotbarSlotPositions[i] = new Vector2(x + xOffset, yHotbar);
            HotbarIconPositions[i] = new Vector2(HotbarSlotPositions[i].X + iconOffset,
                HotbarSlotPositions[i].Y + iconOffset);
            for (int j = 0; j < rows; j++)
            {
                int yOffset = slotWidth * j;
                InventorySlotPositions[i][j] = new Vector2(x + xOffset, yInventory + yOffset);
                InventoryIconPositions[i][j] = new Vector2(InventorySlotPositions[i][j].X + iconOffset, InventorySlotPositions[i][j].Y + iconOffset);
            }

        }
    }
}