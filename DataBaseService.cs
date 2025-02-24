using SQLite;
using System;
using System.IO;
using System.Threading.Tasks;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService()
    {
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "weather.db");
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<WeatherData>().Wait();
    }

    public Task<int> SaveWeatherDataAsync(WeatherData data)
    {
        return _database.InsertAsync(data);
    }

    public Task<List<WeatherData>> GetWeatherHistoryAsync()
    {
        return _database.Table<WeatherData>().ToListAsync();
    }
}