namespace UDV_VK_Test_task.Controllers.Dto.Responces
{
    /// <summary>
    /// Класс обёртка ответа получения статистики вхождения букв в 5 постов с личной страницы пользователя Vk
    /// </summary>
    public class GetWallStatisticsResponce
    {
        /// <summary>
        /// Ссылка на страницу
        /// </summary>
        public string WallUrl { get; set; }

        /// <summary>
        /// Статистика вхождения букв в 5 постов с личной страницы пользователя Vk
        /// </summary>
        public Dictionary<char, long> Statistics { get; set; }
    }
}
