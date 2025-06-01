using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SpriteProcessor
{

    public static Texture2D ChangeColours(Color[][] colourPairs, Texture2D sheet, GraphicsDevice graphicsDevice)
    {
        int width = sheet.Width;
        int height = sheet.Height;
        Color[] pixels = new Color[width * height];
        sheet.GetData(pixels);

        for (int i = 0; i < colourPairs.Length; i++)
        {
            Color originalColor = colourPairs[i][0];
            Color newColor = colourPairs[i][1];
            for (int p = 0; p < pixels.Length; p++)
                if (pixels[p].Equals(originalColor))
                    pixels[p] = newColor;
        }

        Texture2D newTexture = new Texture2D(graphicsDevice, width, height);
        newTexture.SetData(pixels);
        return newTexture;
    }

    public static Texture2D LayerSheets(Texture2D[] sheets, GraphicsDevice graphicsDevice)
    {
        int width = sheets[0].Width;
        int height = sheets[0].Height;

        RenderTarget2D renderTarget = new RenderTarget2D(graphicsDevice, width, height);
        RenderTargetBinding[] previousRenderTargets = graphicsDevice.GetRenderTargets();

        graphicsDevice.SetRenderTarget(renderTarget);
        graphicsDevice.Clear(Color.Transparent);

        using (SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice))
        {
            spriteBatch.Begin();

            foreach (var sheet in sheets)
                spriteBatch.Draw(sheet, Vector2.Zero, Color.White);

            spriteBatch.End();
        }

        // Reset to previous render target
        // If previousRenderTargets is null or empty, set to null to go back to default back buffer
        if (previousRenderTargets != null && previousRenderTargets.Length > 0)
            graphicsDevice.SetRenderTargets(previousRenderTargets);
        else
            graphicsDevice.SetRenderTarget(null);

        return renderTarget;
    }
}