<template>
  <header class="header">
    <div class="user-info">
      <img src="@/assets/user-icon.png" alt="User Icon" class="user-icon" />
      <p>{{ username }}</p>
      <p>{{ email }}</p>
    </div>
    <nav class="navbar">
      <router-link to="/home" class="active">Home</router-link>
      <router-link v-if="isAdmin" to="/wait-approval"
        >Wait Approval</router-link
      >
      <router-link v-if="isCustomer" to="/orders">My Order</router-link>
      <router-link v-if="isCustomer" to="/notifications"
        >Notification</router-link
      >
      <router-link to="/account">Account</router-link>
      <input
        type="text"
        placeholder="Search..."
        class="search-input"
        v-model="searchQuery"
        @input="filterBooks"
      />
    </nav>
  </header>
  <div class="home-container">
    <div style="display: flex; justify-content: space-between">
      <div class="total-books">
        <p>Total books: {{ items.length }}</p>
      </div>
      <div style="justify-self: end">
        <button v-if="isAdmin" @click="AddBookPopup">Add Book</button>
      </div>
    </div>
    <main class="content">
      <div class="card-container">
        <div class="card" v-for="(item, index) in filteredItems" :key="index">
          <img :src="item.CoverImage" alt="Book Cover" class="book-cover" />
          <div class="card-content">
            <h3 class="card-title">{{ item.Title }}</h3>
            <p class="card-description">{{ item.Description }}</p>
            <p class="card-description mt-1">
              Số lượng: {{ item.StockQuantity }}
            </p>
            <div class="btn">
              <button v-if="isAdmin" @click="viewDetails(item)">Details</button>
              <button v-else @click="rentBook(item)">Rent</button>
            </div>
          </div>
        </div>
      </div>
    </main>

    <!-- Pop-up thêm sách -->
    <div v-if="showAddBookPopup" class="popup">
      <div class="popup-content">
        <h2>Add New Book</h2>
        <input v-model="newBook.Title" placeholder="Title" />
        <input v-model="newBook.Author" placeholder="Author" />
        <input v-model="newBook.ISBN" placeholder="ISBN" />
        <textarea
          v-model="newBook.Description"
          placeholder="Description"
        ></textarea>
        <input
          v-model="newBook.CategoryID"
          type="number"
          placeholder="Category ID"
        />
        <input v-model="newBook.PublishedDate" type="date" />
        <input v-model="newBook.Price" type="number" placeholder="Price" />
        <input
          v-model="newBook.StockQuantity"
          type="number"
          placeholder="Stock Quantity"
        />
        <label>
          <input type="checkbox" v-model="newBook.IsAvailableForRent" />
          Available for Rent
        </label>
        <input v-model="newBook.CoverImage" placeholder="Cover Image URL" />
        <button @click="addBook">Add</button>
        <button @click="closeAddBookPopup">Cancel</button>
      </div>
    </div>

    <!-- Pop-up chi tiết sách -->
    <div v-if="showDetailsPopup" class="popup">
      <div class="popup-content">
        <div class="pop-up">
          <img
            :src="selectedBook.CoverImage"
            alt="Book Cover"
            class="popup-book-cover"
          />
          <div class="pop-up-content">
            <h2>Book Details</h2>
            <h3 class="card-title">{{ selectedBook.Title }}</h3>
            <p class="card-description">
              <strong>Author:</strong> {{ selectedBook.Author }}
            </p>
            <p class="card-description">
              <strong>Description:</strong> {{ selectedBook.Description }}
            </p>
            <p class="card-description">
              <strong>ISBN:</strong> {{ selectedBook.ISBN }}
            </p>
            <p class="card-description">
              <strong>Published Date:</strong> {{ selectedBook.PublishedDate }}
            </p>
            <p class="card-description">
              <strong>Price:</strong> {{ selectedBook.Price }}
            </p>
            <p class="card-description">
              <strong>Stock Quantity:</strong> {{ selectedBook.StockQuantity }}
            </p>
            <div class="btn-popup">
              <button @click="editBook">Edit</button>
              <button @click="deleteBook(selectedBook.BookID)">Delete</button>
              <button @click="closePopup">Close</button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Pop-up chỉnh sửa sách -->
    <div v-if="showEditPopup" class="popup">
      <div class="popup-content">
        <div class="pop-up">
          <img
            :src="selectedBook.CoverImage"
            alt="Book Cover"
            class="popup-book-cover"
          />
          <div class="pop-up-content">
            <h2>Edit Book</h2>
            <label for="" style="color: black">Title: </label>
            <input
              v-model="selectedBook.Title"
              placeholder="Title"
              style="width: 100%"
            />
            <label for="" style="color: black">Author: </label>
            <input v-model="selectedBook.Author" placeholder="Author" /> <br />
            <label for="" style="color: black">Description: </label><br />
            <textarea
              v-model="selectedBook.Description"
              placeholder="Description"
            ></textarea
            ><br />
            <label for="" style="color: black">Image: </label>
            <input
              v-model="selectedBook.CoverImage"
              placeholder="Cover Image URL"
            /><br />
            <label for="" style="color: black">ISBN: </label>
            <input v-model="selectedBook.ISBN" placeholder="ISBN" /><br />
            <label for="" style="color: black">Published Date: </label>
            <input v-model="selectedBook.PublishedDate" type="date" /><br />
            <label for="" style="color: black">Price: </label>
            <input
              v-model="selectedBook.Price"
              type="number"
              placeholder="Price"
            /><br />
            <label for="" style="color: black">Stock Quantity: </label>
            <input
              v-model="selectedBook.StockQuantity"
              type="number"
              placeholder="Stock Quantity"
            />
            <div class="btn-popup">
              <button @click="saveBook">Save</button>
              <button @click="closeEditPopup">Cancel</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "@/utils/axios";

