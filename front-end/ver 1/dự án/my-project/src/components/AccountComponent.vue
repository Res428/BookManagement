<template>
  <div class="account-page">
    <header class="header">
      <div class="user-info">
        <div class="user-icon"></div>
        <div class="user-details">
          <p>{{ userName }}</p>
          <p>{{ email }}</p>
        </div>
      </div>
      <nav class="nav-bar">
        <router-link to="/home" class="nav-item">Home</router-link>
        <router-link v-if="isAdmin" to="/wait-approval" class="nav-item"
          >Wait Approval</router-link
        >
        <router-link v-if="isCustomer" to="/orders" class="nav-item"
          >My Order</router-link
        >
        <router-link v-if="isCustomer" to="/notifications" class="nav-item"
          >Notification</router-link
        >
        <router-link to="/account" class="nav-item active">Account</router-link>
      </nav>
    </header>

    <div class="account-content">
      <form class="account-form">
        <div class="form-group">
          <label for="username">Username</label>
          <input
            type="text"
            id="username"
            v-model="userName"
            class="form-control"
            readonly
          />
        </div>
        <div class="form-group">
          <label for="fullName">Full Name</label>
          <input
            type="text"
            id="fullName"
            v-model="fullName"
            class="form-control"
            readonly
          />
        </div>
        <div class="form-group">
          <label for="email">Email</label>
          <input
            type="email"
            id="email"
            v-model="email"
            class="form-control"
            readonly
          />
        </div>
        <div class="form-group">
          <label for="phone">Phone Number</label>
          <input
            type="number"
            id="phone"
            v-model="phone"
            class="form-control"
            readonly
          />
        </div>
        <div class="form-group">
          <label for="address">Address</label>
          <input
            type="text"
            id="address"
            v-model="address"
            class="form-control"
            readonly
          />
        </div>
        <button type="button" @click="logout" class="button">LOG OUT</button>
      </form>
    </div>
  </div>
</template>

<script>
import axios from "@/utils/axios";

export default {
  name: "AccountSetting",
  data() {
    return {
      userName: "",
      fullName: "",
      email: "",
      address: "",
    };
  },
  created() {
    const userData = JSON.parse(localStorage.getItem("user")); // Lấy thông tin người dùng từ localStorage
    if (!userData) {
      this.$router.push("/login");
    } else {
      this.userName = userData.username || "";
      this.fetchUserData(userData.userID); // Gọi phương thức để lấy thông tin chi tiết
    }
  },
  mounted() {
    this.loadUserData();
  },
  methods: {
    loadUserData() {
      const userData = JSON.parse(localStorage.getItem("user"));
      if (userData) {
        this.isAdmin = userData.role === "admin";
        this.isCustomer = userData.role === "customer";
      }
    },
    async fetchUserData(userId) {
      try {
        const response = await axios.get(`/user/users/${userId}`);
        if (response.status === 200) {
          this.fullName = response.data.fullName || "";
          this.email = response.data.email || "";
          this.phone = response.data.phone || "";
          this.address = response.data.address || "";

          localStorage.setItem("email", this.email);
          console.log(response.data);
        } else {
          console.error("Error fetching user data:", response.status);
          alert("Có lỗi xảy ra khi lấy thông tin người dùng.");
        }
      } catch (error) {
        console.error("Error fetching user data:", error);
        if (error.response) {
          alert(
            `Lỗi: ${error.response.status} - ${
              error.response.data.message ||
              "Không tìm thấy thông tin người dùng."
            }`
          );
        } else {
          alert("Có lỗi xảy ra khi kết nối đến máy chủ.");
        }
      }
    },
    logout() {
      localStorage.removeItem("auth");
      localStorage.removeItem("user"); // Xóa thông tin người dùng
      this.$router.push("/login"); // Chuyển hướng đến trang đăng nhập
    },
  },
};
</script>

<style scoped>
.account-page {
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

nav .active {
  font-weight: bold;
  color: #ffcc00;
}

.account-content {
  color: #333;
  display: flex;
  justify-content: center;
  margin-top: 20px;
}

.account-form {
  background-color: #ccc;
  padding: 20px;
  border-radius: 10px;
  width: 300px;
}

.form-group {
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
}

.form-control {
  color: #333;
  width: 100%;
  padding: 8px;
  border: 1px solid #aaa;
  border-radius: 4px;
  background-color: #f9f9f9;
}

.button {
  width: 100%;
  padding: 10px;
  background-color: #ffc107;
  color: white;
  border: none;
  border-radius: 4px;
  font-size: 16px;
  cursor: pointer;
}
</style>
