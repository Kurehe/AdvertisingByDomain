namespace DataAccessLayer.Entities
{
    /// <summary>
    /// Реализация дерева, точнее одного узла
    /// </summary>
    internal class Domain
    {
        /// <summary>
        /// Рекламные площадки
        /// </summary>
        public List<string> Platforms { get; set; } = new List<string>();
        /// <summary>
        /// Словарь под домены
        /// </summary>
        public Dictionary<string, Domain> ChildDomains { get; set; } = new Dictionary<string, Domain>();
    }
}
