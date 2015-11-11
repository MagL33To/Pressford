using System.Configuration;
using Pressford.Code.Interfaces;

namespace Pressford.Code
{
    public class ConfigurationService : IConfigurationService
    {
        public int GetLikedArticlesTakeAmount()
        {
            return int.Parse(ConfigurationManager.AppSettings["LikedArticlesTakeAmount"]);
        }

        public int GetMaxDailyUserLikesAmount()
        {
            return int.Parse(ConfigurationManager.AppSettings["MaxDailyUserLikes"]);
        }
    }
}