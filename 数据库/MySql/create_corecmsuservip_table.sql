-- ----------------------------
-- Table structure for CoreCmsUserVip
-- ----------------------------
DROP TABLE IF EXISTS `CoreCmsUserVip`;
CREATE TABLE `CoreCmsUserVip`  (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列',
  `userId` int(11) NOT NULL COMMENT '用户ID',
  `vipLevel` int(11) NOT NULL COMMENT 'VIP等级',
  `startTime` datetime(0) NOT NULL COMMENT 'VIP开始时间',
  `endTime` datetime(0) NOT NULL COMMENT 'VIP结束时间',
  `status` int(11) NOT NULL COMMENT 'VIP状态 1=有效 2=过期',
  `payAmount` decimal(10, 2) NOT NULL COMMENT '支付金额',
  `payOrderId` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '支付订单号',
  `createTime` datetime(0) NOT NULL COMMENT '创建时间',
  `updateTime` datetime(0) NULL DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = 'VIP会员表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of CoreCmsUserVip
-- ----------------------------