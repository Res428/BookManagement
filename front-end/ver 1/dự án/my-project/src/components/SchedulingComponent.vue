<template>
  <header>
    <div class="user-info">
      <img src="@/assets/user-icon.png" alt="User Icon" class="user-icon" />
      <span>{{ username }}</span>
      <span>{{ email }}</span>
    </div>
    <nav>
      <router-link to="/home">Home</router-link>
      <router-link to="/orders">My Order</router-link>
      <router-link to="/notifications">Notification</router-link>
      <router-link to="/account">Account</router-link>
    </nav>
  </header>
  <div class="scheduling-component">
    <main>
      <div class="scheduling-content">
        <div class="book-details">
          <div class="book-image">
            <img :src="bookInfo.CoverImage" alt="Book Cover" />
          </div>
          <div class="book-info">
            <p>Title: {{ bookInfo.Title }}</p>
            <p>Author: {{ bookInfo.Author }}</p>
            <p>Description: {{ bookInfo.Description }}</p>
            <p>Stock Quantity: {{ bookInfo.StockQuantity }}</p>
          </div>
        </div>
        <div class="scheduler">
          <h2>Scheduling</h2>
          <div class="date-picker">
            <label for="from">From: </label>
            <input
              type="date"
              id="from"
              v-model="fromDate"
              @change="validateDates"
            />
            <label for="to">To:</label>
            <input
              type="date"
              id="to"
              v-model="toDate"
              @change="validateDates"
            />
          </div>
          <button @click="order">Rent</button>
        </div>
      </div>
    </main>
  </div>
</template>

<script>
import axios from "@/utils/axios";
import { mapActions } from "vuex";

export default {
  data() {
    return {
      fromDate: "",
      toDate: "",
      bookInfo: {
        CoverImage: "",
        Title: "",
        Author: "",
        Description: "",
        StockQuantity: 0,
        BookID: "",
      },
      username: "",
      email: "",
    };
  },
  created() {
    this.loadBookData();
    this.loadUserData();
  },
  methods: {
    ...mapActions(["addNotification"]),
    loadBookData() {
      const { title, author, description, coverImage, stockQuantity, bookId } =
        this.$route.query;
      console.log("Book Data from URL:", {
        title,
        author,
        description,
        coverImage,
        stockQuantity,
        bookId,
      });
      this.bookInfo = {
        CoverImage: coverImage,
        Title: title,
        Author: author,
        Description: description,
        StockQuantity: stockQuantity,
        BookID: bookId,
      };
    },
    loadUserData() {
      const userData = JSON.parse(localStorage.getItem("user"));
      if (userData) {
        this.username = userData.username || "";
        this.email = userData.email || "";
        this.token = userData.token;
      }
    },
    validateDates() {
      if (this.fromDate && this.toDate) {
        const from = new Date(this.fromDate);
        const to = new Date(this.toDate);
        if (from > to) {
          alert("Ngày 'From' không thể lớn hơn ngày 'To'. Vui lòng chọn lại.");
          this.toDate = ""; // Reset toDate nếu không hợp lệ
        }
      }
    },
    async order() {
      if (!this.fromDate || !this.toDate) {
        alert("Vui lòng chọn cả ngày 'From' và 'To'.");
        return;
      }

      const userData = JSON.parse(localStorage.getItem("user"));
      const rentalRequest = {
        UserID: userData.userID,
        BookID: this.bookInfo.BookID,
        RentalDate: this.fromDate, // Ngày mượn sách
        DueDate: this.toDate, // Ngày trả sách
      };

      console.log("Rental Request:", rentalRequest); // In yêu cầu ra console

      try {
        const response = await axios.post("/rental/request", rentalRequest, {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${localStorage.getItem("auth")}`,
          },
        });

        alert(response.data.message);
        this.addNotification(`${response.data.message}`);
        this.resetForm(); // Reset form sau khi gửi yêu cầu thành công
      } catch (error) {
        const errorMessage =
          error.response?.data?.message ||
          "Có lỗi xảy ra khi gửi yêu cầu mượn sách.";
        alert(errorMessage);
        this.addNotification(`${errorMessage}`); 
      }
    },
    resetForm() {
      this.fromDate = "";
      this.toDate = "";
    },
  },
};
</script>

<style scoped>
.scheduling-component {
  background-color: #f4f4f4;
  padding: 20px;
  height: 100vh;
  border-radius: 8px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
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

.scheduling-content {
  display: flex;
  justify-content: space-between;
  margin-top: 20px;
}

.book-details {
  display: flex;
  flex-direction: row;
  justify-content: space-around;
  background-color: #f9e5e5;
  width: 40%;
}

.book-image {
  width: 40%;
  display: flex;
  margin-left: 4%;
  align-items: center;
  margin-top: 2%;
  margin-bottom: 2%;
}

.book-image img {
  max-width: 70%;
  border-radius: 5px;
}

.book-info {
  padding: 10px;
  padding-left: 0%;
  border-radius: 8px;
  width: 60%;
  padding-top: 6%;
}

.book-info p {
  padding-bottom: 4%;
}

.scheduler {
  width: 50%;
}

.date-picker {
  margin-bottom: 20px;
}

button {
  background-color: #ffcc00;
  border: none;
  padding: 10px 20px;
  cursor: pointer;
}

button:hover {
  background-color: #e0a800; /* Thay đổi màu khi hover */
}
</style>
