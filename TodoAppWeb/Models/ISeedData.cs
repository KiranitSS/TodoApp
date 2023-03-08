namespace TodoAppWeb.Models
{
    public interface ISeedData
    {
        ISeedData GetSeed();

        void EnsurePopulated(IApplicationBuilder app);
    }
}
