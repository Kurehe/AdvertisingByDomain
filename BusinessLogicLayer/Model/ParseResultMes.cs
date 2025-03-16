namespace BusinessLogicLayer.Model
{
    public class ParseResultMes
    {
        /// <summary>
        /// Рекламная площадка
        /// </summary>
        public required string Platform { get; set; }
        /// <summary>
        /// локация
        /// </summary>
        public required string[] Location { get; set; }
    }
}
