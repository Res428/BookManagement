<template>
  <header>
    <div class="user-info">
      <img src="@/assets/user-icon.png" alt="User Icon" class="user-icon" />
      <span>{{ username }}</span>
      <span>{{ email }}</span>
    </div>
    <nav>
      <router-link to="/home">Home</router-link>
      <router-link to="/orders" class="active">My Order</router-link>
      <router-link to="/notifications">Notification</router-link>
      <router-link to="/account">Account</router-link>
    </nav>
  </header>
  <div class="my-order">
    <div class="date-navigation">
      <div class="date-picker">
        <label for="from">From: </label>
        <input
          type="date"
          id="from"
          v-model="fromDate"
          @change="fetchBorrowedBooks"
        />
        <label for="to">To:</label>
        <input
          type="date"
          id="to"
          v-model="toDate"
          @change="fetchBorrowedBooks"
        />
      </div>
    </div>
    <div class="order-list">
      <div
        v-for="(order, index) in filteredOrders"
        :key="index"
        class="order-item"
      >
        <img :src="order.coverImage" alt="Order Image" />
        <div class="order-details">
          <h2>{{ order.title }}</h2>
          <p>Author: {{ order.author }}</p>
          <span>Rental Date: {{ formatDate(order.rentalDate) }}</span
          ><br />
          <span>Due Date: {{ formatDate(order.dueDate) }}</span
          ><br />
          <span>Status: {{ order.status }}</span>
          <button @click="returnOrder(order.rentalID)">Return</button>
        </div>
      </div>
      <div class="empty-order" v-if="filteredOrders.length === 0">
        <p>No orders available.</p>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "@/utils/axios";
import { mapActions } from "vuex";

export default {
  name: "MyOrderComponent",
  data() {
    return {
      username: "",
      email: "",
      userId: "",
      fromDate: new Date().toISOString().split("T")[0], // Ngày hiện tại
      toDate: new Date().toISOString().split("T")[0],
      orders: [],
    };
  },
  computed: {
    filteredOrders() {
      return this.orders.filter((order) => {
        const rentalDate = new Date(order.rentalDate);
        const dueDate = new Date(order.dueDate);
        return (
          rentalDate >= new Date(this.fromDate) &&
          dueDate <= new Date(this.toDate)
        );
      });
    },
  },
  mounted() {
    this.loadUserData();
    this.fetchBorrowedBooks();
  },
  methods: {
    ...mapActions(["addNotification"]),
    loadUserData() {
      const userData = JSON.parse(localStorage.getItem("user"));
      if (userData) {
        this.username = userData.username || "";
        this.email = userData.email || "";
        this.userId = userData.userID || "";
      }
    },
    async fetchBorrowedBooks() {
      const fromDate = this.fromDate;
      const toDate = this.toDate;
      try {
        const response = await axios.get(
          `/rental/borrowed-books/${this.userId}?from=${fromDate}&to=${toDate}`,
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("auth")}`,
            },
          }
        );
        this.orders = response.data; // Cập nhật danh sách sách mượn
        console.log(this.orders);
      } catch (error) {
        if (error.response && error.response.status === 401) {
          alert("Session expired. Please log in again.");
        } else {
          console.error("Error fetching borrowed books:", error);
        }
      }
    },
    formatDate(dateString) {
      const options = { year: "numeric", month: "2-digit", day: "2-digit" };
      return new Date(dateString).toLocaleDateString(undefined, options);
    },
    async returnOrder(rentalID) {
      try {
        const response = await axios.put(
          `/rental/return/${rentalID}`,
          {},
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("auth")}`,
            },
          }
        );
        alert(response.data.message);
        console.log(rentalID);
        this.addNotification(`${response.data.message}`);

        this.fetchBorrowedBooks();
      } catch (error) {
        console.error("Error returning book:", error);
        this.addNotification(`${error}`);
        console.log(rentalID);
      }
    },
  },
};
</script>

<style scoped>
.my-order {
  padding: 20px;
  background-color: #f0f0f0;
  height: 100vh;
}

header {
  background-color: #1a2238;
  color: white;
  padding: 10px;
  display: flex;
  justify-content: space-between;
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

nav a {
  color: white;
  margin: 0 15px;
  text-decoration: none;
}

nav .active {
  font-weight: bold;
  color: #ffcc00; /* Màu vàng cho liên kết đang hoạt động */
}

.date-navigation {
  margin: 20px 0;
  display: flex;
  justify-content: center;
  align-items: center;
}

.order-list {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 20px;
}

.order-item {
  background-color: #ffcc00; /* Màu nền cho đơn hàng */
  border-radius: 8px;
  padding: 15px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
}

.order-item img {
  max-width: 100%;
  border-radius: 4px;
}

.order-details {
  margin-top: 10px;
}

.order-details h2 {
  font-size: 1.2em;
  margin: 0;
}

.order-details p {
  margin: 5px 0;
}

.order-details button {
  background-color: #ffcc00; /* Màu nền cho nút trả hàng */
  border: none;
  padding: 10px;
  cursor: pointer;
  border-radius: 5px;
  color: #fff;
}

.empty-order {
  grid-column: span 3; /* Chiếm toàn bộ cột nếu không có đơn hàng */
  text-align: center;
  color: #999;
}
</style>
