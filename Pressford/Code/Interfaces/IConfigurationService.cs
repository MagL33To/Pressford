namespace Pressford.Code.Interfaces
{
    public interface IConfigurationService
    {
        int GetLikedArticlesTakeAmount();
        int GetMaxDailyUserLikesAmount();
    }
}