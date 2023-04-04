namespace CambridgeChallenge.Models
{
    public class EventModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Link { get; set; }

        public static int rowsForDate(int numEvents, int numCols)
        {
            return numEvents / numCols + (numEvents % numCols == 0 ? 0 : 1);
        }
    }
}
