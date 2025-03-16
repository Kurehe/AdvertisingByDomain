using DataAccessLayer.Entities;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer
{
    public class DomainRepository
    {
        private readonly ILogger<DomainRepository> logger;
        
        /// <summary>
        /// Корни дерева
        /// </summary>
        private Dictionary<string, Domain> TreePlatform = new Dictionary<string, Domain>();
        /// <summary>
        /// Результат поиска платформ по дереву
        /// </summary>
        private List<string> SearchResultPlatforms = new List<string>();
        
        public DomainRepository(ILogger<DomainRepository> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Очистка дерева от старых данных
        /// </summary>
        public void ClearRepository()
        {
            TreePlatform.Clear();
        }

        /// <summary>
        /// Метод добавления рекламной площадок по её локации
        /// </summary>
        /// <param name="locations">локация платформы (первый элемент - корень, последующие ветви)</param>
        /// <param name="platform">рекламная площадка</param>
        public void Add(string[] locations, string platform)
        {
            if (TreePlatform.ContainsKey(locations[0]))
            {
                AddPlatformOnLocation(platform, TreePlatform[locations[0]], locations);
            }
            else
            {
                Domain domain = new Domain();
                TreePlatform.Add(locations[0], domain);
                AddPlatformOnLocation(platform, domain, locations);
            }
        }

        /// <summary>
        /// Рекурсивное добавление узлов в дерево с добавлением платформы по конкретному пути
        /// </summary>
        /// <param name="platform">Рекламная площадка</param>
        /// <param name="domain">Узел для поиска и добавления</param>
        /// <param name="locations">Локации</param>
        /// <param name="layer">Уровень вложенности</param>
        private void AddPlatformOnLocation(string platform, Domain domain, string[] locations, int layer = 0)
        {
            if (layer + 1 < locations.Length)
            {
                layer++;
            }
            else // Если достигнут последний уровень вложенности, то следует добавить площадку
            {
                domain.Platforms.Add(platform);
                return;
            }

            Domain next;
            // Ключ объекта в словаре является именем домена по которому и происходит обращение.
            if (domain.ChildDomains.ContainsKey(locations[layer]))
            {
                next = domain.ChildDomains[locations[layer]];
                AddPlatformOnLocation(platform, next, locations, layer);    // спускаемся на уровень ниже по дереву
            }
            else
            {
                next = new Domain();
                domain.ChildDomains.Add(locations[layer], next);
                AddPlatformOnLocation(platform, next, locations, layer);
            }
        }

        /// <summary>
        /// Получение рекламных площадок по локации
        /// </summary>
        /// <param name="locations">Локации</param>
        /// <returns>Возвращает список платформ</returns>
        public List<string> GetPlatformsByLocation(string[] locations)
        {
            SearchResultPlatforms = new List<string>();

            if (TreePlatform.ContainsKey(locations[0]))
            {
                GetPlatform(locations, TreePlatform[locations[0]]);
            }

            return SearchResultPlatforms;
        }

        /// <summary>
        /// Рекурсивное прохождение по дереву и добавление результатов в глобальную переменную класса
        /// </summary>
        /// <param name="locations">Локации</param>
        /// <param name="domain">Узел дерева</param>
        /// <param name="layer">Уровень вложенности</param>
        private void GetPlatform(string[] locations, Domain domain, int layer = 0)
        {
            if (layer + 1 < locations.Length)
            {
                layer++;
                SearchResultPlatforms.AddRange(domain.Platforms); // собираем результаты с каждого уровня дерева
            }
            else
            {
                SearchResultPlatforms.AddRange(domain.Platforms);
                return; // здесь мы возвращаем с самого нижнего уровня дерева, платформы
            }

            if (domain.ChildDomains.ContainsKey(locations[layer]))
            {
                Domain next = domain.ChildDomains[locations[layer]];
                GetPlatform(locations, next, layer);
            }
        }
    }
}
