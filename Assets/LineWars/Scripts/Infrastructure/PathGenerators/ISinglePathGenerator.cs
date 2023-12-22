namespace LineWars.Model
{
    public interface ISinglePathGenerator<T>
    {
        public string GeneratePath(int id);
    }
}