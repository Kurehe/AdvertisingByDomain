using BusinessLogicLayer.Model;
using DataAccessLayer;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer
{
    /// <summary>
    /// Слой бизнес логике, который занимается парсингом и выборкой данных
    /// </summary>
    public class DomainServices
    {
        private readonly ILogger<DomainServices> logger;
        private readonly DomainRepository repository;

        public DomainServices(ILogger<DomainServices> logger, DomainRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        /// <summary>
        /// Метод парсинга данных из потока загружаемого файла
        /// </summary>
        /// <param name="stream">Поток файла</param>
        /// <returns>true - если получилось распарсить данные и добавить, false - если возникло исключение</returns>
        public bool ParseAndUpload(Stream stream)
        {
            List<ParseResultMes> parseStr = new List<ParseResultMes>();

            using (var reader = new StreamReader(stream)) // закрыть поток после чтения
            {
                try
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null) // читаем строку
                    {
                        string[] segments = line.Split(':');        // разделить строку на две части рекламная площадка и локации
                        string platform = segments[0];
                        string[] locatins = segments[1].Split(','); // разделить все локации

                        foreach (var item in locatins)
                        {
                            parseStr.Add(new ParseResultMes  // заполнить список платформ и локаций
                            {
                                Platform = platform,
                                Location = item.TrimStart('/').Split("/")   // убираем в начале строки '/' и делим строку на кусочки
                            });
                        }
                    }
                }
                catch (Exception ex) // если вдруг возникла ошибка то вывести её в консоль и очистить список
                {
                    logger.Log(LogLevel.Warning, ex.Message);
                    parseStr.Clear();
                }
            }

            if (parseStr.Count > 0)
            {
                repository.ClearRepository();   // очистка базы данных перед заполнением новыми данными
                foreach (var item in parseStr)
                {
                    repository.Add(item.Location, item.Platform);
                }
                return true;
            }
            else
            {
                logger.Log(LogLevel.Error, "list is empty!");
            }

            return false;
        }

        /// <summary>
        /// Метод парсинга отдельных элементов локации
        /// </summary>
        /// <param name="location">локация</param>
        /// <returns>Возвращает список рекламных площадок</returns>
        public List<string>? ParseLocation(string location)
        {
            string[] ArrayLocations = location.TrimStart('/').Trim(' ').Split('/');

            List<string> Platforms = repository.GetPlatformsByLocation(ArrayLocations);

            if (Platforms.Count > 0)
            {
                return Platforms;
            }

            return null;
        }
    }
}