export default {
  name: "HomeComponent",
  data() {
    return {
      items: [],
      filteredItems: [],
      searchQuery: "",
      username: "",
      email: "",
      isAdmin: false,
      userId: "",
      showDetailsPopup: false,
      showEditPopup: false,
      showAddBookPopup: false,
      selectedBook: {},
      newBook: {
        Title: "",
        Author: "",
        Description: "",
        ISBN: "",
        PublishedDate: "",
        Price: 0,
        StockQuantity: 0,
        IsAvailableForRent: true,
        CategoryID: "",
        CoverImage: "",
      },
    };
  },
  mounted() {
    this.loadUserData();
    this.fetchBooks();
  },
  methods: {
    loadUserData() {
      const userData = JSON.parse(localStorage.getItem("user"));
      if (userData) {
        this.username = userData.username || "";
        this.email = userData.email || "";
        this.isAdmin = userData.role === "admin";
        this.isCustomer = userData.role === "customer";
        this.userId = userData.userID || "";
      }
    },
    async fetchBooks() {
      try {
        const response = await axios.get("/book/books");
        this.items = response.data.data;
        this.filteredItems = this.items;
      } catch (error) {
        console.error("Error fetching books:", error);
      }
    },
    filterBooks() {
      if (this.searchQuery) {
        this.filteredItems = this.items.filter(
          (item) =>
            item.Title.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
            item.Description.toLowerCase().includes(
              this.searchQuery.toLowerCase()
            )
        );
      } else {
        this.filteredItems = this.items;
      }
    },
    viewDetails(item) {
      this.selectedBook = { ...item }; // Sao chép thông tin sách
      this.showDetailsPopup = true; // Hiển thị pop-up chi tiết
    },
    rentBook(item) {
      this.$router.push({
        path: "/rent",
        query: {
          bookId: item.BookID,
          coverImage: item.CoverImage,
          title: item.Title,
          author: item.Author,
          description: item.Description,
          stockQuantity: item.StockQuantity,
        },
      });
    },
    async addBook() {
      if (
        !this.newBook.Title ||
        !this.newBook.Author ||
        !this.newBook.Description ||
        !this.newBook.ISBN ||
        !this.newBook.PublishedDate ||
        this.newBook.Price < 0 ||
        this.newBook.StockQuantity < 0 ||
        this.newBook.CategoryID === null
      ) {
        alert("Please fill in all fields correctly.");
        return;
      }

      try {
        const response = await axios.post("/book/add", this.newBook);
        console.log("Response:", response.data);
        this.closeAddBookPopup();
        this.fetchBooks(); // Cập nhật danh sách sách
      } catch (error) {
        console.error(
          "Error adding book:",
          error.response ? error.response.data : error.message
        );
        alert(
          "Failed to add book: " +
            (error.response?.data?.message || error.message)
        );
      }
      console.log(this.newBook);
    },
    async saveBook() {
      try {
        await axios.post(
          `/book/update/${this.selectedBook.BookID}`,
          this.selectedBook
        );
        this.closeEditPopup();
        this.fetchBooks(); // Cập nhật danh sách sách
      } catch (error) {
        console.error("Error updating book:", error);
      }
    },
    async deleteBook(bookId) {
      try {
        await axios.post(`/book/delete/${bookId}`);
        this.fetchBooks(); // Cập nhật danh sách sách
        this.closePopup(); // Đóng pop-up chi tiết
      } catch (error) {
        console.error("Error deleting book:", error);
      }
    },
    editBook() {
      this.showEditPopup = true; // Hiển thị pop-up chỉnh sửa
      this.showDetailsPopup = false; // Ẩn pop-up chi tiết
    },
    AddBookPopup() {
      this.showAddBookPopup = true;
      this.newBook = {
        // Đặt lại thông tin sách mới
        Title: "",
        Author: "",
        Description: "",
        ISBN: "",
        PublishedDate: "",
        Price: 0,
        StockQuantity: 0,
        CoverImage: "",
      };
    },
    closeAddBookPopup() {
      this.showAddBookPopup = false;
    },
    closePopup() {
      this.showDetailsPopup = false;
    },
    closeEditPopup() {
      this.showEditPopup = false;
      this.showDetailsPopup = true;
    },
  },
};
</script>

