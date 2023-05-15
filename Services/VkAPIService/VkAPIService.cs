using UDV_VK_Test_task.Services.VkAPIService.Interfaces;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace UDV_VK_Test_task.Services.VkAPIService
{
    /// <summary>
    /// Сервис для обращения к VK Api
    /// </summary>
    public class VkAPIService : IVkAPIService
    {
        private readonly VkApi api;

        /// <summary>
        /// Конструктор сервиса апи с заранее выданным сервисным ключом
        /// </summary>
        public VkAPIService()
        {
            this.api = new VkApi();
            api.Authorize(new ApiAuthParams 
            {
                AccessToken = "8ee1bc7b8ee1bc7b8ee1bc7be18df5b93088ee18ee1bc7beabef4c7ee3c498d8942e2a9"
            });
        }

        /// <summary>
        /// Получение текста 5 постов со страницы пользователя или сообщества
        /// </summary>
        /// <param name="domain">наименование пользователя или сообщества</param>
        /// <returns>текст 5 постов</returns>
        public List<string> GetWallPostTexts(string domain)
        {
            var posts = api.Wall.Get(new WallGetParams() { Domain = domain, Count = 5}).WallPosts;
            return posts.Select(post => post.Text).ToList();
        }
    }
}
