namespace UDV_VK_Test_task.Services.VkAPIService.Interfaces
{
    public interface IVkAPIService
    {
        /// <summary>
        /// Получение текста 5 постов со страницы пользователя или сообщества
        /// </summary>
        /// <param name="domain">страница пользователя или сообщества</param>
        /// <returns>текст 5 постов</returns>
        public List<string> GetWallPostTexts(string domain);
    }
}
