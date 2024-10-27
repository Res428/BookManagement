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

      <!-- Scheduling Section -->
      <v-col cols="9">
        <v-row class="mb-4">
          <v-col cols="12">
            <h2 class="text-center">Scheduling</h2>
          </v-col>
        </v-row>
        <v-row>
          <!-- Date selection -->
          <v-col cols="6">
            <v-btn block color="yellow" @click="selectFromDate">
              From: {{ fromDate }}
            </v-btn>
          </v-col>
          <v-col cols="6">
            <v-btn block color="yellow" @click="selectToDate">
              To: {{ toDate }}
            </v-btn>
          </v-col>
        </v-row>

        <!-- Calendar -->
        <v-row class="mt-4">
          <v-col cols="12">
            <v-date-picker v-model="selectedDates" color="yellow"></v-date-picker>
          </v-col>
        </v-row>

        <!-- Order Button -->
        <v-row class="mt-4">
          <v-col class="text-center">
            <v-btn block color="yellow" @click="order">Order</v-btn>
          </v-col>
        </v-row>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  name: 'Scheduling',

  data() {
    return {
      fromDate: 'Mon, 10th Sep 2024',
      toDate: 'Sat, 14th Sep 2024',
      selectedDates: '2024-09-10',
    };
  },
  methods: {
    selectFromDate() {
      // Logic để chọn ngày "From" (có thể thêm modal chọn ngày)
      this.fromDate = 'Mon, 10th Sep 2024'; // Giá trị ví dụ
    },
    selectToDate() {
      // Logic để chọn ngày "To" (có thể thêm modal chọn ngày)
      this.toDate = 'Sat, 14th Sep 2024'; // Giá trị ví dụ
    },
    order() {
      // Logic cho nút "Order"
      alert("Order thành công!"); // Ví dụ thông báo
    }
  }
};
</script>

<style scoped>
.v-date-picker {
  width: 100%;
}
</style>
