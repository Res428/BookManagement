<template>
    <div class="product-page">
      <!-- Navigation Bar -->
      <div class="navbar">
        <a href="#">Home</a>
        <a href="#">My Order</a>
        <a href="#">Notification</a>
        <a href="#">Account</a>
      </div>
  
      <!-- Product Grid -->
      <div class="product-grid">
        <div
          class="product-card"
          v-for="(product, index) in products"
          :key="index"
        >
          <img :src="product.image" :alt="product.title" class="product-image" />
          <div class="product-info">
            <h3>{{ product.title }}</h3>
            <p>{{ product.description }}</p>
          </div>
          <button class="schedule-btn">Scheduling</button>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name : "Home",
    data() {
      return {
        // List of products (for demonstration purposes)
        products: [
          {
            title: "Lorem Ipsum",
            description: "Lorem ipsum Lorem ipsum",
            image: "https://via.placeholder.com/100x150", // Placeholder image
          },
          {
            title: "Lorem Ipsum",
            description: "Lorem ipsum Lorem ipsum",
            image: "https://via.placeholder.com/100x150",
          },
          {
            title: "Lorem Ipsum",
            description: "Lorem ipsum Lorem ipsum",
            image: "https://via.placeholder.com/100x150",
          },
          {
            title: "Lorem Ipsum",
            description: "Lorem ipsum Lorem ipsum",
            image: "https://via.placeholder.com/100x150",
          },
          {
            title: "Lorem Ipsum",
            description: "Lorem ipsum Lorem ipsum",
            image: "https://via.placeholder.com/100x150",
          },
          {
            title: "Lorem Ipsum",
            description: "Lorem ipsum Lorem ipsum",
            image: "https://via.placeholder.com/100x150",
          },
        ],
      };
    },
  };
  </script>
  
  <style scoped>
  /* Navbar styles */
  .navbar {
    display: flex;
    justify-content: space-between;
    margin-bottom: 20px;
    background-color: #333;
    padding: 10px;
  }
  
  .navbar a {
    text-decoration: none;
    color: white;
    padding: 10px;
  }
  
  .navbar a:hover {
    background-color: #555;
  }
  
  /* Product Grid Layout */
  .product-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 20px;
    max-width: 1200px;
    margin: 0 auto;
  }
  
  /* Individual Product Card */
  .product-card {
    background-color: #f9f9f9;
    padding: 20px;
    border-radius: 10px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
    text-align: center;
  }
  
  .product-image {
    width: 100px;
    height: 150px;
    object-fit: cover;
    margin-bottom: 15px;
  }
  
  .product-info h3 {
    margin: 10px 0;
  }
  
  .schedule-btn {
    background-color: #f0c14b;
    border: 1px solid #a88734;
    padding: 10px 20px;
    cursor: pointer;
    border-radius: 5px;
  }
  
  .schedule-btn:hover {
    background-color: #ddb347;
  }
  </style>