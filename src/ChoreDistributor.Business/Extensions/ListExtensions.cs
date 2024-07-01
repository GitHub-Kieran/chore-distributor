namespace ChoreDistributor.Business.Extensions
{
    internal static class ListExtensions
    {
        /// <summary>
        /// We need to try every combination of chores to find the fairest combinations
        /// The array [ 0, 1, 2, 3, 4 ] representing chore indexes becomes something like the following:
        /// [ [0], [1], [2], [0, 1], [0, 2], [0, 3], [0, 4], [1, 2], [1, 3], [1, 4], [0, 1, 2], [0, 1, 3], [0, 1, 4], [1, 2, 3], [1, 2, 4], [0, 1, 2, 3] ]   
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements"></param>
        /// <returns>IEnumerable<IEnumerable<T>></returns>
        public static IEnumerable<IEnumerable<T>> GetCombinations<T>(this IList<T> elements)
        {
            return Enumerable.Range(0, 1 << elements.Count)
                .Select(m => Enumerable.Range(0, elements.Count)
                    .Where(i => (m & (1 << i)) != 0)
                    .Select(i => elements[i]));
        }
    }
}
