<template>
    <div class="calendar">
      <div class="month">September 2024</div>
      <div class="weekdays">
        <span>Sun</span>
        <span>Mon</span>
        <span>Tue</span>
        <span>Wed</span>
        <span>Thu</span>
        <span>Fri</span>
        <span>Sat</span>
      </div>
      <div class="days">
        <span v-for="day in days" :key="day" :class="{ highlighted: isHighlighted(day) }">
          {{ day }}
        </span>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        days: Array.from({ length: 30 }, (_, i) => i + 1),
        highlightedDays: [10, 11, 12, 13, 14],
      };
    },
    methods: {
      isHighlighted(day) {
        return this.highlightedDays.includes(day);
      },
    },
  };
  </script>
  
  <style scoped>
  /* Thêm các kiểu CSS ở đây để căn chỉnh */
  </style>