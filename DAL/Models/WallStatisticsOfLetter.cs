namespace UDV_VK_Test_task.DAL.Models
{
    public class WallStatisticsOfLetter
    {
        public long Id { get; set; }
        public Wall Wall { get; set; }
        public char Letter { get; set; }

        public long Count { get; set; }
    }
}
