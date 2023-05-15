namespace UDV_VK_Test_task.Controllers.Dto.Responces
{
    /// <summary>
    /// Класс обёртка ответа получения информации о сохранённой личной странице пользователя Vk
    /// </summary>
    public class GetWallResponce
    {
        /// <summary>
        /// Id личной странице пользователя Vk
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Ссылка на личную страницу пользователя Vk
        /// </summary>
        public string Url { get; set; }
    }
}