<style scoped>
.home-container {
  color: #ffffff;
  padding: 20px;
}

.header {
  background-color: #1a2238;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px 0;
  color: #ffffff;
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
  color: #f7f6f1;
  margin: 0 15px;
  text-decoration: none;
}

nav .active {
  font-weight: bold;
  color: #ffcc00; /* Màu vàng cho liên kết đang hoạt động */
}
.navbar button {
  margin-left: 10px;
  padding: 5px 10px;
  background-color: #ffcc00;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  color: #000;
}
.search-input {
  padding: 5px;
  border-radius: 5px;
  color: #fff;
}

.content {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  margin-top: 20px;
}

.total-books {
  margin-left: 3%;
  margin-top: 1%;
  margin-bottom: 20px;
  font-size: 1.2em;
  color: #080601;
}
.card-container {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
}
.card {
  display: flex;
  flex-direction: row;
  justify-content: space-around;
  background-color: #7ba0ef;
  border-radius: 10px;
  margin: 10px;
  width: 30%;
  padding: 15px;
  transition: transform 0.2s;
}

.card:hover {
  transform: scale(1.05);
}

.card-title {
  font-size: 1.2em;
  margin: 10px 0;
}

.card-description {
  font-size: 0.9em; 
  color: #333; 
}
.book-cover {
  width: 40%;
  display: flex;
  align-items: center;
  padding-right: 6%;
}

.book-cover img {
  max-width: 30%;
  border-radius: 5px;
}

label {
  font-weight: bold;
}

.popup {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}
.popup-content {
  display: flex;
  justify-content: center;
  align-items: flex-start;
}

.pop-up {
  display: flex;
  flex-direction: row;
  background-color: #7ba0ef;
  border-radius: 10px;
  padding: 15px;
  width: 50%;
}

.pop-up-content {
  margin-left: 15px;
  flex: 1;
  margin-right: 0%;
}

.popup-book-cover {
  width: 40%;
  border-radius: 5px;
}
button {
  margin-top: 10px;
  padding: 5px 10px;
  background-color: #ffcc00;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  color: #000;
}

.btn-popup {
  display: flex;
  justify-content: space-around;
  margin-top: 14%;
  margin-right: 12%;
}

.btn {
  justify-self: end;
}
</style>
