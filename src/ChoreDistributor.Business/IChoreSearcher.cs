using ChoreDistributor.Models;

namespace ChoreDistributor.Business
{
    public interface IChoreSearcher
    {
        IList<Chore> FindBestCombinationForWeight(IList<Chore> chores, float choreContributionWeight);
    }
}
