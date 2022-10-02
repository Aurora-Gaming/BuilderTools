using System.Timers;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;
using Timer = System.Timers.Timer;

namespace buildertools;

[ApiVersion(2, 1)]
public class BuilderTools : TerrariaPlugin
{
    public override string Name => "Buildertools";
    public override string Description => "Provides tools commonly used by builders to the player.";
    public override string Author => "Moon";
    public override Version Version => base.Version;

    public List<int> Items = new();

    public Configuration Config;
    public int cooldown;
    public Timer _timer = new(1000);

    public BuilderTools(Main game) : base(game)
    {
        Order = 1;
    }

    public override void Initialize()
    {
        ServerApi.Hooks.GameInitialize.Register(this, OnInitialize);
        GeneralHooks.ReloadEvent += OnReload;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ServerApi.Hooks.GameInitialize.Deregister(this, OnInitialize);
            GeneralHooks.ReloadEvent -= OnReload;
        }
        base.Dispose(disposing);
    }

    void OnInitialize(EventArgs args)
    {
        Commands.ChatCommands.Add(new Command("buildertools.allowed", Command, "buildertools", "btools"));
        Config = Configuration.Read();

        cooldown = Config.Cooldown;
        _timer.Elapsed += OnSecond;
        _timer.Start();
    }

    void OnReload(ReloadEventArgs args)
    {
        Config = Configuration.Read();
    }

    void OnSecond(object source, ElapsedEventArgs args)
    {
        cooldown--;
    }

    void Command(CommandArgs args)
    {
        if (cooldown <= 0)
        {
            args.Player.SendSuccessMessage($"Gave building tools to {args.Player.Name}.");
            Items = Config.Items;
            for (int a = 0; a < Items.Count / 2; a++) // without the Items.Count / 2, it gives the items twice, funny i know
            {
                args.Player.GiveItem(Items[a], 1, 0);
            }

            args.Player.GiveItem(849, 999, 0);
            args.Player.GiveItem(530, 999, 0);
            cooldown = Config.Cooldown;
        }
        else
        {
            args.Player.SendErrorMessage($"You have {cooldown} seconds until you can use this command again.");
            return;
        }
    }
}
