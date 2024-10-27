<template>
    <div class="account-page">
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
  
      <div class="account-content">
        <form class="account-form" @submit.prevent="editProfile">
          <div class="form-group">
            <label for="username">Username</label>
            <input
              type="text"
              id="username"
              v-model="userName"
              class="form-control"
            />
          </div>
          <div class="form-group">
            <label for="email">Email</label>
            <input
              type="email"
              id="email"
              v-model="userEmail"
              class="form-control"
            />
          </div>
          <div class="form-group">
            <label for="phone">Phone Number</label>
            <input
              type="text"
              id="phone"
              v-model="userPhone"
              class="form-control"
            />
          </div>
          <div class="form-group">
            <label for="address">Your Address</label>
            <input
              type="text"
              id="address"
              v-model="userAddress"
              class="form-control"
            />
          </div>
          <button type="submit" class="edit-button">Edit</button>
          <button type="submit" class="edit-button">Cancel</button>
        </form>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name: "Account",
    data() {
      return {
        userName: "Your name",
        userEmail: "yourname@gmail.com",
        userPhone: "09385556641",
        userAddress: "trảng bom",
      };
    },
    methods: {
      editProfile() {
        // Thực hiện xử lý khi người dùng nhấn nút "Edit"
        alert("Thông tin đã được cập nhật!");
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
  
  .account-content {
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
    width: 100%;
  padding: 8px;
  border: 1px solid #aaa;
  border-radius: 4px;
}

.edit-button {
  width: 100%;
  padding: 10px;
  background-color: #ffc107;
  color: white;
  border: none;
  border-radius: 4px;
  font-size: 16px;
  cursor: pointer;
}

.edit-button:hover {
  background-color: #e0a800;
}
</style>