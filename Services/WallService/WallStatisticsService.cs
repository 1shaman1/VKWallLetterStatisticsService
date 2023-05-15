using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UDV_VK_Test_task.DAL;
using UDV_VK_Test_task.DAL.Models;
using UDV_VK_Test_task.Services.VkAPIService.Interfaces;
using UDV_VK_Test_task.Services.WallService.Interfaces;

namespace UDV_VK_Test_task.Services.WallService
{
    /// <summary>
    /// Сервис подсчёта статистики вхождения одинаковых букв в 5 постов личной страницы пользователя Vk
    /// </summary>
    public class WallStatisticsService : IWallStatisticsService
    {
        private readonly IVkAPIService _vkAPIService;
        private readonly VKWallDbContext _wallDbContext;
        private readonly ILogger<WallStatisticsService> _logger;

        /// <summary>
        /// Конструктор сервиса
        /// </summary>
        /// <param name="vkAPIService">Сервис Vk API</param>
        /// <param name="wallDbContext">Контекст БД</param>
        public WallStatisticsService(IVkAPIService vkAPIService, VKWallDbContext wallDbContext, ILogger<WallStatisticsService> logger)
        {
            _vkAPIService = vkAPIService;
            _wallDbContext = wallDbContext;
            _logger = logger;
        }

        /// <summary>
        /// Подсчёт символов находящихся на странице пользователя и добавление результатов подсчёта в БД
        /// </summary>
        /// <param name="wallUrl">ссылка на личную страницу пользователя</param>
        /// <returns>Id стены пользователя в БД</returns>
        public async Task<long> AddWall(string wallUrl)
        {
            var newWall = new Wall() { Url = wallUrl };
            var createdWall = await _wallDbContext.Walls.AddAsync(newWall);

            
            var statistics = MakeWallLetterStatistics(createdWall.Entity);
            
            foreach (var pair in statistics)
            {
                _wallDbContext.WallStatisticsOfLetters.AddAsync(pair.Value);
            }
            await _wallDbContext.SaveChangesAsync();
            return createdWall.Entity.Id;
        }

