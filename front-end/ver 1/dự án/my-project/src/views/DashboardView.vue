<template>
    <div>
      <!-- Sử dụng component NavBarComponent ở đây -->
      <NavBarComponent />
      <div class="dashboard">
        <div class="books-container">
          <BookCard 
            v-for="book in books" 
            :key="book.id" 
            :image="book.image" 
            :title="book.title" 
            :description="book.description" 
            :backgroundColor="book.backgroundColor" 
          />
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import NavBarComponent from "@/components/NavBarComponent.vue";
  import BookCard from "@/components/BookDetailComponent.vue";
  
  export default {
    name: "DashboardView",
    components: {
      NavBarComponent,
      BookCard,
    },
    data() {
      return {
        books: [
          {
            id: 1,
            image: "https://via.placeholder.com/100",
            title: "Cuộc Chiến",
            description: "Lorem ipsum Lorem ipsum Lorem ipsum",
            backgroundColor: "#ffcccb"
          },
          {
            id: 2,
            image: "https://via.placeholder.com/100",
            title: "Đắc Nhân Tâm",
            description: "Lorem ipsum Lorem ipsum Lorem ipsum",
            backgroundColor: "#add8e6"
          },
          {
            id: 3,
            image: "https://via.placeholder.com/100",
            title: "Kiều Kiều",
            description: "Lorem ipsum Lorem ipsum Lorem ipsum",
            backgroundColor: "#ffcccb"
          }
        ]
      };
    }
  };
  </script>
  
  <style scoped>
  .dashboard {
    display: flex;
    justify-content: center;
    background-color: #2c3e50;
    padding: 20px;
  }
  .books-container {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
  }
  </style>
  