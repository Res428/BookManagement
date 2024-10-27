<template>
    <v-container class="pa-4">
      <v-row>
        <!-- Left Book Card -->
        <v-col cols="3">
          <v-card class="mx-auto">
            <v-img
              src="path/to/book-image.jpg" <!-- Thay bằng đường dẫn tới hình ảnh sách -->
              height="200px"
            ></v-img>
            <v-card-title class="d-flex justify-center">
              Lorem ipsum Lorem ipsum
            </v-card-title>
          </v-card>
        </v-col>
  
        <!-- Right Content -->
        <v-col cols="9">
          <!-- Date selection -->
          <v-row class="mb-4">
            <v-col cols="6">
              <v-btn block color="yellow" @click="selectFromDate">
                Mon, 9 Sep 2024
              </v-btn>
            </v-col>
            <v-col cols="6">
              <v-btn block color="yellow" @click="selectToDate">
                Sat, 14 Sep 2024
              </v-btn>
            </v-col>
          </v-row>
  
          <!-- Content Grid -->
          <v-row>
            <v-col cols="4" v-for="n in 6" :key="n">
              <v-card class="pa-4" :class="getCardClass(n)">
                <!-- Content can be dynamically added here -->
              </v-card>
            </v-col>
          </v-row>
        </v-col>
      </v-row>
    </v-container>
  </template>
  
  <script>
  export default {
    name: "MyOrder",
    methods: {
      selectFromDate() {
        // Logic để chọn ngày "From" (có thể thêm modal chọn ngày)
      },
      selectToDate() {
        // Logic để chọn ngày "To" (có thể thêm modal chọn ngày)
      },
      getCardClass(n) {
        // Hàm trả về class màu sắc cho các thẻ (đỏ/hồng/xanh)
        return n % 2 === 0 ? "bg-blue-gradient" : "bg-pink-gradient";
      }
    }
  };
  </script>
  
  <style scoped>
  .bg-pink-gradient {
    background: linear-gradient(to bottom right, #ff7e5f, #feb47b);
  }
  .bg-blue-gradient {
    background: linear-gradient(to bottom right, #6a11cb, #2575fc);
  }
  .v-btn {
    background: #ffeb3b !important;
    color: #000 !important;
  }
  </style>
  