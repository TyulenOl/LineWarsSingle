namespace LineWars.Model
{
    public class ContextedDrop
    {
        public readonly Drop Drop;
        public readonly Drop OldDrop;

        public ContextedDrop(Drop drop, Drop oldDrop = null)
        {
            Drop = drop;
            OldDrop = oldDrop;
        }
    }
}