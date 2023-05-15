using Microsoft.AspNetCore.Mvc;
using UDV_VK_Test_task.Controllers.Dto;
using UDV_VK_Test_task.Controllers.Dto.Requests;
using UDV_VK_Test_task.Controllers.Dto.Responces;
using UDV_VK_Test_task.Controllers.Mapper;
using UDV_VK_Test_task.Services.WallService.Interfaces;

namespace UDV_VK_Test_task.Controllers
{
    /// <summary>
    /// Контроллер для работы со статистикой вхождения букв в 5 последних постов личной страницы пользователя Vk
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VKWallController : ControllerBase
    {
        /// <summary>
        /// Сервис для работы со статистикой вхождения букв в 5 последних постов личной страницы пользователя Vk
        /// </summary>
        private readonly IWallStatisticsService _wallStatisticsService;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="wallStatisticsService">Сервис для работы со статистикой</param>
        public VKWallController(IWallStatisticsService wallStatisticsService)
        {
            _wallStatisticsService = wallStatisticsService;
        }

        /// <summary>
        /// REST-запрос для получения статистики вхождения букв в 5 последних постов личной страницы пользователя Vk
        /// </summary>
        /// <param name="id">Id личной страницы пользователя</param>
        /// <returns>Статистика</returns>
        [HttpGet("getWallStatistics/{id}")]
        public async Task<GetWallStatisticsResponce> GetWallStisitics([FromRoute] long id)
        {
            var result = await _wallStatisticsService.GetWallStatistics(id);
            return WallMapper.MapWallStatisticsToGetWallStatisticsResponce(result);
        }

        /// <summary>
        /// REST-запрос для получения всех обработанных личных страниц пользователей Vk
        /// </summary>
        /// <returns>Id и ссылки на личные страницы</returns>
        [HttpGet("getAllWalls/")]
        public async Task<List<GetWallResponce>> GetAllWalls()
        {
            var walls = await _wallStatisticsService.GetAllWalls();
            var result = WallMapper.mapWallToGetWallResponce(walls);
            return result;
        }

        /// <summary>
        /// REST-запрос для добавления и обработки личной страницы пользователя Vk
        /// </summary>
        /// <param name="wall">ссылка на стену пользователя</param>
        /// <returns>Id записи стены пользователя в БД</returns>
        [HttpPost("addWall")]
        public async Task<long> AddWall([FromBody] AddWallRequest wall)
        {
            var result = await _wallStatisticsService.AddWall(wall.WallUrl);
            return result;
        }

        /// <summary>
        /// REST-запрос для обновления статистики обработанной личной страницы пользователя Vk
        /// </summary>
        /// <param name="id">Id страницы пользователя в БД</param>
        /// <returns>Обновлённая статистика</returns>
        [HttpPut("updateWallStatistics/{id}")]
        public async Task<GetWallStatisticsResponce> UpdateWallStatistics([FromRoute] int id)
        {
            var result = await _wallStatisticsService.UpdateWallStatistics(id);
            return WallMapper.MapWallStatisticsToGetWallStatisticsResponce(result);
        }

        /// <summary>
        /// REST-запрос для получения Id обработанной личной страницы пользователя Vk по ссылке
        /// </summary>
        /// <param name="url">ссылка на страницу пользователя</param>
        /// <returns>Id записи стены пользователя в БД</returns>
        [HttpPut("takeWallId/")]
        public async Task<long> TakeWallId([FromBody] string url)
        {
            var wallId = await _wallStatisticsService.TakeWallId(url);
            return wallId;
        }

        /// <summary>
        /// REST-запрос для удаления записи о личной странице пользователя Vk и её статистике
        /// </summary>
        /// <param name="id">Id стены пользователя в БД</param>
        [HttpDelete("deleteWallStistics/{id}")]
        public async Task<IActionResult> DeleteWallStistics([FromRoute] int id)
        {
            await _wallStatisticsService.DeleteWall(id);
            return Ok();
        }
    }
}
