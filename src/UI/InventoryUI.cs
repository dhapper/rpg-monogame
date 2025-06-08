using System.Numerics;
using Microsoft.Xna.Framework.Graphics;

public class InventoryUI
{

    public Vector2[] HotbarSlotPositions, HotbarIconPositions;
    public Vector2[][] InventorySlotPositions, InventoryIconPositions;
    public int DefaultSlotSize = 22;

    private Camera2D _camera;
    private Viewport _viewport;

    private int rows = 3;
    private int cols = 9;
    private int iconOffset = (int)(3 * Constants.ScaleFactor);


    public InventoryUI(Camera2D camera, Viewport viewport)
    {
        _camera = camera;
        _viewport = viewport;

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

    public void Update()
    {
        CalculateLayout();
    }

    public void CalculateLayout()
    {
        int defaultSpacing = 20;
        int slotWidth = (int)(defaultSpacing * Constants.ScaleFactor);
        int x = (int)(_camera.Position.X + _viewport.Width/2 - slotWidth * 9 / 2 - 1 * Constants.ScaleFactor);
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