        /// <summary>
        /// Удаление информации и статистики о стене пользователя из БД
        /// </summary>
        /// <param name="wallId">Id стены</param>
        public async Task DeleteWall(long wallId)
        {
            var wall = await _wallDbContext.Walls.FindAsync(wallId);
            _wallDbContext.Walls.Remove(wall);
            await _wallDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Получение из БД записей о всех сохранённых стенах личных страниц VK
        /// </summary>
        /// <returns>Стены личных страниц</returns>
        public async Task<List<Wall>> GetAllWalls()
        {
            return _wallDbContext.Walls.ToList();
        }

        /// <summary>
        /// Получение из БД статистики вхождения одинаковых букв в 5 последних постах со стены личной страницы пользователя
        /// </summary>
        /// <param name="wallId">Id стены личной страницы пользователя</param>
        /// <returns>Пару ссылка, статистика вхождения букв</returns>
        /// <exception cref="ArgumentException">Исключение в случае отсутствия в БД стены с таким Id</exception>
        public async Task<Tuple<string, List<WallStatisticsOfLetter>>> GetWallStatistics(long wallId)
        {
            var wall = await _wallDbContext.Walls.FindAsync(wallId);
            if(wall == null)
            {
                throw new ArgumentException("wall whith this id not exist");
            }

            var result = _wallDbContext.WallStatisticsOfLetters.Include(stat => stat.Wall).Where(statistics => statistics.Wall.Id == wallId)
                .OrderBy(statistics => statistics.Letter).ToList();
            
            return Tuple.Create(wall.Url, result);
        }

        /// <summary>
        /// Обновление статистики вхождения одинаковых букв в последнии 5 постов на стене личной страницы пользователя
        /// </summary>
        /// <param name="wallId">Id стены пользователя</param>
        /// <returns>Пара ссылка, обновлённая статистика стены пользователя</returns>
        public async Task<Tuple<string, List<WallStatisticsOfLetter>>> UpdateWallStatistics(long wallId)
        {
            var wall = await _wallDbContext.Walls.FindAsync(wallId);
            if (wall == null)
            {
                throw new ArgumentException("wall whith this id not exist");
            }

            var newStatistics = MakeWallLetterStatistics(wall);
            var statisticsToRemove = new List<WallStatisticsOfLetter>();
            var oldStatistics = _wallDbContext.WallStatisticsOfLetters.Include(stat => stat.Wall)
                .Where(statistics => statistics.Wall.Id == wallId)
                .OrderBy(statistics => statistics.Letter).ToList();

            

            foreach (var stat in oldStatistics)
            {
                if(!newStatistics.ContainsKey(stat.Letter))
                {
                    statisticsToRemove.Add(stat);
                    continue;
                }
                stat.Count = newStatistics[stat.Letter].Count;
            }

            _wallDbContext.UpdateRange(oldStatistics);
            _wallDbContext.SaveChanges();
            _wallDbContext.RemoveRange(statisticsToRemove);
            _wallDbContext.SaveChanges();

            return Tuple.Create(wall.Url, newStatistics.Values.OrderBy(letterStatistics => letterStatistics.Letter).ToList());
        }

        /// <summary>
        /// Получение идентификатора стены личной страницы пользователя в БД
        /// </summary>
        /// <param name="wallUrl">Ссылка на личную страницу пользователя</param>
        /// <returns>id стены пользователя в БД</returns>
        public async Task<long> TakeWallId(string wallUrl)
        {
            var wall = await _wallDbContext.Walls.FirstAsync(wall => wall.Url.Equals(wallUrl));
            if(wall == null)
            {
                throw new ArgumentException("Такой ссылки на личную страницу пользователя нет в БД");
            }
            return wall.Id;
        }

        /// <summary>
        /// Метод получения из ссылки сокращённого имени пользователя
        /// </summary>
        /// <param name="wallUrl">ссылка на личную страницу пользователя</param>
        /// <returns>домен - сокращённое имя пользователя</returns>
        /// <exception cref="ArgumentException">Исключение при неправильном формате ссылки личной страницы пользователя</exception>
        private static string TakeWallDomain(string wallUrl)
        {
            var validRegex = new Regex("^https?:\\/\\/vk\\.com\\/[-a-zA-Z0-9@:%._\\+~#=]{1,256}");
            var cropRegex = new Regex("^https?:\\/\\/vk\\.com\\/");
            if (!validRegex.IsMatch(wallUrl))
            {
                throw new ArgumentException("Неправильный формат ссылки на личную страницу пользователя");
            }
            var domain = cropRegex.Replace(wallUrl, "");
            return domain;
        }

        /// <summary>
        /// Сбор статистики по вхождению одинаковых букв в записях стены
        /// </summary>
        /// <param name="wall">Стена личной страницы пользователя</param>
        /// <returns>Словарь - статистика вхождения одинаковых букв: ключ - буква, значение - модель обёртка с подсчитанным кол-вом вхождений</returns>
        private Dictionary<char, WallStatisticsOfLetter> MakeWallLetterStatistics(Wall wall)
        {
            _logger.Log(LogLevel.Information, "Старт подсчёта вхождения одинаковых букв");
            var dict = new Dictionary<char, WallStatisticsOfLetter>();
            var posts = _vkAPIService.GetWallPostTexts(TakeWallDomain(wall.Url));
            foreach (var post in posts)
            {
                foreach (var simbol in post)
                {
                    if (!char.IsLetter(simbol))
                    {
                        continue;
                    }
                    var letter = char.ToLower(simbol);
                    if (!dict.ContainsKey(letter))
                    {
                        dict.Add(letter, new WallStatisticsOfLetter() { Wall = wall, Letter = letter, Count = 1 });
                    }
                    else
                    {
                        dict[letter].Count++;
                    }
                }
            }
            _logger.Log(LogLevel.Information, "Конец подсчёта вхождения одинаковых букв");
            return dict;
        }
    }
}
