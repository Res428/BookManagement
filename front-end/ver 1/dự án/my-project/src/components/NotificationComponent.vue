<template>
  <header>
    <div class="user-info">
      <img src="@/assets/user-icon.png" alt="User Icon" class="user-icon" />
      <span>{{ username }}</span>
      <span>{{ email }}</span>
    </div>
    <nav>
      <router-link to="/home">Home</router-link>
      <router-link to="/orders">My Order</router-link>
      <router-link to="/notifications" class="active">Notification</router-link>
      <router-link to="/account">Account</router-link>
    </nav>
  </header>
  <div class="notification">
    <div class="menu-icon" @click="toggleMenu">☰</div>
    <div v-if="menuVisible" class="menu-options">
      <button @click="clearNotifications">Xóa tất cả thông báo</button>
      <button @click="markAllAsRead">Đánh dấu tất cả đã đọc</button>
    </div>
    <div class="notification-list">
      <div
        v-for="(notification, index) in notifications"
        :key="index"
        class="notification-item"
      >
        <p>{{ notification.message }}</p>
      </div>
      <div class="empty-notification" v-if="notifications.length === 0">
        <p>No notifications available.</p>
      </div>
    </div>
    <div class="notification-display">
      <!-- <div v-for="(notification, index) in notifications" :key="index" class="notification-item">
        <p>{{ notification.message }}</p>
      </div> -->
    </div>
  </div>
</template>

<script>
import { mapState } from "vuex";

export default {
  name: "NotificationComponent",
  computed: {
    ...mapState(["notifications"]),
  },
  data() {
    return {
      username: "",
      email: "",
      menuVisible: false,
    };
  },
  mounted() {
    this.loadUserData();
  },
  methods: {
    loadUserData() {
      const userData = JSON.parse(localStorage.getItem("user"));
      if (userData) {
        this.username = userData.username || "";
        this.email = userData.email || "";
      }
    },
    toggleMenu() {
      this.menuVisible = !this.menuVisible;
    },
    clearNotifications() {
      // Gọi action để xóa tất cả thông báo
      this.$store.dispatch("clearNotifications");
      this.menuVisible = false;
    },
    markAllAsRead() {
      // Gọi action để đánh dấu tất cả đã đọc
      // Có thể cần thêm logic nếu bạn muốn thực hiện điều này
      this.menuVisible = false;
    },
  },
};
</script>

<style scoped>
.notification {
  padding: 20px;
  background-color: #f0f0f0;
  height: 100vh;
}

header {
  background-color: #1a2238;
  color: white;
  padding: 10px;
  display: flex;
  justify-content: space-between;
}

.user-info {
  display: flex;
  align-items: center;
}

.user-icon {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  margin-right: 10px;
}

nav a {
  color: white;
  margin: 0 15px;
  text-decoration: none;
}

nav .active {
  font-weight: bold;
  color: #ffcc00; /* Màu vàng cho liên kết đang hoạt động */
}

.menu-icon {
  cursor: pointer;
  font-size: 24px;
  margin-left: 15px;
  justify-self: end;
}

.menu-options {
  position: absolute;
  top: 18%;
  right: 10px;
  background-color: white;
  border: 1px solid #ccc;
  border-radius: 5px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
  z-index: 1000;
}

.menu-options button {
  display: block;
  width: 100%;
  padding: 10px;
  border: none;
  background: none;
  text-align: left;
  cursor: pointer;
}

.menu-options button:hover {
  background-color: #f0f0f0;
}

.notification-list {
  margin-top: 20px;
}

.notification-item {
  background-color: #d6e4ff; /* Màu nền cho thông báo */
  border-radius: 8px;
  padding: 15px;
  margin-bottom: 10px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
}

.empty-notification {
  text-align: center;
  color: #999;
}

.notification-display {
  position: fixed;
  top: 10px;
  right: 10px;
  z-index: 1000;
}

.notification-display .notification-item {
  background-color: #d6e4ff;
  border-radius: 8px;
  padding: 10px;
  margin-bottom: 10px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
}
</style>
