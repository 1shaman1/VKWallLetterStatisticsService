using UDV_VK_Test_task.Controllers.Dto.Responces;
using UDV_VK_Test_task.DAL.Models;

namespace UDV_VK_Test_task.Controllers.Mapper
{
    /// <summary>
    /// Маппер для данных полученных при работе с сервисом сбора статистики вхождения одинаковых букв с 5 постов стены пользователя
    /// </summary>
    public static class WallMapper
    {
        public static List<GetWallResponce> mapWallToGetWallResponce(List<Wall> walls)
        {
            return walls.Select(wall => new GetWallResponce() { Id = wall.Id, Url = wall.Url }).ToList();
        }

        public static GetWallStatisticsResponce MapWallStatisticsToGetWallStatisticsResponce(Tuple<string, List<WallStatisticsOfLetter>> statistics)
        {
            var responce = new GetWallStatisticsResponce() { Statistics = new Dictionary<char, long>()};
            responce.WallUrl = statistics.Item1;
            foreach(var letterStatistics in statistics.Item2)
            {
                responce.Statistics.Add(letterStatistics.Letter, letterStatistics.Count);
            }
            return responce;
        }
    }
}
