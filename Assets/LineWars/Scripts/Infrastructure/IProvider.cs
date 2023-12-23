namespace LineWars.Model
{
    public interface IProvider<T>: ISaver<T>, IDownloader<T>, IAllDownloader<T>
    {
        
    }
}