<template>
    <div class="notification-page">
      <header class="header">
        <div class="user-info">
          <div class="user-icon"></div>
          <div class="user-details">
            <p>{{ userName }}</p>
            <p>{{ userEmail }}</p>
          </div>
        </div>
        <nav class="nav-bar">
          <router-link to="/" class="nav-item">Home</router-link>
          <router-link to="/orders" class="nav-item">My Order</router-link>
          <router-link to="/notifications" class="nav-item">Notification</router-link>
          <router-link to="/account" class="nav-item">Account</router-link>
        </nav>
      </header>
  
      <div class="notification-content">
        <div class="notification-item" v-for="(notification, index) in notifications" :key="index">
          <div class="notification-icon"></div>
          <div class="notification-text">{{ notification.text }}</div>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name: "Notification",
    data() {
      return {
        userName: "YOUR NAME",
        userEmail: "youremail@gmail.com",
        notifications: [
          { text: "Bạn đã mượn sách cuộc chiến đấu tiến thành công vào ngày 09 tháng 09, 2024" },
          { text: "Hôm nay là ngày 08 tháng 09, 2024 tới ngày trả sách" },
          { text: "Có sách mới vừa nhập vào kho nhanh tay xem thử nào" },
          { text: "Thông báo khác ..." },
          // Thêm thông báo khác nếu cần
        ],
      };
    },
  };
  </script>
  
  <style scoped>
  .notification-page {
    display: flex;
    flex-direction: column;
  }
  
  .header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 15px;
    background-color: #333;
    color: #fff;
  }
  
  .user-info {
    display: flex;
    align-items: center;
  }
  
  .user-icon {
    width: 40px;
    height: 40px;
    background-color: #ccc;
    border-radius: 50%;
    margin-right: 10px;
  }
  
  .nav-bar {
    display: flex;
  }
  
  .nav-item {
    color: white;
    padding: 0 15px;
    text-decoration: none;
  }
  
  .notification-content {
    padding: 20px;
  }
  
  .notification-item {
    display: flex;
    background-color: #e6f0fa;
    border-radius: 8px;
    margin: 10px 0;
    padding: 15px;
  }
  
  .notification-icon {
    width: 40px;
    height: 40px;
    background-color: #a5b8d8;
    border-radius: 5px;
    margin-right: 15px;
  }
  
  .notification-text {
    font-size: 16px;
  }
  </style>