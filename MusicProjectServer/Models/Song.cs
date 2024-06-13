namespace MusicProjectServer.Models
{
    public class Song
    {
        public int SongId { get; set; }
        public string SongName { get; set; }
        public string Lyrics { get; set; }
        public string Link { get; set; }
        public string ArtistName { get; set; }

        static DBservices dBservices = new DBservices();

        public Song()
        {
            SongId = 0;
            SongName = "";
            Lyrics = "";
            Link = "";
            ArtistName = "";
        }

        public Song(int songId, string songName, string lyrics, string link, string artistName)
        {
            SongId = songId;
            SongName = songName;
            Lyrics = lyrics;
            Link = link;
            ArtistName = artistName;
        }

        public static bool AddSong(Song sng)
        {
            return dBservices.AddSong(sng);
        }

        public static List<Song> GetSongsByName(string songName)
        {
            return dBservices.GetSongsByName(songName);
        }

        public static List<Song> GetAllSongsByArtistName(string artistName)
        {
            return dBservices.GetAllSongsByArtistName(artistName);
        }

        public static List<Song> GetAllSongsByLyrics(string lyrics)
        {
            return dBservices.GetAllSongsByLyrics(lyrics);
        }

        public static int GetSongPopularityBySongId(int songId)
        {
            return dBservices.GetSongPopularityBySongId(songId);
        }
        public static bool DeleteAllArtistsAndSongs()
        {
            return dBservices.DeleteAllArtistsAndSongs();
        }
        public static List<Song> GetAllSongs()
        {
            return dBservices.GetAllSongs();
        }

        public static Song GetRandomSong()
        {
            return dBservices.GetRandomSong();
        }

        public static List<Song> Get3RandomsSongs(string artistName)
        {
            return dBservices.Get3RandomsSongs(artistName);
        }

        public static Song GetRandomSongByArtist(string artistName)
        {
            return dBservices.GetRandomSongByArtist(artistName);
        }
    }
}
