
namespace BookingService
{
    public class Plan
    {
        public string KundeNavn { get; set; }
        public DateTime OpsamlingsTid { get; set; }
        public string Startsted { get; set; }
        public string Endested { get; set; }

        public Plan(string kundeNavn, DateTime opsamlingsTid, string startsted, string endested)
        {
            KundeNavn = kundeNavn;
            OpsamlingsTid = opsamlingsTid;
            Startsted = startsted;
            Endested = endested;
        }
    }
}