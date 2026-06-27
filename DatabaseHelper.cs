using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CybersecurityBot.GUI
{
    internal class DatabaseHelper
    {
        private string connectionString = "Server=localhost;Database=cyberbot_db;Uid=root;Pwd=;";

        public void AddTask(string title, string description, DateTime? reminderDate)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO tasks (title, description, reminder_date) 
                                 VALUES (@title, @desc, @reminder)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@desc", description);

                    if (reminderDate.HasValue)
                        cmd.Parameters.AddWithValue("@reminder", reminderDate.Value);
                    else
                        cmd.Parameters.AddWithValue("@reminder", DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<TaskItem> GetTasks()
        {
            var tasks = new List<TaskItem>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM tasks ORDER BY created_at DESC";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new TaskItem
                        {
                            Id = reader.GetInt32("id"),
                            Title = reader.GetString("title"),
                            Description = reader.GetString("description"),
                            ReminderDate = reader.IsDBNull(reader.GetOrdinal("reminder_date"))
                                ? (DateTime?)null
                                : reader.GetDateTime("reminder_date"),
                            IsCompleted = reader.GetBoolean("is_completed")
                        });
                    }
                }
            }

            return tasks;
        }

        public void CompleteTask(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE tasks SET is_completed = TRUE WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTask(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM tasks WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    internal class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}