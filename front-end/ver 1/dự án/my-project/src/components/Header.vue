<template>
  <header class="header">
    <div class="user-info">
      <div class="user-icon"></div>
      <div class="user-details">
        <p>{{ userName }}</p>
        <p>{{ userEmail }}</p>
      </div>
    </div>
    <nav class="nav-bar">
      <router-link to="/">Home</router-link>
      <router-link to="/my-order">My Order</router-link>
      <router-link to="/notification">Notification</router-link>
      <router-link to="/account">Account</router-link>
    </nav>
  </header>
</template>

<script>
export default {
  name: "AppHeader",
  data() {
    return {
      userName: "YOUR NAME",
      userEmail: "youremail@gmail.com",
    };
  },
};
</script>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  background-color: #333;
  color: white;
  padding: 10px;
}

.user-info {
  display: flex;
  align-items: center;
}

.nav-bar {
  display: flex;
  gap: 15px;
}
</style>
