<template>
    <div class="home-container">
      <header class="header">
        <div class="user-info">
          <img src="@/assets/user-icon.png" alt="User Icon" class="user-icon" />
          <span class="user-name">YOUR NAME</span>
          <span class="user-email">yoemail@gmail.com</span>
        </div>
        <nav class="navbar">
          <router-link to="/home">Home</router-link>
          <router-link to="/orders">My Order</router-link>
          <router-link to="/notifications">Notification</router-link>
          <router-link to="/account">Account</router-link>
          <input type="text" placeholder="Search..." class="search-input" />
        </nav>
      </header>
  
      <main class="content">
        <div class="card-container">
          <div class="card" v-for="(item, index) in items" :key="index">
            <img :src="item.image" alt="Book Cover" class="book-cover" />
            <div class="card-content">
              <h3>{{ item.title }}</h3>
              <p>{{ item.description }}</p>
              <span class="status">{{ item.status }}</span>
            </div>
          </div>
        </div>
      </main>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    name: "HomeComponent",
    data() {
      return {
        items: [], // Khởi tạo mảng items rỗng
      };
    },
    mounted() {
      this.fetchBooks(); // Gọi hàm fetchBooks khi component được mount
    },
    methods: {
      async fetchBooks() {
        try {
          const response = await axios.get('/book/books'); // Gọi API
          this.items = response.data; // Gán dữ liệu từ API vào items
        } catch (error) {
          console.error("Error fetching books:", error);
        }
      },
    },
  };
  </script>
  
  <style scoped>
  .home-container {
    background-color: #1a2238;
    color: #ffffff;
    padding: 20px;
  }
  
  .header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 10px 0;
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
  
  .navbar {
    display: flex;
    align-items: center;
  }
  
  .navbar a {
    color: #fbbf24;
    margin: 0 15px;
    text-decoration: none;
  }
  
  .search-input {
    padding: 5px;
    border-radius: 5px;
  }
  
  .content {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    margin-top: 20px;
  }
  
  .card-container {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
  }
  
  .card {
    background-color: #e0e3eb;
    border-radius: 10px;
    margin: 10px;
    width: 200px;
    padding: 10px;
    text-align: center;
  }
  
  .book-cover {
    width: 100%;
    height: auto;
    border-radius: 5px;
  }
  
  .status {
    background-color: #fbbf24;
    padding: 5px;
    border-radius: 5px;
    color: #1a2238;
  }
  </style>
  