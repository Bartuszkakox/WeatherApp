using SQLite;
using System;

public class WeatherData
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string City { get; set; }
    public string Temperature { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
}