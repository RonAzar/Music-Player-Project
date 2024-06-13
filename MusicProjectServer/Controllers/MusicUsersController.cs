using Microsoft.AspNetCore.Mvc;
using MusicProjectServer.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicProjectServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicUsersController : ControllerBase
    {
        // POST api/<MusicUsersController>
        [HttpPost]
        [Route("Registration")]
        public bool Registration(MusicUser user)
        {
            return user.Registration();
        }

        

        [HttpGet]
        [Route("CheckIfUserHasFavoriteSong")]
        public bool CheckIfUserHasFavoriteSong(int songId, int userId)
        {
            return MusicUser.CheckIfUserHasFavoriteSong(songId, userId);
        }

       

        [HttpPost]
        [Route("AddToFavorites")]
        public bool AddToFavorites(int userId, int songId)
        {
            return MusicUser.AddToFavorites(userId, songId);
        }

        [HttpPost]
        [Route("RemoveFromFavorites")]
        public bool RemoveFromFavorites(int userId, int songId)
        {
            return MusicUser.RemoveFromFavorites(userId, songId);
        }

        // POST api/<AirBnbUsersController>
        [HttpPost]
        [Route("UpdateUserDetails")]
        public bool UpdateUserDetails(MusicUser user)
        {
            return MusicUser.UpdateUserDetails(user);
        }

        [HttpPost]
        [Route("GetFavorites")]
        public List<Song> GetFavorites(int userId)
        {
            return MusicUser.GetFavorites(userId);
        }

        [HttpPost]
        [Route("LogIn")]
        public MusicUser LogIn(string emailOrUserNameToLogin, string passwordToLogin)
        {
            return MusicUser.LogIn(emailOrUserNameToLogin, passwordToLogin);
        }

        [HttpDelete]
        [Route("DeleteAllUsers")]
        public bool DeleteAllUsers()
        {
            return MusicUser.DeleteAllUsers();
        }

        [HttpPost]
        [Route("InsertScore")]
        public bool InsertScore(Score score)
        {
            return Score.InsertScore(score);
        }

        [HttpGet]
        [Route("GetUserScores")]
        public List<Score> GetUserScores(int userId)
        {
            return Score.GetUserScores(userId);
        }

        [HttpGet]
        [Route("GetTopFiveScoreBoard")]
        public List<Score> GetTopFiveScoreBoard()
        {
            return Score.GetTopFiveScoreBoard();
        }

        [HttpDelete]
        [Route("ResetScoreBoard")]
        public bool ResetScoreBoard()
        {
            return Score.ResetScoreBoard();
        }


    }
}
