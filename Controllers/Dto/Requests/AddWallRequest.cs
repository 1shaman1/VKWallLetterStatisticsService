namespace UDV_VK_Test_task.Controllers.Dto.Requests
{
    /// <summary>
    /// Класс обёртка запроса добавления и обработки личной страницы пользователя Vk
    /// </summary>
    public class AddWallRequest
    {
        /// <summary>
        /// Ссылка на личную страницы пользователя Vk
        /// </summary>
        public string WallUrl { get; set; }
    }
}
