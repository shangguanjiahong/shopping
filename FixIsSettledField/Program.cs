using MySql.Data.MySqlClient;

string connectionString = "Server=127.0.0.1;Port=3306;Database=coreshop;Uid=root;Pwd=123456;CharSet=utf8;pooling=true;SslMode=None;Allow User Variables=true;Convert Zero Datetime=True;Allow Zero Datetime=True;";

try
{
    Console.WriteLine("开始添加 isSettled 字段到 CoreCmsAgentArea 表...");
    
    using (var connection = new MySqlConnection(connectionString))
    {
        connection.Open();
        Console.WriteLine("数据库连接成功");
        
        // 检查 CoreCmsAgentOrder 表中的 isSettlement 字段（int类型）
        Console.WriteLine("检查 CoreCmsAgentOrder 表中的 isSettlement 字段...");
        string checkAgentOrderSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'coreshop' AND TABLE_NAME = 'CoreCmsAgentOrder' AND COLUMN_NAME = 'isSettlement'";
        using (var checkCommand = new MySqlCommand(checkAgentOrderSql, connection))
        {
            int fieldExists = Convert.ToInt32(checkCommand.ExecuteScalar());
            
            if (fieldExists > 0)
            {
                Console.WriteLine("CoreCmsAgentOrder.isSettlement 字段已存在");
            }
            else
            {
                Console.WriteLine("CoreCmsAgentOrder.isSettlement 字段不存在，正在添加...");
                string alterAgentOrderSql = "ALTER TABLE CoreCmsAgentOrder ADD COLUMN isSettlement int NOT NULL DEFAULT 0 COMMENT '是否结算'";
                using (var command = new MySqlCommand(alterAgentOrderSql, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("成功添加 isSettlement 字段到 CoreCmsAgentOrder 表");
                }
            }
        }
        
        // 检查 CoreCmsAgentArea 表中的 isSettled 字段（bool类型）
        Console.WriteLine("检查 CoreCmsAgentArea 表中的 isSettled 字段...");
        string checkAgentAreaSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'coreshop' AND TABLE_NAME = 'CoreCmsAgentArea' AND COLUMN_NAME = 'isSettled'";
        using (var checkCommand = new MySqlCommand(checkAgentAreaSql, connection))
        {
            int fieldExists = Convert.ToInt32(checkCommand.ExecuteScalar());
            
            if (fieldExists > 0)
            {
                Console.WriteLine("CoreCmsAgentArea.isSettled 字段已存在");
            }
            else
            {
                Console.WriteLine("CoreCmsAgentArea.isSettled 字段不存在，正在添加...");
                string alterAgentAreaSql = "ALTER TABLE CoreCmsAgentArea ADD COLUMN isSettled tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否已结算'";
                using (var command = new MySqlCommand(alterAgentAreaSql, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("成功添加 isSettled 字段到 CoreCmsAgentArea 表");
                }
            }
        }
        
        Console.WriteLine("字段检查和添加完成！");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"错误: {ex.Message}");
}

Console.WriteLine("\n按任意键退出...");
Console.ReadKey();
