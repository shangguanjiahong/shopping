<template>
  <view class="container">
    <u-navbar title="开通尊享会员" :fixed="true" :autoBack="true" bgColor="#ffffff"></u-navbar>
    
    <view class="content">
      <!-- VIP卡片 -->
      <view class="vip-card">
        <view class="vip-header">
          <image class="vip-icon" src="/static/images/vip-icon.png" mode="aspectFit"></image>
          <text class="vip-title">尊享会员</text>
        </view>
        
        <view class="vip-price">
          <text class="symbol">¥</text>
          <text class="amount">100</text>
        </view>
        
        <view class="vip-benefits">
          <view class="benefit-item">
            <u-icon name="checkmark-circle" color="#ff4444" size="32"></u-icon>
            <text>自动开通分销资格</text>
          </view>
          <view class="benefit-item">
            <u-icon name="checkmark-circle" color="#ff4444" size="32"></u-icon>
            <text>专享会员折扣</text>
          </view>
          <view class="benefit-item">
            <u-icon name="checkmark-circle" color="#ff4444" size="32"></u-icon>
            <text>优先客服服务</text>
          </view>
        </view>
      </view>
      
      <!-- 支付方式 -->
      <view class="payment-section">
        <view class="section-title">支付方式</view>
        <view class="payment-methods">
          <view class="payment-method" :class="{active: paymentCode === 'wechatpay'}" @tap="selectPayment('wechatpay')">
            <image src="/static/images/wechat-pay.png" mode="aspectFit"></image>
            <text>微信支付</text>
          </view>
        </view>
      </view>
    </view>
    
    <!-- 底部按钮 -->
    <view class="footer">
      <view class="total-price">
        <text>支付金额：</text>
        <text class="price">¥100.00</text>
      </view>
      <u-button type="primary" :loading="loading" @click="handlePay">立即开通</u-button>
    </view>
    
    <u-toast ref="uToast"></u-toast>
  </view>
</template>

<script>
export default {
  data() {
    return {
      paymentCode: 'wechatpay',
      loading: false
    }
  },
  methods: {
    selectPayment(code) {
      this.paymentCode = code;
    },
    
    async handlePay() {
      if (this.loading) return;
      
      this.loading = true;
      
      try {
        // 调用VIP开通接口
        const result = await this.$u.api.createVipOrder({
          vipLevel: 2,  // 添加尊享会员等级参数
          paymentCode: this.paymentCode,
          amount: 0.01
        });
        
        if (result.status) {
          // 跳转到支付页面
          uni.navigateTo({
            url: `/pages/payment/vip/payment?orderId=${result.data.orderId}`
          });
        } else {
          this.$refs.uToast.show({
            title: result.msg || '开通失败',
            type: 'error'
          });
        }
      } catch (error) {
        console.error('VIP开通失败:', error);
        this.$refs.uToast.show({
          title: '网络错误，请重试',
          type: 'error'
        });
      } finally {
        this.loading = false;
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.container {
  min-height: 100vh;
  background-color: #f5f5f5;
  padding-bottom: 120rpx;
}

.content {
  padding: 20rpx;
}

.vip-card {
  background: linear-gradient(135deg, #ff6b6b, #ff8e8e);
  border-radius: 20rpx;
  padding: 40rpx;
  margin-bottom: 30rpx;
  color: white;
  
  .vip-header {
    display: flex;
    align-items: center;
    margin-bottom: 30rpx;
    
    .vip-icon {
      width: 60rpx;
      height: 60rpx;
      margin-right: 20rpx;
    }
    
    .vip-title {
      font-size: 36rpx;
      font-weight: bold;
    }
  }
  
  .vip-price {
    text-align: center;
    margin-bottom: 40rpx;
    
    .symbol {
      font-size: 40rpx;
    }
    
    .amount {
      font-size: 80rpx;
      font-weight: bold;
    }
  }
  
  .vip-benefits {
    .benefit-item {
      display: flex;
      align-items: center;
      margin-bottom: 20rpx;
      
      text {
        margin-left: 20rpx;
        font-size: 28rpx;
      }
    }
  }
}

.payment-section {
  background: white;
  border-radius: 20rpx;
  padding: 30rpx;
  
  .section-title {
    font-size: 32rpx;
    font-weight: bold;
    margin-bottom: 30rpx;
  }
  
  .payment-methods {
    .payment-method {
      display: flex;
      align-items: center;
      padding: 20rpx;
      border: 2rpx solid #e5e5e5;
      border-radius: 10rpx;
      margin-bottom: 20rpx;
      
      &.active {
        border-color: #ff4444;
        background-color: #fff5f5;
      }
      
      image {
        width: 60rpx;
        height: 60rpx;
        margin-right: 20rpx;
      }
      
      text {
        font-size: 28rpx;
      }
    }
  }
}

.footer {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  background: white;
  padding: 30rpx;
  border-top: 1rpx solid #e5e5e5;
  
  .total-price {
    text-align: center;
    margin-bottom: 20rpx;
    
    .price {
      color: #ff4444;
      font-size: 32rpx;
      font-weight: bold;
    }
  }
}
</style>