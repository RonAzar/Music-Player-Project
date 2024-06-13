using Microsoft.AspNetCore.Mvc;
using MusicProjectServer.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicProjectServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        // GET: api/<SongsController>
        [HttpGet]
        [Route("GetSongsByName")]
        public List<Song> GetSongsByName(string songName)
        {
            return Song.GetSongsByName(songName);
        }

        [HttpGet]
        [Route("GetRandomSongByArtist")]
        public Song GetRandomSongByArtist(string artistName)
        {
            return Song.GetRandomSongByArtist(artistName);
        }

        [HttpGet]
        [Route("Get3RandomsSongs")]
        public List<Song> Get3RandomsSongs(string artistName)
        {
            return Song.Get3RandomsSongs(artistName);
        }

        [HttpGet]
        [Route("GetAllSongs")]
        public List<Song> GetAllSongs()
        {
            return Song.GetAllSongs();
        }

        [HttpGet]
        [Route("GetRandomSong")]
        public Song GetRandomSong()
        {
            return Song.GetRandomSong();
        }

        [HttpGet]
        [Route("GetAllSongsByArtistName")]
        public List<Song> GetAllSongsByArtistName(string artistName)
        {
            return Song.GetAllSongsByArtistName(artistName);
        }


        [HttpGet]
        [Route("GetAllSongsByLyrics")]
        public List<Song> GetAllSongsByLyrics(string lyrics)
        {
            return Song.GetAllSongsByLyrics(lyrics);
        }

        [HttpGet]
        [Route("GetSongPopularityBySongId")]
        public int GetSongPopularityBySongId(int songId)
        {
            return Song.GetSongPopularityBySongId(songId);
        }


        // POST api/<SongsController>
        [HttpPost]
        [Route("AddSong")]
        public bool AddSong(Song sng)
        {
            return Song.AddSong(sng);
        }

        [HttpDelete]
        [Route("DeleteAllArtistsAndSongs")]
        public bool DeleteAllArtistsAndSongs()
        {
            return Song.DeleteAllArtistsAndSongs();
        }
    }
}
