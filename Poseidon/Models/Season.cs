namespace Poseidon.Models
{
    public class Season
    {
        public string Id { get; set; }
        public string PoolId { get; set; }
        public string Name { get; set; }
        public long SeasonStart { get; set; }
        public long? SeasonEnd { get; set; }
    }
}
