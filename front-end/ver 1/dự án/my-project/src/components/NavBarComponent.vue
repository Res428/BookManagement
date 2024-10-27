<template>
    <div class="navbar">
      <div class="profile">
        <img class="profile-pic" src="https://via.placeholder.com/50" alt="Profile Picture">
        <div class="profile-info">
          <p class="profile-name">YOUR NAME</p>
          <p class="profile-email">yoemail@email.com</p>
        </div>
      </div>
      <div class="nav-links">
        <a href="#"><i class="fas fa-home"></i> Home</a>
        <a href="#"><i class="fas fa-shopping-cart"></i> My Order</a>
        <a href="#"><i class="fas fa-bell"></i> Notification</a>
        <a href="#"><i class="fas fa-user"></i> Account</a>
      </div>
      <div class="search-bar">
        <input type="text" placeholder="Search...">
        <button class="search-button"><i class="fas fa-search"></i></button>
      </div>
    </div>
  </template>
  
  <script>
export default {
  name: "NavBarComponent", // Đổi tên component thành tên đa từ
};
  </script>
  
  <style scoped>
  .navbar {
    display: flex;
    justify-content: space-between;
    align-items: center;
    background-color: #2c3e50;
    padding: 10px;
    color: white;
  }
  .profile {
    display: flex;
    align-items: center;
  }
  .profile-pic {
    border-radius: 50%;
    margin-right: 10px;
  }
  .profile-info {
    display: flex;
    flex-direction: column;
  }
  .nav-links a {
    margin: 0 10px;
    color: white;
    text-decoration: none;
  }
  .search-bar {
    display: flex;
    align-items: center;
  }
  .search-bar input {
    padding: 5px;
    border: none;
    border-radius: 5px;
  }
  .search-button {
    background-color: #3498db;
    color: white;
    border: none;
    padding: 5px 10px;
    border-radius: 5px;
  }
  </style>
  