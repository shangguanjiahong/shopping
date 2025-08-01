<template>
  <view class="container">
    <u-navbar title="成为尊享会员" :fixed="true" :autoBack="true" bgColor="#ffffff"></u-navbar>
    
    <view class="content">
      <!-- 订单信息 -->
      <view class="order-info">
        <view class="order-title">VIP会员开通</view>
        <view class="order-amount">
          <text>支付金额：</text>
          <text class="amount">¥{{ orderInfo.amount || 100 }}</text>
        </view>
      </view>
      
      <!-- 支付方式选择 -->
      <view class="payment-section">
        <view class="section-title">选择支付方式</view>
        <view class="payment-list">
          <view class="payment-item" :class="{active: selectedPayment === 'wechatpay'}" @tap="selectPayment('wechatpay')">
            <view class="payment-left">
              <image src="/static/images/wechat-pay.png" mode="aspectFit"></image>
              <text>微信支付</text>
            </view>
            <view class="payment-right">
              <u-icon name="checkmark-circle-fill" color="#07c160" v-if="selectedPayment === 'wechatpay'"></u-icon>
              <u-icon name="circle" color="#c8c9cc" v-else></u-icon>
            </view>
          </view>
        </view>
      </view>
    </view>
    
    <!-- 底部支付按钮 -->
    <view class="footer">
      <view class="pay-info">
        <text class="pay-text">实付款：</text>
        <text class="pay-amount">¥{{ orderInfo.amount || 100 }}</text>
      </view>
      <u-button type="primary" :loading="payLoading" @click="handlePayment">确认支付</u-button>
    </view>
    
    <u-toast ref="uToast"></u-toast>
  </view>
</template>

<script>
export default {
  data() {
    return {
      orderId: '',
      orderInfo: {},
      selectedPayment: 'wechatpay',
      payLoading: false
    }
  },
  
  onLoad(options) {
    if (options.orderId) {
      this.orderId = options.orderId;
      this.getOrderInfo();
    }
  },
  
  methods: {
    async getOrderInfo() {
      try {
        const result = await this.$u.api.getVipOrderStatus({
          orderId: this.orderId
        });
        
        if (result.status) {
          this.orderInfo = result.data;
        }
      } catch (error) {
        console.error('获取订单信息失败:', error);
      }
    },
    
    selectPayment(type) {
      this.selectedPayment = type;
    },
    
    async handlePayment() {
      if (this.payLoading) return;
      
      if (!this.selectedPayment) {
        this.$refs.uToast.show({
          title: '请选择支付方式',
          type: 'warning'
        });
        return;
      }
      
      this.payLoading = true;
      
      try {
        // 调用支付接口
        const payResult = await this.$u.api.payOrder({
          ids: this.orderId,
          payment_code: this.selectedPayment,
          payment_type: 6, // VIP订单类型
		  params:null
        });
        console.log("123")
        if (payResult.status) {
          // 调用微信支付
          if (this.selectedPayment === 'wechatpay') {
            await this.wxPay(payResult.data);
          }
        } else {
          this.$refs.uToast.show({
            title: payResult.msg || '支付失败',
            type: 'error'
          });
        }
      } catch (error) {
        console.error('支付失败:', error);
        this.$refs.uToast.show({
          title: '支付失败，请重试',
          type: 'error'
        });
      } finally {
        this.payLoading = false;
      }
    },
    
    async wxPay(payData) {
      return new Promise((resolve, reject) => {
        uni.requestPayment({
          provider: 'wxpay',
          timeStamp: payData.timeStamp,
          nonceStr: payData.nonceStr,
          package: payData.package,
          signType: payData.signType,
          paySign: payData.paySign,
          success: (res) => {
            this.$refs.uToast.show({
              title: 'VIP开通成功！',
              type: 'success'
            });
            
            setTimeout(() => {
              uni.navigateBack({
                delta: 2
              });
            }, 1500);
            
            resolve(res);
          },
          fail: (err) => {
            console.error('微信支付失败:', err);
            this.$refs.uToast.show({
              title: '支付取消或失败',
              type: 'error'
            });
            reject(err);
          }
        });
      });
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

.order-info {
  background: white;
  border-radius: 20rpx;
  padding: 30rpx;
  margin-bottom: 30rpx;
  
  .order-title {
    font-size: 32rpx;
    font-weight: bold;
    margin-bottom: 20rpx;
  }
  
  .order-amount {
    display: flex;
    justify-content: space-between;
    align-items: center;
    
    .amount {
      color: #ff4444;
      font-size: 36rpx;
      font-weight: bold;
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
  
  .payment-list {
    .payment-item {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 20rpx;
      border: 2rpx solid #e5e5e5;
      border-radius: 10rpx;
      margin-bottom: 20rpx;
      
      &.active {
        border-color: #07c160;
        background-color: #f0f9ff;
      }
      
      .payment-left {
        display: flex;
        align-items: center;
        
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
}

.footer {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  background: white;
  padding: 30rpx;
  border-top: 1rpx solid #e5e5e5;
  
  .pay-info {
    display: flex;
    justify-content: center;
    align-items: center;
    margin-bottom: 20rpx;
    
    .pay-text {
      font-size: 28rpx;
      color: #666;
    }
    
    .pay-amount {
      color: #ff4444;
      font-size: 32rpx;
      font-weight: bold;
    }
  }
}
</style>