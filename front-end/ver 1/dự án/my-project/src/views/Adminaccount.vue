<template>
    <div class="account">
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
  
      <main class="form-container">
        <h2>Account</h2>
        <form @submit.prevent="save">
          <div class="form-group">
            <label for="username">Username</label>
            <input
              type="text"
              id="username"
              v-model="account.username"
              placeholder="Enter your username"
            />
          </div>
  
          <div class="form-group">
            <label for="email">Email</label>
            <input
              type="email"
              id="email"
              v-model="account.email"
              placeholder="Enter your email"
            />
          </div>
  
          <div class="form-group">
            <label for="phone">Phone Number</label>
            <input
              type="tel"
              id="phone"
              v-model="account.phone"
              placeholder="Enter your phone number"
            />
          </div>
  
          <div class="form-group">
            <label for="address">Your Address</label>
            <textarea
              id="address"
              v-model="account.address"
              placeholder="Enter your address"
            ></textarea>
          </div>
  
          <button type="submit">Save</button>
        </form>
      </main>
    </div>
  </template>
  
  <script>
  export default {
    name: "Adminaccount",
    data() {
      return {
        account: {
          username: "Nguyễn Thị A",
          email: "admin@email.com",
          phone: "0763 012341",
          address: "Trảng Bom",
        },
      };
    },
    methods: {
      save() {
        // This function can be used to handle form submission
        console.log("Form data saved", this.account);
        alert("Account details saved!");
      },
    },
  };
  </script>
  
  <style scoped>
  body {
    font-family: Arial, sans-serif;
    margin: 0;
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
  
  .form-container {
    margin: 40px auto;
    max-width: 400px;
    background-color: #d1d5db;
    padding: 20px;
    border-radius: 10px;
  }
  
  .form-container h2 {
    text-align: center;
    margin-bottom: 20px;
  }
  
  .form-group {
    margin-bottom: 15px;
  }
  
  .form-group label {
    display: block;
    margin-bottom: 5px;
  }
  
  .form-group input,
  .form-group textarea {
    width: 100%;
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 5px;
  }
  
  textarea {
    resize: none;
    height: 60px;
  }
  
  button {
    width: 100%;
    padding: 10px;
    background-color: #fbbf24;
    border: none;
    color: white;
    font-weight: bold;
    border-radius: 5px;
    cursor: pointer;
  }
  
  button:hover {
    background-color: #f59e0b;
  }
  </style>
  