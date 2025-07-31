-- 为 CoreCmsAgentArea 表添加 isSettled 字段
-- 解决 "Unknown column 'isSettled' in 'field list'" 错误

USE coreshop;

-- 检查字段是否已存在，如果不存在则添加
SET @sql = (
    SELECT IF(
        COUNT(*) = 0,
        'ALTER TABLE CoreCmsAgentArea ADD COLUMN isSettled bit(1) NOT NULL DEFAULT 0 COMMENT "是否结算";',
        'SELECT "字段 isSettled 已存在" AS message;'
    )
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = 'coreshop' 
    AND TABLE_NAME = 'CoreCmsAgentArea' 
    AND COLUMN_NAME = 'isSettled'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- 显示表结构确认
DESC CoreCmsAgentArea;