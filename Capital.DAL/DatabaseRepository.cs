﻿using System;
using System.Data.SQLite;
using System.IO;

namespace Capital.DAL
{
    public class DatabaseRepository
    {
        public static string DbFile
        {
            get { return Environment.CurrentDirectory + "\\CapitalDb.sqlite"; }
        }

        public static void CreateDatabaseRepository()
        {
            SQLiteConnection.CreateFile(DbFile);
        }

        public static void TearDownDatabaseRepository()
        {
            File.Delete(DbFile);
        }

        public static SQLiteConnection SimpleDbConnection()
        {
            return new SQLiteConnection("Data Source=" + DbFile);
        }
    }
}