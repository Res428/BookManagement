<template>
  <div class="divider"></div>
  <div class="login-container">
    <div class="login-box">
      <form @submit.prevent="userlogin">
        <div class="form-group">
          <input type="text" v-model="username" placeholder="Username" />
        </div>
        <div class="form-group">
          <input type="password" v-model="password" placeholder="Password" />
        </div>
        <button type="submit" class="login-button">LOG IN</button>
        <div v-if="loginError" class="error-message">{{ loginError }}</div>
      </form>
      <div class="extra-links">
        <p>
          Don't have an account yet?
          <router-link to="/sign-up">Sign Up</router-link>
        </p>
      </div>
    </div>
    <div class="illustration">
      <img
        :src="imageSrc"
        alt="Illustration of people reading books and learning"
      />
    </div>
  </div>
</template>

<script>
import axios from "../utils/axios";

export default {
  name: "LoginComponent",
  data() {
    return {
      imageSrc: require("@/assets/123456.png"),
      username: "",
      password: "",
      loginError: "", // Khai báo biến để lưu thông báo lỗi
    };
  },
  methods: {
    userlogin() {
      console.log("Logging in with:", {
        UserName: this.username,
        PassWord: this.password,
      });
      axios
        .post("/user/login", {
          UserName: this.username,
          PassWord: this.password,
        })
        .then((response) => {
          console.log(response.data.token);
          localStorage.setItem("auth", response.data.token);
          
          // Lưu thông tin người dùng vào localStorage
          const userData = response.data;
          localStorage.setItem(
            "user",
            JSON.stringify({
              userID: userData.userId, // Đảm bảo tên thuộc tính đúng
              role: userData.role,
              username: userData.username, // Thêm username
            })
          );
          
          // Chuyển hướng đến trang home
          this.$router.push("/home");
        })
        .catch((error) => {
          console.log(error); // Log toàn bộ lỗi
          this.loginError = error.response?.data?.message || "Login failed";
        });
    },
  },
};
</script>

<style scoped>
body {
  background-color: #1a2238;
  font-family: "Arial", sans-serif;
}

.divider {
  width: 10%;
  height: 2px;
  background-color: #fbbf24;
  position: absolute;
  top: 55px;
  left: 50%;
  transform: translateX(-50%);
  border-radius: 5px;
}

.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background-color: #1a2238;
}

.login-box {
  background-color: #e0e0e0;
  padding: 40px;
  border-radius: 20px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
}

.form-group {
  margin-bottom: 20px;
}

input {
  width: 100%;
  padding: 10px;
  margin-top: 5px;
  border: 1px solid #ccc;
  border-radius: 5px;
  background-color: #efefef;
}

.login-button {
  width: 100%;
  padding: 10px;
  background-color: #fbc02d;
  border: none;
  border-radius: 5px;
  color: #fff;
  cursor: pointer;
}

.login-button:hover {
  background-color: #e0a800;
}

.extra-links {
  margin-top: 15px;
  text-align: center;
}

.extra-links a {
  color: #1a73e8;
  text-decoration: none;
}

.extra-links a:hover {
  text-decoration: underline;
}

.illustration {
  margin-left: 50px;
  display: flex;
  align-items: center;
}

img {
  max-width: 100%;
  height: auto;
}

.error-message {
  color: red; /* Thêm kiểu cho thông báo lỗi */
  margin-top: 10px;
}
</style>
