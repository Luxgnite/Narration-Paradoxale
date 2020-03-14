using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

[ExecuteInEditMode]
public class DateChanger : MonoBehaviour
{
    private int year;
    private int month;
    private int day;

    private int hour;
    private int minute;
    private int second;

    public string Date
    {
        get => year.ToString() + "/" + month.ToString() + "/" + day.ToString();
        set
        {
            string[] str = value.Split('/');
            year = int.Parse(str[0]);
            month = int.Parse(str[1]);
            day = int.Parse(str[2]);
        }
    }

    public string Time
    {
        get => hour.ToString() + ":" + minute.ToString() + ":" + second.ToString();
        set
        {
            string[] str = value.Split(':');
            hour = int.Parse(str[0]);
            minute = int.Parse(str[1]);
            second = int.Parse(str[2]);
        }
    }

    public void ApplyDateTime()
    {
        DirectoryInfo directory = new DirectoryInfo("GAMEFILES");

        foreach(FileInfo file in directory.GetFiles())
        {
            File.SetCreationTime(file.FullName, new System.DateTime(year, month, day,hour,minute,second));
            File.SetLastWriteTime(file.FullName, new System.DateTime(year, month, day, hour, minute, second));
            File.SetLastAccessTime(file.FullName, new System.DateTime(year, month, day, hour, minute, second));
        }

        foreach (DirectoryInfo folder in directory.GetDirectories())
        {
            Directory.SetCreationTime(folder.FullName, new System.DateTime(year, month, day, hour, minute, second));
            Directory.SetLastWriteTime(folder.FullName, new System.DateTime(year, month, day, hour, minute, second));
            Directory.SetLastAccessTime(folder.FullName, new System.DateTime(year, month, day, hour, minute, second));
        }
    }
}
