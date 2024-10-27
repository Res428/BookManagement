<template>
    <div class="dashboard">
      <header class="header">
        <div class="admin-info">
          <img src="https://via.placeholder.com/50" alt="Admin" />
          <h1>Admin</h1>
        </div>
        <nav class="nav">
          <a href="#">Home</a>
          <a href="#">Wait Approval</a>
          <a href="#">Account</a>
          <input type="search" placeholder="Search..." />
        </nav>
      </header>
  
      <main class="content">
        <div class="book-grid">
          <div
            v-for="(book, index) in books"
            :key="index"
            class="book-item"
            :style="{ backgroundColor: getColor(index) }"
          >
            <img :src="book.image" :alt="book.title" />
            <p>{{ book.title }}</p>
            <button>Detail</button>
          </div>
        </div>
      </main>
    </div>
  </template>
  
  <script>
  export default {
    name : "Admin",
    data() {
      return {
        books: [
          {
            title: "Book Title 1",
            image: "https://via.placeholder.com/100x150",
          },
          {
            title: "Đắc Nhân Tâm",
            image: "https://via.placeholder.com/100x150",
          },
          {
            title: "Kiều",
            image: "https://via.placeholder.com/100x150",
          },
          // Add more book objects as needed
        ],
      };
    },
    methods: {
      getColor(index) {
        // Alternate background colors based on index
        return index % 2 === 0 ? "#fad3e4" : "#d3d7fa";
      },
    },
  };
  </script>
  
  <style scoped>
  body {
    margin: 0;
    font-family: Arial, sans-serif;
  }
  .header {
    background-color: #1e293b;
    padding: 10px;
    color: white;
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
  .header img {
    border-radius: 50%;
  }
  .header h1 {
    margin: 0;
  }
  .nav {
    display: flex;
    align-items: center;
  }
  .nav a {
    color: white;
    text-decoration: none;
    margin-right: 20px;
    font-weight: bold;
  }
  .nav input {
    padding: 5px;
  }
  .content {
    padding: 20px;
  }
  .book-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 20px;
  }
  .book-item {
    border-radius: 10px;
    padding: 10px;
    text-align: center;
    transition: background-color 0.3s ease;
  }
  .book-item img {
    width: 100px;
    height: 150px;
    margin-bottom: 10px;
  }
  .book-item button {
    background-color: #fbbf24;
    border: none;
    padding: 10px;
    color: white;
    font-weight: bold;
    border-radius: 5px;
    cursor: pointer;
  }
  .book-item button:hover {
    background-color: #f59e0b;
  }
  </style>
  