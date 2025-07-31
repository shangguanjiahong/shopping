using System;
using MySql.Data.MySqlClient;
using System.IO;

class Program
{
    static void Main()
    {
        string connectionString = "Server=127.0.0.1;Port=3306;Database=coreshop;Uid=root;Pwd=123456;CharSet=utf8;pooling=true;SslMode=None;Allow User Variables=true;Convert Zero Datetime=True;Allow Zero Datetime=True;";
        string sqlFile = "temp_menu_config.sql";
        
        try
        {
            string sqlContent = File.ReadAllText(sqlFile);
            
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("数据库连接成功");
                
                // 分割SQL语句
                string[] sqlStatements = sqlContent.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                
                int successCount = 0;
                foreach (string sql in sqlStatements)
                {
                    string trimmedSql = sql.Trim();
                    if (!string.IsNullOrEmpty(trimmedSql) && !trimmedSql.StartsWith("--"))
                    {
                        try
                        {
                            using (var command = new MySqlCommand(trimmedSql, connection))
                            {
                                command.ExecuteNonQuery();
                                successCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"执行SQL失败: {ex.Message}");
                            Console.WriteLine($"SQL: {trimmedSql.Substring(0, Math.Min(100, trimmedSql.Length))}...");
                        }
                    }
                }
                
                Console.WriteLine($"成功执行 {successCount} 条SQL语句");
                
                // 验证安装
                using (var command = new MySqlCommand("SELECT COUNT(*) FROM SysMenu WHERE id BETWEEN 1200 AND 1225", connection))
                {
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"验证结果: 共添加了 {count} 个菜单项");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"错误: {ex.Message}");
        }
        
        Console.WriteLine("按任意键退出...");
        Console.ReadKey();
    }
}