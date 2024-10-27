<template>
    <div class="account-settings">
      <!-- Top Navigation Bar -->
      <div class="navbar">
        <a href="#">Home</a>
        <a href="#">My Order</a>
        <a href="#">Notification</a>
        <a href="#">Account</a>
      </div>
      
      <!-- Account Form Section -->
      <div class="form-container">
        <h2>Account Setting</h2>
        <form @submit.prevent="saveSettings">
          <div class="form-group">
            <label for="email">Email</label>
            <input
              type="email"
              id="email"
              v-model="email"
              placeholder="Enter your email"
              required
            />
          </div>
  
          <div class="form-group">
            <label for="password">Password</label>
            <input
              type="password"
              id="password"
              v-model="password"
              placeholder="Enter your password"
              required
            />
          </div>
  
          <div class="form-group">
            <label for="phone">Phone Number</label>
            <input
              type="tel"
              id="phone"
              v-model="phoneNumber"
              placeholder="Enter your phone number"
              required
            />
          </div>
  
          <div class="form-group">
            <label for="address">Your Address</label>
            <input
              type="text"
              id="address"
              v-model="address"
              placeholder="Enter your address"
              required
            />
          </div>
  
          <button type="submit" class="save-btn">Save</button>
        </form>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name: " Editaccount",
    name: "Editaccount",
    data() {
      return {
        email: "boconganbhaytruocgio@gmail.com",
        password: "********",
        phoneNumber: "0763 012341",
        address: "Trảng Bom"
      };
    },
    methods: {
      saveSettings() {
        // Logic to save form data goes here
        alert('Settings Saved!');
      }
    }
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
  
  /* Form Container */
  .form-container {
    background-color: #f9f9f9;
    padding: 20px;
    border-radius: 10px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
    width: 400px;
    margin: 0 auto;
  }
  
  h2 {
    text-align: center;
    margin-bottom: 20px;
  }
  
  .form-group {
    margin-bottom: 15px;
  }
  
  label {
    display: block;
    margin-bottom: 5px;
  }
  
  input[type="email"],
  input[type="password"],
  input[type="tel"],
  input[type="text"] {
    width: 100%;
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 5px;
  }
  
  .save-btn {
    width: 100%;
    padding: 10px;
    background-color: #f0c14b;
    border: 1px solid #a88734;
    border-radius: 5px;
    cursor: pointer;
  }
  
  .save-btn:hover {
    background-color: #ddb347;
  }
  </style>