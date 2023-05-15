using UDV_VK_Test_task.DAL;
using UDV_VK_Test_task.DAL.Models;

namespace UDV_VK_Test_task.Services.WallService.Interfaces
{
    /// <summary>
    /// Сервис подсчёта статистики вхождения одинаковых букв в 5 постов личной страницы пользователя Vk
    /// </summary>
    public interface IWallStatisticsService
    {
        /// <summary>
        /// Получение из БД статистики вхождения одинаковых букв в 5 последних постах со стены личной страницы пользователя
        /// </summary>
        /// <param name="wallId">Id стены личной страницы пользователя</param>
        /// <returns>Пару ссылка, статистика вхождения букв</returns>
        /// <exception cref="ArgumentException">Исключение в случае отсутствия в БД стены с таким Id</exception>
        public Task<Tuple<string, List<WallStatisticsOfLetter>>> GetWallStatistics(long wallId);

        /// <summary>
        /// Получение из БД записей о всех сохранённых стенах личных страниц VK
        /// </summary>
        /// <returns>Стены личных страниц</returns>
        public Task<List<Wall>> GetAllWalls();

        /// <summary>
        /// Подсчёт символов находящихся на странице пользователя и добавление результатов подсчёта в БД
        /// </summary>
        /// <param name="wallUrl">ссылка на личную страницу пользователя</param>
        /// <returns>Id стены пользователя в БД</returns>
        public Task<long> AddWall(string wallUrl);

        /// <summary>
        /// Удаление информации и статистики о стене пользователя из БД
        /// </summary>
        /// <param name="wallId">Id стены</param>
        public Task DeleteWall(long wallId);

        /// <summary>
        /// Обновление статистики вхождения одинаковых букв в последнии 5 постов на стене личной страницы пользователя
        /// </summary>
        /// <param name="wallId">Id стены пользователя</param>
        /// <returns>Пара ссылка, обновлённая статистика стены пользователя</returns>
        public Task<Tuple<string, List<WallStatisticsOfLetter>>> UpdateWallStatistics(long wallId);

        /// <summary>
        /// Получение идентификатора стены личной страницы пользователя в БД
        /// </summary>
        /// <param name="wallUrl">Ссылка на личную страницу пользователя</param>
        /// <returns>id стены пользователя в БД</returns>
        public Task<long> TakeWallId(string wallUrl);
    }
}
