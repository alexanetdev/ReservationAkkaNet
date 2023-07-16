namespace ReservationChallenge.Dto
{
    public class ClientDto
    {
        public Guid Id { get; set; }

        //Added to fill out perceived record
        public string Name { get; set; }

        //Additional properties continue if required

        public ClientDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
