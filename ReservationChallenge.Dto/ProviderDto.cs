namespace ReservationChallenge.Dto
{
    public class ProviderDto
    {
        public Guid Id { get; set; }

        //Added to fill out perceived record
        public string Name { get; set; }

        //Additional properties continue if required

        public ProviderDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
