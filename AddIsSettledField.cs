using MySql.Data.MySqlClient;
using System;

class AddIsSettledField
{
    static void Main()
    {
        string connectionString = "Server=127.0.0.1;Port=3306;Database=coreshop;Uid=root;Pwd=123456;CharSet=utf8;pooling=true;SslMode=None;Allow User Variables=true;Convert Zero Datetime=True;Allow Zero Datetime=True;";
        
        try
        {
            Console.WriteLine("开始添加 isSettled 字段到 CoreCmsAgentArea 表...");
            
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("数据库连接成功");
                
                // 检查字段是否已存在
                string checkSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'coreshop' AND TABLE_NAME = 'CoreCmsAgentArea' AND COLUMN_NAME = 'isSettled'";
                using (var checkCommand = new MySqlCommand(checkSql, connection))
                {
                    int fieldExists = Convert.ToInt32(checkCommand.ExecuteScalar());
                    
                    if (fieldExists > 0)
                    {
                        Console.WriteLine("isSettled 字段已存在，无需添加。");
                        return;
                    }
                }
                
                // 添加字段
                string alterSql = "ALTER TABLE CoreCmsAgentArea ADD COLUMN isSettled tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否已结算'";
                using (var command = new MySqlCommand(alterSql, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("成功添加 isSettled 字段到 CoreCmsAgentArea 表");
                }
                
                // 验证字段添加
                using (var verifyCommand = new MySqlCommand(checkSql, connection))
                {
                    int fieldExists = Convert.ToInt32(verifyCommand.ExecuteScalar());
                    if (fieldExists > 0)
                    {
                        Console.WriteLine("验证成功：isSettled 字段已成功添加到数据库");
                    }
                    else
                    {
                        Console.WriteLine("验证失败：字段可能未正确添加");
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
    }
}