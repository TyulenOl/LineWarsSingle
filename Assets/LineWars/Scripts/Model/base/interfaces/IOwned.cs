namespace LineWars.Model
{
    public interface IOwned
    {
        /// <summary>
        /// -1 означает что владельца нет 
        /// </summary>
        public int OwnerId { get; } 
        public void ConnectTo(int basePlayerID);
    }
}