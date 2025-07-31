using MySql.Data.MySqlClient;
using System.Text;

string connectionString = "Server=127.0.0.1;Port=3306;Database=coreshop;Uid=root;Pwd=123456;CharSet=utf8;pooling=true;SslMode=None;Allow User Variables=true;Convert Zero Datetime=True;Allow Zero Datetime=True;";
string sqlFile = "../temp_menu_config.sql";

try
{
    Console.WriteLine("开始执行地区代理菜单配置...");
    
    if (!File.Exists(sqlFile))
    {
        Console.WriteLine($"SQL文件不存在: {sqlFile}");
        return;
    }
    
    string sqlContent = File.ReadAllText(sqlFile, Encoding.UTF8);
    
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
                catch (MySqlException ex)
                {
                    if (ex.Number == 1062) // 主键重复错误
                    {
                        Console.WriteLine($"菜单项已存在，跳过: {ex.Message}");
                    }
                    else
                    {
                        Console.WriteLine($"执行SQL失败: {ex.Message}");
                        Console.WriteLine($"SQL: {trimmedSql.Substring(0, Math.Min(100, trimmedSql.Length))}...");
                    }
                }
            }
        }
        
        Console.WriteLine($"成功执行 {successCount} 条SQL语句");
        
        // 验证安装
        using (var command = new MySqlCommand("SELECT COUNT(*) FROM SysMenu WHERE id BETWEEN 1200 AND 1225", connection))
        {
            int count = Convert.ToInt32(command.ExecuteScalar());
            Console.WriteLine($"验证结果: 共添加了 {count} 个菜单项");
            
            if (count > 0)
            {
                Console.WriteLine("\n地区代理菜单配置成功！");
                Console.WriteLine("请重启CoreShop应用程序，然后在角色管理中分配相应权限。");
            }
            else
            {
                Console.WriteLine("\n菜单配置可能失败，请检查错误信息。");
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"错误: {ex.Message}");
}

Console.WriteLine("\n按任意键退出...");
Console.ReadKey();
