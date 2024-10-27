<template>
  <div class="book-detail-container">
    <h2>{{ book.title }}</h2>
    <img :src="book.coverImage" alt="Book Cover" class="book-cover" />
    <p><strong>Author:</strong> {{ book.author }}</p>
    <p><strong>Description:</strong> {{ book.description }}</p>
    <p><strong>ISBN:</strong> {{ book.isbn }}</p>
    <p><strong>Published Date:</strong> {{ book.publishedDate }}</p>
    <p><strong>Price:</strong> {{ book.price }}</p>
    <p><strong>Stock Quantity:</strong> {{ book.stockQuantity }}</p>
    <p><strong>Available for Rent:</strong> {{ book.isAvailableForRent ? 'Yes' : 'No' }}</p>
    <p><strong>Rent Price:</strong> {{ book.rentPrice }}</p>
    <router-link to="/wait-approval">Back to Wait Approval</router-link>
  </div>
</template>

<script>
import axios from '@/utils/axios';

export default {
  name: "BookDetailComponent",
  data() {
    return {
      book: {},
    };
  },
  methods: {
    async fetchBookDetails() {
      const bookId = this.$route.params.id;
      try {
        const response = await axios.get(`/books/${bookId}`);
        this.book = response.data; // Giả sử API trả về đối tượng sách
      } catch (error) {
        console.error("Error fetching book details:", error);
      }
    }
  },
  mounted() {
    this.fetchBookDetails();
  },
};
</script>

<style scoped>
.book-detail-container {
  padding: 20px;
  background-color: #f5f5f5;
}
.book-cover {
  width: 200px;
  border-radius: 5px;
}
</style>
