namespace LineWars.Model
{
    public struct Hit
    {
        private int damage;

        public Hit(int damage)
        {
            this.damage = damage;
        }
        
        public int Damage => damage;
    }
}