<template>
  <header class="header">
    <div class="user-info">
      <img src="@/assets/user-icon.png" alt="User Icon" class="user-icon" />
      <span class="user-name">Admin</span>
    </div>
    <nav class="navbar">
      <router-link to="/home">Home</router-link>
      <router-link to="/wait-approval" class="active"
        >Wait Approval</router-link
      >
      <router-link to="/account">Account</router-link>
    </nav>
  </header>
  <div class="wait-approval-container">
    <div class="date-navigation">
      <div class="date-picker">
        <label for="from">From: </label>
        <input
          type="date"
          id="from"
          v-model="fromDate"
          @change="fetchPendingBooks"
        />
        <label for="to">To:</label>
        <input
          type="date"
          id="to"
          v-model="toDate"
          @change="fetchPendingBooks"
        />
      </div>
    </div>
    <main class="content">
      <div class="books-container">
        <div class="book-card" v-for="(book, index) in books" :key="index">
          <img :src="book.CoverImage" alt="Book Cover" class="book-cover" />
          <div class="book-details">
            <p class="book-title">{{ book.Title }}</p>
            <p class="book-description">{{ book.Description }}</p>
            <div class="book-description">
              <span style="font-weight: bold">Rental Date: </span
              >{{ book.RentalDate }}
            </div>
            <div class="book-description">
              <span style="font-weight: bold">Due Date: </span
              >{{ book.DueDate }}
            </div>
            <div class="actions">
              <button @click="approveBook(book)">Approve</button>
              <button @click="openRejectModal(book)">Reject</button>
            </div>
          </div>
        </div>
      </div>
    </main>

    <!-- Modal for rejection reason -->
    <div v-if="showRejectModal" class="modal">
      <div class="modal-content">
        <span class="close" @click="closeRejectModal">&times;</span>
        <h2>Enter Rejection Reason</h2>
        <p class="book-title">Title: {{ selectedBook.Title }}</p>
        <textarea
          v-model="rejectionReason"
          placeholder="Please enter the reason..."
        ></textarea>
        <button @click="submitRejection">Submit</button>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "@/utils/axios";

export default {
  name: "WaitApprovalComponent",
  data() {
    return {
      fromDate: new Date().toISOString().split("T")[0],
      toDate: new Date().toISOString().split("T")[0],
      books: [],
      showRejectModal: false,
      selectedBook: null,
      rejectionReason: "",
    };
  },
  methods: {
    async fetchPendingBooks() {
      try {
        const response = await axios.get(
          `/waitapprove/pending?from=${this.fromDate}&to=${this.toDate}`
        );
        this.books = response.data.data;
      } catch (error) {
        console.error("Error fetching pending books:", error);
      }
    },
    async approveBook(book) {
      try {
        const response = await axios.post(
          `/waitapprove/approve/${book.RentalID}`
        );
        console.log("Approved:", response.data.message);
        this.fetchPendingBooks();
      } catch (error) {
        console.error("Error approving book:", error);
      }
    },
    openRejectModal(book) {
      this.selectedBook = book;
      this.showRejectModal = true;
      this.rejectionReason = ""; // Reset lý do khi mở modal
    },
    closeRejectModal() {
      this.showRejectModal = false;
    },
    submitRejection() {
      if (this.rejectionReason) {
        axios
          .post(`/waitapprove/reject/${this.selectedBook.RentalID}`, {
            reason: this.rejectionReason,
          })
          .then((response) => {
            alert("Successfull");
            console.log("Rejected:", response.data.message);
            this.fetchPendingBooks();
            this.closeRejectModal();
          })
          .catch((error) => {
            if (error.response) {
              // Yêu cầu đã được gửi và máy chủ đã trả về mã trạng thái khác với 2xx
              console.error("Error rejecting book:", error.response.data);
              console.error("Status Code:", error.response.status);
            } else if (error.request) {
              // Yêu cầu đã được gửi nhưng không nhận được phản hồi
              console.error("No response received:", error.request);
            } else {
              // Một lỗi khác đã xảy ra trong khi thiết lập yêu cầu
              console.error("Error:", error.message);
            }
          });
      } else {
        console.log("Rejection reason is required.");
      }
      console.log(this.selectedBook.RentalID, this.rejectionReason);
    },
  },
  mounted() {
    this.fetchPendingBooks();
  },
};
</script>

<style scoped>
.wait-approval-container {
  padding: 20px;
  background-color: #f5f5f5;
  height: auto;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: #1a2238;
  color: #ffffff;
  padding: 10px;
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
}

.navbar a {
  color: #f7f6f1;
  margin: 0 15px;
  text-decoration: none;
}

.navbar .active {
  font-weight: bold;
  color: #ffcc00;
}

.date-navigation {
  margin: 20px 0;
  display: flex;
  justify-content: center;
  align-items: center;
}
.content{
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  margin-top: 20px;
}

.books-container {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
}

.book-card {
  display: flex;
  flex-direction: row;
  justify-content: space-around;
  background-color: #ffffff;
  border-radius: 10px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  margin: 10px;
  padding: 12px;
  width: 30%;
}

.book-cover {
  width: 45%;
  display: flex;
  align-self: center;
  padding-right: 4%;
  border-radius: 5px;
  height: 100%;
}

.book-cover img {
  max-width: 40%;
  border-radius: 5px;
}

.book-title {
  font-size: 1.2em;
  margin: 10px 0;
}

.book-description {
  font-size: 0.9em;
  color: #333;
}

.actions {
  display: flex;
  justify-content: space-between;
  margin-top: 10px;
}

.actions button {
  background-color: #007bff;
  color: white;
  border: none;
  padding: 5px 10px;
  border-radius: 5px;
  cursor: pointer;
}

.actions button:hover {
  background-color: #0056b3;
}

/* Modal Styles */
.modal {
  display: flex;
  position: fixed;
  z-index: 1;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  overflow: auto;
  background-color: rgba(0, 0, 0, 0.5);
}

.modal-content {
  background-color: #fefefe;
  margin: 15% auto;
  padding: 20px;
  border: 1px solid #888;
  width: 80%;
  max-width: 500px;
  border-radius: 10px;
}

.close {
  color: #e01717;
  float: right;
  font-size: 28px;
  font-weight: bold;
}

.close:hover,
.close:focus {
  color: black;
  text-decoration: none;
  cursor: pointer;
}

textarea {
  width: 100%;
  height: 100px;
  margin-top: 10px;
}
</style>
