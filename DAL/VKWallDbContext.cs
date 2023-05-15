using Microsoft.EntityFrameworkCore;
using UDV_VK_Test_task.DAL.Models;

namespace UDV_VK_Test_task.DAL
{
    /// <summary>
    /// Класс контекста подключения к БД со статистикой вхождения одинаковых букв
    /// </summary>
    public class VKWallDbContext : DbContext
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public VKWallDbContext(DbContextOptions<VKWallDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Таблица стен личных страниц пользователей
        /// </summary>
        public DbSet<Wall> Walls { get; set; }

        /// <summary>
        /// Таблица статистики вхождения одинаковых букв в 5 последних постов личных страниц пользователей
        /// </summary>
        public DbSet<WallStatisticsOfLetter> WallStatisticsOfLetters { get; set; }
    }
}
