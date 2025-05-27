using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace rpg;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    EntityManager _entityManager;
    RenderSystem _renderSystem;
    InputSystem _inputSystem;
    AnimationSystem _animationSystem;

    SpriteFont gameFont;

    private Entity player;
    private PlayerController _playerController;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _entityManager = new EntityManager();

        _inputSystem = new InputSystem();

        // Load
        gameFont = Content.Load<SpriteFont>("gameFont");
        Texture2D playerTexture = Content.Load<Texture2D>("player");

        // Create entity
        player = _entityManager.CreateEntity();
        player.AddComponent(new Position(100, 100));
        int spriteWidth = 19;
        int spriteHeight = 21;
        player.AddComponent(new Sprite(playerTexture, new Rectangle(0 * spriteWidth, 7 * spriteHeight, spriteWidth, spriteHeight)) { Color = Color.Red });
        _playerController = new PlayerController(player, _inputSystem, 4);

        int totalFrames = 4;
        float frameDuration = 1f;
        // Add the Animation component
        player.AddComponent(new Animation(totalFrames, frameDuration));


        // Initialize systems
        _renderSystem = new RenderSystem(_spriteBatch, _entityManager);
        _animationSystem = new AnimationSystem(_entityManager);
    }

    protected override void Update(GameTime gameTime)
    {

        _inputSystem.Update();
        _playerController.Update();

        _animationSystem.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _renderSystem.Draw();

        base.Draw(gameTime);
    }
}
