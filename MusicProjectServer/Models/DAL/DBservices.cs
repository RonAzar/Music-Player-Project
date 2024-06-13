using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Reflection.Metadata.Ecma335;
using MusicProjectServer.Models;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    //--------------------------------------------------------------------------------------------------
    // This method Registers a User to the Users table 
    //--------------------------------------------------------------------------------------------------
    public bool Register(MusicUser user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@firstName", user.FirstName);
        paramDic.Add("@lastname", user.LastName);
        paramDic.Add("@userName", user.UserName);
        paramDic.Add("@email", user.Email);
        paramDic.Add("@userPassword", user.Password);
        paramDic.Add("@phone", user.Phone);

        cmd = CreateCommandWithStoredProcedure("signUp_SP", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            if (numEffected == 0)
            {
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    //--------------------------------------------------------------------------------------------------
    // This method Log in by user mail, password
    //--------------------------------------------------------------------------------------------------
    public MusicUser LogInByEmailAndPassword(string emailOrUserNameToLogin, string passwordToLogin)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@email", emailOrUserNameToLogin);
        paramDic.Add("@userPassword", passwordToLogin);

        cmd = CreateCommandWithStoredProcedure("logIn_SP", con, paramDic);// create the command

        MusicUser user = new MusicUser();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                user.Id = Convert.ToInt32(dataReader["Id"]);
                user.FirstName = dataReader["firstName"].ToString();
                user.LastName = dataReader["lastName"].ToString();
                user.UserName = dataReader["userName"].ToString();
                user.Email = dataReader["email"].ToString();
                user.Password = dataReader["userPassword"].ToString();
                user.Phone = dataReader["phone"].ToString();
                user.DateOfRegistration = Convert.ToDateTime(dataReader["dateOfRegistration"]);
            }

            return user;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    //--------------------------------------------------------------------------------------------------
    // This method Add Song to DB
    //--------------------------------------------------------------------------------------------------
    public bool AddSong(Song sng)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@songName", sng.SongName);
        paramDic.Add("@lyrics", sng.Lyrics);
        paramDic.Add("@link", sng.Link);
        paramDic.Add("@artistName", sng.ArtistName);

        cmd = CreateCommandWithStoredProcedure("SPAddSong", con, paramDic);// create the command

        int numEffected = cmd.ExecuteNonQuery(); // execute the command
        if (numEffected == 0)
        {
            throw new Exception("Please make sure song name and Artist Id exist.");
        }
        try
        {
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method Add Artist to DB
    //--------------------------------------------------------------------------------------------------
    public bool AddArtist(string artName)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@artistName", artName);

        cmd = CreateCommandWithStoredProcedure("SPAddArtist", con, paramDic);// create the command

        int numEffected = cmd.ExecuteNonQuery(); // execute the command
        if (numEffected == 0)
        {
            throw new Exception("Please make all parameters valid!");
        }
        try
        {
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method add a song to the user's favorites
    //--------------------------------------------------------------------------------------------------
    public bool AddToFavorites(int userId, int songId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@userId", userId);
        paramDic.Add("@songId", songId);

        cmd = CreateCommandWithStoredProcedure("SP_addFavorite", con, paramDic);// create the command

        int numEffected = cmd.ExecuteNonQuery(); // execute the command
        if (numEffected == 0)
        {
            throw new Exception("Song not added to favorites");
        }
        try
        {
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method removes a song from the user's favorites
    //--------------------------------------------------------------------------------------------------
    public bool RemoveFromFavorites(int userId, int songId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@userId", userId);
        paramDic.Add("@songId", songId);

        cmd = CreateCommandWithStoredProcedure("SP_RemoveFromFavorites", con, paramDic);// create the command

        int numEffected = cmd.ExecuteNonQuery(); // execute the command
        if (numEffected == 0)
        {
            throw new Exception("Song not added to favorites");
        }
        try
        {
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns all favorites songs of a user
    //--------------------------------------------------------------------------------------------------
    public List<Song> GetFavorites(int userId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@userId", userId);

        cmd = CreateCommandWithStoredProcedure("SP_getFavorites", con, paramDic);// create the command

        List<Song> songs = new List<Song>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Song song = new Song();
                song.SongId = Convert.ToInt32(dataReader["songId"]);
                song.SongName = dataReader["songName"].ToString();
                song.Lyrics = dataReader["lyrics"].ToString();
                song.Link = dataReader["link"].ToString();
                song.ArtistName = dataReader["artistName"].ToString();
                songs.Add(song);
            }

            // Close the dataReader before executing the next command
            dataReader.Close();

            return songs;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns all songs that include the song name
    //--------------------------------------------------------------------------------------------------
    public List<Song> GetSongsByName(string songName)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@songName", songName);

        cmd = CreateCommandWithStoredProcedure("SP_GetSongsByName", con, paramDic);// create the command

        List<Song> songs = new List<Song>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Song song = new Song();
                song.SongId = Convert.ToInt32(dataReader["songId"]);
                song.SongName = dataReader["songName"].ToString();
                song.Lyrics = dataReader["lyrics"].ToString();
                song.Link = dataReader["link"].ToString();
                song.ArtistName = dataReader["artistName"].ToString();
                songs.Add(song);
            }

            // Close the dataReader before executing the next command
            dataReader.Close();

            return songs;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns all songs by Artist name
    //--------------------------------------------------------------------------------------------------
    public List<Song> GetAllSongsByArtistName(string artistName)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@artistName", artistName);

        cmd = CreateCommandWithStoredProcedure("GetAllSongsByArtistName", con, paramDic);// create the command

        List<Song> songs = new List<Song>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Song song = new Song();
                song.SongId = Convert.ToInt32(dataReader["songId"]);
                song.SongName = dataReader["songName"].ToString();
                song.Lyrics = dataReader["lyrics"].ToString();
                song.Link = dataReader["link"].ToString();
                song.ArtistName = dataReader["artistName"].ToString();
                songs.Add(song);
            }

            // Close the dataReader before executing the next command
            dataReader.Close();

            return songs;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns all songs by Lyrics
    //--------------------------------------------------------------------------------------------------
    public List<Song> GetAllSongsByLyrics(string lyr)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@searchLyrics", lyr);

        cmd = CreateCommandWithStoredProcedure("GetAllSongsByLyrics", con, paramDic);// create the command

        List<Song> songs = new List<Song>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Song song = new Song();
                song.SongId = Convert.ToInt32(dataReader["songId"]);
                song.SongName = dataReader["songName"].ToString();
                song.Lyrics = dataReader["lyrics"].ToString();
                song.Link = dataReader["link"].ToString();
                song.ArtistName = dataReader["artistName"].ToString();
                songs.Add(song);
            }

            // Close the dataReader before executing the next command
            dataReader.Close();

            return songs;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns all Artists
    //--------------------------------------------------------------------------------------------------
    public List<ArtistClass> GetAllArtists()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();

        cmd = CreateCommandWithStoredProcedure("SP_GetAllArtist", con, paramDic);// create the command

        List<ArtistClass> artists = new List<ArtistClass>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                ArtistClass art = new ArtistClass();
                art.Id = Convert.ToInt32(dataReader["artistId"]);
                art.Name = dataReader["artistName"].ToString();
                art.Popularity = Convert.ToInt32(dataReader["popularity"]);
                artists.Add(art);
            }

            // Close the dataReader before executing the next command
            dataReader.Close();

            return artists;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns all Songs
    //--------------------------------------------------------------------------------------------------
    public List<Song> GetAllSongs()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();

        cmd = CreateCommandWithStoredProcedure("SP_GetAllSongs", con, paramDic);// create the command

        List<Song> songs = new List<Song>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Song sng = new Song();
                sng.SongId = Convert.ToInt32(dataReader["songId"]);
                sng.SongName = dataReader["songName"].ToString();
                sng.Lyrics = dataReader["lyrics"].ToString();
                sng.Link = dataReader["link"].ToString();
                sng.ArtistName = dataReader["artistName"].ToString();
                songs.Add(sng);
            }

            // Close the dataReader before executing the next command
            dataReader.Close();

            return songs;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns Artist info by ID
    //--------------------------------------------------------------------------------------------------
    public ArtistClass GetArtistById(int artId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@artistId", artId);

        cmd = CreateCommandWithStoredProcedure("SP_GetArtistById", con, paramDic);// create the command

        ArtistClass art = new ArtistClass();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                

                art.Id = Convert.ToInt32(dataReader["artistId"]);
                art.Name = dataReader["artistName"].ToString();
                art.Popularity = Convert.ToInt32(dataReader["popularity"]);
            }
            // Close the dataReader before executing the next command
            dataReader.Close();

            return art;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns Artist Id by his name
    //--------------------------------------------------------------------------------------------------
    public int GetArtistIdByName(string artName)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@artistName", artName);

        cmd = CreateCommandWithStoredProcedure("SP_GetArtistIdByName", con, paramDic);// create the command

        int artId = 0;

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                artId = Convert.ToInt32(dataReader["artistId"]);
            }
            // Close the dataReader before executing the next command
            dataReader.Close();

            return artId;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns If User Has Favorite Song or not
    //--------------------------------------------------------------------------------------------------
    public bool CheckIfUserHasFavoriteSong(int songId, int userId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@userId", userId);
        paramDic.Add("@songId", songId);

        cmd = CreateCommandWithStoredProcedure("SP_CheckIfUserHasFavoriteSong", con, paramDic);// create the command

        bool flag = false;

        try
        {
            
            int result = (int)cmd.ExecuteScalar();
            flag = (result == 1);
            
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

        return flag;
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns Artist Info by his name
    //--------------------------------------------------------------------------------------------------
    public ArtistClass GetArtistInfoByName(string artName)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@artistName", artName);

        cmd = CreateCommandWithStoredProcedure("SP_GetArtistInfoByName", con, paramDic);// create the command

        ArtistClass art = new ArtistClass();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                art.Id = Convert.ToInt32(dataReader["artistId"]);
                art.Name = dataReader["artistName"].ToString();
                art.Popularity = Convert.ToInt32(dataReader["popularity"]);
            }
            // Close the dataReader before executing the next command
            dataReader.Close();

            return art;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns Song Popularity by song Id
    //--------------------------------------------------------------------------------------------------
    public int GetSongPopularityBySongId(int songId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@songId", songId);

        cmd = CreateCommandWithStoredProcedure("GetSongPopularity", con, paramDic);// create the command

        int retPopularity = 0;
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                retPopularity = Convert.ToInt32(dataReader["CountOfValue"]);
            }
            // Close the dataReader before executing the next command
            dataReader.Close();

            return retPopularity;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method deletes ALL favorite songs and ALL users 
    //--------------------------------------------------------------------------------------------------
    public bool DeleteAllFavsAndUsers()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        
        cmd = CreateCommandWithStoredProcedure("DeleteAllUsers", con, null);// create the command

        int numEffected = cmd.ExecuteNonQuery(); // execute the command
        
        try
        {
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method deletes ALL artists and ALL songs 
    //--------------------------------------------------------------------------------------------------
    public bool DeleteAllArtistsAndSongs()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("DeleteAllArtistsAndSongs", con, null);// create the command

        int numEffected = cmd.ExecuteNonQuery(); // execute the command

        try
        {
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method update a User 
    //--------------------------------------------------------------------------------------------------
    public bool UpdateUser(MusicUser user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@userID", user.Id);
        paramDic.Add("@userName", user.UserName);
        paramDic.Add("@firstName", user.FirstName);
        paramDic.Add("@lastName", user.LastName);
        paramDic.Add("@email", user.Email);
        paramDic.Add("@userPassword", user.Password);
        paramDic.Add("@phone", user.Phone);


        cmd = CreateCommandWithStoredProcedure("SP_UpdateUserDetails", con, paramDic);// create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            if (numEffected == 0)
            {
                throw new Exception("Could'nt change user details...");
            }
            return true;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns Top 5 Scores
    //--------------------------------------------------------------------------------------------------
    public List<Score> GetTopFiveScoreBoard()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("SP_GetTopFiveScoreBoard", con, null);// create the command

        List<Score> topTenScores = new List<Score>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Score score = new Score();
                score.Id = Convert.ToInt32(dataReader["id"]);
                score.UserId = Convert.ToInt32(dataReader["userId"]);
                score.UserScore = Convert.ToInt32(dataReader["score"]);
                score.UserName = dataReader["userName"].ToString();
                topTenScores.Add(score);
            }

            // Close the dataReader before executing the next command
            dataReader.Close();

            return topTenScores;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method Add new Score to the scoreboard
    //--------------------------------------------------------------------------------------------------
    public bool InsertScore(Score score)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@score", score.UserScore);
        paramDic.Add("@userId", score.UserId);
        paramDic.Add("@userName", score.UserName);

        cmd = CreateCommandWithStoredProcedure("SP_InsertScore", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            if (numEffected == 0)
            {
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    //--------------------------------------------------------------------------------------------------
    // This method returns All scores for a specific user
    //--------------------------------------------------------------------------------------------------
    public List<Score> GetUserScores(int userId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@userID", userId);

        cmd = CreateCommandWithStoredProcedure("SP_getUserScores", con, paramDic);// create the command

        List<Score> scores = new List<Score>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Score sco = new Score();
                sco.Id = Convert.ToInt32(dataReader["Id"]);
                sco.UserId = Convert.ToInt32(dataReader["userId"]);
                sco.UserScore = Convert.ToInt32(dataReader["score"]);
                sco.UserName = dataReader["userName"].ToString();
                scores.Add(sco);
            }

            // Close the dataReader before executing the next command
            dataReader.Close();

            return scores;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method deletes ALL ScoreBoard
    //--------------------------------------------------------------------------------------------------
    public bool ResetScoreBoard()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("SP_ResetScoreboard", con, null);// create the command

        int numEffected = cmd.ExecuteNonQuery(); // execute the command

        try
        {
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns 3 Random artistsge
    //--------------------------------------------------------------------------------------------------
    public List<ArtistClass> Get3RandomsArtists(string artistName)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@artistName", artistName);

        cmd = CreateCommandWithStoredProcedure("SP_Get3RandomsArtists", con, paramDic);// create the command

        List<ArtistClass> artistsList = new List<ArtistClass>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                ArtistClass art = new ArtistClass();
                art.Id = Convert.ToInt32(dataReader["artistId"]);
                art.Popularity = Convert.ToInt32(dataReader["popularity"]);
                art.Name = dataReader["artistName"].ToString();
                artistsList.Add(art);
            }

            // Close the dataReader before executing the next command
            dataReader.Close();

            return artistsList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns a random Artist
    //--------------------------------------------------------------------------------------------------
    public ArtistClass GetRandomArtist()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();

        cmd = CreateCommandWithStoredProcedure("SP_GetRandomArtist", con, paramDic);// create the command

        ArtistClass artist = new ArtistClass();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                artist.Id = Convert.ToInt32(dataReader["artistId"]);
                artist.Popularity = Convert.ToInt32(dataReader["popularity"]);
                artist.Name = dataReader["artistName"].ToString();
            }

            // Close the dataReader before executing the next command
            dataReader.Close();

            return artist;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns a random Song
    //--------------------------------------------------------------------------------------------------
    public Song GetRandomSong()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();

        cmd = CreateCommandWithStoredProcedure("SP_GetRandomSong", con, paramDic);// create the command

        Song sng = new Song();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                sng.SongId = Convert.ToInt32(dataReader["songId"]);
                sng.SongName = dataReader["songName"].ToString();
                sng.Lyrics = dataReader["lyrics"].ToString();
                sng.Link = dataReader["link"].ToString();
                sng.ArtistName = dataReader["artistName"].ToString();
            }

            // Close the dataReader before executing the next command
            dataReader.Close();

            return sng;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns Random song by artist Name
    //--------------------------------------------------------------------------------------------------
    public Song GetRandomSongByArtist(string artistName)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@artistName", artistName);

        cmd = CreateCommandWithStoredProcedure("SP_GetRandomSongByArtist", con, paramDic);// create the command

        Song song = new Song();
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                song.SongId = Convert.ToInt32(dataReader["songId"]);
                song.SongName = dataReader["songName"].ToString();
                song.Lyrics = dataReader["lyrics"].ToString();
                song.Link = dataReader["link"].ToString();
                song.ArtistName = dataReader["artistName"].ToString();
            }

            // Close the dataReader before executing the next command
            dataReader.Close();

            return song;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns 3 Random songs
    //--------------------------------------------------------------------------------------------------
    public List<Song> Get3RandomsSongs(string artistName)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@artistName", artistName);

        cmd = CreateCommandWithStoredProcedure("SP_Get3RandomsSongs", con, paramDic);// create the command

        List<Song> songsList = new List<Song>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Song song = new Song();
                song.SongId = Convert.ToInt32(dataReader["songId"]);
                song.SongName = dataReader["songName"].ToString();
                song.Lyrics = dataReader["lyrics"].ToString();
                song.Link = dataReader["link"].ToString();
                song.ArtistName = dataReader["artistName"].ToString();
                songsList.Add(song);
            }

            // Close the dataReader before executing the next command
            dataReader.Close();

            return songsList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedure(String spName, SqlConnection con, Dictionary<string, object> paramDic)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        if(paramDic != null)
            foreach (KeyValuePair<string, object> param in paramDic) {
                cmd.Parameters.AddWithValue(param.Key,param.Value);

            }


        return cmd;
    }



}
