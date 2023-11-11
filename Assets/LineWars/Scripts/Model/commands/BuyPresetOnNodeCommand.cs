namespace LineWars.Model
{
    public class BuyPresetOnNodeCommand : ICommand
    {
        public BasePlayer Player { get; private set; }
        public Node Node {  get; private set; } 
        public UnitBuyPreset Preset { get; private set; }

        public BuyPresetOnNodeCommand(BasePlayer basePlayer, Node node, UnitBuyPreset preset)
        {
            Player = basePlayer;
            Node = node;
            Preset = preset;
        }

        public bool CanExecute()
        {
            return Player.CanBuyPreset(Preset, Node);
        }

        public void Execute()
        {
            Player.BuyPreset(Preset, Node);
        }

        public string GetLog()
        {
            return $"Игрок {Player} купил {Preset.Name} в ноде {Node}";
        }
    }
}
