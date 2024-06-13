using System;
namespace MusicProjectServer.Models
{
    public class ArtistClass
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Popularity { get; set; } //number of times in users favorites

        static DBservices dBservices = new DBservices();

        public ArtistClass()
        {
            Name = "";
            Popularity = 0;
        }

        public ArtistClass(string name, int id, int popularity)
        {
            Name = name;
            Id = id;
            Popularity = popularity;

        }

        public static bool AddArtist(string artName)
        {
            return dBservices.AddArtist(artName);
        }

        public static ArtistClass GetArtistById(int artistId)
        {
            return dBservices.GetArtistById(artistId);
        }

        public static ArtistClass GetArtistInfoByName(string artName)
        {
            return dBservices.GetArtistInfoByName(artName);
        }

        public static int GetArtistIdByName(string artName)
        {
            return dBservices.GetArtistIdByName(artName);
        }

        public static List<ArtistClass> GetAllArtists()
        {
            return dBservices.GetAllArtists();
        }

        public static List<ArtistClass> Get3RandomsArtists(string artistName)
        {
            return dBservices.Get3RandomsArtists(artistName);
        }

        public static ArtistClass GetRandomArtist()
        {
            return dBservices.GetRandomArtist();
        }
    }
}

