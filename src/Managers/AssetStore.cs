using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class AssetStore
{
    public SpriteFont GameFont { get; private set; }
    public Texture2D PlayerTexture { get; private set; }

    private ContentManager _content;

    public AssetStore(ContentManager content)
    {
        _content = content;
    }

    public void LoadAll()
    {
        GameFont = _content.Load<SpriteFont>("gameFont");
        // PlayerTexture = _content.Load<Texture2D>("player");
        PlayerTexture = _content.Load<Texture2D>("custom_player_sheet");
    }
}
